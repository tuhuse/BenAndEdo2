using System.Collections;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̃X�e�[�^�X�Ɋ֌W���Ă���l�̕ω����Ǘ����Ă���
/// </summary>
public class ValueManager : MonoBehaviour
{
  
    [SerializeField] private LifeUI _lifeUI = default;
    [SerializeField] private DamageUI _damageUI = default;
    // �萔��`
    private const float MAX_SPEED = 8f; // �ő�ړ����x
    private const float MAX_DASH_HP = 100f; // �_�b�V���p��HP�̍ő�l
    private const float STAMINA_HEAL = 7.5f; // �_�b�V��HP�̉񕜗�
    private const int MAX_PLAYER_HP = 3; // �v���C���[�̍ő�HP
    private const int STAMINA_DECREASE_RATE = 20; // �_�b�V������HP�������x
    private const int RETERN_SPEED = 3;
    private const float SPEED_DECREASE_AMOUNT = 4f; // �_���[�W���̈ړ����x������

    // �v���C���[�̌��݂̏��
    public float MoveSpeed { get; private set; } = MAX_SPEED; // ���݂̈ړ����x
    public float DashHP { get; private set; } = MAX_DASH_HP; // ���݂̃_�b�V��HP
    public int PlayerHP { get; private set; } = MAX_PLAYER_HP; // ���݂̃v���C���[HP
                                                               // �V���O���g���̃C���X�^���X
    public static ValueManager Instance { get; private set; }


    // �V���O���g���̐ݒ�
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           
        }
        else
        {
            Destroy(gameObject); // �d������C���X�^���X��j��
        }
        PlayerHP = MAX_PLAYER_HP;
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
        DashHealthRecovery();
    }

    /// <summary>
    ///  �_�b�V��HP�̉񕜏���
    /// </summary>
    /// <returns></returns>
    private void DashHealthRecovery()
    {
      
        if (DashHP <= MAX_DASH_HP)
        {
            DashHP = Mathf.Min(DashHP + STAMINA_HEAL*Time.deltaTime, MAX_DASH_HP); // HP���ő�l�܂ŉ�
        }
    }

    /// <summary>
    ///  �v���C���[���_���[�W���󂯂����̏���
    /// </summary>
    public void Damage()
    {
        _damageUI.StartDamageUI();
        PlayerHP--; // �v���C���[HP������
        _lifeUI.DamageLife(PlayerHP);
        AudioManager.Instance.DamageSE();
        GameManager.Instance.OnGameOver();
        StartCoroutine(ReturnSpeed()); // �ꎞ�I�Ɉړ����x������
    }

    /// <summary>
    ///  �v���C���[���񕜂����鏈��
    /// </summary>
    public void Heal()
    {
        PlayerHP = Mathf.Min(PlayerHP + 1, MAX_PLAYER_HP); // HP���ő�l�܂ŉ�
        _lifeUI.HealLife(PlayerHP);
    }
}
