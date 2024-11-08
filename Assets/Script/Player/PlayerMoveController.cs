using System.Collections;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [SerializeField] private float dashMultiplier = 2f; 
    private bool _isDash = false;
    private ValueManager _valueManager;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _valueManager = ValueManager.Instance;
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
        else if (Input.GetKeyUp(KeyCode.LeftShift) || _valueManager.DashHP <= 0)
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

        // ダッシュ
        float speed = _valueManager.MoveSpeed * (_isDash ? dashMultiplier : 1f);
        moveDirection = moveDirection.normalized * speed * Time.deltaTime;

        // Rigidbodyを使って移動させる（MovePositionを使用）
        _rb.MovePosition(transform.position + moveDirection);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _valueManager.Damage();
            print(_valueManager.PlayerHP);
        }
    }
}
