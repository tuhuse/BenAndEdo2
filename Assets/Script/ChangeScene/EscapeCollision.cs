using UnityEngine;
using UnityEngine.SceneManagement;
public class EscapeCollision : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        string player = "Player";
        if (other.gameObject.CompareTag(player))
        {
            SceneManager.LoadScene("GameClear");
        }
    }
}
