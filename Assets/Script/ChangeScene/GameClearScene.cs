using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearScene : MonoBehaviour
{

   
    public void OnTitle()
    {
        SceneManager.LoadScene("Title");
    } public void OnReTry()
    {
        SceneManager.LoadScene("MainScene");
    }
}
