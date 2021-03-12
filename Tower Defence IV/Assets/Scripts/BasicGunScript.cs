using System.Collections;
using UnityEngine;

public class BasicGunScript : MonoBehaviour
{

    public Transform target;

    [Header("Attributes")]
    public float range = 12f;
    public float fireRate = 2f;
    private float fireCounddown = 0f;


    [Header("Unity Setup Fields")]
    public string mobTag = "mob";

    public Transform partToRotate;
    public float rotationSpeed = 12f;


    public GameObject laserPrefab;
    public Transform firePoint;
    private float distanceToMob = 0f;
    public float trueShortestDistance = Mathf.Infinity;

    GameObject nearestMob = null;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag(mobTag);
        float shortestDistance = Mathf.Infinity;
        //nearestMob = null;
        foreach (GameObject mob in mobs)
        {
            distanceToMob = Vector3.Distance(transform.position, mob.transform.position);
            if (distanceToMob < shortestDistance)
            {
                shortestDistance = distanceToMob;
                trueShortestDistance = shortestDistance;

                nearestMob = mob;
            }
        }

        if (nearestMob != null && shortestDistance <= range)
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
        Vector3 direction = target.position - partToRotate.position;
        direction.y = direction.y - 0.625f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, 1f);
        //partToRotate.rotation = Quaternion.Euler(rotation.z, rotation.y, 0f);


        if (fireCounddown <= 0)
        {
            Shoot();
            fireCounddown = 1f / fireRate;
        }

        fireCounddown -= Time.deltaTime;
    }

    void Shoot()
    {
        Debug.Log("shoot");
        trueShortestDistance = Vector3.Distance(firePoint.transform.position, nearestMob.transform.position);
        float laserScale = trueShortestDistance;
        GameObject laser = GameObject.Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        laser.transform.localScale = laser.transform.localScale * laserScale;
        Destroy(laser, 0.3f);
        MobScript mob = target.GetComponent<MobScript>();
        mob.Fracture();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

    }
}