 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private GameObject _camera; // カメラオブジェクト
    [SerializeField] private GameObject _legbody; // 脚のオブジェクト

    public Rigidbody _rb; // Rigidbodyの参照
    public PlayerMoveManager _playerMoveManager;

    private const float fallmove = 30; // 落下時の減速量定数
    [SerializeField] private HeadController _head; // 頭のコントローラへの参照
    private HeadController.HeadSituation _headSituation
    {
        get { return _head._headSituation; }
    }
    [SerializeField] private LegController _leg; // 脚のコントローラへの参照
    private LegController.LegSituation _legSituation
    {
        get { return _leg._legSituation; }
    }

    [SerializeField] private CameraManager _cameraManager;
   //[SerializeField] private MoveManager _moveManager;

    private bool _isJump = false; // ジャンプ中かどうかのフラグ
    public enum BodySituation
    {
        HaveLeg,//足がある時
        SwitchLeg,//足があるかつ頭に主導権がある場合
        UnLeg,//足がないときかつ身体に主導権がある場合
        Head,//足がないかつ頭に主導権があるとき
    }
    public BodySituation _bodySituation = default;
    // 初期化処理
    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Rigidbodyコンポーネントの取得
        _bodySituation = BodySituation.HaveLeg;

        

    }

    // 物理演算の更新処理
    private void FixedUpdate()
    {
        if (_bodySituation == BodySituation.UnLeg)
        {
            // ジャンプしていない場合、落下速度を調整
            if (!_isJump)
            {
                _rb.velocity -= new Vector3(0, _playerMoveManager._jumpPower / fallmove, 0); // ジャンプ力に応じた減速
            }
        }

    }

    // 毎フレームの更新処理
    void Update()
    {
        CameraRote();
        switch (_bodySituation) 
        {
            case BodySituation.HaveLeg:
                // 体を脚の位置に連動させる
                _rb.constraints = RigidbodyConstraints.None; // 動きの制約を解除
                _rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転のみ制約
                this.transform.position = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 1.5f, _legbody.transform.position.z); // 体の位置を脚の上に配置

                if (_legSituation == LegController.LegSituation.Head) // 脚が生存状態かつ頭に主導権が移った場合
                {
                    _bodySituation = BodySituation.SwitchLeg;
                }
                break;
            case BodySituation.SwitchLeg:
                _rb.constraints = RigidbodyConstraints.FreezeAll; // 動きを停止
                if (_legSituation == LegController.LegSituation.HaveLeg) // 脚に主導権が移った場合
                {
                    _bodySituation = BodySituation.HaveLeg;
                }
                break;
            case BodySituation.UnLeg:
                _playerMoveManager?.PlayerMove(_rb);
                if (Input.GetKeyDown(KeyCode.Space) && _isJump) // スペースキーが押されたらジャンプ
                {
                    _rb.velocity = new Vector3(_rb.velocity.x, _playerMoveManager._jumpPower, _rb.velocity.z); // Y軸にジャンプ力を設定
                }
                if (_headSituation == HeadController.HeadSituation.Head) // 頭に主導権が移った場合
                {
                    _bodySituation = BodySituation.Head;
                }
                break;
            case BodySituation.Head:
                if (_headSituation ==HeadController.HeadSituation.HaveBody) // 脚が生存状態の場合
                {

                    _bodySituation = BodySituation.UnLeg;
                }
                break;
        }

       
           
        
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
        float roteSpeed = 20f;
        cameraRote.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(cameraRote);
        this.gameObject.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, roteSpeed * Time.deltaTime);
    }
   


    // 地面との接触中の処理
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) // 床に接触している場合
        {
            _isJump = true; // ジャンプ可能状態に設定
            if (_bodySituation==BodySituation.Head) // 操作が体にスイッチされていない場合
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
                _head.HeadSwitch(true); // 頭の操作を有効化
                _bodySituation = BodySituation.Head; // 体の操作を無効化
                _cameraManager.SwitchBody(3);
                int UnBody = 9;
                BodySwitch(false); // 体の操作を無効化
                this.gameObject.layer = UnBody; // レイヤーを9に変更

                // 頭の位置を戻す処理があるか確認
            }
        }
        int unLeg = 10;
        if (collision.gameObject.layer == unLeg)
        {
            if (_legSituation == LegController.LegSituation.Head)
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll; // 動きを完全に停止
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
            _bodySituation = BodySituation.UnLeg;// 体の操作を有効化
            _head.HeadSwitch(false); // 頭の操作を無効化
            //_moveManager.BodyMove(true);
            SetMoveMent<BodyPlayerMoveManager>();
            //_moveManager = GetComponent<Body>();
            //_moveManager.Speed();

        }
       
    }
}
