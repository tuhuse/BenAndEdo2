using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    [SerializeField] private GameObject _cameraPosi;
    private Camera _camera;
    [SerializeField] private float _mouseSensitivity = 100f; // �K�؂ȃ}�E�X���x�ɐݒ�
    [SerializeField] private Vector3 tpsOffset = new Vector3(0, 3, -3); // TPS���_�̃I�t�Z�b�g
    [SerializeField] private float minVerticalAngle = -30f; // �������̊p�x����
    [SerializeField] private float maxVerticalAngle = 60f;  // ������̊p�x����
    private bool _isFPS = true;
    private bool _isForward = false;
    private float verticalRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // �J�[�\�������b�N
        _camera = _cameraPosi.GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        // X���̃}�E�X���͂��擾���A�v���C���[�����E��]
        float xMouse = Input.GetAxis("Mouse X") * _mouseSensitivity*Time.deltaTime;
        if (Mathf.Abs(xMouse) > 0)
        {
            transform.RotateAround(transform.position, Vector3.up, xMouse);
        }

        if (_isForward)
        {
            Forward();
        }
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
        // Y���̃}�E�X���͂��擾���ăJ�������㉺�ɉ�]
        float yMouse = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        if (Mathf.Abs(yMouse) > 0)
        {
            // �c��]�̓��͂����݂̊p�x�ɉ��Z���A�p�x������K�p
            verticalRotation -= yMouse;
            verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

            // �J�����̉�]�𐧌����ꂽ�p�x�ɍX�V
            _cameraPosi.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
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
    public void LightSwitch()
    {
        if (_isFPS)
        {
            ItemManager.Instance.FlashLight[0].enabled = true;
            ItemManager.Instance.FlashLight[1].enabled = false;
        }
        else
        {
            ItemManager.Instance.FlashLight[0].enabled = false;
            ItemManager.Instance.FlashLight[1].enabled = true;
        }
    }
    public void LightOff()
    {
        ItemManager.Instance.FlashLight[0].enabled = false;
        ItemManager.Instance.FlashLight[1].enabled = false;
    }
}
