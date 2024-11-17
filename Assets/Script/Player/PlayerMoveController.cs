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
        moveDirection = moveDirection.normalized * speed * Time.deltaTime;

        // Rigidbodyを使った移動
        _rb.MovePosition(transform.position + moveDirection);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _valueManager.Damage();
        }
    }
}
