using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour
{
    // �V���O���g���p�^�[���p�̃C���X�^���X
    public static ItemManager Instance { get; private set; }

    // �U���A�C�e���֘A
    [SerializeField] private BoxCollider _weaponCollider; // ����̃R���C�_�[

    // ���A�C�e���֘A
    [SerializeField]
    private GameObject _perfectKey; // ����������
    [SerializeField]
    private GameObject _unionKey; // ���̃p�[�c

    // �����d���֘A
    private const int MAX_BATTERY_LIFE = 60; // �o�b�e���[�����̍ő�l
    [SerializeField] private Light _flashLight; // �����d���̃��C�g
    [SerializeField] private float _batteryLife = 60f; // ���݂̃o�b�e���[�����i�b�j

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
    public void LightActive()
    {
        // �����d���̃I���I�t�؂�ւ�
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

    private void UpdateLight()
    {
        // �����d���̃o�b�e���[�����
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

    public void GetBattery()
    {
        // �o�b�e���[�A�C�e�����擾���ăo�b�e���[��������
        if (_batteryLife < MAX_BATTERY_LIFE)
        {
            _batteryLife = MAX_BATTERY_LIFE;
        }
    }
    #endregion

    #region �U���A�C�e������
    private IEnumerator WeaponAttack()
    {
        // ����U���̏����i��莞�ԃR���C�_�[��L�����j
        _weaponCollider.enabled = true; // ����̃R���C�_�[��L����
        yield return new WaitForSeconds(1f); // 1�b��ɖ�����
        _weaponCollider.enabled = false;
    }

    public void WeaponStart()
    {
        // ����U���̊J�n
        StartCoroutine(WeaponAttack());
    }
    #endregion

    public void KeyCountPlus()
    {
        // ���p�[�c���������ꍇ�Ɋ����������𐶐�
        if (InventoryManager.Instance.KeyCount == 3)
        {
            _perfectKey = Instantiate(_unionKey, _perfectKey.transform.position, Quaternion.identity);
        }
    }
}
