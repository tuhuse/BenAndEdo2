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
        _moveSpeed = 60f; // �ʏ�̑��x
        _jumpPower = 60f; // �ʏ�̃W�����v��

    }
    public override void PlayerMove(Rigidbody rb)
    {
        Vector3 moveDirection = Vector3.zero;

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
        rb.MovePosition(transform.position + moveDirection);

        // �W�����v����
        if (Input.GetKeyDown(KeyCode.Space)) // �X�y�[�X�L�[�������ꂽ��W�����v
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpPower, rb.velocity.z); // Y���ɃW�����v�͂�ݒ�
        }
    }
}
public class OneLegPlayerMoveManager: TwoLegPlayerMoveManager
{
    private void Start()
    {
        _moveSpeed = 50f; // �ʏ�̑��x
        _jumpPower = 50f; // �ʏ�̃W�����v��

    }
    public override void PlayerMove(Rigidbody rb)
    {
        Vector3 moveDirection = Vector3.zero;

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
        rb.MovePosition(transform.position + moveDirection);

        // �W�����v����
        if (Input.GetKeyDown(KeyCode.Space)) // �X�y�[�X�L�[�������ꂽ��W�����v
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpPower, rb.velocity.z); // Y���ɃW�����v�͂�ݒ�
        }
    }
}
public class BodyPlayerMoveManager : OneLegPlayerMoveManager
{
    private void Start()
    {
        _moveSpeed = 40f; // �ʏ�̑��x
        _jumpPower = 40f; // �ʏ�̃W�����v��

    }
    public override void PlayerMove(Rigidbody rb)
    {
        Vector3 moveDirection = Vector3.zero;

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
        rb.MovePosition(transform.position + moveDirection);

        // �W�����v����
        if (Input.GetKeyDown(KeyCode.Space)) // �X�y�[�X�L�[�������ꂽ��W�����v
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpPower, rb.velocity.z); // Y���ɃW�����v�͂�ݒ�
        }
    }
}
public class HeadPlayerMove : BodyPlayerMoveManager
{
    private void Start()
    {
        _moveSpeed = 30f; // �ʏ�̑��x
        _jumpPower = 30f; // �ʏ�̃W�����v��

    }
    public override void PlayerMove(Rigidbody rb)
    {
        Vector3 moveDirection = Vector3.zero;

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
        rb.MovePosition(transform.position + moveDirection);

        // �W�����v����
        if (Input.GetKeyDown(KeyCode.Space)) // �X�y�[�X�L�[�������ꂽ��W�����v
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpPower, rb.velocity.z); // Y���ɃW�����v�͂�ݒ�
        }
    }
}

