using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegController : MonoBehaviour
{
    [SerializeField] private GameObject _bodyObject; // �̃I�u�W�F�N�g
    [SerializeField] private GameObject _otherLeg; // �ʂ̑��I�u�W�F�N�g
    [SerializeField] private GameObject _camera; // �J�����I�u�W�F�N�g
    [SerializeField] private BodyController _body; // BodyController�̎Q��
    [SerializeField] private HeadController _headbody; // HeadController�̎Q��
    //[SerializeField] private MoveManager _moveManager;
    public PlayerMoveManager _playerMoveManager;
    [SerializeField] private CameraManager _cameraManager;

    private bool _isJump = false; // �W�����v�����ǂ����̃t���O

    private const float FALLSPEED = 30;

    private Rigidbody _rb; // Rigidbody�̎Q��

    public enum LegSituation
    {
        HaveLeg,
        Head,
        UnLeg
    }
    public LegSituation _legSituation = LegSituation.HaveLeg;

    // ����������
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        SetMoveMent<TwoLegPlayerMoveManager>();
        //_moveManager.TwoLegMove(true);
    }

    // �������Z�̍X�V����
    private void FixedUpdate()
    {
        if (_legSituation == LegSituation.HaveLeg)
        {
            // �W�����v���Ă��Ȃ��ꍇ�A�������x�𒲐�
            if (!_isJump)
            {
                _rb.velocity -= new Vector3(0, _playerMoveManager._jumpPower / FALLSPEED, 0); // �W�����v�͂ɉ���������
            }
        }

    }

    // ���t���[���̍X�V����
    void Update()
    {
        switch (_legSituation)
        {
            case LegSituation.HaveLeg:
                _playerMoveManager?.PlayerMove(_rb); // �ړ�����
                if (Input.GetKeyDown(KeyCode.Space)&&_isJump) // �X�y�[�X�L�[�������ꂽ��W�����v
                {
                    _rb.velocity = new Vector3(_rb.velocity.x, _playerMoveManager._jumpPower, _rb.velocity.z); // Y���ɃW�����v�͂�ݒ�
                }
                break;

            case LegSituation.Head:
                _rb.constraints = RigidbodyConstraints.FreezeAll;
                break;

            case LegSituation.UnLeg:
                // �������Ȃ�
                break;
        }

        CameraRote();
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
        cameraRote.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(cameraRote);
        float roteSpeed = 20f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, roteSpeed * Time.deltaTime);
    }

  

    // �ՓˊJ�n���̏���
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Yaiba") && _legSituation == LegSituation.HaveLeg)
        {
            int unLegLayer = 10;

            //if (_otherLeg.layer == unLegLayer)
            //{
                _cameraManager.SwitchBody(2);
                LegSwitch(false);
                _legSituation = LegSituation.UnLeg;
                _body.BodySwitch(true); // �̂𑀍�\��
                gameObject.layer = unLegLayer;
            //}
            //else
            //{
            //    gameObject.layer = unLegLayer;
            //    LegController otherLeg = _otherLeg.GetComponent<LegController>();
            //    _moveManager.OneLegMove(true);
            //}
        }
    }

    // �Փ˒��̏���
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = true;
            if (_legSituation == LegSituation.UnLeg)
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    // �Փ˂��I���������̏���
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    // �r�̑����؂�ւ��鏈��
    public void LegSwitch(bool legSwitch)
    {
        if (legSwitch)
        {
            _legSituation = LegSituation.HaveLeg;
            _cameraManager.SwitchBody(1);
            _rb.constraints = RigidbodyConstraints.None;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            SetMoveMent<TwoLegPlayerMoveManager>();
        }
        else if (_legSituation == LegSituation.HaveLeg)
        {
            _legSituation = LegSituation.Head;
        }
        else
        {
            _legSituation = LegSituation.UnLeg;
        }

    }

    // ���X�|�[������
    public void RespawnWait()
    {
        _body.BodySwitch(false);
        _bodyObject.layer = 6;
        _headbody.HeadSwitch(false);
        
        // �r�̃��Z�b�g
        gameObject.layer = 8;
        LegSwitch(true);
        _headbody._isHeadAlive = true;
        SetMoveMent<TwoLegPlayerMoveManager>();
        _legSituation = LegSituation.HaveLeg;
    }
}
