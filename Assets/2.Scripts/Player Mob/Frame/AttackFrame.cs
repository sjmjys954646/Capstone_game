using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFrame : MonoBehaviour
{
    public bool Attack = false;
    public bool AttackPos = true;
    //���� ��Ÿ��
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
    //���� ���� �ð�
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
