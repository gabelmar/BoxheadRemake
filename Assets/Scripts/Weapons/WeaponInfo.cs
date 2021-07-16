using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "ScriptableObjects/WeaponInfo", order = 1)]
public class WeaponInfo : ScriptableObject
{
    public string DisplayName;
    public int Damage;
    public int RPM;
    public float ProjectileSpeed;
    public int MaxAmmo;
    public WeaponType Type;
}
