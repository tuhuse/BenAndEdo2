using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private Transform _legRespawnPosition;
    [SerializeField] private Transform _headRespawnPosition;
    [SerializeField] private Transform _bodyRespawnPosition;
    [SerializeField] private GameObject _leg;
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _body;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        string player = "Player";
        if (collision.gameObject.CompareTag(player))
        {
            StartCoroutine(Fusion());
            _head.GetComponent<HeadController>()._isHeadAlive = false;
        }
    }
    private IEnumerator Fusion()
    {
        float waittime = 1f;
        _camera.transform.position = new Vector3(_leg.transform.position.x, _leg.transform.position.y + 5, _leg.transform.position.z - 8);
        _camera.transform.rotation = Quaternion.Euler(20, 0, 0);
        _leg.GetComponent<LegController>()._box.enabled = true;
        _leg.transform.position = _legRespawnPosition.position;
        yield return new WaitForSeconds(waittime);
        _body.transform.position = _bodyRespawnPosition.position;
        yield return new WaitForSeconds(waittime);
        _head.transform.position = _headRespawnPosition.position;
        _leg.GetComponent<LegController>().RespawnWait();

    }
}
