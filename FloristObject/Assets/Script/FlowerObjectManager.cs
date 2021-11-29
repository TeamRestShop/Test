using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowerType //나중에 꽃종류 추가하면
{

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

    public FlowerType type {get; set;}
    public FlowerStage stage {get; set;}

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer sunBarSpriteRenderer;
    private GameObject sunBarObject;

    private bool inSunlight = false;

    private float sunlightPercent = 0f; // 햇빛 게이지 0%~100%
    private float sunlightChangeRate = 10f; //햇빞에 있거나 없으면 1초에 햇빛 게이지 몇퍼센트 변하는지

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sunBarObject = transform.GetChild(0).gameObject;
        sunBarSpriteRenderer = sunBarObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        changeSunlightPercent();

        //임시
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePose = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePose.x, mousePose.y, 0);
        }
        //
    }

    private void changeSunlightPercent() 
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
}
