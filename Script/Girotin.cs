using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girotin : MonoBehaviour
{
    private float _moveSpeed =0.2f;
    private bool _isMaxRight = true;
    private bool _isMaxLeft = false;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _endPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var position = new Vector3[] { _startPoint.position, _endPoint.position, };
        _lineRenderer?.SetPositions(position);
        if (_isMaxRight)
        {
            this.transform.Rotate(0, 0, _moveSpeed*Time.deltaTime*1000f);
        }
        if (_isMaxLeft)
        {
            this.transform.Rotate(0, 0, -_moveSpeed * Time.deltaTime * 1000f);
        }
        // 角度のチェックをeulerAnglesで行う
        float zRotation = transform.rotation.eulerAngles.z;

        if (zRotation > 69f && zRotation < 71f)
        {
            _isMaxRight = false;
            _isMaxLeft = true;
        }
        else if (zRotation > 289f && zRotation < 291f)
        {
            _isMaxRight = true;
            _isMaxLeft = false;
        }
    }
}
