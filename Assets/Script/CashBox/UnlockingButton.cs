using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockingButton : MonoBehaviour
{
    [SerializeField] private CashBox _cashBox=default;
    private CountDownTimer _countDownTimer = default;
    private GameObjectGenerator _objectGenerator=default;
    private AudioManager _audioManager = default;
    private EnemyAI _enemyAI = default;
    private bool _unDoor = true;


    private void Start()
    {
        _objectGenerator = FindFirstObjectByType<GameObjectGenerator>();
        _countDownTimer = FindFirstObjectByType<CountDownTimer>();
        _audioManager = FindFirstObjectByType<AudioManager>();
        _enemyAI = FindFirstObjectByType<EnemyAI>();
       
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
                _audioManager.EscapeTrue();
                _enemyAI.EveryChaseON();
                MissionUI.Instance.Mission5();
            }
        }
       
    }
   
}
