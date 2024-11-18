using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour
{
    // シングルトンパターン用のインスタンス
    public static ItemManager Instance { get; private set; }

    // 攻撃アイテム関連
    [SerializeField] private BoxCollider _weaponCollider; // 武器のコライダー

    // 鍵アイテム関連
    [SerializeField]
    private GameObject _perfectKey; // 完成した鍵
    [SerializeField]
    private GameObject _unionKey; // 鍵のパーツ

    // 懐中電灯関連
    private const int MAX_BATTERY_LIFE = 60; // バッテリー寿命の最大値
    [SerializeField] private Light _flashLight; // 懐中電灯のライト
    [SerializeField] private float _batteryLife = 60f; // 現在のバッテリー寿命（秒）

    public bool LightOn { get; private set; } // 懐中電灯がオンかどうか
    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // インスタンスが重複している場合は破棄
            return;
        }
    }

    void Start()
    {
        // ゲーム開始時に懐中電灯をオフに設定
        _flashLight.enabled = false;
    }

    void Update()
    {
        // 懐中電灯がオンの場合、バッテリー消費処理を開始
        if (LightOn)
        {
            UpdateLight();
        }
    }

    #region 懐中電灯処理
    public void LightActive()
    {
        // 懐中電灯のオンオフ切り替え
        if (!LightOn)
        {
            LightOn = true;
            if (_batteryLife > 0)
            {
                _flashLight.enabled = true; // バッテリーがあればライトをオン
            }
        }
        else
        {
            _flashLight.enabled = false; // ライトをオフ
            LightOn = false;
        }
    }

    private void UpdateLight()
    {
        // 懐中電灯のバッテリー消費処理
        if (_batteryLife > 0)
        {
            if (!_flashLight.enabled)
            {
                _flashLight.enabled = true; // バッテリーがある場合ライトをオン
            }

            _batteryLife -= Time.deltaTime; // バッテリーを減少させる

            if (_batteryLife <= 0)
            {
                _flashLight.enabled = false; // バッテリーが切れたらライトをオフ
                LightOn = false;
            }
        }
    }

    public void GetBattery()
    {
        // バッテリーアイテムを取得してバッテリー寿命を回復
        if (_batteryLife < MAX_BATTERY_LIFE)
        {
            _batteryLife = MAX_BATTERY_LIFE;
        }
    }
    #endregion

    #region 攻撃アイテム処理
    private IEnumerator WeaponAttack()
    {
        // 武器攻撃の処理（一定時間コライダーを有効化）
        _weaponCollider.enabled = true; // 武器のコライダーを有効化
        yield return new WaitForSeconds(1f); // 1秒後に無効化
        _weaponCollider.enabled = false;
    }

    public void WeaponStart()
    {
        // 武器攻撃の開始
        StartCoroutine(WeaponAttack());
    }
    #endregion

    public void KeyCountPlus()
    {
        // 鍵パーツが揃った場合に完成した鍵を生成
        if (InventoryManager.Instance.KeyCount == 3)
        {
            _perfectKey = Instantiate(_unionKey, _perfectKey.transform.position, Quaternion.identity);
        }
    }
}
