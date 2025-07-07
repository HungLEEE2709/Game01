using UnityEngine;
using System.Collections;

public class FallPlatform : MonoBehaviour
{
    public float fallDelay = 0.5f;
    public float destroyDelay = 2f;

    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (rb != null)
            rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            StartCoroutine(StartFall());
        }
    }

    private IEnumerator StartFall()
    {
        yield return new WaitForSeconds(fallDelay);
        Fall();
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    private void Fall()
    {
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
