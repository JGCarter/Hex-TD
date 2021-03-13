using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderArcherScript : MonoBehaviour
{

    public Rigidbody rb;

    private Vector3 targetPosition;

    public float maxHealth = 100f;
    public float currentHealth = 0f;

    public string targetMobTag = "Attacker";
    public float range = 18f;
    public float attackCountdown;
    public float attackSpeed = 0.25f;
    public float baseDamage = 100f;
    public float projectileSpeed = 100f;
    public string weaponType = "arrow";

    private AudioSource audiosource;

    Animator animator;

    GameObject nearestMob = null;
    float shortestDistance = Mathf.Infinity;

    public GameObject arrowPrefab;
    public Transform firePoint;


    //initial setup for stats and the attacking check routine
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);
        targetPosition = transform.position;
        //Debug.Log(targetPosition);
        //InvokeRepeating("Attack", 0f, attackSpeed);
        attackCountdown = 0f;
        attackCountdown = attackCountdown - Random.Range(0f, 0.1f);
        audiosource = GetComponent<AudioSource>();
        animator.speed = 2*attackSpeed;
        StartCoroutine(AttackCheck());

        IEnumerator AttackCheck()
        {
            while (true)
            {
                if (attackCountdown <= 0)
                {
                    Attack();
                }

                attackCountdown -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }

    }


    void Update()
    {
        //if (attackCountdown <= 0)
        //{
        //    shortestDistance = Mathf.Infinity;
        //    GameObject[] mobs = GameObject.FindGameObjectsWithTag(targetMobTag);
        //    foreach (GameObject mob in mobs)
        //    {
        //        float distanceToMob = Vector3.Distance(transform.position, mob.transform.position);
        //        if (distanceToMob < shortestDistance)
        //        {
        //            shortestDistance = distanceToMob;
        //            nearestMob = mob;
        //        }
        //    }
        //}

        if (nearestMob != null && shortestDistance <= range)
        {
            Rigidbody targetRidgidbody = nearestMob.GetComponent<Rigidbody>();
            Vector3 direction = nearestMob.transform.position + targetRidgidbody.velocity * (30f / projectileSpeed) - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
        }

    }



    public void Attack()
    {
        Vector3 impact = transform.forward * 5;

        {
            shortestDistance = Mathf.Infinity;
            GameObject[] mobs = GameObject.FindGameObjectsWithTag(targetMobTag);
            foreach (GameObject mob in mobs)
            {
                float distanceToMob = Vector3.Distance(transform.position, mob.transform.position);
                if (distanceToMob < shortestDistance)
                {
                    shortestDistance = distanceToMob;
                    nearestMob = mob;
                }
            }
        }

        if (nearestMob != null && shortestDistance <= range)
        {

            float hitDelay = ((1f / attackSpeed) * 0.34f) ;
            Invoke("CreateArrow", hitDelay);
            hitDelay = hitDelay + shortestDistance / projectileSpeed;

            attackCountdown = 1f / attackSpeed;
            //audiosource.PlayDelayed(hitDelay);
            BaseAttackerScript mob = nearestMob.GetComponent<BaseAttackerScript>();
            mob.Hit(baseDamage, impact, hitDelay, weaponType);
            animator.SetTrigger("firing");
        }

        return;

    }

    private void CreateArrow()
    {
        GameObject arrowGO = GameObject.Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        ArrowScript arrow = arrowGO.GetComponent<ArrowScript>();

        if (arrow != null)
            arrow.Seek(nearestMob, projectileSpeed);

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);

    }
}