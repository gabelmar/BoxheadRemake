using UnityEngine;

public class PlayerHealthBar : HealthBar
{
    [SerializeField]
    private Transform player;

    private Vector3 relativePosToPlayer;

    protected override void Start()
    {
        base.Start();
        health.OnHeal += UpdateSliderValue;
    }
}
