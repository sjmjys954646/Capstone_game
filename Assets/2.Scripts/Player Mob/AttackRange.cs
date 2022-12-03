using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : AttackFrame
{
    public float AttackCool;
    public float AttackJudgeCool = 0.1f;

    public void Attacked()
    {
        Attack = true;
        StartCoroutine(AttackCoolTime(AttackCool));
        StartCoroutine(AttackTime(AttackJudgeCool));
    }
}
