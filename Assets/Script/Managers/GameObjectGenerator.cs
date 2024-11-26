using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectGenerator : MonoBehaviour
{
    [SerializeField] private List<SpawnPrefabsData> _spawnItemDataList; // �S�A�C�e���f�[�^�����X�g�ŊǗ�
    [SerializeField] private SpawnPrefabsData _playerData;              // �v���C���[�f�[�^�i�ʊǗ��j
    [SerializeField] private SpawnPrefabsData _flashlightData;          // �����d���f�[�^�i�ʊǗ��j
    [SerializeField] private SpawnPrefabsData _cashBoxData;          // ���Ƀf�[�^�i�ʊǗ��j
    [SerializeField] private SpawnPrefabsData _escapeDoorData;          // �E�o�h�A�f�[�^�i�ʊǗ��j
    private UnlockingButton _unlockingButton = default;
    private GameObject _southAndNorthGoalDoor;
    private const int MAX_INDEX = 4;
    private const int MIN_INDEX = 0;
    private int _selectPosition = default;
    // Start is called before the first frame update
    void OnEnable()
    {
       
        // �v���C���[�Ɖ����d���̏�������
        InstantiateItem(_playerData);
        InstantiateItem(_flashlightData);
        InstantiateItem(_cashBoxData);
        //// �e�A�C�e���̃X�|�[������
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
    // �w�肵���A�C�e����S�X�|�[���|�C���g�ɐ���
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

    // �P��̃A�C�e������
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