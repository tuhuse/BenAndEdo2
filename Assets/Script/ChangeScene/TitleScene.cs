using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    

    public void OnStart()
    {
        SceneManager.LoadScene("MainScene");
    }public void OnEnd()
    {
        Application.Quit();
    }
}
