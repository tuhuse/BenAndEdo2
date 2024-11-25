using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockingButton : MonoBehaviour
{
    [SerializeField] private CashBox _cashBox;
    [SerializeField] private CountDownTimer _countDownTimer;
    [SerializeField] private GameObject _goalDoor;
     private GameObject _southAndNorthGoalDoor;
    [SerializeField] private Transform[] _goalSpawn;
    private const int MAX_INDEX = 4;
    private const int MIN_INDEX = 0;
    private int _selectPosition = default;
    private bool _unDoor = true;

    void Update()
    {
        if (_cashBox.OpenDoor&&_unDoor)
        {
            if (Input.GetMouseButtonDown(1))
            {
                PositionSelect();
                _unDoor = false;
               _countDownTimer.StartTimer = true;
            }
        }
       
    }
    private void PositionSelect()
    {
        _selectPosition = Random.Range(MIN_INDEX, MAX_INDEX);
        switch(_selectPosition)
        {
            case 0:
                Instantiate(_goalDoor, _goalSpawn[_selectPosition].position, Quaternion.identity);
                break;
            case 1:
                Instantiate(_goalDoor, _goalSpawn[_selectPosition].position, Quaternion.identity);
                break;
            case 2:
                _southAndNorthGoalDoor= Instantiate(_goalDoor, _goalSpawn[_selectPosition].position, Quaternion.identity);
                _southAndNorthGoalDoor.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case 3:
                _southAndNorthGoalDoor= Instantiate(_goalDoor, _goalSpawn[_selectPosition].position, Quaternion.identity);
                _southAndNorthGoalDoor.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;

           
        }
    }
}
