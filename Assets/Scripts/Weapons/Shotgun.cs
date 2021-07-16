using UnityEngine;

public class Shotgun : RayCastBasedWeapon
{
    [SerializeField]
    private float spreadAngle;

    public override void Shoot(Transform firePoint)
    {
        Collider hitCollider;
        if (IsWeaponInsideBlockingObject(firePoint.position, out hitCollider))
        {
            HitObject(hitCollider.transform, weaponInfo.Damage);
            currentAmmo--;
            return;
        }

        EnableLineRenderes(true);
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            Vector3 direction;
            if (i == 0)
                direction = firePoint.forward;
            else if (i == 1)
                direction = Quaternion.AngleAxis(-spreadAngle / 2.0f, Vector3.up) * firePoint.forward;
            else if (i == 2)
                direction = Quaternion.AngleAxis(spreadAngle / 2.0f, Vector3.up) * firePoint.forward;
            else
                direction = Vector3.zero;

            // position of the firepos has to be corrected slightly since it is a bit away from the actual gun. 
            // Therefore Raycasts might not go through a very close zombie and wont detect a hit.
            Vector3 correctedFirePoint = firePoint.localPosition;
            correctedFirePoint.z = correctedFirePointLocalZ;
            correctedFirePoint = correctedFirePoint - firePoint.localPosition;
            lineRenderers[i].SetPosition(0, firePoint.TransformPoint(correctedFirePoint));
            ShootWithRay(firePoint.TransformPoint(correctedFirePoint), direction, i);
        }
        currentAmmo--;
        StartCoroutine(DisableLineRenderersAfterDelay());
    }
}
