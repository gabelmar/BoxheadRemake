using System.Collections;
using System.Linq;
using UnityEngine;

public class RayCastBasedWeapon : Weapon, IUpgradeFireRate
{
    [SerializeField]
    protected LineRenderer[] lineRenderers;

    [SerializeField]
    protected float maxRange;

    /// <summary>
    /// The correct z position of the FirePoint in local space where you want the actual ray to start.
    /// Look up FirePoint Transform in Player prefab and find good value  
    /// </summary>
    [SerializeField]
    protected float correctedFirePointLocalZ;

    protected int amountOfRays;
    protected RaycastHit[] hitInfos;

    protected override void Start()
    {
        base.Start();
        amountOfRays = lineRenderers.Length;
        hitInfos = new RaycastHit[amountOfRays];
    }
    public override void Shoot(Transform firePoint)
    {
        Collider hitCollider;
        if (IsWeaponInsideBlockingObject(firePoint.position, out hitCollider))
        {
            HitObject(hitCollider.transform, weaponInfo.Damage);
            currentAmmo--;
            return;
        }

        if (amountOfRays == 1)
        {
            EnableLineRenderes(true);

            // position of the firepos has to be corrected slightly since it is a bit away from the actual gun. 
            // Therefore Raycasts might not go through a very close zombie and wont detect a hit.
            Vector3 correctedFirePoint = firePoint.localPosition;
            correctedFirePoint.z = correctedFirePointLocalZ;
            correctedFirePoint = correctedFirePoint - firePoint.localPosition;
            lineRenderers[0].SetPosition(0, firePoint.TransformPoint(correctedFirePoint));
            ShootWithRay(firePoint.TransformPoint(correctedFirePoint), firePoint.forward, 0);

            currentAmmo--;
            StartCoroutine(DisableLineRenderersAfterDelay());
        }
        else
            Debug.LogError("Base class RayCastBasedWeapon does not fully support shooting multiple rays yet. Consider deriving from this class with own logic (like Shotgun)");
    }

    protected void ShootWithRay(Vector3 origin, Vector3 direction, int index)
    {
        if (Physics.Raycast(origin, direction, out hitInfos[index], maxRange))
        {
            lineRenderers[index].SetPosition(1, hitInfos[index].point);
            HitObject(hitInfos[index].transform, weaponInfo.Damage / amountOfRays);
        }
        else
            lineRenderers[index].SetPosition(1, origin + direction * maxRange);
    }

    protected bool IsWeaponInsideBlockingObject(Vector3 position, out Collider hitCollider)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, 0.7f);
        Collider closest = hitColliders.FirstOrDefault(col => col.CompareTag("Enemy") || col.CompareTag("Wall"));
        if (closest != null)
        {
            hitCollider = closest;
            return true;
        }
        hitCollider = null;
        return false;
    }

    protected void HitObject(Transform hitObjectTransform, int damage) 
    {
        if (hitObjectTransform.CompareTag("Enemy"))
        {
            hitObjectTransform.GetComponent<HealthSystem>().TakeDamage(damage);
            AudioManager.Instance.Play("hitmarker");
        }
        else if (hitObjectTransform.CompareTag("Wall"))
        {
            var barrel = hitObjectTransform.transform.GetComponent<ExplosiveBarrel>();
            if (barrel != null)
                barrel.Explode();
        }
    }

    protected void EnableLineRenderes(bool enabled)
    {
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            lineRenderers[i].enabled = enabled;
        }
    }

    protected IEnumerator DisableLineRenderersAfterDelay(float time = 0.1f)
    {
        if (time == 0.0f)
            yield return null;
        else
            yield return new WaitForSeconds(time);

        EnableLineRenderes(false);
    }

    public void UpgradeFireRate(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            weaponInfo.RPM = (int)(weaponInfo.RPM * value);
        else
            weaponInfo.RPM = (int)(weaponInfo.RPM + value);

        timeBetweenShotsIsDirty = true;
        Debug.Log("Raycast based Fire rate Upgrade made, new fire rate: " + weaponInfo.RPM);
    }
}
