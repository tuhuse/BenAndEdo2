using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{[SerializeField] private PlayerMoveManager _playerMoveManager;

    [SerializeField] Rigidbody[] _rb;
    private enum PlayerSituationMove
    {
        TwoLeg,
        OneLeg,
        Body,
        Head
    }
    private PlayerSituationMove _player = default;
    private void Update()
    {
        SwitchSituation();
    }
    private void SwitchSituation()
    {
        switch (_player)
        {
            case PlayerSituationMove.TwoLeg:
                _playerMoveManager.PlayerMove(_rb[0]);
                break;
            case PlayerSituationMove.OneLeg:
                _playerMoveManager.PlayerMove(_rb[1]);
                break; 
            case PlayerSituationMove.Body:
                _playerMoveManager.PlayerMove(_rb[2]);
                break; 
            case PlayerSituationMove.Head:
                _playerMoveManager.PlayerMove(_rb[3]);
                break;
        }
    }
    public void SetMoveMent<T>() where T : PlayerMoveManager
    {
        if (_playerMoveManager != null)
        {
            Destroy(_playerMoveManager);
        }
        _playerMoveManager = gameObject.AddComponent<T>();
    }
    public void BodyMove(bool juge)
    {
        if (juge)
        {
            SetMoveMent<BodyPlayerMoveManager>();
            _player = PlayerSituationMove.Body;
        }
    }  public void HeadMove(bool juge)
    {
        if (juge)
        {
            SetMoveMent<HeadPlayerMoveManager>();
            _player = PlayerSituationMove.Head;
        }
    } public void TwoLegMove(bool juge)
    {
        if (juge)
        {
            SetMoveMent<TwoLegPlayerMoveManager>();
            _player = PlayerSituationMove.TwoLeg;
        }
    }public void OneLegMove(bool juge)
    {
        if (juge)
        {
            SetMoveMent<OneLegPlayerMoveManager>();
            _player = PlayerSituationMove.OneLeg;
        }
    }

}

