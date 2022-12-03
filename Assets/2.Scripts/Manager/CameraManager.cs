using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public float CameraPosY;
    public float CameraPosZ;

    void Start()
    {

    }

    // LateUpdate is called after Update each frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;
        
        transform.position = new Vector3(playerPos.x, playerPos.y + CameraPosY, playerPos.z - CameraPosZ);
    }
}