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
    [SerializeField] private CameraManager _cameraManager;

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
        CameraRote();
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
    private void CameraRote()
    {
        Vector3 cameraRote = _camera.transform.forward;
        float roteSpeed = 15f;
        cameraRote.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(cameraRote);
        this.gameObject.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, roteSpeed * Time.deltaTime);
    }
    // �v���C���[�̈ړ�����
    private void PlayerMove()
    {

        //// �J������̂ɒǏ]������
        //_camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8); // �J�����̈ʒu���X�V
        //_camera.transform.rotation = Quaternion.Euler(20, 0, 0); // �J�����̊p�x���X�V

        // ���͂Ɋ�Â��Ĉړ��������v�Z
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.D)) // �E�ړ�
        {
            moveDirection += transform.right * _moveSpeed;
        }
        if (Input.GetKey(KeyCode.A)) // ���ړ�
        {
            moveDirection -= transform.right * _moveSpeed;
        }
        if (Input.GetKey(KeyCode.W)) // �O�i
        {
            moveDirection += transform.forward * _moveSpeed;
        }
        if (Input.GetKey(KeyCode.S)) // ���
        {
            moveDirection -= transform.forward * _moveSpeed;
        }

        // �v�Z�����ړ������Ɋ�Â��ăv���C���[���ړ�
        moveDirection = moveDirection.normalized * _moveSpeed * Time.deltaTime;

        // Rigidbody���g���Ĉړ�������iMovePosition���g�p�j
        _rb.MovePosition(transform.position + moveDirection);

        // �W�����v����
        if (_isJump && Input.GetKeyDown(KeyCode.Space)) // �X�y�[�X�L�[�������ꂽ��W�����v
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpPower, _rb.velocity.z); // Y���ɃW�����v�͂�ݒ�
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
                _cameraManager.SwitchBody(3);
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
