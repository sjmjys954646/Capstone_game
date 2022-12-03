using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : TileFrame
{
    public float keepPosTime;
    bool coroutineCheck = false;
    // Update is called once per frame
    void Update()
    {
        if(isPlayerIn)
        {
            if (!coroutineCheck)
            {
                StartCoroutine(AttackTime(keepPosTime));
                coroutineCheck = true;
            }
        }
    }

    public IEnumerator AttackTime(float keepPosTime)
    {
        while (keepPosTime > 0.0f)
        {
            keepPosTime -= Time.deltaTime;
            if(!isPlayerIn)
            {
                coroutineCheck = false;
                 yield break;
            }
            yield return new WaitForFixedUpdate();
        }
        player.getDamage(1);
        coroutineCheck = false;
    }
}
