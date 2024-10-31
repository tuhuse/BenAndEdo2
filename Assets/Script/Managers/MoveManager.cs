//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MoveManager : MonoBehaviour
//{
//    private PlayerMoveManager _playerMoveManager;

//    [SerializeField] private Rigidbody[] _rb;

//    private enum PlayerSituationMove
//    {
//        TwoLeg,
//        //OneLeg,
//        Body,
//        Head
//    }

//    private PlayerSituationMove _player = PlayerSituationMove.TwoLeg;

//    private void Start()
//    {
//        _playerMoveManager = gameObject.AddComponent<PlayerMoveManager>();
//        TwoLegMove(true);
//    }

//    private void Update()
//    {
//        SwitchSituation();
//    }

//    private void SwitchSituation()
//    {
//        if (_playerMoveManager == null)
//        {
//            Debug.LogWarning("PlayerMoveManager is not set!");
//            return;
//        }

//        if (_rb.Length == 0)
//        {
//            Debug.LogWarning("Rigidbody array is empty!");
//            return;
//        }

//        switch (_player)
//        {
//            case PlayerSituationMove.TwoLeg:
//                _playerMoveManager.PlayerMove(_rb[0]);
//                break;
//            //case PlayerSituationMove.OneLeg:
//            ////    _playerMoveManager.PlayerMove(_rb[1]);
//            //    break;
//            case PlayerSituationMove.Body:
//                _playerMoveManager.PlayerMove(_rb[2]);
//                break;
//            case PlayerSituationMove.Head:
//                _playerMoveManager.PlayerMove(_rb[3]);
//                break;
//        }
//    }

//    public void SetMoveMent<T>() where T : PlayerMoveManager
//    {
//        if (_playerMoveManager != null)
//        {
//            Destroy(_playerMoveManager);
//        }

//        _playerMoveManager = gameObject.AddComponent<T>();
//        Debug.Log("SetMoveMent called, PlayerMoveManager set to: " + typeof(T).Name);
//    }

//    public void BodyMove(bool isActive)
//    {
//        if (isActive)
//        {
//            SetMoveMent<BodyPlayerMoveManager>();
//            _player = PlayerSituationMove.Body;
//        }
//    }

//    public void HeadMove(bool isActive)
//    {
//        if (isActive)
//        {
//            SetMoveMent<HeadPlayerMoveManager>();
//            _player = PlayerSituationMove.Head;
//        }
//    }

//    public void TwoLegMove(bool isActive)
//    {
//        if (isActive)
//        {
//            SetMoveMent<TwoLegPlayerMoveManager>();
//            _player = PlayerSituationMove.TwoLeg;
//        }
//    }

//    //public void OneLegMove(bool isActive)
//    //{
//    //    if (isActive)
//    //    {
//    //        SetMoveMent<OneLegPlayerMoveManager>();
//    //        _player = PlayerSituationMove.OneLeg;
//    //    }
//    //}
//}
