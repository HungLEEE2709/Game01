using UnityEngine;

public class Iceball : BaseProjectile
{
    public GameObject iceExplosion;

    protected override void OnHit()
    {
        if (iceExplosion != null)
            Instantiate(iceExplosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
