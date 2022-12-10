using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : TileFrame
{
    private void Update()
    {
        material.color = myColor;


        if (amIbreaker && amImbreakerCoroutineOn)
        {
            amImbreakerCoroutineOn = false;
            StartCoroutine(FadeTextToFullAlpha());
        }
    }
    public IEnumerator FadeTextToFullAlpha() // ���İ� 0���� 1�� ��ȯ
    {
        while (myColor.r < 1.0f)
        {
            myColor = new Color(myColor.r + (Time.deltaTime / 2.0f), myColor.g + (Time.deltaTime / 2.0f), myColor.b + (Time.deltaTime / 2.0f), myColor.a );
            yield return null;
        }
        StartCoroutine(FadeTextToZeroAlpha());
    }

    public IEnumerator FadeTextToZeroAlpha()  // ���İ� 1���� 0���� ��ȯ
    {
        while (myColor.a > 0.0f)
        {
            myColor = new Color(myColor.r - (Time.deltaTime / 2.0f), myColor.g - (Time.deltaTime / 2.0f), myColor.b - (Time.deltaTime / 2.0f), myColor.a );
            yield return null;
        }
        StartCoroutine(FadeTextToFullAlpha());
    }
}
