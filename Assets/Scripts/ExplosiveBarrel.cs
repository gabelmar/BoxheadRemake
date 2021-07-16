using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IExplosive
{
    [SerializeField]
    private float explosionRadius = 2f;
    [SerializeField]
    private int Damage;

    [SerializeField]
    private Transform explosionPosition;

    [SerializeField]
    private Collider col;

    [SerializeField]
    private GameObject explosionEffect;

    [SerializeField]
    public LayerMask explosionInducingLayers;

    [SerializeField]
    private LayerMask explosionAffectedLayers;

    private MeshRenderer mRenderer;

    private bool isExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding Object is on the layer that causes an explosion
        if (explosionInducingLayers == (explosionInducingLayers | (1 << collision.gameObject.layer))) 
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (isExploded)
            return;
        isExploded = true;

        mRenderer.enabled = false;
        col.enabled = false;
        explosionEffect.gameObject.SetActive(true);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            HealthSystem health = hitColliders[i].GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(Damage);
            }
            else
            {
                if (hitColliders[i].gameObject.GetComponent<MonoBehaviour>() is IExplosive explosive)
#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
                    if (explosive != (this))          // Only trigger explosion on the found explosive if it is not the object inctance itself
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
                        explosive.Explode();
            }
        }

        AudioManager.Instance.Play("explosion");
        Destroy(gameObject, 2.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
