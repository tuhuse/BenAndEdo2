
using UnityEngine;

public class CashBox : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    private float _openRange = 2f;
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
            OpenDoor = true;
            
        }
        else
        {
            Debug.Log("近くにありません");
        }
    }

}
