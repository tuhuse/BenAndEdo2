using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraView : MonoBehaviour
{
    [SerializeField] private GameObject _cameraPosi;
    private Camera _camera;
    [SerializeField] private float _mouseSensitivity = 100f; // �}�E�X���x
    [SerializeField] private Vector3 tpsOffset = new Vector3(0, 2, -4); // TPS���_�̃I�t�Z�b�g�i����ɔz�u�j

    private bool _isFPS = false;
    private bool _isForward = false;

    private void Start()
    {
        // �J�[�\�������b�N���ĉ�ʒ����ɌŒ�
        Cursor.lockState = CursorLockMode.Locked;
        _camera = _cameraPosi.GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        float xMouse = Input.GetAxis("Mouse X") * _mouseSensitivity;
        if (Mathf.Abs(xMouse) > 0)
        {
            transform.RotateAround(transform.position, Vector3.up, xMouse);
        }
        //if (_isForward)
        //{
        //    Forward();
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SwitchCamera();
        }
        
    }
    private void Forward()
    {
        if (Input.GetMouseButton(1))
        {
            float yMouse = Input.GetAxis("Mouse Y") * _mouseSensitivity;
            if (Mathf.Abs(yMouse) > 0)
            {
                _cameraPosi.transform.RotateAround(transform.position, Vector3.right, yMouse);
            }
        }

    }
    private void SwitchCamera()
    {
        int playerLayer = 3;

        if (_isFPS)
        {
            // FPS���_�ɐ؂�ւ�
            _cameraPosi.transform.position = transform.position + new Vector3(0, 1.5f, 0);
            _cameraPosi.transform.rotation = transform.rotation; // �v���C���[�̉�]�ɍ��킹��
            _camera.cullingMask &= ~(1 << playerLayer); // ���C���[���\��
            _isForward = true;
            _isFPS = false;
        }
        else
        {
            // TPS���_�ɐ؂�ւ��i�v���C���[�̌���ɔz�u�j
            Vector3 tpsPosition = transform.position + transform.forward * tpsOffset.z + Vector3.up * tpsOffset.y;
            _cameraPosi.transform.position = tpsPosition;

            // �J�����̉�]��25�x������ɌŒ肵�āA�L�����N�^�[�̌��������
            _cameraPosi.transform.rotation = Quaternion.Euler(25, transform.eulerAngles.y, 0);
            _camera.cullingMask |= (1 << playerLayer); // ���C���[��\��
            _isForward = false;
            _isFPS = true;
        }
    }
}
