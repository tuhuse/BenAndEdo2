using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CountDownTimer : MonoBehaviour
{
    [SerializeField] private Text _countDownText;
    
    [SerializeField] private float _timeValue;

    
    private const float END_TIME_VALUE = 0f;
    public bool StartTimer { get; set; } = false;
    void Update()
    {
        UpdateTimer();
    }
    private void UpdateTimer()
    {
        if (StartTimer)
        {
            if (_timeValue > END_TIME_VALUE)
            {
                _timeValue -= Time.deltaTime;
                _countDownText.text = _timeValue.ToString();
            }
            else
            {
                GameManager.Instance.OnGameOver2();
            }
        }
    }
   

}
