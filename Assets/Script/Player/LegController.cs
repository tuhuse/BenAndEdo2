using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LegController : MonoBehaviour
{
    [SerializeField] private GameObject _bodyObject; // �̃I�u�W�F�N�g
    [SerializeField] private GameObject _otherLeg; // �ʂ̑��I�u�W�F�N�g
    [SerializeField] private GameObject _camera; // �J�����I�u�W�F�N�g
    [SerializeField] private BodyController _body; // BodyController�̎Q��
    [SerializeField] private BodyController.BodySituation _bodySituation; // BodyController�̎Q��
    [SerializeField] private HeadController _headbody; // HeadController�̎Q��
     private HeadController.HeadSituation _headSituation; // HeadController�̎Q��
    private MoveManager _moveManager;
    [SerializeField] private CameraManager _cameraManager;

    private bool _isJump = false; // �W�����v�����ǂ���

    private Rigidbody _rb; // Rigidbody�̎Q��

    public enum LegSituation
    {
        HaveLeg,
        Head,
        UnLeg
    }
    public LegSituation _legSituation = default;
    // ����������
    void Start()
    {

        _rb = GetComponent<Rigidbody>(); // Rigidbody�R���|�[�l���g���擾
        _moveManager=gameObject.AddComponent<MoveManager>();
        _moveManager.Speed();
        _legSituation = LegSituation.HaveLeg;
    }

    // ���t���[���̍X�V����
    void Update()
    {
        switch (_legSituation)
        {
            case LegSituation.HaveLeg:
                //�ړ�����
                PlayerMove(_moveManager._moveSpeed,_moveManager._jumpPower);
                break;

            case LegSituation.UnLeg:
                //���������Ȃ�
                break;
            case LegSituation.Head:
                _rb.constraints = RigidbodyConstraints.FreezeAll;
                break;
        }
        CameraRote();
      
    }
    private void CameraRote()
    {
        Vector3 cameraRote = _camera.transform.forward;
        float roteSpeed = 15f;
        cameraRote.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(cameraRote);
        this.gameObject.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, roteSpeed * Time.deltaTime);
    }
    // �v���C���[�ړ�����
    private void PlayerMove(float moveSpeed,float jumpPower)
    {


        // ���͂Ɋ�Â��Ĉړ��������v�Z
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.D)) // �E�ړ�
        {
            moveDirection += transform.right * moveSpeed;
        }
        if (Input.GetKey(KeyCode.A)) // ���ړ�
        {
            moveDirection -= transform.right * moveSpeed;
        }
        if (Input.GetKey(KeyCode.W)) // �O�i
        {
            moveDirection += transform.forward * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S)) // ���
        {
            moveDirection -= transform.forward * moveSpeed;
        }

        // �v�Z�����ړ������Ɋ�Â��ăv���C���[���ړ�
        moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;

        // Rigidbody���g���Ĉړ�������iMovePosition���g�p�j
        _rb.MovePosition(transform.position + moveDirection);

        // �W�����v����
        if (_isJump && Input.GetKeyDown(KeyCode.Space)) // �X�y�[�X�L�[�������ꂽ��W�����v
        {
            _rb.velocity = new Vector3(_rb.velocity.x, jumpPower, _rb.velocity.z); // Y���ɃW�����v�͂�ݒ�
        }
    }

    // �ՓˊJ�n���̏���
    private void OnCollisionEnter(Collision collision)
    {
        int leg = 8;
        int unLeg = 10; // �r�����������ꂽ���̃��C���[�ԍ�

        // "Yaiba"�Ƃ����^�O�̃I�u�W�F�N�g�ɐڐG�����ꍇ
        if (collision.gameObject.CompareTag("Yaiba") && this.gameObject.layer == leg)
        {
            if (_otherLeg.layer == unLeg)
            {
                _cameraManager.SwitchBody(2);
                LegSwitch(false);
                _legSituation = LegSituation.UnLeg;
                _body.BodySwitch(true); // �̂𑀍�\�ɂ���
                this.gameObject.layer = unLeg; // �r�̃��C���[��ύX
            }
            else
            {
               LegController otherLeg = _otherLeg.GetComponent<LegController>();
              otherLeg._moveManager =_otherLeg.GetComponent<OneLleg>();
              otherLeg._moveManager.Speed();
               
            }


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
            if (_legSituation == LegSituation.UnLeg)
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
            _legSituation = LegSituation.HaveLeg;
            _cameraManager.SwitchBody(1);
            _rb.constraints = RigidbodyConstraints.None; // �����̐��������
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // ��]�݂̂𐧌�
          
        }
        else // �r�̑��삪���������ꂽ�ꍇ
        {
          
            if (_legSituation == LegSituation.HaveLeg)
            {
                _legSituation = LegSituation.Head;
            } else
            {
                _legSituation = LegSituation.UnLeg;
            }
        }
    }

    // ���X�|�[������
    public void RespawnWait()
    {
        _moveManager = GetComponent<MoveManager>();
        _moveManager.Speed();
        // �̂̃��Z�b�g
        _body.BodySwitch(false); // �̂̑���𖳌���
        _bodyObject.layer = 6; // �̂̃��C���[�����Z�b�g
        _bodySituation = BodyController.BodySituation.HaveLeg;

        // ���̃��Z�b�g
        _headbody.HeadSwitch(false); // ���̑���𖳌���
        _headSituation = HeadController.HeadSituation.HaveLeg;

        // �r�̃��Z�b�g
        int leg = 8; // �r�̃��C���[�ԍ����Đݒ�
        this.gameObject.layer = leg;
        LegSwitch(true); // �r�̑�����ĂїL����
        _headbody._isHeadAlive = true; // ���𐶑���Ԃɐݒ�
    }
}
