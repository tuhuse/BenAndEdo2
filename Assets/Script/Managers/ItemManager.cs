using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour
{
 
    // �U���A�C�e���֘A
    [SerializeField] private BoxCollider _weaponCollider; // ����̃R���C�_�[

    // ���A�C�e���֘A
    [SerializeField]
    private Item _completedKey;
    private const float WAIT_TIME =0.5f ;
    // �����d���֘A
    private const int MAX_BATTERY_LIFE = 60; // �o�b�e���[�����̍ő�l
    [SerializeField] private Light _flashLight; // �����d���̃��C�g
    [SerializeField] private float _batteryLife = 60f; // ���݂̃o�b�e���[�����i�b�j

    // �V���O���g���p�^�[���p�̃C���X�^���X
    public static ItemManager Instance { get; private set; }
    public bool LightOn { get; private set; } // �����d�����I�����ǂ���
    private void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �C���X�^���X���d�����Ă���ꍇ�͔j��
            return;
        }
    }

    void Start()
    {
        // �Q�[���J�n���ɉ����d�����I�t�ɐݒ�
        _flashLight.enabled = false;
    }

    void Update()
    {
        // �����d�����I���̏ꍇ�A�o�b�e���[��������J�n
        if (LightOn)
        {
            UpdateLight();
        }
    }

    #region �����d������
    /// <summary>
    /// �����d���̃I���I�t�؂�ւ�
    /// </summary>
    public void LightActive()
    {
        if (!LightOn)
        {
            LightOn = true;
            if (_batteryLife > 0)
            {
                _flashLight.enabled = true; // �o�b�e���[������΃��C�g���I��
            }
        }
        else
        {
            _flashLight.enabled = false; // ���C�g���I�t
            LightOn = false;
        }
    }
    /// <summary>
    /// �����d���̃o�b�e���[�����
    /// </summary>
    private void UpdateLight()
    {
        if (_batteryLife > 0)
        {
            if (!_flashLight.enabled)
            {
                _flashLight.enabled = true; // �o�b�e���[������ꍇ���C�g���I��
            }

            _batteryLife -= Time.deltaTime; // �o�b�e���[������������

            if (_batteryLife <= 0)
            {
                _flashLight.enabled = false; // �o�b�e���[���؂ꂽ�烉�C�g���I�t
                LightOn = false;
            }
        }
    }
    /// <summary>
    /// �d�r���g�p�����Ƃ��o�b�e���[��
    /// </summary>
    public void GetBattery()
    {

        if (_batteryLife < MAX_BATTERY_LIFE)
        {
            _batteryLife = MAX_BATTERY_LIFE;
        }
    }
    #endregion

    #region �U���A�C�e������
    /// <summary>
    /// �U������
    /// </summary>
    /// <returns></returns>
    private IEnumerator WeaponAttack()
    {
        // ����U���̏����i��莞�ԃR���C�_�[��L�����j
        _weaponCollider.enabled = true; // ����̃R���C�_�[��L����
        yield return new WaitForSeconds(1f); // 1�b��ɖ�����
        _weaponCollider.enabled = false;
    }
    /// <summary>
    /// �U�����}
    /// </summary>
    public void WeaponStart()
    {
        // ����U���̊J�n
        StartCoroutine(WeaponAttack());
    }
    #endregion

    public void KeyInstantiate()
    {
        StartCoroutine(AddKey());
    }
    private IEnumerator AddKey()
    {
        yield return new WaitForSeconds(WAIT_TIME);
        InventoryManager.Instance.AddItem(_completedKey);
    }
    public void MakeKey()
    {
        if (InventoryManager.Instance.KeyCount < 3)
        {
            Debug.Log("�f�ނ�����܂���");
        }
        else
        {
            KeyInstantiate();
        }
    }
}
