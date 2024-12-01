using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 音関連一括管理
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioBGM;
    [SerializeField] private AudioSource _audioSE;
    [SerializeField] private AudioClip[] _audioClip;

    [SerializeField] private EnemyAI _enemyAI;
    private EnemyAI.EnemyState _enemyState;

    private bool _isEscape = false;
    private bool _isWarningBGM = true;
    public static AudioManager Instance { get; private set; }
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
        _audioBGM.clip = _audioClip[0];
        _audioBGM.Play();
           
    }

    void Update()
    {
        StateSwitch();
    }
    private void StateSwitch()
    {
        if (!_isEscape)
        {
            EnemyAI.EnemyState currentStatus = _enemyAI.EnemyCurrentState;

            // ステータスが変わった時だけ処理を実行
            if (_enemyState != currentStatus)
            {
                BGMSwitch(currentStatus);
                _enemyState = currentStatus; // ステータスを更新
            }
        }
        else
        {
            if (_isWarningBGM)
            {
                StartCoroutine(WarningBGM());
                _isWarningBGM = false;
            }
            
        }

    }
    private void BGMSwitch(EnemyAI.EnemyState enemyState)
    {
        switch (enemyState)
        {
            case EnemyAI.EnemyState.Patrol:
                if (_audioBGM.clip != _audioClip[0])
                {
                    _audioBGM.clip = _audioClip[0];
                    _audioBGM.Play();
                }

                break;
            case EnemyAI.EnemyState.Chase:
                if (_audioBGM.clip != _audioClip[1])
                {
                    _audioBGM.clip = _audioClip[1];
                    _audioBGM.Play();
                }
                break;
        }
    }
private IEnumerator WarningBGM()
    {
        _audioBGM.clip = _audioClip[4];
        _audioBGM.Play();
        int waitTime = 4;
        yield return new WaitForSeconds(waitTime);
        _audioBGM.clip = _audioClip[1];
        _audioBGM.pitch = 2;
        _audioBGM.Play();
    }
    public void EscapeTrue()
    {
        _isEscape = true;
    }
    public void DamageSE()
    {
        _audioSE.clip = _audioClip[2];
        _audioSE.PlayOneShot(_audioClip[2]);
    }
    public void AttackSE()
    {
        _audioSE.clip = _audioClip[3];
        _audioSE.PlayOneShot(_audioClip[3]);
    }
    public void HealSE()
    {
        _audioSE.clip = _audioClip[5];
        _audioSE.PlayOneShot(_audioClip[5]);
    }
    public void OpenCashBoxSE()
    {
        _audioSE.clip = _audioClip[6];
        _audioSE.PlayOneShot(_audioClip[6]);
    }
    public void PushButtonSE()
    {
        _audioSE.clip = _audioClip[7];
        _audioSE.PlayOneShot(_audioClip[7]);
    }
    public void ItemGetSE()
    {
        _audioSE.clip = _audioClip[8];
        _audioSE.PlayOneShot(_audioClip[8]);
    }
    
    public void KeyInstantiateSE()
    {
        _audioSE.clip = _audioClip[9];
        _audioSE.PlayOneShot(_audioClip[9]);
    }
  
  
    public void BatteryUseSE()
    {
        _audioSE.clip = _audioClip[10];
        _audioSE.PlayOneShot(_audioClip[10]);
    }
    public void FlashLightSE()
    {
        _audioSE.clip = _audioClip[11];
        _audioSE.PlayOneShot(_audioClip[11]);
    }    
}
