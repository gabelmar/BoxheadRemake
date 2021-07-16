using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody rb;
    protected float speed;
    protected int damage;
    protected Vector3 direction = Vector3.zero;

    public Vector3 Direction 
    {
        get => direction;
        set => direction = value;
    }

    private void Start()
    {
        if (direction == Vector3.zero)
            direction = transform.forward;
    }
    public int Damage
    {
        get => damage;
        set => damage = value;
    }
    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag.Equals("Wall")){
            Destroy(gameObject);
        }

        if (other.tag.Equals("Enemy"))
        {
            AudioManager.Instance.Play("hitmarker");
            other.GetComponent<HealthSystem>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
