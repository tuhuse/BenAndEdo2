using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController: MonoBehaviour
{
 
    [SerializeField] private GameObject _camera; // �J�����I�u�W�F�N�g
    
    private float _moveSpeed 
    {
        get { return _valueManager._moveSpeed; } 
        set { _valueManager._moveSpeed = value; }
    }
    private float _dashHP 
    {
        get { return _valueManager._dashHP; }
        set { _valueManager._dashHP = value; }
    }

    private bool _isDash = false;

   [SerializeField] private ValueManager _valueManager;

    
    private Rigidbody _rb; // Rigidbody�̎Q��
    // ����������
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        PlayerMove();
    }
    // ���t���[���̍X�V����
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isDash = false;
            _valueManager.StartDashRecovery();
        }

        
    }
   

    private void PlayerMove()
    {
        Vector3 moveDirection = Vector3.zero;
        if (!_isDash)
        {
            if (Input.GetKey(KeyCode.D)) // �E�ړ�
            {
                moveDirection += transform.right * _moveSpeed;
            }
            if (Input.GetKey(KeyCode.A)) // ���ړ�
            {
                moveDirection -= transform.right * _moveSpeed;
            }
            if (Input.GetKey(KeyCode.W)) // �O�i
            {
                moveDirection += transform.forward * _moveSpeed;
            }
            if (Input.GetKey(KeyCode.S)) // ���
            {
                moveDirection -= transform.forward * _moveSpeed;
            }
            // �v�Z�����ړ������Ɋ�Â��ăv���C���[���ړ�
            moveDirection = moveDirection.normalized * _moveSpeed * Time.deltaTime;

            // Rigidbody���g���Ĉړ�������iMovePosition���g�p�j
            _rb.MovePosition(transform.position + moveDirection);
        }
        else
        {
            float dashspeed = 2f;
            if (Input.GetKey(KeyCode.D)) // �E�ړ�
            {
                moveDirection += transform.right * _moveSpeed*dashspeed;
            }
            if (Input.GetKey(KeyCode.A)) // ���ړ�
            {
                moveDirection -= transform.right * _moveSpeed * dashspeed;
            }
            if (Input.GetKey(KeyCode.W)) // �O�i
            {
                moveDirection += transform.forward * _moveSpeed * dashspeed;
            }
            if (Input.GetKey(KeyCode.S)) // ���
            {
                moveDirection -= transform.forward * _moveSpeed * dashspeed;
            }
            // �v�Z�����ړ������Ɋ�Â��ăv���C���[���ړ�
            moveDirection = moveDirection.normalized * _moveSpeed *dashspeed* Time.deltaTime;

            // Rigidbody���g���Ĉړ�������iMovePosition���g�p�j
            _rb.MovePosition(transform.position + moveDirection);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isDash = true;
            _valueManager.DashHPDecrease();
            if (_dashHP <= 0)
            {
                _dashHP = 0;
                _isDash = false;
            }
            _valueManager.StopDashRecovery();
        }
       

       

    }

    // �ՓˊJ�n���̏���
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _valueManager.Damage();
        }
    }

    // �Փ˒��̏���
    private void OnCollisionStay(Collision collision)
    {
       
    }

    // �Փ˂��I���������̏���
    private void OnCollisionExit(Collision collision)
    {
      
    }

  
    }

