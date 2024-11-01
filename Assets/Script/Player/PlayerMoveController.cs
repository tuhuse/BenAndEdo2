using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController: MonoBehaviour
{
 
    [SerializeField] private GameObject _camera; // カメラオブジェクト
    [SerializeField]private float _moveSpeed;
    private Rigidbody _rb; // Rigidbodyの参照
    // 初期化処理
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    // 毎フレームの更新処理
    void Update()
    {
        PlayerMove();
        CameraRote();
    }
    private void CameraRote()
    {
        Vector3 cameraRote = _camera.transform.forward;
        cameraRote.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(cameraRote);
        float roteSpeed = 25f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, roteSpeed * Time.deltaTime);
    }

    private void PlayerMove()
    {
        Vector3 moveDirection = Vector3.zero;

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

    // 衝突開始時の処理
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Yaiba"))
        {
    
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

