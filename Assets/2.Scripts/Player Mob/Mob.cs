using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Player_Mob_Frame
{
    public GameObject player;
    public Rigidbody rigidbody;
    public int goAwayLength;
    // Start is called before the first frame update

    void Awake()
    {
        if (GetComponent<Rigidbody>() != null)
            rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(goDown)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
        }

        if (transform.position.y <= -20)
            Killed();

        //속도가 이상해졌을때
        if (rigidbody.velocity.x != 0 || rigidbody.velocity.z != 0)
            rigidbody.velocity = Vector3.zero;
    }

    public void Attacked()
    {
        //몬스터 무적처리
        if (!invincible)
            health -= player.GetComponent<Player>().attackDamage;

        //체력 0 이하시 사망처리
        if(health <= 0)
        {
            Killed();
        }
        invincible = true;
        StartCoroutine(invincibility(invincibilityTime));
        Vector3 Pos = player.transform.position - gameObject.transform.position;
        Vector3 movement = Pos.normalized * goAwayLength * Time.deltaTime * -1 * 10;

        rigidbody.MovePosition(transform.position + movement);
    }

    public void Killed()
    {
        Destroy(gameObject);
    }
}
