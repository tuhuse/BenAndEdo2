using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{

    public float _moveSpeed;
    public float _jumpPower;
    public virtual void Speed()
    {
        _moveSpeed = 55f;
        _jumpPower = 55f;
    }
}

public class OneLleg : MoveManager {
    public override void Speed()
    {
        _moveSpeed = 50f;
        _jumpPower = 50f;
    }
    
}
public class Body : OneLleg
{
    public override void Speed()
    {
        _moveSpeed = 40f;
        _jumpPower = 40f;
    }
}public class Head : Body
{
    public override void Speed()
    {
        _moveSpeed = 30f;
        _jumpPower = 30f;
    }
}public class ReturnBody : Body
{
    public override void Speed()
    {
        base.Speed();
    }
}public class ReturnOneLeg : OneLleg
{
    public override void Speed()
    {
        base.Speed();
    }
}
