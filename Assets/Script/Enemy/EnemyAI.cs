using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �GAI�N���X
/// �v���C���[�����m���AWaypoints������܂��͒ǐՂ��s��
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints; // Waypoints�i����n�_�̔z��j
    [SerializeField] private float _detectionRange = 20f; // �v���C���[�����m����͈́i�P�ʁF���[�g���j
    [SerializeField] private float _waypointStoppingDistance = 0.5f; // Waypoint�ɓ��B�����Ƃ݂Ȃ�����

    private int _previousWaypointIndex = -1; // �O���Waypoint�C���f�b�N�X
    private int _randomIndex = default;
    private const int RAY_LENGTH = 20;
    private const int ENEMY_STAY_TIME = 30;
    private const int WAIT_TIME = 3;
    private const int DAMAGE_WAIT_TIME = 5;
    private string _playerTag = "Player";
    private Transform _player; // �v���C���[��Transform�i�Q�[���J�n���Ɏ擾�j
    private NavMeshAgent _agent;
    private bool _isAttack = false;
    private bool _isStay = true;
    private bool _isInvincible = false;
    private bool _isEveryChase=false;
    /// <summary>
    /// �G�̍s����Ԃ��`����񋓌^
    /// </summary>
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack,
        Idle,
        EveryChase
    }
    public EnemyState EnemyCurrentState { get;private set; } // ���݂̏��

    private RaycastHit _hit;
    /// <summary>
    /// ����������
    /// �v���C���[��NavMeshAgent�̎Q�Ƃ��擾���A�ŏ���Waypoint�ֈړ�����
    /// </summary>
    void Start()
    {
        // �v���C���[�̃^�O�����Transform���擾
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (_player == null)
        {
            Debug.LogError("Player�I�u�W�F�N�g��������܂���B�^�O���m�F���Ă��������B");
            return;
        }

        // NavMeshAgent���擾
        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent���A�^�b�`����Ă��܂���B");
            return;
        }

        // ������Ԃ�����ɐݒ肵�A�ŏ���Waypoint�Ɍ�����
        EnemyCurrentState = EnemyState.Idle;
        //MoveToNextWaypoint();
    }

    /// <summary>
    /// ���t���[���X�V����
    /// ��Ԃɉ�������������s���A�v���C���[�����m����
    /// </summary>
    void Update()
    {

        UpadteState();
    }

    private void UpadteState()
    {
        if (_player == null || _agent == null) return; // �K�v�ȃR���|�[�l���g���Ȃ��ꍇ�͏������X�L�b�v
        if (!_isEveryChase)
        {
            if (!_isAttack && !_isStay)
            {
                CurrentState(); // ���݂̏�ԂɊ�Â������������s
                DetectPlayer(); // �v���C���[�����m
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
    /// ���݂̏�Ԃɉ����������؂�ւ���
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
    /// ���񓮍�
    /// Waypoints�����Ԃɏ��񂷂�
    /// </summary>
    private void Patrol()
    {
        // ���݂�Waypoint�ɓ��B�����ꍇ�A����Waypoint�Ɉړ�
        if (!_agent.pathPending && _agent.remainingDistance < _waypointStoppingDistance)
        {
            MoveToNextWaypoint();
        }
    }

    /// <summary>
    /// �v���C���[�ǐՓ���
    /// �v���C���[�����m�͈͊O�ɏo���ꍇ�A����ɖ߂�
    /// </summary>
    private void ChasePlayer()
    {
        // �v���C���[�����m�͈͊O�ɏo���珄��ɖ߂�
        if (Vector3.Distance(transform.position, _player.position) > _detectionRange)
        {
            EnemyCurrentState = EnemyState.Patrol;
            MoveToNextWaypoint();
        }
        else
        {
            // �v���C���[�̈ʒu�Ɍ�����
            _agent.SetDestination(_player.position);
        }
    }

    /// <summary>
    /// �����_���Ɏ���Waypoint�Ɉړ�
    /// </summary>
    private void MoveToNextWaypoint()
    {
        if (_waypoints.Length == 0) return; // Waypoints���ݒ肳��Ă��Ȃ��ꍇ�͉������Ȃ�
        do
        {
            _randomIndex = Random.Range(0, _waypoints.Length); // �����_���ȃC���f�b�N�X��I��
        } while (_randomIndex == _previousWaypointIndex && _waypoints.Length > 1);

        _previousWaypointIndex = _randomIndex; // �O��I�������C���f�b�N�X���L�^
        _agent.SetDestination(_waypoints[_randomIndex].position);
    }


    private void EveryChasePlayer()
    {
            // �v���C���[�̈ʒu�Ɍ�����
            _agent.SetDestination(_player.position);
        
    }
    /// <summary>
    /// �v���C���[�����m
    /// ���m�͈͓��Ƀv���C���[������ꍇ�A�ǐՏ�Ԃɐ؂�ւ���
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
                EnemyCurrentState = EnemyState.Chase; // �ǐՏ�Ԃɐ؂�ւ�
            }

        }
        if (Physics.Raycast(rightRay, out _hit, RAY_LENGTH))
        {
            if (_hit.collider.CompareTag(_playerTag))
            {
                EnemyCurrentState = EnemyState.Chase; // �ǐՏ�Ԃɐ؂�ւ�
            }

        }
        if (Physics.Raycast(leftRay, out _hit, RAY_LENGTH))
        {
            if (_hit.collider.CompareTag(_playerTag))
            {
                EnemyCurrentState = EnemyState.Chase; // �ǐՏ�Ԃɐ؂�ւ�
            }

        }
        //Debug.DrawRay(rightRay.origin, rightRay.direction * RAY_LENGTH, Color.red, 100, false);
        //Debug.DrawRay(forwardRay.origin, forwardRay.direction * RAY_LENGTH, Color.red, 100, false);
        //Debug.DrawRay(leftRay.origin, leftRay.direction * RAY_LENGTH, Color.red, 100, false); ;
        //float sqrDetectionRange = _detectionRange * _detectionRange; // �͈͔���̌�����
        //if ((transform.position - _player.position).sqrMagnitude <= sqrDetectionRange)
        //{
        //    EnemyCurrentState = EnemyState.Chase; // �ǐՏ�Ԃɐ؂�ւ�
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
