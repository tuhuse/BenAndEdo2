using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpawnPrefabsData", menuName = "Spawn/SpawnPrefabsData")]
public class SpawnPrefabsData : ScriptableObject
{

    [SerializeField] private GameObject _itemPrefab = default;       // �A�C�e����Prefab
    [SerializeField] private Transform[] _spawnPoints = default;    // �X�|�[���|�C���g�z��

    public enum PrefabName
    {
        Player,
        Heal,
        Weapon,
        KeyPiece,
        FlashLight,
        LightBattery,
        CashBox,
        EscapeDoor
    }
    [SerializeField] private PrefabName _prefabName = default;
    public PrefabName Name => _prefabName;
    public Transform[] SpawnPoints => _spawnPoints;
    public GameObject ItemPrefab => _itemPrefab;
}
