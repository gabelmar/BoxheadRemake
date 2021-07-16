using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Weapon> weapons;

    private int currentWeaponIndex = 0;

    private void Start()
    {
        GetComponent<Player>().OnWeaponEquipped += UpdateCurrentWeaponIndex;
    }

    public Weapon GetWeaponOfType(WeaponType weaponType) => weapons.Find(w => w.GetWeaponInfo().Type == weaponType);

    public void AddWeapon(Weapon weapon) 
    {
        if (!ContainsWeaponOfType(weapon.GetWeaponInfo().Type))
            weapons.Add(weapon);
        else
            Debug.LogWarning("Inventory already contains weapon: " + weapon.GetWeaponInfo().DisplayName);
    }

    public void InitializeWithStartWeapon(Weapon starterWeapon)
    {
        weapons = new List<Weapon>();
        AddWeapon(starterWeapon);
        currentWeaponIndex = 0;
    }

    public Weapon GetNextWeapon(bool next) 
    {
        if (weapons.Count == 1)
            return null;
        if (next) 
        {
            int index = currentWeaponIndex + 1;
            if (index >= weapons.Count)
                index = 0;
            return weapons[index];
        }
        else 
        {
            int index = currentWeaponIndex- 1;
            if (index < 0)
                index = weapons.Count - 1;
            return weapons[index];
        }
    }

    public void MakeWeaponUpgrade(UpgradeInfo upgrade) 
    {
        Weapon weapon = GetWeaponOfType(upgrade.WeaponType);
        weapon.UpgradeWith(upgrade);
        weapon.RestockAmmo();
    }

    public void RestockWeaponOfType(WeaponType weaponType) 
    {
        Weapon weapon = GetWeaponOfType(weaponType);
        if (weapon != null)
            weapon.RestockAmmo();
        else
            Debug.LogError("Weapon of type: " + weaponType + " is not present in the inventory");
    }

    private void UpdateCurrentWeaponIndex(Weapon weapon)
    {
        currentWeaponIndex = weapons.IndexOf(weapon);
    }

    private bool ContainsWeaponOfType(WeaponType weaponType) => weapons.Any(w => w.GetWeaponInfo().Type == weaponType);
}
