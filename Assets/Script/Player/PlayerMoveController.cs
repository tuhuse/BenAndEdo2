using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController: MonoBehaviour
{
 
    [SerializeField] private GameObject _camera; // �J�����I�u�W�F�N�g
    [SerializeField]private float _moveSpeed;
    private Rigidbody _rb; // Rigidbody�̎Q��
    // ����������
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    // ���t���[���̍X�V����
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

    // �ՓˊJ�n���̏���
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Yaiba"))
        {
    
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

