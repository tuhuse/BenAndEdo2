using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour
{
 
    // 攻撃アイテム関連
    [SerializeField] private BoxCollider _weaponCollider; // 武器のコライダー

    // 鍵アイテム関連
    [SerializeField]
    private Item _completedKey;
    private const float WAIT_TIME =0.5f ;
    // 懐中電灯関連
    private const int MAX_BATTERY_LIFE = 60; // バッテリー寿命の最大値
    [SerializeField] private Light _flashLight; // 懐中電灯のライト
    [SerializeField] private float _batteryLife = 60f; // 現在のバッテリー寿命（秒）

    // シングルトンパターン用のインスタンス
    public static ItemManager Instance { get; private set; }
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
    /// <summary>
    /// 懐中電灯のオンオフ切り替え
    /// </summary>
    public void LightActive()
    {
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
    /// <summary>
    /// 懐中電灯のバッテリー消費処理
    /// </summary>
    private void UpdateLight()
    {
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
    /// <summary>
    /// 電池を使用したときバッテリー回復
    /// </summary>
    public void GetBattery()
    {

        if (_batteryLife < MAX_BATTERY_LIFE)
        {
            _batteryLife = MAX_BATTERY_LIFE;
        }
    }
    #endregion

    #region 攻撃アイテム処理
    /// <summary>
    /// 攻撃処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator WeaponAttack()
    {
        // 武器攻撃の処理（一定時間コライダーを有効化）
        _weaponCollider.enabled = true; // 武器のコライダーを有効化
        yield return new WaitForSeconds(1f); // 1秒後に無効化
        _weaponCollider.enabled = false;
    }
    /// <summary>
    /// 攻撃合図
    /// </summary>
    public void WeaponStart()
    {
        // 武器攻撃の開始
        StartCoroutine(WeaponAttack());
    }
    #endregion

    public void KeyInstantiate()
    {
        StartCoroutine(AddKey());
    }
    private IEnumerator AddKey()
    {
        yield return new WaitForSeconds(WAIT_TIME);
        InventoryManager.Instance.AddItem(_completedKey);
    }
    public void MakeKey()
    {
        if (InventoryManager.Instance.KeyCount < 3)
        {
            Debug.Log("素材が足りません");
        }
        else
        {
            KeyInstantiate();
        }
    }
}
