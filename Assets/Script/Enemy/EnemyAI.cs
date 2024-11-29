using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵AIクラス
/// プレイヤーを検知し、Waypointsを巡回または追跡を行う
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints; // Waypoints（巡回地点の配列）
    [SerializeField] private float _detectionRange = 20f; // プレイヤーを検知する範囲（単位：メートル）
    [SerializeField] private float _waypointStoppingDistance = 0.5f; // Waypointに到達したとみなす距離

    private int _previousWaypointIndex = -1; // 前回のWaypointインデックス
    private int _randomIndex = default;
    private const int RAY_LENGTH = 20;
    private const int ENEMY_STAY_TIME = 30;
    private const int WAIT_TIME = 3;
    private const int DAMAGE_WAIT_TIME = 5;
    private string _playerTag = "Player";
    private Transform _player; // プレイヤーのTransform（ゲーム開始時に取得）
    private NavMeshAgent _agent;
    private bool _isAttack = false;
    private bool _isStay = true;
    private bool _isInvincible = false;
    private bool _isEveryChase=false;
    /// <summary>
    /// 敵の行動状態を定義する列挙型
    /// </summary>
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack,
        Idle,
        EveryChase
    }
    public EnemyState EnemyCurrentState { get;private set; } // 現在の状態

    private RaycastHit _hit;
    /// <summary>
    /// 初期化処理
    /// プレイヤーとNavMeshAgentの参照を取得し、最初のWaypointへ移動する
    /// </summary>
    void Start()
    {
        // プレイヤーのタグを基にTransformを取得
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (_player == null)
        {
            Debug.LogError("Playerオブジェクトが見つかりません。タグを確認してください。");
            return;
        }

        // NavMeshAgentを取得
        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
        {
            Debug.LogError("NavMeshAgentがアタッチされていません。");
            return;
        }

        // 初期状態を巡回に設定し、最初のWaypointに向かう
        EnemyCurrentState = EnemyState.Idle;
        //MoveToNextWaypoint();
    }

    /// <summary>
    /// 毎フレーム更新処理
    /// 状態に応じた動作を実行し、プレイヤーを検知する
    /// </summary>
    void Update()
    {

        UpadteState();
    }

    private void UpadteState()
    {
        if (_player == null || _agent == null) return; // 必要なコンポーネントがない場合は処理をスキップ
        if (!_isEveryChase)
        {
            if (!_isAttack && !_isStay)
            {
                CurrentState(); // 現在の状態に基づいた処理を実行
                DetectPlayer(); // プレイヤーを検知
            }
        }
        else
        {
            if (!_isAttack)
            {
                CurrentState();
            }

        }
    }
    /// <summary>
    /// 現在の状態に応じた動作を切り替える
    /// </summary>
    private void CurrentState()
    {
        switch (EnemyCurrentState)
        {
            case EnemyState.Patrol:
                Patrol();
                _agent.speed = 5;
                break;
            case EnemyState.Chase:
                ChasePlayer();
                _agent.speed = 10;
                break;
            case EnemyState.Attack:
                _agent.speed = 0f;
                break;
            case EnemyState.Idle:
                _agent.speed = 0;
                break;
            case EnemyState.EveryChase:
                EveryChasePlayer();
                _agent.speed = 15;
                break;



        }
    }

    /// <summary>
    /// 巡回動作
    /// Waypointsを順番に巡回する
    /// </summary>
    private void Patrol()
    {
        // 現在のWaypointに到達した場合、次のWaypointに移動
        if (!_agent.pathPending && _agent.remainingDistance < _waypointStoppingDistance)
        {
            MoveToNextWaypoint();
        }
    }

    /// <summary>
    /// プレイヤー追跡動作
    /// プレイヤーが検知範囲外に出た場合、巡回に戻る
    /// </summary>
    private void ChasePlayer()
    {
        // プレイヤーが検知範囲外に出たら巡回に戻る
        if (Vector3.Distance(transform.position, _player.position) > _detectionRange)
        {
            EnemyCurrentState = EnemyState.Patrol;
            MoveToNextWaypoint();
        }
        else
        {
            // プレイヤーの位置に向かう
            _agent.SetDestination(_player.position);
        }
    }

    /// <summary>
    /// ランダムに次のWaypointに移動
    /// </summary>
    private void MoveToNextWaypoint()
    {
        if (_waypoints.Length == 0) return; // Waypointsが設定されていない場合は何もしない
        do
        {
            _randomIndex = Random.Range(0, _waypoints.Length); // ランダムなインデックスを選択
        } while (_randomIndex == _previousWaypointIndex && _waypoints.Length > 1);

        _previousWaypointIndex = _randomIndex; // 前回選択したインデックスを記録
        _agent.SetDestination(_waypoints[_randomIndex].position);
    }


    private void EveryChasePlayer()
    {
            // プレイヤーの位置に向かう
            _agent.SetDestination(_player.position);
        
    }
    /// <summary>
    /// プレイヤーを検知
    /// 検知範囲内にプレイヤーがいる場合、追跡状態に切り替える
    /// </summary>
    private void DetectPlayer()
    {
        Ray forwardRay = new Ray(this.gameObject.transform.position, transform.forward);
        Ray rightRay = new Ray(this.gameObject.transform.position, transform.forward + transform.right);
        Ray leftRay = new Ray(this.gameObject.transform.position, transform.forward - transform.right);

        if (Physics.Raycast(forwardRay, out _hit, RAY_LENGTH))
        {
            if (_hit.collider.CompareTag(_playerTag))
            {
                EnemyCurrentState = EnemyState.Chase; // 追跡状態に切り替え
            }

        }
        if (Physics.Raycast(rightRay, out _hit, RAY_LENGTH))
        {
            if (_hit.collider.CompareTag(_playerTag))
            {
                EnemyCurrentState = EnemyState.Chase; // 追跡状態に切り替え
            }

        }
        if (Physics.Raycast(leftRay, out _hit, RAY_LENGTH))
        {
            if (_hit.collider.CompareTag(_playerTag))
            {
                EnemyCurrentState = EnemyState.Chase; // 追跡状態に切り替え
            }

        }
        //Debug.DrawRay(rightRay.origin, rightRay.direction * RAY_LENGTH, Color.red, 100, false);
        //Debug.DrawRay(forwardRay.origin, forwardRay.direction * RAY_LENGTH, Color.red, 100, false);
        //Debug.DrawRay(leftRay.origin, leftRay.direction * RAY_LENGTH, Color.red, 100, false); ;
        //float sqrDetectionRange = _detectionRange * _detectionRange; // 範囲判定の効率化
        //if ((transform.position - _player.position).sqrMagnitude <= sqrDetectionRange)
        //{
        //    EnemyCurrentState = EnemyState.Chase; // 追跡状態に切り替え
        //}
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!_isInvincible)
        {
            if (collision.gameObject.CompareTag(_playerTag))
            {
                if (!_isEveryChase)
                {
                    StartCoroutine(EnemyAttack());
                    ValueManager.Instance.Damage();
                    StartCoroutine(CantAttack());
                }
                else
                {
                    StartCoroutine(EveryEnemyAttack());
                    ValueManager.Instance.Damage();
                    StartCoroutine(CantAttack());
                }
              
            }
          
        }
       
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            StartCoroutine(EnemyDamage());
        }
    }
    private IEnumerator EnemyAttack()
    {
        _isAttack = true;
        EnemyCurrentState = EnemyState.Attack;
        yield return new WaitForSeconds(WAIT_TIME);
        EnemyCurrentState = EnemyState.Patrol;
        _isAttack = false;
    }private IEnumerator EveryEnemyAttack()
    {
        _isAttack = true;
        EnemyCurrentState = EnemyState.Attack;
        yield return new WaitForSeconds(WAIT_TIME);
        EnemyCurrentState = EnemyState.EveryChase;
        _isAttack = false;
    }
    public IEnumerator EnemyMoveStart()
    {
        yield return new WaitForSeconds(ENEMY_STAY_TIME);
        _isStay = false;
        EnemyCurrentState = EnemyState.Patrol;
        MoveToNextWaypoint();
    }
    private IEnumerator EnemyDamage()
    {
        EnemyCurrentState = EnemyState.Idle;
        _isStay = true;
        yield return new WaitForSeconds(DAMAGE_WAIT_TIME);
        EnemyCurrentState = EnemyState.Patrol;
        _isStay = false;
    }
    private IEnumerator CantAttack()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(DAMAGE_WAIT_TIME);
        _isInvincible = false;
    }
    public void EveryChaseON()
    {
        _isEveryChase = true;
        EnemyCurrentState = EnemyState.EveryChase;
    }
}
