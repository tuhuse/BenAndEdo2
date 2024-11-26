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

        // �ړ����͂̏���
        if (Input.GetKey(KeyCode.D))
            moveDirection += transform.right;
        if (Input.GetKey(KeyCode.A))
            moveDirection -= transform.right;
        if (Input.GetKey(KeyCode.W))
            moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S))
            moveDirection -= transform.forward;

        // ��Ԃ̍X�V
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

        // �_�b�V���̑��x�v�Z
        float speed = _valueManager.MoveSpeed * (_isDash ? _dashMultiplier : 1f);

        // �ǂƂ̏Փ˂ɑΉ������ړ��v�Z
        moveDirection = moveDirection.normalized * speed;
        Vector3 adjustedVelocity = AdjustDirectionForCollisions(moveDirection);

        // Rigidbody��velocity��ݒ肵�Ĉړ�
        _rb.velocity = new Vector3(adjustedVelocity.x, _rb.velocity.y, adjustedVelocity.z);
    }

    /// <summary>
    /// �ǂƂ̏Փ˂ɉ����Ĉړ������𒲐�����
    /// </summary>
    /// <param name="desiredDirection">���͂��ꂽ�ړ�����</param>
    /// <returns>�������ꂽ�ړ�����</returns>
    private Vector3 AdjustDirectionForCollisions(Vector3 desiredDirection)
    {
        RaycastHit hit;
        Vector3 adjustedDirection = desiredDirection;

        // �Փ˔���
        if (Physics.Raycast(transform.position, desiredDirection, out hit, 0.5f))
        {
            // �ǂɂԂ������ꍇ�A�ǂ̖@�����������O���Ċ���悤�ɂ���
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
