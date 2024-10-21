using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private GameObject _camera; // カメラオブジェクト
    [SerializeField] private GameObject _legbody; // 脚のオブジェクト

    private Rigidbody _rb; // Rigidbodyの参照

    [SerializeField]
    private float _moveSpeed; // 移動速度
    [SerializeField]
    private float _jumpPower; // ジャンプ力

    private const float fallmove = 30; // 落下時の減速量定数

    [SerializeField] private HeadController _head; // 頭のコントローラへの参照
    [SerializeField] private LegController _leg; // 脚のコントローラへの参照
   
    private bool _isJump = false; // ジャンプ中かどうかのフラグ
    private bool _isSwitch = false; // 操作が体にスイッチされているかのフラグ
    private bool _isUnLeg = false; // 脚が取り外されているかのフラグ
    public bool _isUnBody = false; // 体が取り外されているかのフラグ

    // 初期化処理
    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Rigidbodyコンポーネントの取得
    }

    // 物理演算の更新処理
    private void FixedUpdate()
    {
        // ジャンプしていない場合、落下速度を調整
        if (!_isJump)
        {
            _rb.velocity -= new Vector3(0, _jumpPower / fallmove, 0); // ジャンプ力に応じた減速
        }
    }

    // 毎フレームの更新処理
    void Update()
    {
        // 操作が体にスイッチされており、脚が外されていて体も外されていない場合に移動処理を行う
        if (_isSwitch && _isUnLeg && !_isUnBody)
        {
            PlayerMove(); // プレイヤーの移動処理を実行
        }

        // 脚が外されておらず、体の操作が無効化されていない場合
        if (!_isUnLeg && !_isSwitch && !_isUnBody)
        {
            // 体を脚の位置に連動させる
            _rb.constraints = RigidbodyConstraints.None; // 動きの制約を解除
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転のみ制約
            this.transform.position = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 1.5f, _legbody.transform.position.z); // 体の位置を脚の上に配置
        }

        // 脚が外されている場合
        if (_isUnLeg)
        {
            if (_leg._isAlive) // 脚が生存状態の場合
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll; // 動きを完全に停止
            }
        }
    }

    // プレイヤーの移動処理
    private void PlayerMove()
    {
        // カメラを体に追従させる
        _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8); // カメラの位置を更新
        _camera.transform.rotation = Quaternion.Euler(20, 0, 0); // カメラの角度を更新

        // キー入力に応じて移動方向を変更
        if (Input.GetKey(KeyCode.D)) // 右移動
        {
            _rb.velocity = new Vector3(_moveSpeed, _rb.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.A)) // 左移動
        {
            _rb.velocity = new Vector3(-_moveSpeed, _rb.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.W)) // 前進
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, _moveSpeed);
        }
        if (Input.GetKey(KeyCode.S)) // 後退
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, -_moveSpeed);
        }

        // スペースキーでジャンプ処理
        if (_isJump)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _rb.velocity = new Vector3(_rb.velocity.x, _jumpPower, 0); // ジャンプ力を加える
            }
        }
    }

    // 脚の取り外し状態を管理
    public void UnLeg(bool unleg)
    {
        _isUnLeg = unleg;
    }

    // 地面との接触中の処理
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) // 床に接触している場合
        {
            _isJump = true; // ジャンプ可能状態に設定
            if (!_isSwitch) // 操作が体にスイッチされていない場合
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll; // 動きを停止
            }
        }
    }

    // 刃物オブジェクトとの接触時処理
    private void OnCollisionEnter(Collision collision)
    {
        int body=6;
        if (this.gameObject.layer == body)
        {
            if (collision.gameObject.CompareTag("Yaiba")) // 刃物に接触した場合
            {
                int UnBody = 9;
                BodySwitch(false); // 体の操作を無効化
                _isUnBody = true; // 体が外された状態にする
                this.gameObject.layer = UnBody; // レイヤーを9に変更

                // 頭の位置を戻す処理があるか確認
            }
        }
       
    }


    // 地面から離れたときの処理
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false; // ジャンプ不可状態に設定
        }
    }

    // 体の操作を有効化・無効化する処理
    public void BodySwitch(bool bodyswitch)
    {
        if (bodyswitch)
        {
            int body = 6;
            this.gameObject.layer = body; // 操作用のレイヤーに変更
            _rb.constraints = RigidbodyConstraints.None; //動きの制約を解除
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転のみ制約
            _head.HeadSwitch(false); // 頭の操作を無効化
            _isSwitch = true; // 体の操作を有効化
        }
        else
        {
            _isSwitch = false; // 体の操作を無効化
            _head.HeadSwitch(true); // 頭の操作を有効化
        }
    }
}
