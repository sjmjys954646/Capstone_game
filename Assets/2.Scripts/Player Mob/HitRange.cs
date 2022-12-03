using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRange : MonoBehaviour
{
    public GameObject mob;
    public List<Vector3> curPos;
    public GameObject playerAttackRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerAttackRange && playerAttackRange.GetComponent<PlayerAttackRange>().Attack)
        {
            mob.GetComponent<Mob>().Attacked();
        }
    }
}
