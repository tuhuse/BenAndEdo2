using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectGenerator : MonoBehaviour
{
    [SerializeField] private List<SpawnPrefabsData> _spawnItemDataList; // 全アイテムデータをリストで管理
    [SerializeField] private SpawnPrefabsData _playerData;              // プレイヤーデータ（個別管理）
    [SerializeField] private SpawnPrefabsData _flashlightData;          // 懐中電灯データ（個別管理）
    [SerializeField] private SpawnPrefabsData _cashBoxData;          // 金庫データ（個別管理）
    [SerializeField] private SpawnPrefabsData _escapeDoorData;          // 脱出ドアデータ（個別管理）
    private UnlockingButton _unlockingButton = default;
    private GameObject _southAndNorthGoalDoor;
    private const int MAX_INDEX = 4;
    private const int MIN_INDEX = 0;
    private int _selectPosition = default;
    // Start is called before the first frame update
    void OnEnable()
    {
       
        // プレイヤーと懐中電灯の初期生成
        InstantiateItem(_playerData);
        InstantiateItem(_flashlightData);
        InstantiateItem(_cashBoxData);
        //// 各アイテムのスポーン処理
        foreach (var spawnData in _spawnItemDataList)
        {
            SpawnItems(spawnData);
        }
        _unlockingButton = FindFirstObjectByType<UnlockingButton>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // 指定したアイテムを全スポーンポイントに生成
    private void SpawnItems(SpawnPrefabsData spawnData)
    {
        if (spawnData.Name != SpawnPrefabsData.PrefabName.KeyPiece)
        {
            foreach (var spawnPoint in spawnData.SpawnPoints)
            {
                Instantiate(spawnData.ItemPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
        else
        {

        }
        
    }

    // 単一のアイテム生成
    private void InstantiateItem(SpawnPrefabsData spawnData)
    {
        if (spawnData.ItemPrefab == null)
        {
            Debug.LogError("ItemPrefab is not assigned in " + spawnData);
            return;
        }

        if (spawnData.SpawnPoints == null)
        {
            Debug.LogError("SpawnPoints are not assigned or empty in " + spawnData);
            return;
        }

        Instantiate(spawnData.ItemPrefab, spawnData.SpawnPoints[0].position, spawnData.SpawnPoints[0].rotation);
    }
    public void PositionSelect()
    {
        _selectPosition = Random.Range(MIN_INDEX, MAX_INDEX);
        switch (_selectPosition)
        {
            case 0:
                Instantiate(_escapeDoorData.ItemPrefab, _escapeDoorData.SpawnPoints[_selectPosition].position, Quaternion.identity);
                break;
            case 1:
                Instantiate(_escapeDoorData.ItemPrefab, _escapeDoorData.SpawnPoints[_selectPosition].position, Quaternion.identity);
                break;
            case 2:
               _southAndNorthGoalDoor= Instantiate(_escapeDoorData.ItemPrefab, _escapeDoorData.SpawnPoints[_selectPosition].position, Quaternion.identity);
                _southAndNorthGoalDoor.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case 3:
                _southAndNorthGoalDoor = Instantiate(_escapeDoorData.ItemPrefab, _escapeDoorData.SpawnPoints[_selectPosition].position, Quaternion.identity);
                _southAndNorthGoalDoor.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;


        }
    }
}
