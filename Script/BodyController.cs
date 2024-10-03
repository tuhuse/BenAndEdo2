using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _legbody;
    private Rigidbody _rb;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _jumpPower;
    [SerializeField] private HeadController _head;
    [SerializeField] private LegController _leg;
    private const float fallmove = 30;
    private bool _isJump = false;
    private bool _isSwitch = false;
    private bool _isUnLeg = false;
    public bool _isUnBody = false;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
       
    }
    private void FixedUpdate()
    {
        if (!_isJump)
        {
            _rb.velocity -= new Vector3(0, _jumpPower / fallmove, 0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_isSwitch&&_isUnLeg)
        {
            PlayerMove();
        }
        if(!_isUnLeg&&!_isSwitch&&!_isUnBody)
        {
        
            _rb.constraints = RigidbodyConstraints.None;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            this.transform.position = new Vector3(_legbody.transform.position.x, _legbody.transform.position.y + 1.5f, _legbody.transform.position.z);
            
        }
        if (_isUnLeg)
        {
            if (_leg._isAlive)
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
     

      



    }
    private void PlayerMove()
    {
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
    public void UnLeg(bool unleg)
    {
        if (unleg)
        {
            _isUnLeg = true;
        }
        else
        {
            _isUnLeg = false;
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = true;
            if (!_isSwitch)
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Yaiba"))
        {
            BodySwitch(false);
            _head.HeadSwitch(true);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false;
        }
    }
    public void BodySwitch(bool bodyswitch)
    {
        if (bodyswitch)
        {
            _rb.constraints = RigidbodyConstraints.None;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            _head.HeadSwitch(false);
            _isSwitch = true;
            _camera.transform.SetParent(this.transform, true);
            _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z - 8);
            _camera.transform.rotation = Quaternion.Euler(20, 0, 0);
        }
        else
        {
            _isSwitch = false;
            UnLeg(true);
            _camera.transform.SetParent(this.transform, false);
        }
    }

}
