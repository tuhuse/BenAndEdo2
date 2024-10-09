using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LegController : MonoBehaviour
{
    [SerializeField] private GameObject _bodyObject;
    [SerializeField] private GameObject _camera;
    [SerializeField] private BodyController _body;
    [SerializeField] private HeadController _headbody;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _jumpPower;
    private bool _isJump = false;
    public bool _isAlive = true;
    public bool _isSwitch = true;
    private Rigidbody _rb;
    public BoxCollider _box;
    // Start is called before the first frame update
    void Start()
    {
        _camera.transform.SetParent(this.transform, true);
        _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8);
        _camera.transform.rotation = Quaternion.Euler(20, 0, 0);
        _rb = GetComponent<Rigidbody>();
        _box = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
      
        if(_isAlive && _isSwitch)
        {
            PlayerMove();
        }
    }
    private void PlayerMove()
    {
        _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8);
        _camera.transform.rotation = Quaternion.Euler(20, 0, 0);
        if (Input.GetKey(KeyCode.D))
        {
            _rb.velocity = new Vector3(_moveSpeed, _rb.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _rb.velocity = new Vector3(-_moveSpeed, _rb.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, _moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, -_moveSpeed);
        }
        if (_isJump)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _rb.velocity = new Vector3(_rb.velocity.x, _jumpPower, 0);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        int unLeg = 10;
        if (collision.gameObject.CompareTag("Yaiba"))
        {
            _body.UnLeg(true);
            _camera.transform.SetParent(this.transform, false);
            _headbody._isHaveLeg = false;
            _body.BodySwitch(true);
            _isAlive = false;
            this.gameObject.layer = unLeg;
        }
      
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = true;
            if (!_isAlive|| !_isSwitch)
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll;
            }
           

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false;
        }
    }
   public void LegSwitch(bool legswitch)
    {
        if (legswitch)
        {
            _isSwitch = true;
            _rb.constraints = RigidbodyConstraints.None;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            _isSwitch = false;
          
        }
     
    }
    public void RespawnWait()
    {//体のリセット
        _body.BodySwitch(false);
        _body.UnLeg(false);
        _body._isUnBody = false;
        _bodyObject.layer = 6;
        //頭のリセット
        _headbody.HeadSwitch(false);
        _headbody._isHaveLeg = true;
        //足のリセット
        int leg = 8;
        this.gameObject.layer = leg;
        _isAlive = true;
        LegSwitch(true);
        _headbody._isHeadAlive = true;
    }
}
