using System;
using AgentUtils;
using CustomPropertyDrawers;
using ShootOut;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Gun : MonoBehaviour, ISpawnable
{
    [SerializeField] private int magSize,currentBulletCount;
    [SerializeField] private float rps,reloadTime;
    public int CurrentBulletCount { get => currentBulletCount; }
    public bool IsReloading { get => isReloading; }

    public bool ColliderEnabled
    {
        get => collider.enabled;
        set => collider.enabled = value;
    }
        
    private bool isReloading;
    private float maxTimeBetweenShots;
    private float timeBetweenShots;
    private float reloadCountDown;

    private Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        reloadCountDown = 0;
        timeBetweenShots = 0;
        currentBulletCount = magSize;
        maxTimeBetweenShots = 1f / rps;
    }

    private void FixedUpdate()
    {
        timeBetweenShots = Mathf.Clamp(timeBetweenShots - Time.fixedDeltaTime, -0.1f, maxTimeBetweenShots);
        if (isReloading)
        {
            reloadCountDown = Mathf.Clamp(reloadCountDown - Time.fixedDeltaTime, -0.1f, reloadTime);
            if (reloadCountDown <= 0)
            {
                currentBulletCount = magSize;
                isReloading = false;
                reloadCountDown = -0.1f;
            }
        }
    }

    // Tries to shoot, looks at the current bullet count,and if your time between shots is less than 0, and if we are not reloading
    public bool TryShoot()
    {
        if (currentBulletCount > 0 && timeBetweenShots <= 0 && !isReloading)
        {
            Shoot();
            return true;
        }
        if (currentBulletCount < 1) // If we have less than 1 bullet, we want to reload automatically
        {
            Reload();
        }
        return false;
    }

    public void Reload()
    {
        if (!isReloading)
        {
            isReloading = true;
            reloadCountDown = reloadTime;
        }
    }

    private void Shoot()
    {
        timeBetweenShots = maxTimeBetweenShots;
        currentBulletCount--;
    }

    public void ResetGun()
    {
        reloadCountDown = 0;
        timeBetweenShots = 0;
        currentBulletCount = magSize;
        maxTimeBetweenShots = 1f / rps;
        isReloading = false;
    }
    
    public void SpawnPosition(Vector3 pPosition)
    {
        transform.position = new Vector3(pPosition.x, 0, pPosition.z);
    }

    public void SpawnRotation(Quaternion pRotation)
    {
        transform.rotation = pRotation;
    }

    public void SpawnPositionRotation(Vector3 pPosition, Quaternion pRotation)
    {
        transform.position = new Vector3(pPosition.x, 0, pPosition.z);
        transform.rotation = pRotation;
    }
}