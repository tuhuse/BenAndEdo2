using System.Collections;
using UnityEngine;

/// <summary>
/// キャラクターのステータスに関係している値の変化を管理している
/// </summary>
public class ValueManager : MonoBehaviour
{
    // シングルトンのインスタンス
    public static ValueManager Instance { get; private set; }

    // 定数定義
    private const float MAX_SPEED = 8f; // 最大移動速度
    private const float MAX_DASH_HP = 100f; // ダッシュ用のHPの最大値
    private const float STAMINA_HEAL = 10f; // ダッシュHPの回復量
    private const int MAX_PLAYER_HP = 3; // プレイヤーの最大HP
    private const int STAMINA_DECREASE_RATE = 20; // ダッシュ時のHP減少速度
    private const int STAMINA_RECOVERY_WAIT_TIME = 1; // ダッシュHP回復の間隔
    private const int STAMINA_RECOVERY_INITIAL_DELAY = 2; // ダッシュHP回復開始までの遅延
    private const int RETERN_SPEED = 3;
    private const float SPEED_DECREASE_AMOUNT = 4f; // ダメージ時の移動速度減少量

    // プレイヤーの現在の状態
    public float MoveSpeed { get; private set; } = MAX_SPEED; // 現在の移動速度
    public float DashHP { get; private set; } = MAX_DASH_HP; // 現在のダッシュHP
    public int PlayerHP { get; private set; } = MAX_PLAYER_HP; // 現在のプレイヤーHP

    private Coroutine _dashRecoveryCoroutine = default; // ダッシュHP回復のコルーチン管理用

    // シングルトンの設定
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン間で破棄されないようにする
        }
        else
        {
            Destroy(gameObject); // 重複するインスタンスを破棄
        }
    }

    /// <summary>
    /// 一定時間でスピードを戻す
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReturnSpeed()
    {
        MoveSpeed -= SPEED_DECREASE_AMOUNT; // 移動速度を減少
        yield return new WaitForSeconds(RETERN_SPEED); // 3秒後に元に戻す
        MoveSpeed = MAX_SPEED;
    }

    /// <summary>
    ///  ダッシュ時にHPを減少させる処理
    /// </summary>
    public void DashHPDecrease()
    {
        DashHP = Mathf.Max(0, DashHP - STAMINA_DECREASE_RATE * Time.deltaTime); // HPが0を下回らないようにする
    }

    /// <summary>
    /// ダッシュHPの回復処理を開始
    /// </summary>    
    public void StartDashRecovery()
    {
        if (_dashRecoveryCoroutine == null)
        {
            _dashRecoveryCoroutine = StartCoroutine(DashHealthRecovery());
        }
    }

    /// <summary>
    /// ダッシュHPの回復処理を停止
    /// </summary>
    public void StopDashRecovery()
    {
        if (_dashRecoveryCoroutine != null)
        {
            StopCoroutine(_dashRecoveryCoroutine);
            _dashRecoveryCoroutine = null;
        }
    }

    /// <summary>
    ///  ダッシュHPの回復処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator DashHealthRecovery()
    {
        yield return new WaitForSeconds(STAMINA_RECOVERY_INITIAL_DELAY); // 回復開始までの遅延

        while (DashHP < MAX_DASH_HP)
        {
            DashHP = Mathf.Min(DashHP + STAMINA_HEAL, MAX_DASH_HP); // HPを最大値まで回復
            yield return new WaitForSeconds(STAMINA_RECOVERY_WAIT_TIME); // 指定の間隔で回復
        }

        _dashRecoveryCoroutine = null; // 回復処理が完了したらコルーチンをリセット
    }

    /// <summary>
    ///  プレイヤーがダメージを受けた時の処理
    /// </summary>
    public void Damage()
    {
        PlayerHP--; // プレイヤーHPを減少
        StartCoroutine(ReturnSpeed()); // 一時的に移動速度を減少
    }

    /// <summary>
    ///  プレイヤーを回復させる処理
    /// </summary>
    public void Heal()
    {
        PlayerHP = Mathf.Min(PlayerHP + 1, MAX_PLAYER_HP); // HPを最大値まで回復
    }
}
