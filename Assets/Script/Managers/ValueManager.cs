using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour
{
    // シングルトンのインスタンス
    public static ValueManager Instance { get; private set; }

    private const float MAX_SPEED = 10f;
    private const float MAX_DASHHP = 100f;
    private const float HEAL = 10;
    private const int MAX_PLAYER_HP = 3;

    public float MoveSpeed { get; private set; } = MAX_SPEED;
    public float DashHP { get; private set; } = MAX_DASHHP;
    public int PlayerHP { get; private set; } = MAX_PLAYER_HP;

    private Coroutine _dashRecoveryCoroutine;


    // シングルトンの設定
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
       
    }

    private IEnumerator ReturnSpeed()
    {
        MoveSpeed -= 4;
        yield return new WaitForSeconds(3f);
        MoveSpeed = MAX_SPEED;
    }

    public void DashHPDecrease()
    {
        DashHP = Mathf.Max(0, DashHP - 20*Time.deltaTime);
        Debug.Log(DashHP);
    }

    public void StartDashRecovery()
    {
        if (_dashRecoveryCoroutine == null)
        {
            _dashRecoveryCoroutine = StartCoroutine(DashHealthRecovery());
        }
    }

    public void StopDashRecovery()
    {
        if (_dashRecoveryCoroutine != null)
        {
            StopCoroutine(_dashRecoveryCoroutine);
            _dashRecoveryCoroutine = null;
        }
    }

    private IEnumerator DashHealthRecovery()
    {
        int waitTime = 1;
        int firstwaitTime = 2;
        yield return new WaitForSeconds(firstwaitTime);
        while (DashHP < MAX_DASHHP)
        {
            DashHP += HEAL;
            DashHP = Mathf.Min(DashHP, MAX_DASHHP);

            yield return new WaitForSeconds(waitTime);
        }
        _dashRecoveryCoroutine = null;
    }

    public void Damage()
    {
        PlayerHP--;
        StartCoroutine(ReturnSpeed());

    }

    public void Heal()
    {

        if (PlayerHP < MAX_PLAYER_HP)
        {
            PlayerHP++;

        }

    }
}
