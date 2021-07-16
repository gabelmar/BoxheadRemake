using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int maxHP;
    [SerializeField]
    private HealthSystem health;
    [SerializeField]
    private Score score;
    [SerializeField]
    private PlayerShoot playerShoot;
    [SerializeField]
    private Inventory inventory;

    public event Action<Weapon> OnPickup;
    public event Action<Weapon> OnWeaponEquipped;
    public event Action<Enemy> OnEnemyKilled;
    public event Action<Weapon, UpgradeInfo> OnWeaponUpgrade;
    public event Action<Weapon> OnWeaponUnlocked;
    

    // Start is called before the first frame update
    void Start()
    {
        health.OnDeath += Die;
        inventory.InitializeWithStartWeapon(playerShoot.GetCurrentWeapon());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Time.timeScale = 3f;
        if (Input.GetKeyUp(KeyCode.T))
            Time.timeScale = 1f;
    }

    public void KilledEnemy(Enemy enemy) 
    {
        score.Increase(enemy.PointsForKilling);
        OnEnemyKilled?.Invoke(enemy);
    }

    public void PickupWeaponOfType(WeaponType weaponType) 
    {
        inventory.RestockWeaponOfType(weaponType);

        /// TODO: CHANGE THIS WHEN PICKUP SYSTEM IS FINISHED SO THAT ONLY WEAPON THAT ARE IN THE INVENTORY CAN BE PIKCED UP
        Weapon pickedUpWeapon = inventory.GetWeaponOfType(weaponType);
        if (pickedUpWeapon != null)
            OnPickup?.Invoke(pickedUpWeapon);
    }
    public void PickupHealth(int hp) 
    {
        health.Heal(hp);
    }

    public void SwitchWeapon(bool next) 
    {
        Weapon weapon = inventory.GetNextWeapon(next);
        if (weapon != null) 
        {
            Debug.Log(weapon.GetWeaponInfo().DisplayName);
            playerShoot.EquipWeapon(weapon);
            OnWeaponEquipped?.Invoke(weapon);
        }   
    }

    public void MakeWeaponUpgrade(UpgradeInfo upgrade) 
    {
        inventory.MakeWeaponUpgrade(upgrade);
        OnWeaponUpgrade?.Invoke(inventory.GetWeaponOfType(upgrade.WeaponType), upgrade);
    }

    public void UnlockWeapon(Weapon weapon) 
    {
        inventory.AddWeapon(weapon);
        OnWeaponUnlocked?.Invoke(weapon);
    }
    
    private void Die(int hp)
    {
        SceneManager.LoadScene(0);
    }
}
