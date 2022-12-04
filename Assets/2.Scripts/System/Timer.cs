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

    IEnumerator mapBreakCoroutine()
    {
        float keepPosTime = 1f;
        if(mapManager.GetComponent<MapManager>().curSection != 2)
            mapManager.GetComponent<MapManager>().curSection += 1;
        if(curSec != 1)
            curSec += 1;
        while (keepPosTime > 0.0f)
        {
            keepPosTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        notUpdate = true;
    }
}
