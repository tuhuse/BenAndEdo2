using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private const float WAIT_TIME = 3f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 重複するインスタンスを破棄
        }
    }

   
    public void OnGameOver()
    {
        if (ValueManager.Instance.PlayerHP == 0)
        {
            StartCoroutine(GameOverTransition());
        }
       
    }public void OnGameOver2()
    {
        StartCoroutine(GameOverTransition());

    }
    private IEnumerator GameOverTransition()
    {
        Cursor.lockState = CursorLockMode.None;
        //アニメーション流したいこの頃
        yield return new WaitForSeconds(WAIT_TIME);
        SceneManager.LoadScene("GameOver");
    }

    public void OnGameClear()
    {
        Cursor.lockState = CursorLockMode.None;  
        SceneManager.LoadScene("GameClear");
    }
}
