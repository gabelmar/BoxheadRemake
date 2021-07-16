using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeInfo", menuName = "ScriptableObjects/UpgradeInfo", order = 2)]
public class UpgradeInfo : ScriptableObject
{
    public WeaponType WeaponType;
    public UpgradeType Type;
    public float Value;
    public bool IsMultiplicativeValue;
    public int UnlockMultiplier;
    public string DisplayName;
    public GameObject NewWeaponPrefab;
}
