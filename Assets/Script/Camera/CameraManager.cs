using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _cinemachineCamera;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _leg;

    // Start is called before the first frame update
    void Start()
    {
        LookBody(_leg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwitchBody(int switchnumber)
    {
        if (switchnumber==1)
        {
            LookBody(_leg);
         
        }else if (switchnumber == 2)
        {
            LookBody(_body);
           
        }
        else if (switchnumber == 3)
        {
            LookBody(_head);
           
        }
    }
    public void LookBody(Transform target)
    {
        _cinemachineCamera.Follow = target;
        _cinemachineCamera.LookAt = target;

    }
}
