using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject muzzleParticlePrefab;

    private bool isFiring;
    private float shotTimeCounter;

    [SerializeField]
    private Transform firePoint;

    private ParticleSystem muzzleFlash;
    private bool isMuzzleFlashGOActive;

    [SerializeField]
    private Weapon currentWeapon;

    public event Action<Weapon, int> OnWeaponShot;

    private float delayAfterWeaponSwap = 0.25f;
    
    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash = Instantiate(muzzleParticlePrefab, firePoint.position, firePoint.rotation, firePoint).GetComponent<ParticleSystem>();
        muzzleFlash.gameObject.SetActive(false);

        if (currentWeapon)
            currentWeapon = Instantiate(currentWeapon);
    }
    /// <summary>
    /// Reset isFiring to false when game has been resumed from pause. 
    /// Otherwise we would keep firing when button was held until pause since we disable PlayerController on Pause => no input detection.
    /// </summary>
    private void OnEnable()
    {
        GameManager.OnResume += ResetIsFiring;
    }
    private void OnDisable()
    {
        GameManager.OnResume -= ResetIsFiring;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFiring)
        {
            shotTimeCounter -= Time.deltaTime;
            if (shotTimeCounter <= 0)
            {
                // Out of ammo
                if (currentWeapon.GetCurrentAmmo() == 0) 
                {
                    AudioManager.Instance.Play("emptymag");
                    shotTimeCounter = currentWeapon.TimeBetweenShots;
                    return;
                }

                //Trigger Shooting
                shotTimeCounter = currentWeapon.TimeBetweenShots;
                if (!isMuzzleFlashGOActive) 
                {
                    muzzleFlash.gameObject.SetActive(true);
                    isMuzzleFlashGOActive = true;
                }
                currentWeapon.Shoot(firePoint);
                AudioManager.Instance.PlayWeaponShotSound(currentWeapon.GetWeaponInfo().Type);
                muzzleFlash.Play(true);
                OnWeaponShot?.Invoke(currentWeapon, currentWeapon.GetCurrentAmmo());
            }
        }
        else
        {
            shotTimeCounter -= Time.deltaTime;
            if (shotTimeCounter <= 0)
                shotTimeCounter = 0;
        }
    }

    public void EquipWeapon(Weapon weapon) 
    {
        currentWeapon = weapon;
        shotTimeCounter = delayAfterWeaponSwap;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void SetFiring(bool firing){
        isFiring = firing;
    }

    private void ResetIsFiring()
    {
        isFiring = false;
    }
}
