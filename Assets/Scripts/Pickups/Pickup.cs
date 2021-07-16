using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    protected WeaponType pickupWeaponType;
    [SerializeField]
    private int restoredHealthAmount;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.transform.CompareTag("Player"))
        {
            AudioManager.Instance.Play("pickup");
            Player player = other.transform.GetComponent<Player>();
            player.PickupWeaponOfType(pickupWeaponType);
            player.PickupHealth(restoredHealthAmount);
            Destroy(gameObject);
        }
    }
}
