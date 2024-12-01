using System.Collections;
using UnityEngine;

/// <summary>
/// �v���C���[�̋���
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

            // �_�b�V���̑��x�v�Z
            float speed = _valueManager.MoveSpeed * (_isDash ? _dashMultiplier : 1f);

            // �ǂƂ̏Փ˂ɑΉ������ړ��v�Z
            moveDirection = moveDirection.normalized * speed;
            Vector3 adjustedVelocity = AdjustDirectionForCollisions(moveDirection);

            // Rigidbody��velocity��ݒ肵�Ĉړ�
            _rb.velocity = new Vector3(adjustedVelocity.x, _rb.velocity.y, adjustedVelocity.z);
        }
       
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

        // �Փ˔���: �ǂƂ̋�����Z���ݒ�
        if (Physics.Raycast(transform.position, desiredDirection.normalized, out hit, 0.5f))
        {
            // �ǂ̖@���������擾
            Vector3 wallNormal = hit.normal;

            // �ǂɉ����������v�Z
            Vector3 wallTangent = Vector3.Cross(wallNormal, Vector3.up).normalized;

            // �ǂ̕����ɓ��e���ꂽ�ړ��������v�Z
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
