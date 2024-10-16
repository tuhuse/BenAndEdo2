using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LegController : MonoBehaviour
{
    [SerializeField] private GameObject _bodyObject; // �̃I�u�W�F�N�g
    [SerializeField] private GameObject _camera; // �J�����I�u�W�F�N�g
    [SerializeField] private BodyController _body; // BodyController�̎Q��
    [SerializeField] private HeadController _headbody; // HeadController�̎Q��
    [SerializeField] private float _moveSpeed; // �ړ����x
    [SerializeField] private float _jumpPower; // �W�����v��
    private bool _isJump = false; // �W�����v�����ǂ���
    public bool _isAlive = true; // �����������Ă��邩�ǂ���
    public bool _isSwitch = true; // ���݋r������\���ǂ���
    private Rigidbody _rb; // Rigidbody�̎Q��
    public BoxCollider _box; // BoxCollider�̎Q��

    // ����������
    void Start()
    {
      
        _rb = GetComponent<Rigidbody>(); // Rigidbody�R���|�[�l���g���擾
        _box = GetComponent<BoxCollider>(); // BoxCollider�R���|�[�l���g���擾
    }

    // ���t���[���̍X�V����
    void Update()
    {
        // �����������Ă��đ���\�ȏꍇ�Ɉړ��������s��
        if (_isAlive && _isSwitch)
        {
            PlayerMove();
        }
    }

    // �v���C���[�ړ�����
    private void PlayerMove()
    {
        // �J�����̈ʒu���X�V���A��ɋr�ɒǏ]������
        _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8);
        _camera.transform.rotation = Quaternion.Euler(20, 0, 0);

        // �L�[���͂ɂ��ړ�����
        if (Input.GetKey(KeyCode.D)) // �E�ړ�
        {
            _rb.velocity = new Vector3(_moveSpeed, _rb.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.A)) // ���ړ�
        {
            _rb.velocity = new Vector3(-_moveSpeed, _rb.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.W)) // �O�i
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, _moveSpeed);
        }
        if (Input.GetKey(KeyCode.S)) // ���
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, -_moveSpeed);
        }

        // �W�����v����
        if (_isJump)
        {
            if (Input.GetKey(KeyCode.Space)) // �X�y�[�X�L�[�ŃW�����v
            {
                _rb.velocity = new Vector3(_rb.velocity.x, _jumpPower, 0);
            }
        }
    }

    // �ՓˊJ�n���̏���
    private void OnCollisionEnter(Collision collision)
    {
        int unLeg = 10; // �r�����������ꂽ���̃��C���[�ԍ�

        // "Yaiba"�Ƃ����^�O�̃I�u�W�F�N�g�ɐڐG�����ꍇ
        if (collision.gameObject.CompareTag("Yaiba"))
        {
            _body.UnLeg(true); // �̂���r�����O��
            _headbody._isHaveLeg = false; // ���ɋr���Ȃ���Ԃɐݒ�
            _body.BodySwitch(true); // �̂𑀍�\�ɂ���
            _isAlive = false; // �r���������Ă��Ȃ���Ԃɐݒ�
            this.gameObject.layer = unLeg; // �r�̃��C���[��ύX
        }
    }

    // �Փ˒��̏���
    private void OnCollisionStay(Collision collision)
    {
        // "Floor"�Ƃ����^�O�̃I�u�W�F�N�g�ɐڐG���Ă���ꍇ�A�W�����v�\�ȏ�Ԃɐݒ�
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = true;

            // �r���������Ă��Ȃ��ꍇ�⑀��\�łȂ��ꍇ�́A�S�Ă̓������~
            if (!_isAlive || !_isSwitch)
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    // �Փ˂��I���������̏���
    private void OnCollisionExit(Collision collision)
    {
        // "Floor"���痣�ꂽ�ꍇ�A�W�����v�s��Ԃɐݒ�
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false;
        }
    }

    // �r�̑����؂�ւ��鏈��
    public void LegSwitch(bool legswitch)
    {
        if (legswitch) // �r�̑��삪�L�������ꂽ�ꍇ
        {
            _isSwitch = true; // �r�̑����L����
            _rb.constraints = RigidbodyConstraints.None; // �����̐��������
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // ��]�݂̂𐧌�
        }
        else // �r�̑��삪���������ꂽ�ꍇ
        {
            _isSwitch = false; // �r�̑���𖳌���
        }
    }

    // ���X�|�[������
    public void RespawnWait()
    {
        // �̂̃��Z�b�g
        _body.BodySwitch(false); // �̂̑���𖳌���
        _body.UnLeg(false); // �r���đ���
        _body._isUnBody = false; // �̂��O��Ă��Ȃ���Ԃɖ߂�
        _bodyObject.layer = 6; // �̂̃��C���[�����Z�b�g

        // ���̃��Z�b�g
        _headbody.HeadSwitch(false); // ���̑���𖳌���
        _headbody._isHaveLeg = true; // ���ɋr���Đڑ�

        // �r�̃��Z�b�g
        int leg = 8; // �r�̃��C���[�ԍ����Đݒ�
        this.gameObject.layer = leg;
        _isAlive = true; // �r�𐶑���Ԃɐݒ�
        LegSwitch(true); // �r�̑�����ĂїL����
        _headbody._isHeadAlive = true; // ���𐶑���Ԃɐݒ�
    }
}
