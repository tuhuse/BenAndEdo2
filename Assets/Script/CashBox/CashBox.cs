using System.Collections;
using UnityEngine;

public class CashBox : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject _KeyHoleMesh;
    [SerializeField] private MeshRenderer _cashBoxMesh;
    private float _openRange = 2f;
    private const float WAIT_TIME = 0.2f;
    /// <summary>
    /// 金庫が開いたかどうかのプロパティ
    /// </summary>
   public bool OpenDoor { get; private set; } = false;


    private void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }
    public void OpenCashBox()
    {
        float distancePlayer = Vector3.Distance(this.transform.position, _playerTransform.position);

        if (distancePlayer < _openRange)
        {
            StartCoroutine(StateBool());        
        }
        else
        {
            Debug.Log("近くにありません");
        }
    }
    private IEnumerator StateBool()
    {
        yield return new WaitForSeconds(WAIT_TIME);
        OpenDoor = true;
        _KeyHoleMesh.SetActive(false);
        _cashBoxMesh.enabled = false;
    }
    
}
