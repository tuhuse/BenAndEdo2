using System.Collections;
using System.Collections.Generic;
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
            if (_audioBGM.clip != _audioClip[1])
            {
                _audioBGM.clip = _audioClip[1];
                _audioBGM.pitch = 2;
                _audioBGM.Play();
            }
            else
            {
                _audioBGM.pitch = 2;
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

    }
    public void ItemGetSE()
    {
        _audioSE.clip = _audioClip[3];
        _audioSE.PlayOneShot(_audioClip[3]);
    }

    public void KeySE()
    {
        _audioSE.clip = _audioClip[4];
        _audioSE.PlayOneShot(_audioClip[4]);
    }
    public void PushButtonSE()
    {
        _audioSE.clip = _audioClip[5];
        _audioSE.PlayOneShot(_audioClip[5]);
    }
  
    public void TimerSE()
    {
        _audioSE.clip = _audioClip[7];
        _audioSE.PlayOneShot(_audioClip[7]);
    }
    public void OpenCashBoxSE()
    {
        _audioSE.clip = _audioClip[8];
        _audioSE.PlayOneShot(_audioClip[8]);
    }
}
