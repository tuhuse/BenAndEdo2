using UnityEngine;
using System.Collections;
public class ItemManager : MonoBehaviour
{
    //懐中電灯
    public static ItemManager Instance { get; private set; }

    [SerializeField] private Inventory _inventory;
    private const int MAX_Battery_Life = 60;
    [SerializeField] private Light _flashlight;
    [SerializeField] private float _batteryLife = 60f;  // バッテリー寿命（秒）
    public bool LightOn { get; private set; }

    //攻撃アイテム
    [SerializeField] private BoxCollider _weapon;

    //鍵的な奴
    [SerializeField]
    private GameObject _perfectKey;
    [SerializeField]
    private GameObject _unionKey;
    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);  // インスタンスが重複している場合、破棄する
            return;
        }

    }
    void Start()
    {
        _flashlight.enabled = false;
    }

    void Update()
    {

        if (LightOn)
        {
            LightStart();

        }


    }
    #region 懐中電灯処理
    public void LightActive()
    {
        if (!LightOn)
        {
            LightOn = true;
            if (_batteryLife > 0)
            {
                _flashlight.enabled = true;
            }
        }
        else
        {
            _flashlight.enabled = false;
            LightOn = false;
        }
    }
    private void LightStart()
    {

        if (_batteryLife > 0)
        {

            if (!_flashlight.enabled)
            {
                _flashlight.enabled = true;

            }
            _batteryLife -= Time.deltaTime;  // バッテリー残量を減らす

            if (_batteryLife <= 0)
            {
                _flashlight.enabled = false;  // バッテリーがなくなったらオフ
                LightOn = false;


            }
        }
    }
    public void GetBattery()
    {
        if (_batteryLife < MAX_Battery_Life)
        {
            _batteryLife = MAX_Battery_Life;
        }
    }
    #endregion
    #region 攻撃アイテム処理
    private IEnumerator WeaponAttack()
    {
        _weapon.enabled = true;
        yield return new WaitForSeconds(1f);
        _weapon.enabled = false;
    }
    public void WeaponStart()
    {
        StartCoroutine(WeaponAttack());
    }
    #endregion
    public void KeyCountPlus()
    {
        if (_inventory.KeyCnt == 3)
        {
            _perfectKey = Instantiate(_unionKey, _perfectKey.transform.position, Quaternion.identity);
        }
    }
}
