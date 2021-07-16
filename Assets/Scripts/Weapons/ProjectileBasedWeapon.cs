using UnityEngine;

public class ProjectileBasedWeapon : Weapon, IUpgradeFireRate
{
    [SerializeField]
    protected Projectile projectilePrefab;

    public override void Shoot(Transform firePoint)
    {
        Projectile newBullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        newBullet.Speed = weaponInfo.ProjectileSpeed;
        newBullet.Damage = weaponInfo.Damage;
        if(!HasUnlimitedAmmo)
            currentAmmo--;
    }

    public void UpgradeFireRate(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            weaponInfo.RPM = (int)(weaponInfo.RPM * value);
        else
            weaponInfo.RPM = (int)(weaponInfo.RPM + value);

        timeBetweenShotsIsDirty = true;
        Debug.Log("Fire rate Upgrade made, new fire rate: " + weaponInfo.RPM);
    }
}
