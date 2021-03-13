using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public Transform target;

    [Header("Attributes")]
    public float range = 12f;
    public float fireRate = 1f;
    private float fireCounddown = 0f;


    [Header("Unity Setup Fields")]
    public string mobTag = "mob";

    public Transform partToRotate;
    public float rotationSpeed = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;



    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag(mobTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestMob = null;
        foreach (GameObject mob in mobs)
        {
            float distanceToMob = Vector3.Distance(transform.position, mob.transform.position);
            if (distanceToMob < shortestDistance)
            {
                shortestDistance = distanceToMob;
                nearestMob = mob;
            }
        }

        if(nearestMob != null && shortestDistance <= range)
        {
            target = nearestMob.transform;
        }
        else
        {
            target = null;
        }
     
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.z, rotation.y, rotation.x);


        if(fireCounddown <= 0)
        {
            Shoot();
            fireCounddown = 1f / fireRate;
        }

        fireCounddown -= Time.deltaTime;
    }

    void Shoot()
    {
        Debug.Log("shoot");
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletScript bullet = bulletGO.GetComponent<BulletScript>();
        if (bullet != null)
            bullet.Seek(target);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

    }
}
