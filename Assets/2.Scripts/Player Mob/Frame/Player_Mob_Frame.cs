using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mob_Frame : MonoBehaviour
{
    public int index;
    public int health;
    public float speed;
    public float invincibilityTime;
    public int attackDamage;
    public bool goDown = false;
    public bool invincible = false;

    public IEnumerator invincibility(float invincibilityTime)
    {
        while (invincibilityTime > 0.0f)
        {
            invincibilityTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        invincible = false;
    }
}
