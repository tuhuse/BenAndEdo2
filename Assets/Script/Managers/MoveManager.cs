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
}public class Leg : Body
{
    public void LegSpeed()
    {
        _moveSpeed = 30f;
        _jumpPower = 30f;
    }
}
