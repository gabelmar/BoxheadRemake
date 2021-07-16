using UnityEngine;

public class Rocket : Projectile, IExplosive
{
    [SerializeField]
    private GameObject explosionEffect;

    [SerializeField]
    private Collider col;

    [SerializeField]
    private LayerMask explosionInducingLayers;

    [SerializeField]
    private LayerMask explosionAffectedLayers;

    [SerializeField]
    private float explosionRadius;

    private bool isExploded = false;
    protected override void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        // Check if the colliding Object is on the layer that causes an explosion
        if (explosionInducingLayers == (explosionInducingLayers | (1 << other.layer)))
        {
            Explode();  
        }
    }

    public void Explode() 
    {
        if (isExploded)
            return;
        isExploded = true;
        col.enabled = false;
        Destroy(Instantiate(explosionEffect, transform.position, Quaternion.identity), 3f);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionAffectedLayers);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            HealthSystem health = hitColliders[i].GetComponent<HealthSystem>();
            if (health != null) 
            {
                health.TakeDamage(damage);
            }  
            else 
            {
                if (hitColliders[i].gameObject.GetComponent<MonoBehaviour>() is IExplosive explosive)
#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
                    if (explosive != (this))            // Only trigger explosion on the found explosive if it is not the object inctance itself
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
                        explosive.Explode();
            }   
        }
        AudioManager.Instance.Play("small explosion");
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
