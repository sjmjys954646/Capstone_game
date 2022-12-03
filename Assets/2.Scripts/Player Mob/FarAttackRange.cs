using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarAttackRange : AttackFrame
{
    public GameObject bullet; 
    public float AttackCool;

    private void OnTriggerStay(Collider other)
    {
        GameObject collide = other.gameObject;
        if (collide.tag == "PlayerHitRange")
        {
            if(AttackPos)
            {
                Attack = true;
                GameObject p = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                p.GetComponent<Mob>().player = gameObject.transform.parent.GetComponent<Mob>().player;
                p.GetComponent<Bullet>().master = gameObject.transform.parent.gameObject;
                
                StartCoroutine(AttackCoolTime(AttackCool));
                StartCoroutine(AttackTime(0.1f));
            }
        }
    }

}
