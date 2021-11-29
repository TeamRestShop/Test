using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowerType //나중에 꽃종류 추가하면
{

}

public enum AttackType
{
    Continuous,
    Discrete
}

public enum FlowerStage
{
    Seed = 0,
    Sprout,
    Flower
}

public class FlowerObjectManager : MonoBehaviour
{
    [SerializeField] private Sprite flowerSprite;
    [SerializeField] private Sprite sproutSprite;
    [SerializeField] private Sprite seedSprite;
    [SerializeField] private Sprite[] sunBarSprite; //총 10개의 햇빛게이지 표시 이미지; 햇빛이 찰수록 게이지가 차게

    [SerializeField] private GameObject attackObject;

    public FlowerType type {get; set;}
    public FlowerStage stage {get; set;}
    public AttackType attackType{get; set;} //지속형 또는 일시형
    public GameObject targetEnemy {get; set;}

    public float attackRange{get;set;} //circle collider의 반지름
    public float attackSpeed{get;set;} //1초에 몇번 공격할 수 있는지

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer sunBarSpriteRenderer;
    private GameObject sunBarObject;

    private bool inSunlight = false;
    private bool isAttacking = false;

    private float sunlightPercent = 0f; // 햇빛 게이지 0%~100%
    private float sunlightChangeRate = 10f; //햇빞에 있거나 없으면 1초에 햇빛 게이지 몇퍼센트 변하는지

    private void InitStats() //공격범위, 공격속도 등을 꽃 종류 따라서 초기화
    {
        attackRange = 3.5f;
        attackSpeed = 2f;
        attackType = AttackType.Discrete;
    }

    private void Awake()
    {
        InitStats();
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sunBarObject = transform.GetChild(0).gameObject;
        sunBarSpriteRenderer = sunBarObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ChangeSunlightPercent();

        //임시
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePose = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePose.x, mousePose.y, 0);
        }
        //
    }

    private void ChangeSunlightPercent() 
    {
        if(inSunlight) //꽃이 햇빛에 있냐 없냐 체크 --> 햇빛 게이지 바꾸기
        {
            sunlightPercent = (float)Mathf.Clamp(sunlightPercent + sunlightChangeRate*Time.deltaTime, 0, 100);
        }
        else
        {
            sunlightPercent = (float)Mathf.Clamp(sunlightPercent - sunlightChangeRate*Time.deltaTime, 0, 100);
        }

        switch(sunlightPercent) 
        {
            case float n when n < 30f:
                stage = FlowerStage.Seed;
                spriteRenderer.sprite = seedSprite;
                break;
            case float n when n < 60f:
                stage = FlowerStage.Sprout;
                spriteRenderer.sprite = sproutSprite;
                break;
            default:
                stage = FlowerStage.Flower;
                spriteRenderer.sprite = flowerSprite;
                break;
        } //햇빛 게이지 따라서 상태 (씨앗, 새싹, 꽃) 바꾸기

        sunBarSpriteRenderer.sprite = sunlightPercent != 100f ? sunBarSprite[(int)sunlightPercent/10] : sunBarSprite[10];
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Sunlight")
        {
            inSunlight = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Sunlight")
        {
            inSunlight = false;
        }
    }

    public void StartAttack()
    {
        if(!isAttacking && stage != FlowerStage.Seed)
        {
            StartCoroutine(Attack());
            isAttacking = true;
        }
    }

    public void StopAttack()
    {
        if(isAttacking)
        {
            StopCoroutine(Attack());
            isAttacking = false;
        }
    }

    IEnumerator Attack()
    {
        while(targetEnemy != null)
        {
            if(attackType == AttackType.Discrete)
            {
                Debug.Log("attack");
                GameObject attackInstance = Instantiate(attackObject, transform.position, Quaternion.identity);
                (attackInstance.GetComponent<AttackManager>() as AttackManager).targetObject = targetEnemy;
            }

            Debug.Log(1f/attackSpeed*1000);

            yield return new WaitForSeconds(1f/attackSpeed);
        }
    }
}
