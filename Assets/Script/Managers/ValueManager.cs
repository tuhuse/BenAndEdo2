using System.Collections;
using UnityEngine;

public class ValueManager : MonoBehaviour
{
    // シングルトンのインスタンス
    public static ValueManager Instance { get; private set; }

    // 定数定義
    private const float MAX_SPEED = 10f;
    private const float MAX_DASHHP = 100f;
    private const float HEAL = 10f;
    private const int MAX_PLAYER_HP = 3;
    private const int DASH_HP_DECREASE_RATE = 20;
    private const int DASH_RECOVERY_WAIT_TIME = 1;
    private const int DASH_RECOVERY_INITIAL_DELAY = 2;
    private const float SPEED_DECREASE_AMOUNT = 4f;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ReturnSpeed()
    {
        MoveSpeed -= SPEED_DECREASE_AMOUNT;
        yield return new WaitForSeconds(3f);
        MoveSpeed = MAX_SPEED;
    }

    public void DashHPDecrease()
    {
        DashHP = Mathf.Max(0, DashHP - DASH_HP_DECREASE_RATE * Time.deltaTime);
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
        yield return new WaitForSeconds(DASH_RECOVERY_INITIAL_DELAY);
        while (DashHP < MAX_DASHHP)
        {
            DashHP = Mathf.Min(DashHP + HEAL, MAX_DASHHP);
            yield return new WaitForSeconds(DASH_RECOVERY_WAIT_TIME);
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
        PlayerHP = Mathf.Min(PlayerHP + 1, MAX_PLAYER_HP);
    }
}
