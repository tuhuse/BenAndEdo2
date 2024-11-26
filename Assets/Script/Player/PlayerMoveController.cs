using System.Collections;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [SerializeField] private float _dashMultiplier = 2f;
    private bool _isDash = false;
    private ValueManager _valueManager;
    private Rigidbody _rb;

    public enum PlayerStatus
    {
        Idle,
        Walk,
        Run
    }
    public PlayerStatus _playerStatus { get; private set; }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _valueManager = ValueManager.Instance;
        _playerStatus = PlayerStatus.Idle;
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    void Update()
    {
        HandleDashInput();
    }

    private void HandleDashInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _valueManager.DashHP > 0)
        {
            _isDash = true;
            _valueManager.DashHPDecrease();
            _valueManager.StopDashRecovery();
        }
        else
        {
            _isDash = false;
            _valueManager.StartDashRecovery();
        }
    }

    private void PlayerMove()
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
            _playerStatus = PlayerStatus.Idle;
        }
        else if (_isDash)
        {
            _playerStatus = PlayerStatus.Run;
        }
        else
        {
            _playerStatus = PlayerStatus.Walk;
        }

        // ダッシュの速度計算
        float speed = _valueManager.MoveSpeed * (_isDash ? _dashMultiplier : 1f);

        // 壁との衝突に対応した移動計算
        moveDirection = moveDirection.normalized * speed;
        Vector3 adjustedVelocity = AdjustDirectionForCollisions(moveDirection);

        // Rigidbodyのvelocityを設定して移動
        _rb.velocity = new Vector3(adjustedVelocity.x, _rb.velocity.y, adjustedVelocity.z);
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

        // 衝突判定
        if (Physics.Raycast(transform.position, desiredDirection, out hit, 0.5f))
        {
            // 壁にぶつかった場合、壁の法線方向を除外して滑るようにする
            Vector3 wallNormal = hit.normal;
            adjustedDirection = Vector3.ProjectOnPlane(desiredDirection, wallNormal);
        }

        return adjustedDirection;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _valueManager.Damage();
        }
    }
}
