using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public float lifeTime = 3f;
    public LayerMask playerLayer;

    protected Transform target;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime); 
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
        else
            Debug.LogWarning("Player not found!");
    }

    protected virtual void Update()
    {
        if (target == null) return;

        // Di chuyển theo hướng người chơi
        Vector2 dir = (target.position - transform.position).normalized;
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            PlayerHealth hp = collision.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
            OnHit();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            OnHit();
        }
    }

    protected abstract void OnHit(); 
}
