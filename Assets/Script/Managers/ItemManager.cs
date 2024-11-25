using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{

    // 攻撃アイテム関連
    [SerializeField] 
    private BoxCollider _weaponCollider = default; // 武器のコライダー

    // 鍵アイテム関連
    [SerializeField]
    private Item _completedKey = default;
    [SerializeField]
    private CashBox _cashBox = default;
    private const float WAIT_TIME = 0.5f;
    private const int MAX_KEY_COUNT = 3;
  
    // 懐中電灯関連
    private const int MAX_BATTERY_LIFE = 100; // バッテリー寿命の最大値
    [SerializeField]
    private Light[] _flashLight = default;
    [SerializeField] 
    private float _batteryLife = MAX_BATTERY_LIFE; // 現在のバッテリー寿命（秒）
    private int _lightInventorySlotNumber = default;
    [SerializeField] private InventoryUI _inventoryUI = default;
    [SerializeField] private CameraView _cameraView = default;

    // シングルトンパターン用のインスタンス
    public static ItemManager Instance { get; private set; }
    public bool LightOn { get; private set; } // 懐中電灯がオンかどうか

    public Light[] FlashLight { get => _flashLight; set=>_flashLight=value; }
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
                _cameraView.LightSwitch(); // バッテリーがあればライトをオン
            }
        }
        else
        {
            _cameraView.LightOff();// ライトをオフ
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

            _batteryLife -= Time.deltaTime; // バッテリーを減少させる

            // バッテリー残量をインベントリに更新
            _inventoryUI.UpdateBatteryText(_lightInventorySlotNumber, _batteryLife, MAX_BATTERY_LIFE);

            if (_batteryLife <= 0)
            {
                _cameraView.LightOff(); // バッテリーが切れたらライトをオフ
                LightOn = false;
            }
        }
    }
    /// <summary>
    /// 懐中電灯をどこのインベントリーでゲットしたか
    /// </summary>
    /// <param name="slotIndex"></param>
    public void GetLightInventoryNumber(int slotIndex)
    {
        _lightInventorySlotNumber = slotIndex;
        // 初期バッテリー残量をインベントリに反映
        _inventoryUI.UpdateBatteryText(_lightInventorySlotNumber, _batteryLife, MAX_BATTERY_LIFE);
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
    #region 鍵関連の処理
    /// <summary>
    /// 完成したカギをインベントリに格納する処理
    /// </summary>
    public void KeyInstantiate()
    {
        StartCoroutine(AddKey());
    }
    private IEnumerator AddKey()
    {
        yield return new WaitForSeconds(WAIT_TIME);
        InventoryManager.Instance.AddItem(_completedKey);
    }
    /// <summary>
    /// 鍵のかけらがあれば鍵を作る処理
    /// </summary>
    public void MakeKey()
    {
        if (InventoryManager.Instance.KeyCount < MAX_KEY_COUNT)
        {
            Debug.Log("素材が足りません");
        }
        else
        {
            KeyInstantiate();
        }

    }
    /// <summary>
    /// 金庫との距離が近かったら鍵で金庫をあける処理
    /// </summary>
    public void OpenKey()
    {
        _cashBox.OpenCashBox();
    }
    #endregion
}
