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
    [SerializeField] private MoveManager _moveManager;
    public PlayerMoveManager _playermoveManager;
    [SerializeField] private CameraManager _cameraManager;

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
        _moveManager.TwoLegMove(true);
    }

    // ���t���[���̍X�V����
    void Update()
    {
        switch (_legSituation)
        {
            case LegSituation.HaveLeg:
                _playermoveManager?.PlayerMove(_rb); // �ړ�����
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

    private void CameraRote()
    {
        Vector3 cameraRote = _camera.transform.forward;
        cameraRote.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(cameraRote);
        float roteSpeed = 15f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, roteSpeed * Time.deltaTime);
    }

  

    // �ՓˊJ�n���̏���
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Yaiba") && _legSituation == LegSituation.HaveLeg)
        {
            int unLegLayer = 10;

            if (_otherLeg.layer == unLegLayer)
            {
                _cameraManager.SwitchBody(2);
                LegSwitch(false);
                _legSituation = LegSituation.UnLeg;
                _body.BodySwitch(true); // �̂𑀍�\��
                gameObject.layer = unLegLayer;
            }
            else
            {
                gameObject.layer = unLegLayer;
                LegController otherLeg = _otherLeg.GetComponent<LegController>();
                _moveManager.OneLegMove(true);
            }
        }
    }

    // �Փ˒��̏���
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
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
        }
        else
        {
            _legSituation = _legSituation == LegSituation.HaveLeg ? LegSituation.Head : LegSituation.UnLeg;
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
    }
}
