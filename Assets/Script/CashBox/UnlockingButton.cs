using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockingButton : MonoBehaviour
{
    [SerializeField] private CashBox _cashBox;
    [SerializeField] private GameObject _goalDoor;
    [SerializeField] private Transform[] _goalSpawn;
    private const int MAX_INDEX = 4;
    private const int MIN_INDEX = 0;

    public bool StartTimer { get; set; }

    void Update()
    {
        if (_cashBox.OpenDoor)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Instantiate(_goalDoor, _goalSpawn[Random.Range(MIN_INDEX, MAX_INDEX)].position, Quaternion.identity);

            }
        }
       
    }
}
