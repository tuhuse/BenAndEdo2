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
    [SerializeField] private HeadController _headbody; // HeadControllerの参照
    private MoveManager _moveManager;
    [SerializeField] private CameraManager _cameraManager;
    private float _moveSpeed; // 移動速度
    private float _jumpPower; // ジャンプ力
    private bool _isJump = false; // ジャンプ中かどうか
    public bool _isAlive = true; // 足が生存しているかどうか
    public bool _isSwitch = true; // 現在脚が操作可能かどうか
    private Rigidbody _rb; // Rigidbodyの参照

    // 初期化処理
    void Start()
    {

        _rb = GetComponent<Rigidbody>(); // Rigidbodyコンポーネントを取得
        _moveManager=gameObject.AddComponent<MoveManager>();
        _moveManager.Speed();
        _moveSpeed = _moveManager._moveSpeed;
        _jumpPower = _moveManager._jumpPower;

    }

    // 毎フレームの更新処理
    void Update()
    {
        CameraRote();
        // 足が生存していて操作可能な場合に移動処理を行う
        if (_isAlive && _isSwitch)
        {

            PlayerMove();
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
    // プレイヤー移動処理
    private void PlayerMove()
    {


        // 入力に基づいて移動方向を計算
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.D)) // 右移動
        {
            moveDirection += transform.right * _moveSpeed;
        }
        if (Input.GetKey(KeyCode.A)) // 左移動
        {
            moveDirection -= transform.right * _moveSpeed;
        }
        if (Input.GetKey(KeyCode.W)) // 前進
        {
            moveDirection += transform.forward * _moveSpeed;
        }
        if (Input.GetKey(KeyCode.S)) // 後退
        {
            moveDirection -= transform.forward * _moveSpeed;
        }

        // 計算した移動方向に基づいてプレイヤーを移動
        moveDirection = moveDirection.normalized * _moveSpeed * Time.deltaTime;

        // Rigidbodyを使って移動させる（MovePositionを使用）
        _rb.MovePosition(transform.position + moveDirection);

        // ジャンプ処理
        if (_isJump && Input.GetKeyDown(KeyCode.Space)) // スペースキーが押されたらジャンプ
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpPower, _rb.velocity.z); // Y軸にジャンプ力を設定
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
                _body.UnLeg(true); // 体から脚を取り外す
                _headbody._isHaveLeg = false; // 頭に脚がない状態に設定
                _body.BodySwitch(true); // 体を操作可能にする
                _isAlive = false; // 脚が生存していない状態に設定
                this.gameObject.layer = unLeg; // 脚のレイヤーを変更
            }
            else
            {
                _moveManager = GetComponent<OneLleg>();
                _moveManager.Speed();
                _moveSpeed = _moveManager._moveSpeed;
                _moveSpeed = _moveManager._jumpPower;
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
            if (!_isAlive || !_isSwitch)
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
            _isSwitch = true; // 脚の操作を有効化
            _cameraManager.SwitchBody(1);
            _rb.constraints = RigidbodyConstraints.None; // 動きの制約を解除
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転のみを制限
        }
        else // 脚の操作が無効化された場合
        {
            _isSwitch = false; // 脚の操作を無効化
        }
    }

    // リスポーン処理
    public void RespawnWait()
    {
        // 体のリセット
        _body.BodySwitch(false); // 体の操作を無効化
        _body.UnLeg(false); // 脚を再装着
        _body._isUnBody = false; // 体を外れていない状態に戻す
        _bodyObject.layer = 6; // 体のレイヤーをリセット

        // 頭のリセット
        _headbody.HeadSwitch(false); // 頭の操作を無効化
        _headbody._isHaveLeg = true; // 頭に脚を再接続

        // 脚のリセット
        int leg = 8; // 脚のレイヤー番号を再設定
        this.gameObject.layer = leg;
        _isAlive = true; // 脚を生存状態に設定
        LegSwitch(true); // 脚の操作を再び有効化
        _headbody._isHeadAlive = true; // 頭を生存状態に設定
    }
}
