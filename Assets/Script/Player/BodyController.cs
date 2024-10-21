using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private GameObject _camera; // �J�����I�u�W�F�N�g
    [SerializeField] private GameObject _legbody; // �r�̃I�u�W�F�N�g

    private Rigidbody _rb; // Rigidbody�̎Q��

    [SerializeField]
    private float _moveSpeed; // �ړ����x
    [SerializeField]
    private float _jumpPower; // �W�����v��

    private const float fallmove = 30; // �������̌����ʒ萔

    [SerializeField] private HeadController _head; // ���̃R���g���[���ւ̎Q��
    [SerializeField] private LegController _leg; // �r�̃R���g���[���ւ̎Q��
   
    private bool _isJump = false; // �W�����v�����ǂ����̃t���O
    private bool _isSwitch = false; // ���삪�̂ɃX�C�b�`����Ă��邩�̃t���O
    private bool _isUnLeg = false; // �r�����O����Ă��邩�̃t���O
    public bool _isUnBody = false; // �̂����O����Ă��邩�̃t���O

    // ����������
    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Rigidbody�R���|�[�l���g�̎擾
    }

    // �������Z�̍X�V����
    private void FixedUpdate()
    {
        // �W�����v���Ă��Ȃ��ꍇ�A�������x�𒲐�
        if (!_isJump)
        {
            _rb.velocity -= new Vector3(0, _jumpPower / fallmove, 0); // �W�����v�͂ɉ���������
        }
    }

    // ���t���[���̍X�V����
    void Update()
    {
        // ���삪�̂ɃX�C�b�`����Ă���A�r���O����Ă��đ̂��O����Ă��Ȃ��ꍇ�Ɉړ��������s��
        if (_isSwitch && _isUnLeg && !_isUnBody)
        {
            PlayerMove(); // �v���C���[�̈ړ����������s
        }

        // �r���O����Ă��炸�A�̂̑��삪����������Ă��Ȃ��ꍇ
        if (!_isUnLeg && !_isSwitch && !_isUnBody)
        {
            // �̂��r�̈ʒu�ɘA��������
            _rb.constraints = RigidbodyConstraints.None; // �����̐��������
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // ��]�̂ݐ���
            this.transform.position = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 1.5f, _legbody.transform.position.z); // �̂̈ʒu���r�̏�ɔz�u
        }

        // �r���O����Ă���ꍇ
        if (_isUnLeg)
        {
            if (_leg._isAlive) // �r��������Ԃ̏ꍇ
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll; // ���������S�ɒ�~
            }
        }
    }

    // �v���C���[�̈ړ�����
    private void PlayerMove()
    {
        // �J������̂ɒǏ]������
        _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8); // �J�����̈ʒu���X�V
        _camera.transform.rotation = Quaternion.Euler(20, 0, 0); // �J�����̊p�x���X�V

        // �L�[���͂ɉ����Ĉړ�������ύX
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

        // �X�y�[�X�L�[�ŃW�����v����
        if (_isJump)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _rb.velocity = new Vector3(_rb.velocity.x, _jumpPower, 0); // �W�����v�͂�������
            }
        }
    }

    // �r�̎��O����Ԃ��Ǘ�
    public void UnLeg(bool unleg)
    {
        _isUnLeg = unleg;
    }

    // �n�ʂƂ̐ڐG���̏���
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) // ���ɐڐG���Ă���ꍇ
        {
            _isJump = true; // �W�����v�\��Ԃɐݒ�
            if (!_isSwitch) // ���삪�̂ɃX�C�b�`����Ă��Ȃ��ꍇ
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll; // �������~
            }
        }
    }

    // �n���I�u�W�F�N�g�Ƃ̐ڐG������
    private void OnCollisionEnter(Collision collision)
    {
        int body=6;
        if (this.gameObject.layer == body)
        {
            if (collision.gameObject.CompareTag("Yaiba")) // �n���ɐڐG�����ꍇ
            {
                int UnBody = 9;
                BodySwitch(false); // �̂̑���𖳌���
                _isUnBody = true; // �̂��O���ꂽ��Ԃɂ���
                this.gameObject.layer = UnBody; // ���C���[��9�ɕύX

                // ���̈ʒu��߂����������邩�m�F
            }
        }
       
    }


    // �n�ʂ��痣�ꂽ�Ƃ��̏���
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false; // �W�����v�s��Ԃɐݒ�
        }
    }

    // �̂̑����L�����E���������鏈��
    public void BodySwitch(bool bodyswitch)
    {
        if (bodyswitch)
        {
            int body = 6;
            this.gameObject.layer = body; // ����p�̃��C���[�ɕύX
            _rb.constraints = RigidbodyConstraints.None; //�����̐��������
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // ��]�̂ݐ���
            _head.HeadSwitch(false); // ���̑���𖳌���
            _isSwitch = true; // �̂̑����L����
        }
        else
        {
            _isSwitch = false; // �̂̑���𖳌���
            _head.HeadSwitch(true); // ���̑����L����
        }
    }
}
