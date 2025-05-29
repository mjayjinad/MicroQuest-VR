using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MiniCharacter"))
        {
            GameManager.Instance.OnCheckpointReached();
        }
    }
}