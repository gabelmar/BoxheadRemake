using UnityEngine;

public class RocketLauncher : ProjectileBasedWeapon, IUpgradeWeapon
{
    public void UpgradeWeapon(GameObject newPrefab)
    {
        projectilePrefab = newPrefab.GetComponent<Projectile>();
        Debug.Log("Upgraded to bigger rockets");
    }
}
