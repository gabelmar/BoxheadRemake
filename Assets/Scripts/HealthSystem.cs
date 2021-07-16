using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHP;

    private int hp;

    public event Action<int> OnDamageTaken = delegate { };
    public event Action<int> OnDeath = delegate { };
    public event Action<int> OnHeal;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    public void TakeDamage(int damage) 
    {
        if (hp == 0)
            return;

        hp -= damage;
        if (hp < 0)
            hp = 0;

        OnDamageTaken?.Invoke(hp);
        if (hp == 0)
            OnDeath?.Invoke(hp);
    }

    public void Heal(int health) 
    {
        hp += health;
        if (hp > maxHP)
            hp = maxHP;
        OnHeal?.Invoke(hp);
    }
}
