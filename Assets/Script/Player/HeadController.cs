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

    [SerializeField] private float _moveSpeed; // 移動速度
    [SerializeField] private float _jumpPower; // ジャンプ力
    private MoveManager _moveManager;
    private const float FALLSPEED = 30; // 落下時の減速定数


    [SerializeField] private BodyController _bodyplayer; // BodyControllerへの参照
    private BodyController.BodySituation _bodySituation; // BodyControllerへの参照
    [SerializeField] private LegController _leg; // LegControllerへの参照
    private LegController.LegSituation _legSituation; // LegControllerへの参照
    [SerializeField] private CameraManager _cameraManager;

    private bool _isJump = false; // ジャンプ中かどうか
    public bool _isHeadAlive = true; // 頭が生存しているか
    private bool _isChange = false;
    private Rigidbody _rb; // Rigidbodyの参照

    public enum HeadSituation
    {
        HaveLeg,//足に主導権があるとき
        HaveBody,//身体に主導権があるとき
        Head//頭に主導権があるとき
    }
    public HeadSituation _headSituation = default;

    // 初期化処理
    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Rigidbodyコンポーネントを取得
        _moveManager = gameObject.GetComponent<MoveManager>();
        _headSituation = HeadSituation.HaveLeg;
    }
    // 物理演算の更新処理
    private void FixedUpdate()
    {
        if (_headSituation == HeadSituation.Head)
        {
            // ジャンプしていない場合、落下速度を調整
            if (!_isJump)
            {
                _rb.velocity -= new Vector3(0, _moveManager._jumpPower / FALLSPEED, 0); // ジャンプ力に応じて減速
            }
        }
      
    }

    // 毎フレームの更新処理
    void Update()
    {
        switch (_headSituation)
        {
            case HeadSituation.HaveLeg:
                this.transform.position 
                    = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 2.75f, _legbody.transform.position.z);
                if (_legSituation == LegController.LegSituation.UnLeg)
                {
                    _headSituation = HeadSituation.HaveBody;
                }
                break;
            case HeadSituation.HaveBody:
                this.transform.position 
                    = new Vector3(_body.transform.position.x, _body.transform.position.y + 1.25f, _body.transform.position.z);
               
                break;
            case HeadSituation.Head:
                // 頭の移動処理を実行
                //HeadMove(_moveManager._moveSpeed,_moveManager._jumpPower);
                // 'Q'キーが押された場合の処理
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    BodyChange();
                }
                if (_isJump) // スペースキーが押されたらジャンプ
                {
                   
                }
                break;
        }
        CameraRote();
        // 'G'キーを押した時の処理
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (_legSituation==LegController.LegSituation.HaveLeg) // 脚が生存している場合
            {
                _cameraManager.SwitchBody(3);
                _body.layer = 9;
                HeadSwitch(true); // 頭の操作を有効化
                _leg.LegSwitch(false); // 脚の操作を無効化
            }
            else
            {
                _cameraManager.SwitchBody(3);
                _body.layer = 9;
                HeadSwitch(true); // 頭の操作を有効化
            }
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
    // 頭の移動処理
   

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
            if (_headSituation==HeadSituation.Head) // 頭が操作可能な場合
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
            if (_legSituation==LegController.LegSituation.Head) // 脚が生存している場合
            {
                HeadSwitch(false); // 頭の操作を無効化
                _headSituation = HeadSituation.HaveLeg;
                _leg.LegSwitch(true); // 脚の操作を有効化
                _cameraManager.SwitchBody(1);
          
            }
            else
            {
                HeadSwitch(false); // 頭の操作を無効化
                _headSituation = HeadSituation.HaveBody;
                _bodyplayer.BodySwitch(true); // 体の操作を有効化
                _cameraManager.SwitchBody(2);

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
            _headSituation = HeadSituation.Head;
            //_moveManager = GetComponent<Head>();
            //_moveManager.Speed();
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転を制約
            _fusionUi.SetActive(false); // 合体UIを非表示 
            _isChange = false;
        }
    }
}
