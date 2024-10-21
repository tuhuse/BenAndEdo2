using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    [SerializeField] private GameObject _camera; // カメラオブジェクト
    [SerializeField] private GameObject _body; // 体オブジェクト
    [SerializeField] private GameObject _legbody; // 脚オブジェクト
    [SerializeField] private GameObject _fusionUi; // UIオブジェクト（合体用）

    [SerializeField] private float MoveSpeed; // 移動速度
    [SerializeField] private float JumpPower; // ジャンプ力

    private const float fallmove = 30; // 落下時の減速定数

    [SerializeField] private BodyController _bodyplayer; // BodyControllerへの参照
    [SerializeField] private LegController _leg; // LegControllerへの参照

   
    private bool _isSwitch = false; // 頭が現在操作可能かどうか
    private bool _isJump = false; // ジャンプ中かどうか
    public bool _isHeadAlive = true; // 頭が生存しているか
    public bool _isHaveLeg = true; // 脚があるかどうか
    private bool _isChange = false;
    private Rigidbody _rb; // Rigidbodyの参照

    // 初期化処理
    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Rigidbodyコンポーネントを取得
    }

    // 物理演算の更新処理
    private void FixedUpdate()
    {
        // ジャンプしていない場合、落下速度を調整
        if (!_isJump)
        {
            _rb.velocity -= new Vector3(0, JumpPower / fallmove, 0); // ジャンプ力に応じて減速
        }
    }

    // 毎フレームの更新処理
    void Update()
    {
        // 'G'キーを押した時の処理
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (_leg._isAlive) // 脚が生存している場合
            {
                _body.layer = 9;
                _bodyplayer.BodySwitch(false); // 体の操作を無効化
                HeadSwitch(true); // 頭の操作を有効化
                _bodyplayer.UnLeg(true); // 脚を取り外す
                _leg.LegSwitch(false); // 脚の操作を無効化
            }
            else
            {
                _body.layer = 9;
                _bodyplayer.BodySwitch(false); // 体の操作を無効化
                HeadSwitch(true); // 頭の操作を有効化
                _bodyplayer.UnLeg(true); // 脚を取り外す
            }
        }
      
        // 頭の移動処理を実行
        HeadMove();
    }

    // 頭の移動処理
    private void HeadMove()
    {
        if (_isSwitch && _isHeadAlive) // 頭が操作可能で生存している場合
        {
            // カメラを頭に追従させる
            _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8);
            _camera.transform.rotation = Quaternion.Euler(20, 0, 0);

            // キー入力による移動処理
            if (Input.GetKey(KeyCode.D)) // 右移動
            {
                _rb.velocity = new Vector3(MoveSpeed, _rb.velocity.y, 0);
            }
            if (Input.GetKey(KeyCode.A)) // 左移動
            {
                _rb.velocity = new Vector3(-MoveSpeed, _rb.velocity.y, 0);
            }
            if (Input.GetKey(KeyCode.W)) // 前進
            {
                _rb.velocity = new Vector3(0, _rb.velocity.y, MoveSpeed);
            }
            if (Input.GetKey(KeyCode.S)) // 後退
            {
                _rb.velocity = new Vector3(0, _rb.velocity.y, -MoveSpeed);
            }

            // ジャンプ処理
            if (_isJump)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    _rb.velocity = new Vector3(_rb.velocity.x, JumpPower, 0); // ジャンプ力を加える
                }
            }
        }

        // 頭が操作可能でなく、脚がある場合は脚の位置に移動
        if (!_isSwitch && _isHaveLeg)
        {
            this.transform.position = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 2.75f, _legbody.transform.position.z);
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // 脚がなく、体がある場合は体の位置に移動
        if (!_isSwitch && !_isHaveLeg)
        {
            this.transform.position = new Vector3(_body.transform.position.x, _body.transform.position.y + 1.25f, _body.transform.position.z);
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        // 'Q'キーが押された場合の処理
        if (Input.GetKeyDown(KeyCode.Q))
        { 
        BodyChange();
        }
           
    }

    // 地面との接触時の処理
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) // 床に接触している場合
        {
            _isJump = true; // ジャンプ可能状態に設定
        }

        // 特定のレイヤー（unBody）に衝突した場合の処理
        int unBody = 9;
        if (collision.gameObject.layer == unBody)
        {
            if (_isSwitch) // 頭が操作可能な場合
            {
                _fusionUi.SetActive(true); // 合体UIを表示
                _isChange = true;
            }
           
           
        }
    }

    // 衝突が終了した時の処理
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false; // ジャンプ不可状態に設定
        }
        int unBody = 9;
        if (collision.gameObject.layer == unBody)
        {
            _fusionUi.SetActive(false); // 合体UIを非表示
            _isChange = false;
        }
    }

    private void BodyChange()
    {
        if (_isChange)
        {
            _isChange = false;
            if (_leg._isAlive) // 脚が生存している場合
            {
                HeadSwitch(false); // 頭の操作を無効化
                _bodyplayer.UnLeg(false); // 脚を取り付ける
                _leg.LegSwitch(true); // 脚の操作を有効化
                
            }
            else
            {
                HeadSwitch(false); // 頭の操作を無効化
                _bodyplayer.BodySwitch(true); // 体の操作を有効化
                _bodyplayer._isUnBody = false; // 体が外されていない状態に戻す
               
            }
        }
        
        
    }
    // 頭の操作を有効化・無効化する処理
    public void HeadSwitch(bool headswitch)
    {
        if (headswitch)
        {
            _rb.constraints = RigidbodyConstraints.None; // 動きの制約を解除
            _rb.constraints = RigidbodyConstraints.FreezeRotationZ; // Z軸の回転のみ制約
            _isSwitch = true; // 頭の操作を有効化
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転を制約
            _isSwitch = false; // 頭の操作を無効化
            _fusionUi.SetActive(false); // 合体UIを非表示
            _isChange = false;
        }
    }
}
