using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraView : MonoBehaviour
{ 

    [SerializeField] CinemachineFreeLook cinemachineCamera; // �V�l�}�V�[���J�����̎Q��
    [SerializeField]private float mouseSensitivity = 100f; // �}�E�X���x

    private void Start()
    {
        // �J�[�\�������b�N���ĉ�ʒ����ɌŒ�
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // �}�E�X��X����Y���̓��͂��擾
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // �J�����̉�]���X�V
        float xRotation = cinemachineCamera.m_Lens.FieldOfView; // �J������FOV���擾
        xRotation -= mouseY; // Y���ŏ㉺�ɓ�����
        xRotation = Mathf.Clamp(xRotation, 10f, 80f); // ��]�p�x�𐧌�
        cinemachineCamera.m_Lens.FieldOfView = xRotation;

        // �J���������E�ɉ�]
        cinemachineCamera.transform.Rotate(Vector3.up * mouseX);
    }
}


