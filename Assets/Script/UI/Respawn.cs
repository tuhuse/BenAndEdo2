using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject _camera; // カメラオブジェクト
    [SerializeField] private Transform _legRespawnPosition; // 脚のリスポーン位置
    [SerializeField] private Transform _headRespawnPosition; // 頭のリスポーン位置
    [SerializeField] private Transform _bodyRespawnPosition; // 体のリスポーン位置
    [SerializeField] private GameObject _leg; // 脚オブジェクト
    [SerializeField] private GameObject _head; // 頭オブジェクト
    [SerializeField] private GameObject _body; // 体オブジェクト

    [SerializeField] private CameraManager _cameraManager;
    // 衝突時の処理
    private void OnCollisionEnter(Collision collision)
    {
        string player = "Player"; // プレイヤーを識別するタグ

        // 衝突したオブジェクトがプレイヤーの場合
        if (collision.gameObject.CompareTag(player))
        {
            // コルーチンを開始して融合処理を行う
            StartCoroutine(Fusion());
            // 頭が生存していない状態に設定
            _head.GetComponent<HeadController>()._isHeadAlive = false;

        }
    }

    // 融合処理を行うコルーチン
    private IEnumerator Fusion()
    {
        float waittime = 1f; // 各ステップでの待機時間
        yield return new WaitForSeconds(2f);

        // 体を"外れている"状態に設定
        _body.GetComponent<BodyController>()._isUnBody = true;

        // 脚をリスポーン位置に移動
        _leg.transform.position = _legRespawnPosition.position;


        // 脚が復活した後にカメラを脚の位置に移動
        _cameraManager.SwitchBody(1);
        _camera.transform.rotation = Quaternion.Euler(20, 0, 0);

        // 1秒間待機
        yield return new WaitForSeconds(waittime);

        // 体をリスポーン位置に移動
        _body.transform.position = _bodyRespawnPosition.position;

        // 再び1秒間待機
        yield return new WaitForSeconds(waittime);

        // 頭をリスポーン位置に移動
        _head.transform.position = _headRespawnPosition.position;

        // 脚をリスポーンさせる処理を実行
        _leg.GetComponent<LegController>().RespawnWait();
    }

}
