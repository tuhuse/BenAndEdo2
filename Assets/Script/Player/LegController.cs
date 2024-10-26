using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LegController : MonoBehaviour
{
    [SerializeField] private GameObject _bodyObject; // 体オブジェクト
    [SerializeField] private GameObject _otherLeg; // 別の足オブジェクト
    [SerializeField] private GameObject _camera; // カメラオブジェクト
    [SerializeField] private BodyController _body; // BodyControllerの参照
    [SerializeField] private BodyController.BodySituation _bodySituation; // BodyControllerの参照
    [SerializeField] private HeadController _headbody; // HeadControllerの参照
     private HeadController.HeadSituation _headSituation; // HeadControllerの参照
    private MoveManager _moveManager;
    [SerializeField] private CameraManager _cameraManager;

    private bool _isJump = false; // ジャンプ中かどうか

    private Rigidbody _rb; // Rigidbodyの参照

    public enum LegSituation
    {
        HaveLeg,
        Head,
        UnLeg
    }
    public LegSituation _legSituation = default;
    // 初期化処理
    void Start()
    {

        _rb = GetComponent<Rigidbody>(); // Rigidbodyコンポーネントを取得
        _moveManager=gameObject.AddComponent<MoveManager>();
        _moveManager.Speed();
        _legSituation = LegSituation.HaveLeg;
    }

    // 毎フレームの更新処理
    void Update()
    {
        switch (_legSituation)
        {
            case LegSituation.HaveLeg:
                //移動処理
                PlayerMove(_moveManager._moveSpeed,_moveManager._jumpPower);
                break;

            case LegSituation.UnLeg:
                //何もさせない
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
    // プレイヤー移動処理
    private void PlayerMove(float moveSpeed,float jumpPower)
    {


        // 入力に基づいて移動方向を計算
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.D)) // 右移動
        {
            moveDirection += transform.right * moveSpeed;
        }
        if (Input.GetKey(KeyCode.A)) // 左移動
        {
            moveDirection -= transform.right * moveSpeed;
        }
        if (Input.GetKey(KeyCode.W)) // 前進
        {
            moveDirection += transform.forward * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S)) // 後退
        {
            moveDirection -= transform.forward * moveSpeed;
        }

        // 計算した移動方向に基づいてプレイヤーを移動
        moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;

        // Rigidbodyを使って移動させる（MovePositionを使用）
        _rb.MovePosition(transform.position + moveDirection);

        // ジャンプ処理
        if (_isJump && Input.GetKeyDown(KeyCode.Space)) // スペースキーが押されたらジャンプ
        {
            _rb.velocity = new Vector3(_rb.velocity.x, jumpPower, _rb.velocity.z); // Y軸にジャンプ力を設定
        }
    }

    // 衝突開始時の処理
    private void OnCollisionEnter(Collision collision)
    {
        int leg = 8;
        int unLeg = 10; // 脚が無効化された時のレイヤー番号

        // "Yaiba"というタグのオブジェクトに接触した場合
        if (collision.gameObject.CompareTag("Yaiba") && this.gameObject.layer == leg)
        {
            if (_otherLeg.layer == unLeg)
            {
                _cameraManager.SwitchBody(2);
                LegSwitch(false);
                _legSituation = LegSituation.UnLeg;
                _body.BodySwitch(true); // 体を操作可能にする
                this.gameObject.layer = unLeg; // 脚のレイヤーを変更
            }
            else
            {
               LegController otherLeg = _otherLeg.GetComponent<LegController>();
              otherLeg._moveManager =_otherLeg.GetComponent<OneLleg>();
              otherLeg._moveManager.Speed();
               
            }


        }

    }

    // 衝突中の処理
    private void OnCollisionStay(Collision collision)
    {
        // "Floor"というタグのオブジェクトに接触している場合、ジャンプ可能な状態に設定
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = true;

            // 脚が生存していない場合や操作可能でない場合は、全ての動きを停止
            if (_legSituation == LegSituation.UnLeg)
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll;
            }
              
            
        }
    }

    // 衝突が終了した時の処理
    private void OnCollisionExit(Collision collision)
    {
        // "Floor"から離れた場合、ジャンプ不可状態に設定
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false;
        }
    }

    // 脚の操作を切り替える処理
    public void LegSwitch(bool legswitch)
    {
        if (legswitch) // 脚の操作が有効化された場合
        {       
            _legSituation = LegSituation.HaveLeg;
            _cameraManager.SwitchBody(1);
            _rb.constraints = RigidbodyConstraints.None; // 動きの制約を解除
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転のみを制限
          
        }
        else // 脚の操作が無効化された場合
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

    // リスポーン処理
    public void RespawnWait()
    {
        _moveManager = GetComponent<MoveManager>();
        _moveManager.Speed();
        // 体のリセット
        _body.BodySwitch(false); // 体の操作を無効化
        _bodyObject.layer = 6; // 体のレイヤーをリセット
        _bodySituation = BodyController.BodySituation.HaveLeg;

        // 頭のリセット
        _headbody.HeadSwitch(false); // 頭の操作を無効化
        _headSituation = HeadController.HeadSituation.HaveLeg;

        // 脚のリセット
        int leg = 8; // 脚のレイヤー番号を再設定
        this.gameObject.layer = leg;
        LegSwitch(true); // 脚の操作を再び有効化
        _headbody._isHeadAlive = true; // 頭を生存状態に設定
    }
}
