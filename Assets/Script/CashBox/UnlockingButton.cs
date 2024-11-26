using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockingButton : MonoBehaviour
{
    [SerializeField] private CashBox _cashBox;
    private CountDownTimer _countDownTimer;
    private GameObjectGenerator _objectGenerator;
    private bool _unDoor = true;


    private void Start()
    {
        _objectGenerator = FindFirstObjectByType<GameObjectGenerator>();
        _countDownTimer = FindFirstObjectByType<CountDownTimer>();
    }
    void Update()
    {
        if (_cashBox.OpenDoor&&_unDoor)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _objectGenerator.PositionSelect();
                _unDoor = false;
               _countDownTimer.StartTimer = true;
            }
        }
       
    }
   
}
