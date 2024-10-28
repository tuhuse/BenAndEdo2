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
        
    //        //// �J�����𓪂ɒǏ]������
    //        //_camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8);
    //        //_camera.transform.rotation = Quaternion.Euler(20, 0, 0);
    //        // ���͂Ɋ�Â��Ĉړ��������v�Z
    //        Vector3 moveDirection = Vector3.zero;

    //        if (Input.GetKey(KeyCode.D)) // �E�ړ�
    //        {
    //            moveDirection += transform.right * moveSpeed;
    //        }
    //        if (Input.GetKey(KeyCode.A)) // ���ړ�
    //        {
    //            moveDirection -= transform.right * moveSpeed;
    //        }
    //        if (Input.GetKey(KeyCode.W)) // �O�i
    //        {
    //            moveDirection += transform.forward * moveSpeed;
    //        }
    //        if (Input.GetKey(KeyCode.S)) // ���
    //        {
    //            moveDirection -= transform.forward * moveSpeed;
    //        }

    //        // �v�Z�����ړ������Ɋ�Â��ăv���C���[���ړ�
    //        moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;

    //        // Rigidbody���g���Ĉړ�������iMovePosition���g�p�j
    //        _rb.MovePosition(transform.position + moveDirection);

    //        // �W�����v����
    //        if (Input.GetKeyDown(KeyCode.Space)) // �X�y�[�X�L�[�������ꂽ��W�����v
    //        {
    //            _rb.velocity = new Vector3(_rb.velocity.x, jumpPower, _rb.velocity.z); // Y���ɃW�����v�͂�ݒ�
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
