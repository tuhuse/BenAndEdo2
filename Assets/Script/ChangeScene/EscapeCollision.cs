using UnityEngine;
public class EscapeCollision : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        string player = "Player";
        if (other.gameObject.CompareTag(player))
        {
            GameManager.Instance.OnGameClear();
        }
    }
}
