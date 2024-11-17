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

        // �����X�e�[�^�X��ۑ�
        _previousStatus = _player._playerStatus;
    }

    void Update()
    {
        // ���݂̃v���C���[�X�e�[�^�X���擾
        var currentStatus = _player._playerStatus;

        // �X�e�[�^�X���ς�������������������s
        if (_previousStatus != currentStatus)
        {
            HandleStatusChange(currentStatus);
            _previousStatus = currentStatus; // �X�e�[�^�X���X�V
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
                Debug.LogWarning("���Ή��̃X�e�[�^�X�ł�: " + newStatus);
                break;
        }
    }
}
