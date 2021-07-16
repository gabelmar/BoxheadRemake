using UnityEngine;

public class DevilFireball : Projectile
{
    protected override void OnCollisionEnter(Collision collision)
    {
        HealthSystem health = collision.transform.GetComponent<HealthSystem>();
        if (health != null)
        {
            health.TakeDamage(damage);
            AudioManager.Instance.Play("hitmarker");
        }
        Destroy(gameObject);
    }
}
