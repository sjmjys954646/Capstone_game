using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swamp : TileFrame
{
    public float slow;
    // Update is called once per frame
    void Update()
    {
        if (isPlayerIn)
        {
            player.speed *= slow;
            if (player.speed <= 1)
                player.speed = 1;
            player.DashPos = false;
        }
    }

}
