using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public MapManager mapManager;
    public PlayerManager playerManager;
    public GameObject gameFinishUI;

    public float aliveTime = 0;

    public int holeDamage = 0;
    public int swampDamage = 0;
    public int thornDamage = 0;
    public int attackDamage = 0;
    public int bulletDamage = 0;

    public List<TMP_Text> fintext;
    

    public bool gameFinish = false;

    private void Update()
    {
        if(gameFinish)
        {
            gameFinish = false;
            Time.timeScale = 0f;
            gameFinishUI.SetActive(true);

            for(int i = 0;i< fintext.Count ;i++)
            {
                switch(i)
                {
                    case 0:
                        fintext[i].text = holeDamage.ToString();
                        break;
                    case 1:
                        fintext[i].text = swampDamage.ToString();
                        break;
                    case 2:
                        fintext[i].text = thornDamage.ToString();
                        break;
                    case 3:
                        fintext[i].text = attackDamage.ToString();
                        break;
                    case 4:
                        fintext[i].text = bulletDamage.ToString();
                        break;


                }
            }
        }
    }
}
