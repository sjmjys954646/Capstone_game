using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject mapManager;
    public GameObject player;
    // Start is called before the first frame update
    
    public void SetPlayerPosition()
    {
        for(int i = 40;i<50 ;i++)
        {
            for(int j = 40;j<50 ;j++)
            {
                //Debug.Log( mapManager.GetComponent<MapManager>().mapTile[i, j].transform.position);
                if (mapManager.GetComponent<MapManager>().mapTileInt[i,j] == 0)
                {
                    Vector3 pos = mapManager.GetComponent<MapManager>().mapTile[i, j].transform.position;
                    pos = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                    player.transform.position =pos ;
                    break;
                }
            }
        }
    }
}
