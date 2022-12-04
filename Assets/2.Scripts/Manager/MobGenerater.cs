using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGenerater : MonoBehaviour
{
    public GameObject MapManager;
    public GameObject player;
    public GameObject MobGroup;

    public List<GameObject> Mob;
    public List<int> mobWeight;

    public int randomOffset = 5;
    public float generateCool;
    public bool generateReady = false;

    

    private void Update()
    {

        if (generateReady)
        {
            generateReady = false;
            StartCoroutine(countTime(generateCool));
        }
    }

    public void MobGenerate()
    {
        //initialize
        int totWeight = 0; 
        int selectedMobIndex = 0;

        //총 가중치계산
        for (int i = 0; i < mobWeight.Count; i++)
            totWeight += mobWeight[i];

        //랜덤한 가중치안의 수인지 확인후 index 로 설정;
        int randIndex = Random.Range(0, totWeight);
        int pile = 0;
        for (int i = 0; i < mobWeight.Count; i++)
        {
            pile += mobWeight[i];
            if(randIndex < pile)
            {
                selectedMobIndex = i;
                break;
            }
        }

        //player주위의 적절한 spawn위치 계산
        int playerRow = player.transform.GetChild(1).GetComponent<PlayerHitRange>().curTile.GetComponent<TileFrame>().tileRow;
        int playerCol = player.transform.GetChild(1).GetComponent<PlayerHitRange>().curTile.GetComponent<TileFrame>().tileColumn;

        List<int> huboRow = new List<int>();
        List<int> huboCol = new List<int>();
        for (int i = playerRow - randomOffset; i < playerRow + randomOffset ;i++)
        {
            for (int j = playerCol - randomOffset; j < playerCol + playerCol; j++)
            {
                if (i < 0 || j < 0 || i >= MapManager.GetComponent<MapManager>().row || j >= MapManager.GetComponent<MapManager>().column)
                    continue;
                if (MapManager.GetComponent<MapManager>().mapTileInt[i, j] == 1 || MapManager.GetComponent<MapManager>().mapTileInt[i, j] == 4)
                    continue;

                huboRow.Add(i);
                huboCol.Add(j);
            }
        }
        //가능 구역중 랜덤위치 
        int randomPos = Random.Range(0, huboRow.Count);
        Vector3 spawnPos = MapManager.GetComponent<MapManager>().mapTile[huboRow[randomPos], huboCol[randomPos]].transform.position;
        spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z);
        GameObject inst = Instantiate(Mob[selectedMobIndex], spawnPos, Quaternion.identity);
        inst.GetComponent<Mob>().player = player;
        inst.transform.GetChild(1).GetComponent<HitRange>().playerAttackRange = player.transform.GetChild(0).gameObject;
        inst.transform.parent = MobGroup.transform;
    }

    public IEnumerator countTime(float cool)
    {
        MobGenerate();
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        generateReady = true;
    }

}
