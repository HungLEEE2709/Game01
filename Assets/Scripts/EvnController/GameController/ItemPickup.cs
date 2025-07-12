using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int value = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance?.AddScore(value); 
            Destroy(gameObject);
        }
    }
}
