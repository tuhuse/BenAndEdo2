using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController: MonoBehaviour
{
 
    [SerializeField] private GameObject _camera; // カメラオブジェクト
    
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

    
    private Rigidbody _rb; // Rigidbodyの参照
    // 初期化処理
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        PlayerMove();
    }
    // 毎フレームの更新処理
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
            if (Input.GetKey(KeyCode.D)) // 右移動
            {
                moveDirection += transform.right * _moveSpeed;
            }
            if (Input.GetKey(KeyCode.A)) // 左移動
            {
                moveDirection -= transform.right * _moveSpeed;
            }
            if (Input.GetKey(KeyCode.W)) // 前進
            {
                moveDirection += transform.forward * _moveSpeed;
            }
            if (Input.GetKey(KeyCode.S)) // 後退
            {
                moveDirection -= transform.forward * _moveSpeed;
            }
            // 計算した移動方向に基づいてプレイヤーを移動
            moveDirection = moveDirection.normalized * _moveSpeed * Time.deltaTime;

            // Rigidbodyを使って移動させる（MovePositionを使用）
            _rb.MovePosition(transform.position + moveDirection);
        }
        else
        {
            float dashspeed = 2f;
            if (Input.GetKey(KeyCode.D)) // 右移動
            {
                moveDirection += transform.right * _moveSpeed*dashspeed;
            }
            if (Input.GetKey(KeyCode.A)) // 左移動
            {
                moveDirection -= transform.right * _moveSpeed * dashspeed;
            }
            if (Input.GetKey(KeyCode.W)) // 前進
            {
                moveDirection += transform.forward * _moveSpeed * dashspeed;
            }
            if (Input.GetKey(KeyCode.S)) // 後退
            {
                moveDirection -= transform.forward * _moveSpeed * dashspeed;
            }
            // 計算した移動方向に基づいてプレイヤーを移動
            moveDirection = moveDirection.normalized * _moveSpeed *dashspeed* Time.deltaTime;

            // Rigidbodyを使って移動させる（MovePositionを使用）
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

    // 衝突開始時の処理
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _valueManager.Damage();
        }
    }

    // 衝突中の処理
    private void OnCollisionStay(Collision collision)
    {
       
    }

    // 衝突が終了した時の処理
    private void OnCollisionExit(Collision collision)
    {
      
    }

  
    }

