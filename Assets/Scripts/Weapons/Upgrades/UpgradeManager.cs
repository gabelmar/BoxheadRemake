using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    private List<UpgradeInfo> allUpgrades;
    [SerializeField]
    private List<Weapon> weaponsToUnlock;

    [SerializeField]
    private Player player;
    public UpgradeInfo testUpgrade;

    private int maxMultiplierReached = 1;

    // maps multiplier to corresponding upgrade that unlocks at this multiplier
    private Dictionary<int, UpgradeInfo> upgradeTable = new Dictionary<int, UpgradeInfo>();

    private void Awake()
    {
        PopulateUpgradeTable();
    }

    private void OnEnable()
    {
        Score.OnMultiplierIncreased += HandleMultiplierIncreased;
    }
    private void OnDisable()
    {
        Score.OnMultiplierIncreased -= HandleMultiplierIncreased;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) 
        {
            if (testUpgrade.Type == UpgradeType.UnlocksWeapon)
            {
                Debug.Log("Instantiate");
                Weapon weapon = Instantiate(weaponsToUnlock.Find(w => w.GetWeaponInfo().Type == testUpgrade.WeaponType));
                player.UnlockWeapon(weapon);
            }
            else
            {
                player.MakeWeaponUpgrade(testUpgrade);
            }
        }
    }

    private void PopulateUpgradeTable()
    {
        for (int i = 0; i < allUpgrades.Count; i++) 
        {
            if (upgradeTable.ContainsKey(allUpgrades[i].UnlockMultiplier))
                Debug.LogWarning("There is more than one UpgradeInfo that has the  UnlockMultiplier of " + allUpgrades[i].UnlockMultiplier);
            else
                upgradeTable.Add(allUpgrades[i].UnlockMultiplier, allUpgrades[i]);
        }
    }

    private void HandleMultiplierIncreased(int newMultiplier)
    {
        if (newMultiplier > maxMultiplierReached) 
        {
            maxMultiplierReached = newMultiplier;
            if (upgradeTable.ContainsKey(newMultiplier)) 
            {
                TriggerUpgrade(upgradeTable[newMultiplier]);
            }   
        }
    }

    private void TriggerUpgrade(UpgradeInfo upgrade) 
    {
        if (upgrade.Type == UpgradeType.UnlocksWeapon)
        {
            Weapon weapon = Instantiate(weaponsToUnlock.Find(w => w.GetWeaponInfo().Type == upgrade.WeaponType));
            player.UnlockWeapon(weapon);
        }
        else
        {
            player.MakeWeaponUpgrade(upgrade);
        }
    }
}
