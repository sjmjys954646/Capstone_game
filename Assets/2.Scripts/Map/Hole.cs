using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : TileFrame
{
    // Update is called once per frame
    void Update()
    {
        if (isPlayerIn && !player.GetComponent<Player_Mob_Frame>().goDown)
        {
            player.GetComponent<Player_Mob_Frame>().goDown = true;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1.5f, player.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collide = other.gameObject;
        if (collide.tag == "HitRange")
        {
            collide.transform.parent.transform.position = new Vector3(collide.transform.parent.transform.position.x, collide.transform.parent.transform.position.y - 0.5f, collide.transform.parent.transform.position.z);
            collide.transform.parent.GetComponent<Player_Mob_Frame>().goDown = true;
        }
    }
}
