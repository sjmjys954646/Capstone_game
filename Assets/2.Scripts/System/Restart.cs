using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void SceneLoad()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("IngameScene");
    }
}
