using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のアニメーション
/// </summary>
public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private RuntimeAnimatorController _runController;
    [SerializeField] private RuntimeAnimatorController _walkController;
    [SerializeField] private RuntimeAnimatorController _attackController;
    [SerializeField] private RuntimeAnimatorController _idleController;
    [SerializeField] private EnemyAI _enemyAI;

    private EnemyAI.EnemyState _enemyState;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _idleController;
    }

  
    void Update()
    {
        EnemyAI.EnemyState currentStatus = _enemyAI.EnemyCurrentState;

        // ステータスが変わった時だけ処理を実行
        if (_enemyState != currentStatus)
        {
            HandleStatusChange(currentStatus);
            _enemyState = currentStatus; // ステータスを更新
        }
       
    }
    private void HandleStatusChange(EnemyAI.EnemyState newStatus)
    {
        switch (newStatus)
        {
            case EnemyAI.EnemyState.Patrol:
                _animator.runtimeAnimatorController =_walkController;
                break;

            case EnemyAI.EnemyState.Chase:
                _animator.runtimeAnimatorController = _runController;
                break;
            case EnemyAI.EnemyState.Attack:

                _animator.runtimeAnimatorController = _attackController;

                   break;
            case EnemyAI.EnemyState.Idle:
                _animator.runtimeAnimatorController = _idleController;
                break;
            case EnemyAI.EnemyState.EveryChase:
                _animator.runtimeAnimatorController = _runController;
                break;
        }
    }
}
