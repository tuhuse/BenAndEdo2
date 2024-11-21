using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CountDownTimer : MonoBehaviour
{
    [SerializeField] private Text _countDownText;
    [SerializeField] private UnlockingButton _unlockingButton;
    [SerializeField] private float _timeValue;

    private const float WAIT_TIME = 3f;
    private const float END_TIME_VALUE = 0f;
    void Update()
    {
        UpdateTimer();
    }
    private void UpdateTimer()
    {
        if (_unlockingButton.StartTimer)
        {
            if (_timeValue > END_TIME_VALUE)
            {
                _timeValue -= Time.deltaTime;
            }
            else
            {
                StartCoroutine(GameOverTransition());
            }
        }
    }
    private IEnumerator GameOverTransition()
    {
        //�A�j���[�V���������������̍�
        yield return new WaitForSeconds(WAIT_TIME);
        //�V�[���ړ�
    }


}