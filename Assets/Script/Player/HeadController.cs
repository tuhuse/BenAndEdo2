using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _legbody;
    [SerializeField] private GameObject _fusionUi;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;

    private const float FALLSPEED = 30;
   [SerializeField] private MoveManager _moveManager;
    private Rigidbody _rb;
    private bool _isJump = false;
    public bool _isHeadAlive = true;
    private bool _isChange = false;

    [SerializeField] private BodyController _bodyplayer;
    private BodyController.BodySituation _bodySituation;
    [SerializeField] private LegController _leg;
    private LegController.LegSituation _legSituation;
    [SerializeField] private CameraManager _cameraManager;

    public enum HeadSituation
    {
        HaveLeg,
        HaveBody,
        Head
    }

    public HeadSituation _headSituation = default;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        _headSituation = HeadSituation.HaveLeg;
    }

    private void FixedUpdate()
    {
        if (_headSituation == HeadSituation.Head && !_isJump)
        {
            //_rb.velocity -= new Vector3(0, _moveManager._jumpPower / FALLSPEED, 0);
        }
    }

    void Update()
    {
        HandleHeadSituation();
        HandleInput();
        CameraRotate();
    }

    private void HandleHeadSituation()
    {
        switch (_headSituation)
        {
            case HeadSituation.HaveLeg:
                AlignWithLegBody();
                if (_legSituation == LegController.LegSituation.UnLeg)
                    _headSituation = HeadSituation.HaveBody;
                break;

            case HeadSituation.HaveBody:
                AlignWithBody();
                break;

            case HeadSituation.Head:
                _leg._playermoveManager?.PlayerMove(_rb);
                break;
        }
    }

    private void HandleInput()
    {
        if (_headSituation == HeadSituation.Head && Input.GetKeyDown(KeyCode.Q))
            BodyChange();

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (_legSituation == LegController.LegSituation.HaveLeg)
                SwitchToHeadMode();
            else
                SetHeadControl();
        }
    }

    private void AlignWithLegBody()
    {
        transform.position = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 2.75f, _legbody.transform.position.z);
    }

    private void AlignWithBody()
    {
        transform.position = new Vector3(_body.transform.position.x, _body.transform.position.y + 1.25f, _body.transform.position.z);
    }

    private void CameraRotate()
    {
        Vector3 cameraRotation = _camera.transform.forward;
        cameraRotation.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(cameraRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
    }

    private void SwitchToHeadMode()
    {
        _cameraManager.SwitchBody(3);
        _body.layer = 9;
        HeadSwitch(true);
        _leg.LegSwitch(false);
    }

    private void SetHeadControl()
    {
        _cameraManager.SwitchBody(3);
        _body.layer = 9;
        HeadSwitch(true);
    }

    private void BodyChange()
    {
        if (_isChange)
        {
            _isChange = false;
            if (_legSituation == LegController.LegSituation.HaveLeg)
            {
                HeadSwitch(false);
                _headSituation = HeadSituation.HaveLeg;
                _leg.LegSwitch(true);
                _cameraManager.SwitchBody(1);
               
            }
            else
            {
                HeadSwitch(false);
                _headSituation = HeadSituation.HaveBody;
                _bodyplayer.BodySwitch(true);
                _cameraManager.SwitchBody(2);
                _moveManager.BodyMove(true);
            }
        }
    }

    public void HeadSwitch(bool headswitch)
    {
        if (headswitch)
        {
            _rb.constraints = headswitch ? RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationZ : RigidbodyConstraints.FreezeRotation;
            _headSituation = headswitch ? HeadSituation.Head : _headSituation;
            _fusionUi.SetActive(headswitch);
            _isChange = headswitch;
            _moveManager.HeadMove(true);
        }
      
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
            _isJump = true;

        if (collision.gameObject.layer == 9 && _headSituation == HeadSituation.Head)
        {
            _fusionUi.SetActive(true);
            _isChange = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
            _isJump = false;

        if (collision.gameObject.layer == 9)
        {
            _fusionUi.SetActive(false);
            _isChange = false;
        }
    }
}
