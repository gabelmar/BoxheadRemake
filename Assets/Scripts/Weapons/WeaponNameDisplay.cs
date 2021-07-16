using TMPro;
using UnityEngine;

public class WeaponNameDisplay : MonoBehaviour
{
    private TextMeshProUGUI weaponNameText;
    private readonly string unformattedString = "{0}: {1}";
    private string currentWeaponName;

    [SerializeField]
    private Player player;
    private PlayerShoot playerShoot;

    private void Awake()
    {
        playerShoot = player.GetComponentInChildren<PlayerShoot>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        weaponNameText = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        player.OnWeaponEquipped += UpdateWeaponName;
        playerShoot.OnWeaponShot += UpdateWeaponAmmo;
        player.OnWeaponUpgrade += RefreshWeaponAmmo;  
    }

    
    private void OnDisable()
    {
        var player = FindObjectOfType<Player>();
        if (player)     // check against null pointers when shutting down
        {
            player.OnWeaponEquipped -= UpdateWeaponName;
            playerShoot.OnWeaponShot -= UpdateWeaponAmmo;
            player.OnWeaponUpgrade -= RefreshWeaponAmmo;            
        }                                         
    }

    private void UpdateWeaponName(Weapon weapon)
    {
        currentWeaponName = weapon.GetWeaponInfo().DisplayName;
        if (weapon.HasUnlimitedAmmo)
            weaponNameText.text = currentWeaponName;
        else
            weaponNameText.text = string.Format(unformattedString, currentWeaponName, weapon.GetCurrentAmmo());
    }
    private void UpdateWeaponAmmo(Weapon weapon, int ammo)
    {
        if (currentWeaponName == null)
            currentWeaponName = weapon.GetWeaponInfo().DisplayName;

        if (weapon.HasUnlimitedAmmo)
            weaponNameText.text = weapon.GetWeaponInfo().DisplayName;
        else 
            weaponNameText.text = string.Format(unformattedString, currentWeaponName, ammo);      
    }

    /// <summary>
    /// Called whenever a weapon upgrade is made. Updates the ammo accordingly if its an ammo upgrade.
    /// </summary>
    /// <param name="weapon">weapon that has been upgraded</param>
    /// <param name="upgrade">the upgrade info</param>
    private void RefreshWeaponAmmo(Weapon weapon, UpgradeInfo upgrade)
    {
        if(upgrade.Type == UpgradeType.Ammo)
            weaponNameText.text = string.Format(unformattedString, weapon.GetWeaponInfo().DisplayName, weapon.GetCurrentAmmo());
    }
}
