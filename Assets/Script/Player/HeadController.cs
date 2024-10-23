using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    [SerializeField] private GameObject _camera; // �J�����I�u�W�F�N�g
    [SerializeField] private GameObject _body; // �̃I�u�W�F�N�g
    [SerializeField] private GameObject _legbody; // �r�I�u�W�F�N�g
    [SerializeField] private GameObject _fusionUi; // UI�I�u�W�F�N�g�i���̗p�j

    [SerializeField] private float _moveSpeed; // �ړ����x
    [SerializeField] private float _jumpPower; // �W�����v��

    private const float FALLSPEED = 30; // �������̌����萔
    

    [SerializeField] private BodyController _bodyplayer; // BodyController�ւ̎Q��
    [SerializeField] private LegController _leg; // LegController�ւ̎Q��
    [SerializeField] private CameraManager _cameraManager;
   
    private bool _isSwitch = false; // �������ݑ���\���ǂ���
    private bool _isJump = false; // �W�����v�����ǂ���
    public bool _isHeadAlive = true; // �����������Ă��邩
    public bool _isHaveLeg = true; // �r�����邩�ǂ���
    private bool _isChange = false;
    private Rigidbody _rb; // Rigidbody�̎Q��

    // ����������
    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Rigidbody�R���|�[�l���g���擾
    }

    // �������Z�̍X�V����
    private void FixedUpdate()
    {
        // �W�����v���Ă��Ȃ��ꍇ�A�������x�𒲐�
        if (!_isJump)
        {
            _rb.velocity -= new Vector3(0, _jumpPower / FALLSPEED, 0); // �W�����v�͂ɉ����Č���
        }
    }

    // ���t���[���̍X�V����
    void Update()
    {
        CameraRote();
        // 'G'�L�[�����������̏���
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (_leg._isAlive) // �r���������Ă���ꍇ
            {
                _cameraManager.SwitchBody(3);
                _body.layer = 9;
                _bodyplayer.BodySwitch(false); // �̂̑���𖳌���
                HeadSwitch(true); // ���̑����L����
                _bodyplayer.UnLeg(true); // �r�����O��
                _leg.LegSwitch(false); // �r�̑���𖳌���
            }
            else
            {
                _cameraManager.SwitchBody(3);
                _body.layer = 9;
                _bodyplayer.BodySwitch(false); // �̂̑���𖳌���
                HeadSwitch(true); // ���̑����L����
                _bodyplayer.UnLeg(true); // �r�����O��
            }
        }
      
        // ���̈ړ����������s
        HeadMove();
    }

    private void CameraRote()
    {
        Vector3 cameraRote = _camera.transform.forward;
        float roteSpeed = 15f;
        cameraRote.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(cameraRote);
        this.gameObject.transform.rotation = Quaternion.Slerp(this.transform.rotation,targetRotation,roteSpeed*Time.deltaTime);
    }
    // ���̈ړ�����
    private void HeadMove()
    {
        if (_isSwitch && _isHeadAlive) // ��������\�Ő������Ă���ꍇ
        {
            //// �J�����𓪂ɒǏ]������
            //_camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8);
            //_camera.transform.rotation = Quaternion.Euler(20, 0, 0);
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

        // ��������\�łȂ��A�r������ꍇ�͋r�̈ʒu�Ɉړ�
        if (!_isSwitch && _isHaveLeg)
        {
            this.transform.position = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 2.75f, _legbody.transform.position.z);
        
        }

        // �r���Ȃ��A�̂�����ꍇ�͑̂̈ʒu�Ɉړ�
        if (!_isSwitch && !_isHaveLeg)
        {
            this.transform.position = new Vector3(_body.transform.position.x, _body.transform.position.y + 1.25f, _body.transform.position.z);
            
        }
        // 'Q'�L�[�������ꂽ�ꍇ�̏���
        if (Input.GetKeyDown(KeyCode.Q))
        { 
        BodyChange();
        }
           
    }

    // �n�ʂƂ̐ڐG���̏���
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) // ���ɐڐG���Ă���ꍇ
        {
            _isJump = true; // �W�����v�\��Ԃɐݒ�
        }

        // ����̃��C���[�iunBody�j�ɏՓ˂����ꍇ�̏���
        int unBody = 9;
        if (collision.gameObject.layer == unBody)
        {
            if (_isSwitch) // ��������\�ȏꍇ
            {
                _fusionUi.SetActive(true); // ����UI��\��
                _isChange = true;
            }
           
           
        }
    }

    // �Փ˂��I���������̏���
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false; // �W�����v�s��Ԃɐݒ�
        }
        int unBody = 9;
        if (collision.gameObject.layer == unBody)
        {
            _fusionUi.SetActive(false); // ����UI���\��
            _isChange = false;
        }
    }

    private void BodyChange()
    {
        if (_isChange)
        {
            _isChange = false;
            if (_leg._isAlive) // �r���������Ă���ꍇ
            {
                HeadSwitch(false); // ���̑���𖳌���
                _bodyplayer.UnLeg(false); // �r�����t����
                _leg.LegSwitch(true); // �r�̑����L����
                _cameraManager.SwitchBody(1);

            }
            else
            {
                HeadSwitch(false); // ���̑���𖳌���
                _bodyplayer.BodySwitch(true); // �̂̑����L����
                _bodyplayer._isUnBody = false; // �̂��O����Ă��Ȃ���Ԃɖ߂�
                _cameraManager.SwitchBody(2);

            }
           
        }
        
        
    }
    // ���̑����L�����E���������鏈��
    public void HeadSwitch(bool headswitch)
    {
        if (headswitch)
        {
            _rb.constraints = RigidbodyConstraints.None; // �����̐��������
            _rb.constraints = RigidbodyConstraints.FreezeRotationZ; // Z���̉�]�̂ݐ���
            _isSwitch = true; // ���̑����L����
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // ��]�𐧖�
            _isSwitch = false; // ���̑���𖳌���
            _fusionUi.SetActive(false); // ����UI���\��
            _isChange = false;
        }
    }
}
