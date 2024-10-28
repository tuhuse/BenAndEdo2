using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMoveManager : MonoBehaviour
{
    public float _moveSpeed;
    public float _jumpPower;
    public abstract void PlayerMove(Rigidbody rb);
}
public class TwoLegPlayerMoveManager : PlayerMoveManager {

    private void Start()
    {
        _moveSpeed = 60f; // 通常の速度
        _jumpPower = 60f; // 通常のジャンプ力

    }
    public override void PlayerMove(Rigidbody rb)
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
        rb.MovePosition(transform.position + moveDirection);

        // ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space)) // スペースキーが押されたらジャンプ
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpPower, rb.velocity.z); // Y軸にジャンプ力を設定
        }
    }
}
public class OneLegPlayerMoveManager: TwoLegPlayerMoveManager
{
    private void Start()
    {
        _moveSpeed = 50f; // 通常の速度
        _jumpPower = 50f; // 通常のジャンプ力

    }
    public override void PlayerMove(Rigidbody rb)
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
        rb.MovePosition(transform.position + moveDirection);

        // ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space)) // スペースキーが押されたらジャンプ
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpPower, rb.velocity.z); // Y軸にジャンプ力を設定
        }
    }
}
public class BodyPlayerMoveManager : OneLegPlayerMoveManager
{
    private void Start()
    {
        _moveSpeed = 40f; // 通常の速度
        _jumpPower = 40f; // 通常のジャンプ力

    }
    public override void PlayerMove(Rigidbody rb)
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
        rb.MovePosition(transform.position + moveDirection);

        // ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space)) // スペースキーが押されたらジャンプ
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpPower, rb.velocity.z); // Y軸にジャンプ力を設定
        }
    }
}
public class HeadPlayerMove : BodyPlayerMoveManager
{
    private void Start()
    {
        _moveSpeed = 30f; // 通常の速度
        _jumpPower = 30f; // 通常のジャンプ力

    }
    public override void PlayerMove(Rigidbody rb)
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
        rb.MovePosition(transform.position + moveDirection);

        // ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space)) // スペースキーが押されたらジャンプ
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpPower, rb.velocity.z); // Y軸にジャンプ力を設定
        }
    }
}

