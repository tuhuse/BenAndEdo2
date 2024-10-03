using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorEnemy : MonoBehaviour
{
    [Header("‰Šú’n“_")]
    [SerializeField]
    private Transform _firstPosition;
    private bool _fall;
    private bool _timestart;
    private float _timeCount = 0;
    private const float _fallspeed = 0.02f;
    private const float _timemax = 1f;
    // Start is called before the first frame update
    void Start()
    {
        _fall = false;
        _timestart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_fall)
        {
            this.transform.position -= new Vector3(0, _fallspeed*Time.deltaTime*1000f,0);
            StartCoroutine(FallTime());
        }
        else
        {
            this.transform.position = _firstPosition.position;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_timestart)
            {
                _timeCount += Time.deltaTime;
            }
            if (_timeCount > _timemax)
            {
                _fall = true;
            }
           
        }
    }private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _timestart = true;
         
        }
    }private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _timestart = false;
            _timeCount = 0;

         
        }
    }
    private IEnumerator FallTime()
    {
        yield return new WaitForSeconds(3f);
        _fall = false;
    }
}
