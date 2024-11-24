using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    [SerializeField] private GameObject _cameraPosi;
    private Camera _camera;
    [SerializeField] private float _mouseSensitivity = 100f; // 適切なマウス感度に設定
    [SerializeField] private Vector3 tpsOffset = new Vector3(0, 3, -3); // TPS視点のオフセット
    [SerializeField] private float minVerticalAngle = -30f; // 下方向の角度制限
    [SerializeField] private float maxVerticalAngle = 60f;  // 上方向の角度制限
    private bool _isFPS = true;
    private bool _isForward = false;
    private float verticalRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // カーソルをロック
        _camera = _cameraPosi.GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        // X軸のマウス入力を取得し、プレイヤーを左右回転
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
        // Y軸のマウス入力を取得してカメラを上下に回転
        float yMouse = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        if (Mathf.Abs(yMouse) > 0)
        {
            // 縦回転の入力を現在の角度に加算し、角度制限を適用
            verticalRotation -= yMouse;
            verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

            // カメラの回転を制限された角度に更新
            _cameraPosi.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
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
