using System.Collections;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̃X�e�[�^�X�Ɋ֌W���Ă���l�̕ω����Ǘ����Ă���
/// </summary>
public class ValueManager : MonoBehaviour
{
    // �V���O���g���̃C���X�^���X
    public static ValueManager Instance { get; private set; }

    // �萔��`
    private const float MAX_SPEED = 8f; // �ő�ړ����x
    private const float MAX_DASH_HP = 100f; // �_�b�V���p��HP�̍ő�l
    private const float STAMINA_HEAL = 10f; // �_�b�V��HP�̉񕜗�
    private const int MAX_PLAYER_HP = 3; // �v���C���[�̍ő�HP
    private const int STAMINA_DECREASE_RATE = 20; // �_�b�V������HP�������x
    private const int STAMINA_RECOVERY_WAIT_TIME = 1; // �_�b�V��HP�񕜂̊Ԋu
    private const int STAMINA_RECOVERY_INITIAL_DELAY = 2; // �_�b�V��HP�񕜊J�n�܂ł̒x��
    private const int RETERN_SPEED = 3;
    private const float SPEED_DECREASE_AMOUNT = 4f; // �_���[�W���̈ړ����x������

    // �v���C���[�̌��݂̏��
    public float MoveSpeed { get; private set; } = MAX_SPEED; // ���݂̈ړ����x
    public float DashHP { get; private set; } = MAX_DASH_HP; // ���݂̃_�b�V��HP
    public int PlayerHP { get; private set; } = MAX_PLAYER_HP; // ���݂̃v���C���[HP

    private Coroutine _dashRecoveryCoroutine = default; // �_�b�V��HP�񕜂̃R���[�`���Ǘ��p

    // �V���O���g���̐ݒ�
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���ԂŔj������Ȃ��悤�ɂ���
        }
        else
        {
            Destroy(gameObject); // �d������C���X�^���X��j��
        }
    }

    /// <summary>
    /// ��莞�ԂŃX�s�[�h��߂�
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReturnSpeed()
    {
        MoveSpeed -= SPEED_DECREASE_AMOUNT; // �ړ����x������
        yield return new WaitForSeconds(RETERN_SPEED); // 3�b��Ɍ��ɖ߂�
        MoveSpeed = MAX_SPEED;
    }

    /// <summary>
    ///  �_�b�V������HP�����������鏈��
    /// </summary>
    public void DashHPDecrease()
    {
        DashHP = Mathf.Max(0, DashHP - STAMINA_DECREASE_RATE * Time.deltaTime); // HP��0�������Ȃ��悤�ɂ���
    }

    /// <summary>
    /// �_�b�V��HP�̉񕜏������J�n
    /// </summary>    
    public void StartDashRecovery()
    {
        if (_dashRecoveryCoroutine == null)
        {
            _dashRecoveryCoroutine = StartCoroutine(DashHealthRecovery());
        }
    }

    /// <summary>
    /// �_�b�V��HP�̉񕜏������~
    /// </summary>
    public void StopDashRecovery()
    {
        if (_dashRecoveryCoroutine != null)
        {
            StopCoroutine(_dashRecoveryCoroutine);
            _dashRecoveryCoroutine = null;
        }
    }

    /// <summary>
    ///  �_�b�V��HP�̉񕜏���
    /// </summary>
    /// <returns></returns>
    private IEnumerator DashHealthRecovery()
    {
        yield return new WaitForSeconds(STAMINA_RECOVERY_INITIAL_DELAY); // �񕜊J�n�܂ł̒x��

        while (DashHP < MAX_DASH_HP)
        {
            DashHP = Mathf.Min(DashHP + STAMINA_HEAL, MAX_DASH_HP); // HP���ő�l�܂ŉ�
            yield return new WaitForSeconds(STAMINA_RECOVERY_WAIT_TIME); // �w��̊Ԋu�ŉ�
        }

        _dashRecoveryCoroutine = null; // �񕜏���������������R���[�`�������Z�b�g
    }

    /// <summary>
    ///  �v���C���[���_���[�W���󂯂����̏���
    /// </summary>
    public void Damage()
    {
        PlayerHP--; // �v���C���[HP������
        StartCoroutine(ReturnSpeed()); // �ꎞ�I�Ɉړ����x������
    }

    /// <summary>
    ///  �v���C���[���񕜂����鏈��
    /// </summary>
    public void Heal()
    {
        PlayerHP = Mathf.Min(PlayerHP + 1, MAX_PLAYER_HP); // HP���ő�l�܂ŉ�
    }
}
