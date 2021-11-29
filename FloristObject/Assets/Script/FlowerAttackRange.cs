using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerAttackRange : MonoBehaviour
{
    FlowerObjectManager flowerScript;
    CircleCollider2D circleCollider;

    private void Start()
    {
        flowerScript = transform.parent.GetComponent<FlowerObjectManager>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = flowerScript.attackRange;
    }

    private void OnTriggerStay2D(Collider2D enemy) 
    {
        if(enemy.gameObject.tag == "Monster")
        {
            flowerScript.targetEnemy = enemy.gameObject;
            flowerScript.StartAttack();
        }
    }
    private void OnTriggerExit2D(Collider2D enemy) 
    {
        if(enemy.gameObject.tag == "Monster")
        {
            flowerScript.targetEnemy = null;
            flowerScript.StopAttack();
        }
    }
}
