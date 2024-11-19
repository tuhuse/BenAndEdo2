
using UnityEngine;

public class CashBox : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    private float _openRange = 2f;
   public bool OpenDoor { get; private set; } = false;


    public void OpenCashBox()
    {
        float distancePlayer = Vector3.Distance(this.transform.position, _playerTransform.position);

        if (distancePlayer < _openRange)
        {
            OpenDoor = true;
        }
    }

}
