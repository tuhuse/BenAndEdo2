using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{

    public float _moveSpeed;
    public float _jumpPower;
    public virtual void Speed()
    {

    }
    //public abstract void PlayerMove(float moveSpeed, float jumpPower)
    //{
        
    //        //// カメラを頭に追従させる
    //        //_camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8);
    //        //_camera.transform.rotation = Quaternion.Euler(20, 0, 0);
    //        // 入力に基づいて移動方向を計算
    //        Vector3 moveDirection = Vector3.zero;

    //        if (Input.GetKey(KeyCode.D)) // 右移動
    //        {
    //            moveDirection += transform.right * moveSpeed;
    //        }
    //        if (Input.GetKey(KeyCode.A)) // 左移動
    //        {
    //            moveDirection -= transform.right * moveSpeed;
    //        }
    //        if (Input.GetKey(KeyCode.W)) // 前進
    //        {
    //            moveDirection += transform.forward * moveSpeed;
    //        }
    //        if (Input.GetKey(KeyCode.S)) // 後退
    //        {
    //            moveDirection -= transform.forward * moveSpeed;
    //        }

    //        // 計算した移動方向に基づいてプレイヤーを移動
    //        moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;

    //        // Rigidbodyを使って移動させる（MovePositionを使用）
    //        _rb.MovePosition(transform.position + moveDirection);

    //        // ジャンプ処理
    //        if (Input.GetKeyDown(KeyCode.Space)) // スペースキーが押されたらジャンプ
    //        {
    //            _rb.velocity = new Vector3(_rb.velocity.x, jumpPower, _rb.velocity.z); // Y軸にジャンプ力を設定
    //        }
        


        

    //}
}

public class OneLleg : MoveManager {
    public override void Speed()
    {
        _moveSpeed = 50f;
        _jumpPower = 50f;
    }
    
}
public class Body : OneLleg
{
    public override void Speed()
    {
        _moveSpeed = 40f;
        _jumpPower = 40f;
    }
}public class Head : Body
{
    public override void Speed()
    {
        _moveSpeed = 30f;
        _jumpPower = 30f;
    }
}public class ReturnBody : Body
{
    public override void Speed()
    {
        base.Speed();
    }
}public class ReturnOneLeg : OneLleg
{
    public override void Speed()
    {
        base.Speed();
    }
}
