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
    private MoveManager _moveManager;
    private const float FALLSPEED = 30; // �������̌����萔


    [SerializeField] private BodyController _bodyplayer; // BodyController�ւ̎Q��
    private BodyController.BodySituation _bodySituation; // BodyController�ւ̎Q��
    [SerializeField] private LegController _leg; // LegController�ւ̎Q��
    private LegController.LegSituation _legSituation; // LegController�ւ̎Q��
    [SerializeField] private CameraManager _cameraManager;

    private bool _isJump = false; // �W�����v�����ǂ���
    public bool _isHeadAlive = true; // �����������Ă��邩
    private bool _isChange = false;
    private Rigidbody _rb; // Rigidbody�̎Q��

    public enum HeadSituation
    {
        HaveLeg,//���Ɏ哱��������Ƃ�
        HaveBody,//�g�̂Ɏ哱��������Ƃ�
        Head//���Ɏ哱��������Ƃ�
    }
    public HeadSituation _headSituation = default;

    // ����������
    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Rigidbody�R���|�[�l���g���擾
        _moveManager = gameObject.GetComponent<MoveManager>();
        _headSituation = HeadSituation.HaveLeg;
    }
    // �������Z�̍X�V����
    private void FixedUpdate()
    {
        if (_headSituation == HeadSituation.Head)
        {
            // �W�����v���Ă��Ȃ��ꍇ�A�������x�𒲐�
            if (!_isJump)
            {
                _rb.velocity -= new Vector3(0, _moveManager._jumpPower / FALLSPEED, 0); // �W�����v�͂ɉ����Č���
            }
        }
      
    }

    // ���t���[���̍X�V����
    void Update()
    {
        switch (_headSituation)
        {
            case HeadSituation.HaveLeg:
                this.transform.position 
                    = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 2.75f, _legbody.transform.position.z);
                if (_legSituation == LegController.LegSituation.UnLeg)
                {
                    _headSituation = HeadSituation.HaveBody;
                }
                break;
            case HeadSituation.HaveBody:
                this.transform.position 
                    = new Vector3(_body.transform.position.x, _body.transform.position.y + 1.25f, _body.transform.position.z);
               
                break;
            case HeadSituation.Head:
                // ���̈ړ����������s
                //HeadMove(_moveManager._moveSpeed,_moveManager._jumpPower);
                // 'Q'�L�[�������ꂽ�ꍇ�̏���
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    BodyChange();
                }
                if (_isJump) // �X�y�[�X�L�[�������ꂽ��W�����v
                {
                   
                }
                break;
        }
        CameraRote();
        // 'G'�L�[�����������̏���
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (_legSituation==LegController.LegSituation.HaveLeg) // �r���������Ă���ꍇ
            {
                _cameraManager.SwitchBody(3);
                _body.layer = 9;
                HeadSwitch(true); // ���̑����L����
                _leg.LegSwitch(false); // �r�̑���𖳌���
            }
            else
            {
                _cameraManager.SwitchBody(3);
                _body.layer = 9;
                HeadSwitch(true); // ���̑����L����
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
    // ���̈ړ�����
   

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
            if (_headSituation==HeadSituation.Head) // ��������\�ȏꍇ
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
            if (_legSituation==LegController.LegSituation.Head) // �r���������Ă���ꍇ
            {
                HeadSwitch(false); // ���̑���𖳌���
                _headSituation = HeadSituation.HaveLeg;
                _leg.LegSwitch(true); // �r�̑����L����
                _cameraManager.SwitchBody(1);
          
            }
            else
            {
                HeadSwitch(false); // ���̑���𖳌���
                _headSituation = HeadSituation.HaveBody;
                _bodyplayer.BodySwitch(true); // �̂̑����L����
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
            _headSituation = HeadSituation.Head;
            //_moveManager = GetComponent<Head>();
            //_moveManager.Speed();
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // ��]�𐧖�
            _fusionUi.SetActive(false); // ����UI���\�� 
            _isChange = false;
        }
    }
}
