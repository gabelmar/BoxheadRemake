using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyType type;
    [SerializeField]
    private int pointsForKilling;
    [SerializeField]
    private HealthSystem health; 

    public EnemyType Type => type;
    public int PointsForKilling 
    {
        get => pointsForKilling;
        set => pointsForKilling = value; 
    }
}
