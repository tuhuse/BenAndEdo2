using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClip;

    [SerializeField] private EnemyAI _enemyAI;
    private EnemyAI.EnemyState _enemyState;

    private bool _isEscape = false;
    private void Awake()
    {
        _audioSource.clip = _audioClip[0];
        _audioSource.Play();
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
            if (_audioSource.clip != _audioClip[1])
            {
                _audioSource.clip = _audioClip[1];
                _audioSource.pitch = 2;
                _audioSource.Play();
            }
            else
            {
                _audioSource.pitch = 2;
            }
        }

    }
    private void BGMSwitch(EnemyAI.EnemyState enemyState)
    {
        switch (enemyState)
        {
            case EnemyAI.EnemyState.Patrol:
                if (_audioSource.clip != _audioClip[0])
                {
                    _audioSource.clip = _audioClip[0];
                    _audioSource.Play();
                }
                         
                break;
            case EnemyAI.EnemyState.Chase:
                if(_audioSource.clip != _audioClip[1])
                {
                    _audioSource.clip = _audioClip[1];
                    _audioSource.Play();
                }
                break;
        }
    }
    public void EscapeTrue()
    {
        _isEscape = true;
    }
    public void ItemUseSE()
    {

    }
    public void DamageSE()
    {

    }
    public void KeySE()
    {

    }
    public void PushButtonSE()
    {

    }public void AttackSE()
    {

    }public void TimerSE()
    {

    }
    public void OpenCashBoxSE()
    {

    }
}
