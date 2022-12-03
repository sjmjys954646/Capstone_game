using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFrame : MonoBehaviour
{
    public int index;
    public Player player;
    public bool isPlayerIn = false;
    public List<Vector3> tilePos;

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> arr = new List<Vector3>();
        double[] dX = { -0.5, 0.5, 0.5, -0.5 };
        double[] dZ = { 0.5, 0.5, -0.5, -0.5 };

        for (int i = 0; i < 4; i++)
        {
            arr.Add(new Vector3(transform.position.x + (float)dX[i], 0, transform.position.z + (float)dZ[i]));
        }

        tilePos = arr;
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject collide = other.gameObject;
        if (collide.tag == "PlayerHitRange")
        {
            List<Vector3> playerCurPos = collide.GetComponent<PlayerHitRange>().curPos;
            //player가 Tile 내부에 완전히 있는지 확인하는 함수
            CheckInside(playerCurPos);
        }

    }

    public void CheckInside(List<Vector3> playerCurPos)
    {
        bool check = true;
        for(int i = 0; i< 4;i++)
        {
            Vector3 cur = playerCurPos[i];
            if(!(tilePos[0].x <= cur.x  && cur.x <= tilePos[1].x  && 
                tilePos[2].z <= cur.z && cur.z <= tilePos[0].z))
            {
                check = false;
            }
        }

        isPlayerIn = check;
    }
}
