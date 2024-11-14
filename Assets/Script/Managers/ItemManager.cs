using UnityEngine;
using System.Collections;
public class ItemManager : MonoBehaviour
{
    //懐中電灯
    public static ItemManager Instance { get; private set; }
    private const int MAX_BatteryLife = 60;
    [SerializeField] private Light _flashlight;
    [SerializeField] private float _batteryLife = 60f;  // バッテリー寿命（秒）
    public bool _lightOn { get; private set; }

    //攻撃アイテム
    [SerializeField] private BoxCollider _weapon;

    //鍵的な奴
    private int _keyCount = 0;
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

        if (_lightOn)
        {
            LightStart();

        }


    }
    #region 懐中電灯処理
    public void LightActive()
    {
        if (!_lightOn)
        {
            _lightOn = true;
            if (_batteryLife > 0)
            {
                _flashlight.enabled = true;
            }
        }
        else
        {
            _flashlight.enabled = false;
            _lightOn = false;
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
                _lightOn = false;


            }
        }
    }
    public void GetBattery()
    {
        if (_batteryLife < MAX_BatteryLife)
        {
            _batteryLife = MAX_BatteryLife;
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
    public void KeyCount()
    {
        if (_keyCount == 3)
        {
            _perfectKey = Instantiate(_unionKey, _perfectKey.transform.position, Quaternion.identity);
        }
    }
}
