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

    [SerializeField] private float MoveSpeed; // �ړ����x
    [SerializeField] private float JumpPower; // �W�����v��

    private const float fallmove = 30; // �������̌����萔

    [SerializeField] private BodyController _bodyplayer; // BodyController�ւ̎Q��
    [SerializeField] private LegController _leg; // LegController�ւ̎Q��

   
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
            _rb.velocity -= new Vector3(0, JumpPower / fallmove, 0); // �W�����v�͂ɉ����Č���
        }
    }

    // ���t���[���̍X�V����
    void Update()
    {
        // 'G'�L�[�����������̏���
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (_leg._isAlive) // �r���������Ă���ꍇ
            {
                _body.layer = 9;
                _bodyplayer.BodySwitch(false); // �̂̑���𖳌���
                HeadSwitch(true); // ���̑����L����
                _bodyplayer.UnLeg(true); // �r�����O��
                _leg.LegSwitch(false); // �r�̑���𖳌���
            }
            else
            {
                _body.layer = 9;
                _bodyplayer.BodySwitch(false); // �̂̑���𖳌���
                HeadSwitch(true); // ���̑����L����
                _bodyplayer.UnLeg(true); // �r�����O��
            }
        }
      
        // ���̈ړ����������s
        HeadMove();
    }

    // ���̈ړ�����
    private void HeadMove()
    {
        if (_isSwitch && _isHeadAlive) // ��������\�Ő������Ă���ꍇ
        {
            // �J�����𓪂ɒǏ]������
            _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8);
            _camera.transform.rotation = Quaternion.Euler(20, 0, 0);

            // �L�[���͂ɂ��ړ�����
            if (Input.GetKey(KeyCode.D)) // �E�ړ�
            {
                _rb.velocity = new Vector3(MoveSpeed, _rb.velocity.y, 0);
            }
            if (Input.GetKey(KeyCode.A)) // ���ړ�
            {
                _rb.velocity = new Vector3(-MoveSpeed, _rb.velocity.y, 0);
            }
            if (Input.GetKey(KeyCode.W)) // �O�i
            {
                _rb.velocity = new Vector3(0, _rb.velocity.y, MoveSpeed);
            }
            if (Input.GetKey(KeyCode.S)) // ���
            {
                _rb.velocity = new Vector3(0, _rb.velocity.y, -MoveSpeed);
            }

            // �W�����v����
            if (_isJump)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    _rb.velocity = new Vector3(_rb.velocity.x, JumpPower, 0); // �W�����v�͂�������
                }
            }
        }

        // ��������\�łȂ��A�r������ꍇ�͋r�̈ʒu�Ɉړ�
        if (!_isSwitch && _isHaveLeg)
        {
            this.transform.position = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 2.75f, _legbody.transform.position.z);
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // �r���Ȃ��A�̂�����ꍇ�͑̂̈ʒu�Ɉړ�
        if (!_isSwitch && !_isHaveLeg)
        {
            this.transform.position = new Vector3(_body.transform.position.x, _body.transform.position.y + 1.25f, _body.transform.position.z);
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
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
                
            }
            else
            {
                HeadSwitch(false); // ���̑���𖳌���
                _bodyplayer.BodySwitch(true); // �̂̑����L����
                _bodyplayer._isUnBody = false; // �̂��O����Ă��Ȃ���Ԃɖ߂�
               
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
