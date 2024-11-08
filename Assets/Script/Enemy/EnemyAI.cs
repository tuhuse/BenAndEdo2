using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform _way;
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _dashSpeed;
    private enum MoveState
    {
        RandomMove,
        FollowMove
    }
    private MoveState _moveState = default;
    void Start()
    {
        _moveState = MoveState.RandomMove;
    }
    void Update()
    {
        SwicthState();
    }
    private void SwicthState()
    {
        switch (_moveState)
        {
            case MoveState.RandomMove:

                break;
            case MoveState.FollowMove:

                break;
        }
    }
    private void DistanceState()
    {
        float playerDistance = Vector2.Distance(this.transform.position, _player.position);
        if (playerDistance > 50)
        {
            
        }
    }
     
}
