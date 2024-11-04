using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraView : MonoBehaviour
{
    [SerializeField] private GameObject _cameraPosi;
    private Camera _camera;
    [SerializeField] private float _mouseSensitivity = 100f; // マウス感度
    [SerializeField] private Vector3 tpsOffset = new Vector3(0, 2, -4); // TPS視点のオフセット（後方に配置）

    private bool _isFPS = false;
    private bool _isForward = false;

    private void Start()
    {
        // カーソルをロックして画面中央に固定
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
            // FPS視点に切り替え
            _cameraPosi.transform.position = transform.position + new Vector3(0, 1.5f, 0);
            _cameraPosi.transform.rotation = transform.rotation; // プレイヤーの回転に合わせる
            _camera.cullingMask &= ~(1 << playerLayer); // レイヤーを非表示
            _isForward = true;
            _isFPS = false;
        }
        else
        {
            // TPS視点に切り替え（プレイヤーの後方に配置）
            Vector3 tpsPosition = transform.position + transform.forward * tpsOffset.z + Vector3.up * tpsOffset.y;
            _cameraPosi.transform.position = tpsPosition;

            // カメラの回転を25度上向きに固定して、キャラクターの後方を向く
            _cameraPosi.transform.rotation = Quaternion.Euler(25, transform.eulerAngles.y, 0);
            _camera.cullingMask |= (1 << playerLayer); // レイヤーを表示
            _isForward = false;
            _isFPS = true;
        }
    }
}
