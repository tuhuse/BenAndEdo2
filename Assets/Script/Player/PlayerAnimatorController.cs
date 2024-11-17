using System;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private RuntimeAnimatorController _idle;
    [SerializeField] private RuntimeAnimatorController _runController;
    [SerializeField] private RuntimeAnimatorController _walkController;
    [SerializeField] private PlayerMoveController _player;

    private PlayerMoveController.PlayerStatus _previousStatus;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _idle;

        // 初期ステータスを保存
        _previousStatus = _player._playerStatus;
    }

    void Update()
    {
        // 現在のプレイヤーステータスを取得
        var currentStatus = _player._playerStatus;

        // ステータスが変わった時だけ処理を実行
        if (_previousStatus != currentStatus)
        {
            HandleStatusChange(currentStatus);
            _previousStatus = currentStatus; // ステータスを更新
        }
    }

    private void HandleStatusChange(PlayerMoveController.PlayerStatus newStatus)
    {
        switch (newStatus)
        {
            case PlayerMoveController.PlayerStatus.Idle:
                _animator.runtimeAnimatorController = _idle;
                break;

            case PlayerMoveController.PlayerStatus.Walk:
                _animator.runtimeAnimatorController = _walkController;
                break;

            case PlayerMoveController.PlayerStatus.Run:
                _animator.runtimeAnimatorController = _runController;
                break;

            default:
                Debug.LogWarning("未対応のステータスです: " + newStatus);
                break;
        }
    }
}
