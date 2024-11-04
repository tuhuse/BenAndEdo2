 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private GameObject _camera; // �J�����I�u�W�F�N�g
    [SerializeField] private GameObject _legbody; // �r�̃I�u�W�F�N�g

    public Rigidbody _rb; // Rigidbody�̎Q��
    public PlayerMoveManager _playerMoveManager;

    private const float fallmove = 30; // �������̌����ʒ萔
    [SerializeField] private HeadController _head; // ���̃R���g���[���ւ̎Q��
    private HeadController.HeadSituation _headSituation
    {
        get { return _head._headSituation; }
    }
    [SerializeField] private LegController _leg; // �r�̃R���g���[���ւ̎Q��
    private LegController.LegSituation _legSituation
    {
        get { return _leg._legSituation; }
    }

    [SerializeField] private CameraManager _cameraManager;
   //[SerializeField] private MoveManager _moveManager;

    private bool _isJump = false; // �W�����v�����ǂ����̃t���O
    public enum BodySituation
    {
        HaveLeg,//�������鎞
        SwitchLeg,//�������邩���Ɏ哱��������ꍇ
        UnLeg,//�����Ȃ��Ƃ����g�̂Ɏ哱��������ꍇ
        Head,//�����Ȃ������Ɏ哱��������Ƃ�
    }
    public BodySituation _bodySituation = default;
    // ����������
    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Rigidbody�R���|�[�l���g�̎擾
        _bodySituation = BodySituation.HaveLeg;

        

    }

    // �������Z�̍X�V����
    private void FixedUpdate()
    {
        if (_bodySituation == BodySituation.UnLeg)
        {
            // �W�����v���Ă��Ȃ��ꍇ�A�������x�𒲐�
            if (!_isJump)
            {
                _rb.velocity -= new Vector3(0, _playerMoveManager._jumpPower / fallmove, 0); // �W�����v�͂ɉ���������
            }
        }

    }

    // ���t���[���̍X�V����
    void Update()
    {
        CameraRote();
        switch (_bodySituation) 
        {
            case BodySituation.HaveLeg:
                // �̂��r�̈ʒu�ɘA��������
                _rb.constraints = RigidbodyConstraints.None; // �����̐��������
                _rb.constraints = RigidbodyConstraints.FreezeRotation; // ��]�̂ݐ���
                this.transform.position = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 1.5f, _legbody.transform.position.z); // �̂̈ʒu���r�̏�ɔz�u

                if (_legSituation == LegController.LegSituation.Head) // �r��������Ԃ����Ɏ哱�����ڂ����ꍇ
                {
                    _bodySituation = BodySituation.SwitchLeg;
                }
                break;
            case BodySituation.SwitchLeg:
                _rb.constraints = RigidbodyConstraints.FreezeAll; // �������~
                if (_legSituation == LegController.LegSituation.HaveLeg) // �r�Ɏ哱�����ڂ����ꍇ
                {
                    _bodySituation = BodySituation.HaveLeg;
                }
                break;
            case BodySituation.UnLeg:
                _playerMoveManager?.PlayerMove(_rb);
                if (Input.GetKeyDown(KeyCode.Space) && _isJump) // �X�y�[�X�L�[�������ꂽ��W�����v
                {
                    _rb.velocity = new Vector3(_rb.velocity.x, _playerMoveManager._jumpPower, _rb.velocity.z); // Y���ɃW�����v�͂�ݒ�
                }
                if (_headSituation == HeadController.HeadSituation.Head) // ���Ɏ哱�����ڂ����ꍇ
                {
                    _bodySituation = BodySituation.Head;
                }
                break;
            case BodySituation.Head:
                if (_headSituation ==HeadController.HeadSituation.HaveBody) // �r��������Ԃ̏ꍇ
                {

                    _bodySituation = BodySituation.UnLeg;
                }
                break;
        }

       
           
        
    }
    private void SetMoveMent<T>() where T : PlayerMoveManager
    {
        if (_playerMoveManager != null)
        {
            Destroy(_playerMoveManager);
        }

        _playerMoveManager = gameObject.AddComponent<T>();
        Debug.Log("SetMoveMent called, PlayerMoveManager set to: " + typeof(T).Name);
    }
    private void CameraRote()
    {
        Vector3 cameraRote = _camera.transform.forward;
        float roteSpeed = 20f;
        cameraRote.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(cameraRote);
        this.gameObject.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, roteSpeed * Time.deltaTime);
    }
   


    // �n�ʂƂ̐ڐG���̏���
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) // ���ɐڐG���Ă���ꍇ
        {
            _isJump = true; // �W�����v�\��Ԃɐݒ�
            if (_bodySituation==BodySituation.Head) // ���삪�̂ɃX�C�b�`����Ă��Ȃ��ꍇ
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
                _head.HeadSwitch(true); // ���̑����L����
                _bodySituation = BodySituation.Head; // �̂̑���𖳌���
                _cameraManager.SwitchBody(3);
                int UnBody = 9;
                BodySwitch(false); // �̂̑���𖳌���
                this.gameObject.layer = UnBody; // ���C���[��9�ɕύX

                // ���̈ʒu��߂����������邩�m�F
            }
        }
        int unLeg = 10;
        if (collision.gameObject.layer == unLeg)
        {
            if (_legSituation == LegController.LegSituation.Head)
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll; // ���������S�ɒ�~
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
            _bodySituation = BodySituation.UnLeg;// �̂̑����L����
            _head.HeadSwitch(false); // ���̑���𖳌���
            //_moveManager.BodyMove(true);
            SetMoveMent<BodyPlayerMoveManager>();
            //_moveManager = GetComponent<Body>();
            //_moveManager.Speed();

        }
       
    }
}
