using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFrame : MonoBehaviour
{
    public bool Attack = false;
    public bool AttackPos = true;
    //공격 쿨타임
    public IEnumerator AttackCoolTime(float cool)
    {
        AttackPos = false;
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        AttackPos = true;
    }
    //공격 판정 시간
    public IEnumerator AttackTime(float cool)
    {
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        Attack = false;
    }
}
