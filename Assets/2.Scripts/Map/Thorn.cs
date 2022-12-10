using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thorn : TileFrame
{
    public GameObject gameManager;
    public float keepPosTime;
    bool coroutineCheck = false;
    public GameObject thornDamagedUI;
    
    // Update is called once per frame
    void Update()
    {
        material.color = myColor;


        if (amIbreaker && amImbreakerCoroutineOn)
        {
            amImbreakerCoroutineOn = false;
            StartCoroutine(FadeTextToFullAlpha());
        }

        if (isPlayerIn)
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
        Color UIcolor = thornDamagedUI.GetComponent<Image>().color;
        while (keepPosTime > 0.0f)
        {
            keepPosTime -= Time.deltaTime;
            thornDamagedUI.GetComponent<Image>().color = new Color(UIcolor.r , UIcolor.g , UIcolor.b, UIcolor.a + 5* (Time.deltaTime / 1.0f));
            
            if (!isPlayerIn)
            {
                thornDamagedUI.GetComponent<Image>().color = new Color(UIcolor.r, UIcolor.g, UIcolor.b, 0);
                coroutineCheck = false;
                 yield break;
            }
            yield return new WaitForFixedUpdate();
        }
        thornDamagedUI.GetComponent<Image>().color = new Color(UIcolor.r, UIcolor.g, UIcolor.b, 0);

        gameManager.GetComponent<GameManager>().thornDamage += 1;
        gameManager.GetComponent<GameManager>().attackDamage -= 1;
        player.getDamage(1);
        coroutineCheck = false;
    }

    public IEnumerator FadeTextToFullAlpha() // 알파값 0에서 1로 전환
    {
        while (myColor.r < 1.0f)
        {
            myColor = new Color(myColor.r + (Time.deltaTime / 2.0f), myColor.g + (Time.deltaTime / 2.0f), myColor.b + (Time.deltaTime / 2.0f), myColor.a);
            yield return null;
        }
        StartCoroutine(FadeTextToZeroAlpha());
    }

    public IEnumerator FadeTextToZeroAlpha()  // 알파값 1에서 0으로 전환
    {
        while (myColor.a > 0.0f)
        {
            myColor = new Color(myColor.r - (Time.deltaTime / 2.0f), myColor.g - (Time.deltaTime / 2.0f), myColor.b - (Time.deltaTime / 2.0f), myColor.a);
            yield return null;
        }
        StartCoroutine(FadeTextToFullAlpha());
    }
}
