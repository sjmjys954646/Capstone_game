using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mob : Player_Mob_Frame
{
    public GameObject player;
    public Rigidbody rigidbody;
    public int goAwayLength;
    // 길을 찾아서 이동할 에이전트
    public NavMeshAgent agent;
    public bool moveable = false;
    public bool findTarget = true;

    void Awake()
    {
        var rigid = GetComponent<Rigidbody>();
        if (rigid != null)
            rigidbody = GetComponent<Rigidbody>();
        var navMesh = GetComponent<NavMeshAgent>();
        if (navMesh != null)
        {
            moveable = true;
            agent = GetComponent<NavMeshAgent>();
        }
    }

    void Update()
    {

        //속도가 이상해졌을때
        if (rigidbody.velocity.x != 0 || rigidbody.velocity.z != 0)
            rigidbody.velocity = Vector3.zero;

        //목표선택후 따라가기
        if(moveable && findTarget)
        {
            findTarget = false;
            agent.SetDestination(player.transform.position);
            StartCoroutine(findCool(1f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mob")
            return;
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

        if (agent)
            agent.enabled = !agent.enabled;

        StartCoroutine(moved(0.2f));
    }

    public IEnumerator moved(float cool)
    {
        Vector3 Pos = player.transform.position - gameObject.transform.position;
        Vector3 movement = Pos.normalized * goAwayLength * Time.deltaTime * -1 * 10;

        //transform.Translate(movement, Space.Self);
        rigidbody.MovePosition(transform.position + movement);
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (agent)
            agent.enabled = !agent.enabled;
    }

    public void Killed()
    {
        Destroy(gameObject);
    }

    public IEnumerator findCool(float cool)
    {
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        findTarget = true;
    }
}
