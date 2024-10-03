using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField]private GameObject _body;
    [SerializeField]private GameObject _legbody;
    [SerializeField] private GameObject _fusionUi;
   
    [SerializeField]
    private float MoveSpeed;   
    [SerializeField]
    private float JumpPower; 
    [SerializeField]
    private BodyController _bodyplayer;
    [SerializeField] 
    private LegController _leg;
    private const float fallmove = 30;

    private bool _isSwitch=false;
    private bool _isJump = false;
    public bool _isHaveLeg= true;
    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {

        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (!_isJump)
        {
            _rb.velocity -= new Vector3(0, JumpPower / fallmove, 0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (_leg._isAlive)
            {
            _bodyplayer.BodySwitch(false);
            HeadSwitch(true);
            _bodyplayer.UnLeg(true);
            _leg.LegSwitch(false);
            }
            else
            {
                _bodyplayer.BodySwitch(false);
                HeadSwitch(true);
                _bodyplayer.UnLeg(true);
                
            }
           
        }
        HeadMove();
    }
    private void HeadMove()
    {
        if (_isSwitch)
        {
            if (Input.GetKey(KeyCode.D))
            {
                _rb.velocity = new Vector3(MoveSpeed, _rb.velocity.y, 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                _rb.velocity = new Vector3(-MoveSpeed, _rb.velocity.y, 0);
            }
            if (Input.GetKey(KeyCode.W))
            {
                _rb.velocity = new Vector3(0, _rb.velocity.y, MoveSpeed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                _rb.velocity = new Vector3(0, _rb.velocity.y, -MoveSpeed);
            }
            if (_isJump)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    _rb.velocity = new Vector3(_rb.velocity.x, JumpPower, 0);
                }
            }
        }
        if (!_isSwitch && _isHaveLeg)
        {
            this.transform.position = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 2.75f, _legbody.transform.position.z);

        }
        if (!_isSwitch && !_isHaveLeg)
        {
            this.transform.position = new Vector3(_body.transform.position.x, _body.transform.position.y + 1.25f, _body.transform.position.z);
        }


    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = true;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
           
            if (_isSwitch)
            {
                _fusionUi.SetActive(true);
            }
           
            if (Input.GetKeyDown(KeyCode.Q))
            {
                HeadSwitch(false);
               
                if (_leg._isAlive)
                {
                    _bodyplayer.UnLeg(false);
                    _leg.LegSwitch(true);
                }
                else
                {
                    _bodyplayer.BodySwitch(true);
                }
                
            }
                   
        }
    }
   
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            _fusionUi.SetActive(false);         
        }
       
    }
    public void HeadSwitch(bool headswitch)
    {
        if (headswitch)
        {
            
            _isSwitch = true;
            _camera.transform.SetParent(this.transform, true);
            _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8);
            _camera.transform.rotation = Quaternion.Euler(20, 0,0);
        }
        else
        {
           
            _isSwitch = false;
            _camera.transform.SetParent(this.transform, false);
        }
    }
}
