using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraView : MonoBehaviour
{ 

    [SerializeField] CinemachineFreeLook cinemachineCamera; // シネマシーンカメラの参照
    [SerializeField]private float mouseSensitivity = 100f; // マウス感度

    private void Start()
    {
        // カーソルをロックして画面中央に固定
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // マウスのX軸とY軸の入力を取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // カメラの回転を更新
        float xRotation = cinemachineCamera.m_Lens.FieldOfView; // カメラのFOVを取得
        xRotation -= mouseY; // Y軸で上下に動かす
        xRotation = Mathf.Clamp(xRotation, 10f, 80f); // 回転角度を制限
        cinemachineCamera.m_Lens.FieldOfView = xRotation;

        // カメラを左右に回転
        cinemachineCamera.transform.Rotate(Vector3.up * mouseX);
    }
}


