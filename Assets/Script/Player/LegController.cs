using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegController : MonoBehaviour
{
    [SerializeField] private GameObject _bodyObject; // 体オブジェクト
    [SerializeField] private GameObject _otherLeg; // 別の足オブジェクト
    [SerializeField] private GameObject _camera; // カメラオブジェクト
    [SerializeField] private BodyController _body; // BodyControllerの参照
    [SerializeField] private HeadController _headbody; // HeadControllerの参照
    //[SerializeField] private MoveManager _moveManager;
    public PlayerMoveManager _playerMoveManager;
    [SerializeField] private CameraManager _cameraManager;

    private bool _isJump = false; // ジャンプ中かどうかのフラグ

    private const float FALLSPEED = 30;

    private Rigidbody _rb; // Rigidbodyの参照

    public enum LegSituation
    {
        HaveLeg,
        Head,
        UnLeg
    }
    public LegSituation _legSituation = LegSituation.HaveLeg;

    // 初期化処理
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        SetMoveMent<TwoLegPlayerMoveManager>();
        //_moveManager.TwoLegMove(true);
    }

    // 物理演算の更新処理
    private void FixedUpdate()
    {
        if (_legSituation == LegSituation.HaveLeg)
        {
            // ジャンプしていない場合、落下速度を調整
            if (!_isJump)
            {
                _rb.velocity -= new Vector3(0, _playerMoveManager._jumpPower / FALLSPEED, 0); // ジャンプ力に応じた減速
            }
        }

    }

    // 毎フレームの更新処理
    void Update()
    {
        switch (_legSituation)
        {
            case LegSituation.HaveLeg:
                _playerMoveManager?.PlayerMove(_rb); // 移動処理
                if (Input.GetKeyDown(KeyCode.Space)&&_isJump) // スペースキーが押されたらジャンプ
                {
                    _rb.velocity = new Vector3(_rb.velocity.x, _playerMoveManager._jumpPower, _rb.velocity.z); // Y軸にジャンプ力を設定
                }
                break;

            case LegSituation.Head:
                _rb.constraints = RigidbodyConstraints.FreezeAll;
                break;

            case LegSituation.UnLeg:
                // 何もしない
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

  

    // 衝突開始時の処理
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
                _body.BodySwitch(true); // 体を操作可能に
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

    // 衝突中の処理
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

    // 衝突が終了した時の処理
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    // 脚の操作を切り替える処理
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

    // リスポーン処理
    public void RespawnWait()
    {
        _body.BodySwitch(false);
        _bodyObject.layer = 6;
        _headbody.HeadSwitch(false);
        
        // 脚のリセット
        gameObject.layer = 8;
        LegSwitch(true);
        _headbody._isHeadAlive = true;
        SetMoveMent<TwoLegPlayerMoveManager>();
        _legSituation = LegSituation.HaveLeg;
    }
}
