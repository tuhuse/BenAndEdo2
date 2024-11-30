using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverScene : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public void OnMain()
    {
        SceneManager.LoadScene("MainScene");
    }
}
