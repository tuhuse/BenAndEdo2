using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private Canvas _pauseCanvas;
    private const float WAIT_TIME = 3f;
    private bool _isOpen = false;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                _pauseCanvas.enabled = true;
                Time.timeScale = 0;
                _isOpen = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                _pauseCanvas.enabled = false;
                Time.timeScale = 1;
                _isOpen = false;
            }
        }
    }
    public void OnGameOver()
    {
        if (ValueManager.Instance.PlayerHP == 0)
        {
            StartCoroutine(GameOverTransition());
        }

    }
    public void OnGameOver2()
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
    public void OnTitle()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }public void OnRetry()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
