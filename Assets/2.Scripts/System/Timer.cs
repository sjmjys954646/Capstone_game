using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public MapManager mapManager;
    public TMP_Text timeText;
    public float totalsec;
    public float intervalSec;
    public float sec;
    public int min;
    List<int> mapBreakInterval = new List<int>();
    public List<int> whenSectionChange = new List<int>();
    public bool findMapBreakFin = true;
    [SerializeField]
    int curSec = 0;
    [SerializeField]
    bool notUpdate = true;

    private void Start()
    {
        mapBreakInterval = mapManager.GetComponent<MapManager>().mapBreakInterval;
        whenSectionChange = mapManager.GetComponent<MapManager>().whenSectionChange;
    }

    private void Update()
    {
        if(intervalSec >= mapBreakInterval[mapManager.GetComponent<MapManager>().curSection] - 3 && findMapBreakFin)
        {
            findMapBreakFin = false;
            StartCoroutine(mapFindCoroutine());
        }

        if (intervalSec >= mapBreakInterval[mapManager.GetComponent<MapManager>().curSection])
        {
            mapManager.GetComponent<MapManager>().mapBreak = true;
            Time.timeScale = 0f;
        }


        if ((curSec == 2 || totalsec >= whenSectionChange[curSec]) && notUpdate)
        {
            notUpdate = false;
            StartCoroutine(mapBreakCoroutine());
        }
        timeGo();
    }

    void timeGo()
    {
        sec += Time.deltaTime;
        totalsec += Time.deltaTime;
        intervalSec += Time.deltaTime;

        timeText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);

        if((int)sec > 59)
        {
            sec = 0;
            min++;
        }
    }

    IEnumerator mapFindCoroutine()
    {
        mapManager.GetComponent<MapManager>().findWhichTobreak();
        yield return new WaitUntil(() => mapManager.GetComponent<MapManager>().findTobreak);
        int[,] turnOnButtonGroup = mapManager.GetComponent<MapManager>().mapTileIntTmp;

        //맵 삭제될 애들 미리 반짝이게 선택
        for (int i = 0; i < mapManager.GetComponent<MapManager>().row; i++)
        {
            for (int j = 0; j < mapManager.GetComponent<MapManager>().column; j++)
            {
                if (turnOnButtonGroup[i, j] == 1 && mapManager.GetComponent<MapManager>().mapTile[i, j].GetComponent<TileFrame>().index != 1)
                {
                    mapManager.GetComponent<MapManager>().mapTile[i, j].GetComponent<TileFrame>().amIbreaker = true;
                }
            }
        }

        mapManager.GetComponent<MapManager>().findTobreak = false;
    }

    IEnumerator mapBreakCoroutine()
    {
        float keepPosTime = 1f;
        while (keepPosTime > 0.0f)
        {
            keepPosTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (mapManager.GetComponent<MapManager>().curSection != 2)
            mapManager.GetComponent<MapManager>().curSection += 1;
        if (curSec != 1)
            curSec += 1;
        notUpdate = true;
    }
}
