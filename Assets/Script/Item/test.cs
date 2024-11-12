using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private LightItem _lih;
    // Start is called before the first frame update
    void Start()
    {
        _lih = GameObject.FindGameObjectWithTag("Player").GetComponent<LightItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
           _lih.LightActive();
        }    
    }

}
