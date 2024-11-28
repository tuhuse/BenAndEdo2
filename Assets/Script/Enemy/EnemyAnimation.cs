using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private RuntimeAnimatorController _runController;
    [SerializeField] private RuntimeAnimatorController _walkController;
    [SerializeField] private RuntimeAnimatorController _attackController;
    [SerializeField] private EnemyAI _enemyAI;

   
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _walkController;
    }

  
    void Update()
    {
        HandleStatusChange(_enemyAI.EnemyCurrentState);
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
        }
    }
}
