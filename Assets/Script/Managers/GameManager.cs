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
            SceneManager.LoadScene("GameOver");
        }
       
    }public void OnGameOver2()
    {
        StartCoroutine(GameOverTransition());

    }
    private IEnumerator GameOverTransition()
    {
        //アニメーション流したいこの頃
        yield return new WaitForSeconds(WAIT_TIME);
        SceneManager.LoadScene("GameOver");
    }

    public void OnGameClear()
    {
        SceneManager.LoadScene("GameClear");
    }
}
