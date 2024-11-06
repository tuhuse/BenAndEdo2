using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour
{
    private const float MAXSPEED=10f;
    private const float MAXDASHHP=100f;
    private const float HEAL = 10;

    public float _moveSpeed { get; set; }
    public float _dashHP;
   
    public int _playerHP { get; set; }
    private Coroutine _dashRecoveryCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        _moveSpeed = 10f;
        _dashHP = 100f;
        _playerHP = 3;
        
    }
   
    
    private IEnumerator ReturnSpeed()
    {
        _moveSpeed -= 10;
        yield return new WaitForSeconds(3f);
        _moveSpeed = MAXSPEED;
    }
    public void DashHPDecrease()
    {
        _dashHP --;
    }
   public void StartDashRecovery()
    {
        if (_dashRecoveryCoroutine == null)
        {
            _dashRecoveryCoroutine = StartCoroutine(DashHealthRecovery());
        }
        StartCoroutine(DashHealthRecovery());
    }public void StopDashRecovery()
    {
        if (_dashRecoveryCoroutine != null)
        {
             StopCoroutine(DashHealthRecovery());
            _dashRecoveryCoroutine = null;  
        }
       
    }
    private IEnumerator DashHealthRecovery()
    {
        int waitTime = 1;
        while (_dashHP < MAXDASHHP)
        {
            _dashHP +=HEAL; // ‰ñ•œ—Ê‚ð’Ç‰Á
            _dashHP = Mathf.Min(_dashHP, MAXDASHHP); // Å‘å‘Ì—Í‚ð’´‚¦‚È‚¢‚æ‚¤‚É§ŒÀ

            yield return new WaitForSeconds(waitTime); // Žw’è‚µ‚½ŠÔŠu‚¾‚¯‘Ò‹@
        }
        _dashRecoveryCoroutine = null;
    }
    public void Damage()
    {      
            StartCoroutine(ReturnSpeed());
            _playerHP--;
    }
    public void Heal()
    {
        _playerHP++;
    }
}
