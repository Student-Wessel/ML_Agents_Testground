using System;
using System.Collections;
using System.Collections.Generic;
using CustomPropertyDrawers;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    [SerializeField] private GameObject gunParentTransform;
    [SerializeField] private GameObject gunShootDirection;
    [SerializeField] private GameObject gunWorldParent;

    [SerializeField] private LayerMask shootLayer;

    [TagSelector] [SerializeField] private string targetTag;
    
    public event Action<GameObject> onTargetHit;
    [SerializeField] private Gun gun;
    public bool HasGun() => gun != null;
    private void Start()
    {
        if (gun != null)
        {
            var gunTransform = gun.transform;
            gunTransform.SetParent(gunParentTransform.transform);
            gunTransform.localPosition = Vector3.zero;
        }
    }

    // Return true if we shot
    public bool TryShoot()
    {
        if (!HasGun())
            return false;

        if (!gun.TryShoot())
            return false;
        
        var shotTransform = gunShootDirection.transform;
        
        if (Physics.Raycast(shotTransform.position,shotTransform.forward, out RaycastHit hit,float.MaxValue,shootLayer))
        {
            var shootable = hit.collider.GetComponent<IShootable>();

            if (shootable != null)
                shootable.OnShot();
            
            if (hit.collider.CompareTag(targetTag))
            {
                onTargetHit?.Invoke(hit.collider.gameObject);
                Debug.DrawLine(shotTransform.position,hit.point,Color.green,0.75f);
            }
            Debug.DrawLine(shotTransform.position,hit.point,Color.red,0.75f);
        }
        else
        {
            Debug.DrawRay(shotTransform.position,shotTransform.forward*3f,Color.blue,0.75f);
        }

        return true;
    }

    // Completely reset everything from the handler
    // If reset gun is true, also resets the gun
    public void ResetGunInHandler()
    {
        gun.ResetGun();
    }
    
    public void GiveGun(Gun pGun)
    {
        DropGun();

        if (pGun == null)
        {
            gun = null;
            return;
        }
        
        gun = pGun;
        gun.ColliderEnabled = false;

        var gunTransform = gun.transform;
        gunTransform.SetParent(gunParentTransform.transform);
        gunTransform.localPosition = Vector3.zero;
        gunTransform.localRotation = Quaternion.identity;
    }

    public void DropGun()
    {
        if (HasGun())
        {
            gun.transform.SetParent(gunWorldParent.transform);
            gun.ColliderEnabled = true;
            gun = null;
        }
    }
    
    public int BulletCount()
    {
        if (!HasGun())
            return -1;

        return gun.CurrentBulletCount;
    }
}
