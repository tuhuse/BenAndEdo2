using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤーの挙動
/// </summary>
public class PlayerMoveController : MonoBehaviour
{
    [SerializeField] private float _dashMultiplier = 2f;
    private bool _isDash = false;
    private bool _isTired = false;
    private const float UNTIRED_HP = 30f;
    private ValueManager _valueManager;
    private Rigidbody _rb;

    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Death
    }
    public PlayerState PlayerCurrentsState { get; private set; }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _valueManager = ValueManager.Instance;
        PlayerCurrentsState = PlayerState.Idle;
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    void Update()
    {
        HandleDashInput();
        if (_valueManager.PlayerHP == 0)
        {
            PlayerCurrentsState = PlayerState.Death;
        }
    }

    private void HandleDashInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _valueManager.DashHP > 0&&!_isTired)
        {
            _isDash = true;
            _valueManager.DashHPDecrease();
        }
        else if(_valueManager.DashHP==0)
        {
            _isTired = true;
            _valueManager.StartDashRecovery();
           
        }
        else
        {
            _isDash = false;
            _valueManager.StartDashRecovery();
        }
        if (_isTired)
        {
            if (_valueManager.DashHP >= UNTIRED_HP)
            {
                _isTired = false;
            }
        }
    }

    private void PlayerMove()
    {
        if (_valueManager.PlayerHP == 0)
        {
            return;
        }
        else
        {
            Vector3 moveDirection = Vector3.zero;

            // 移動入力の処理
            if (Input.GetKey(KeyCode.D))
                moveDirection += transform.right;
            if (Input.GetKey(KeyCode.A))
                moveDirection -= transform.right;
            if (Input.GetKey(KeyCode.W))
                moveDirection += transform.forward;
            if (Input.GetKey(KeyCode.S))
                moveDirection -= transform.forward;

            // 状態の更新
            if (moveDirection == Vector3.zero)
            {
                PlayerCurrentsState = PlayerState.Idle;
            }
            else if (_isDash)
            {
                PlayerCurrentsState = PlayerState.Run;
            }
            else
            {
                PlayerCurrentsState = PlayerState.Walk;
            }

            // ダッシュの速度計算
            float speed = _valueManager.MoveSpeed * (_isDash ? _dashMultiplier : 1f);

            // 壁との衝突に対応した移動計算
            moveDirection = moveDirection.normalized * speed;
            Vector3 adjustedVelocity = AdjustDirectionForCollisions(moveDirection);

            // Rigidbodyのvelocityを設定して移動
            _rb.velocity = new Vector3(adjustedVelocity.x, _rb.velocity.y, adjustedVelocity.z);
        }
       
    }

    /// <summary>
    /// 壁との衝突に応じて移動方向を調整する
    /// </summary>
    /// <param name="desiredDirection">入力された移動方向</param>
    /// <returns>調整された移動方向</returns>
    private Vector3 AdjustDirectionForCollisions(Vector3 desiredDirection)
    {
        RaycastHit hit;
        Vector3 adjustedDirection = desiredDirection;

        // 衝突判定: 壁との距離を短く設定
        if (Physics.Raycast(transform.position, desiredDirection.normalized, out hit, 0.5f))
        {
            // 壁の法線方向を取得
            Vector3 wallNormal = hit.normal;

            // 壁に沿う方向を計算
            Vector3 wallTangent = Vector3.Cross(wallNormal, Vector3.up).normalized;

            // 壁の方向に投影された移動方向を計算
            adjustedDirection = Vector3.Dot(desiredDirection, wallTangent) > 0
                ? wallTangent * desiredDirection.magnitude
                : -wallTangent * desiredDirection.magnitude;
        }

        return adjustedDirection;
    }



    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        _valueManager.Damage();
    //    }
    //}
}
