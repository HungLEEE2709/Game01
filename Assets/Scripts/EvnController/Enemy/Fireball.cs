using UnityEngine;

public class Fireball : BaseProjectile
{
    public GameObject fireExplosion;

    protected override void OnHit()
    {
        if (fireExplosion != null)
            Instantiate(fireExplosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
