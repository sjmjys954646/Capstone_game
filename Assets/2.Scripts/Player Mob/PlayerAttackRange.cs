using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRange : AttackFrame
{
    public GameObject Player;

    public float rotateSpeed;       // 회전 속도
    public bool attackMode = false; //공격버튼을 누른상태

    float h, v;

    //공격변수
    public float AttackCool;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //공격 구현
        if (AttackPos && Input.GetMouseButtonDown(0))
        {
            Attack = true;
            StartCoroutine(AttackCoolTime(AttackCool));
            StartCoroutine(AttackTime(0.1f));
        }
    }

    void FixedUpdate()
    {

        float distance;
        Vector3 worldPosition = new Vector3(0, 0, 0);
        Plane plane = new Plane(Vector3.up, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
        float x = Mathf.Clamp(worldPosition.x, Player.transform.position.x - 0.5f, Player.transform.position.x + 0.5f);
        float z = Mathf.Clamp(worldPosition.z, Player.transform.position.z - 0.5f, Player.transform.position.z + 0.5f);
        transform.position = new Vector3(x, transform.position.y, z);
    }


}
