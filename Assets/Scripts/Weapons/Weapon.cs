using NUnit.Framework.Constraints;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IUpgradeDamage, IUpgradeAmmo
{
    [SerializeField]
    protected WeaponInfo weaponInfo;

    protected int currentAmmo;

    protected bool timeBetweenShotsIsDirty = false;
    private float timeBetweenShots;
    public float TimeBetweenShots 
    {
        get 
        {
            if (timeBetweenShotsIsDirty) 
            {
                timeBetweenShots = 60.0f / weaponInfo.RPM;
                timeBetweenShotsIsDirty = false;
                return timeBetweenShots;
            }
            return timeBetweenShots;
        }
    }
         
    public bool HasUnlimitedAmmo => weaponInfo.MaxAmmo == -1;

    
    protected virtual void Start()
    {
        // small workaround for avoiding saving and serializing changed values (e.g. after an upgrade) of the WeaponInfo SO after leaving playmode 
        // we simply create a runtime instance of the SO that we can change whatever we like. Wont be saved into the SO in Assets folder
        weaponInfo = Instantiate(weaponInfo);
        RestockAmmo();
        timeBetweenShots = 60.0f / weaponInfo.RPM;
    }
    public abstract void Shoot(Transform firePoint);

    public WeaponInfo GetWeaponInfo() => weaponInfo;

    public int GetCurrentAmmo() => currentAmmo;

    public void RestockAmmo() => currentAmmo = weaponInfo.MaxAmmo;

    public void UpgradeWith(UpgradeInfo upgradeInfo) 
    {
        switch (upgradeInfo.Type)
        {
            case UpgradeType.Damage:
            {
                if (this is IUpgradeDamage upgradable) 
                    upgradable.UpgradeDamage(upgradeInfo.Value, upgradeInfo.IsMultiplicativeValue);
                else
                    Debug.LogError(weaponInfo.DisplayName + " is not upgradable with Upgrade of type: " + upgradeInfo.Type);
                break;
            }

            case UpgradeType.Ammo:
            {
                if (this is IUpgradeAmmo upgradable)
                    upgradable.UpgradeAmmo(upgradeInfo.Value, upgradeInfo.IsMultiplicativeValue);
                else
                    Debug.LogError(weaponInfo.DisplayName + " is not upgradable with Upgrade of type: " + upgradeInfo.Type);
                break;
            }
                
            case UpgradeType.FireRate:
            {
                if (this is IUpgradeFireRate upgradable)
                    upgradable.UpgradeFireRate(upgradeInfo.Value, upgradeInfo.IsMultiplicativeValue);
                else
                    Debug.LogError(weaponInfo.DisplayName + " is not upgradable with Upgrade of type: " + upgradeInfo.Type);
                break;
            }
            case UpgradeType.WeaponChange:
            {
                if (this is IUpgradeWeapon upgradable)
                    upgradable.UpgradeWeapon(upgradeInfo.NewWeaponPrefab);
                else
                    Debug.LogError(weaponInfo.DisplayName + " is not upgradable with Upgrade of type: " + upgradeInfo.Type);
                break;
            }

            default:
            {
                Debug.LogWarning("Weapon.cs cannot handle upgrade of type: " + upgradeInfo.Type);
                break;
            }
        }
    }
    #region Interface Implementations
    public void UpgradeDamage(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            weaponInfo.Damage = (int)(weaponInfo.Damage * value);
        else
            weaponInfo.Damage = (int)(weaponInfo.Damage + value);

        Debug.Log("Damage Upgrade made, new damage: " + weaponInfo.Damage);
    }
    public void UpgradeAmmo(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            weaponInfo.MaxAmmo = (int)(weaponInfo.MaxAmmo * value);
        else
            weaponInfo.MaxAmmo = (int)(weaponInfo.MaxAmmo + value);

        Debug.Log("Ammo Upgrade made, new ammo: " + weaponInfo.MaxAmmo);
    }

    #endregion
}
