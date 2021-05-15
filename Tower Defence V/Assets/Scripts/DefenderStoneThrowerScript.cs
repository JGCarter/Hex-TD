using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderStoneThrowerScript : MonoBehaviour
{
    GameMasterScript gameMasterScript;

    public Rigidbody rb;

    private Vector3 targetPosition;

    float maxHealth = 100f;
    public float currentHealth = 0f;

    public string targetMobTag = "Attacker";
    public float range = 18f;
    public float animationLength = 2f;
    public float attackCountdown;
    public float attackSpeed = 0.5f;
    public float baseDamage = 10f;
    public float projectileSpeed = 40f;
    public string weaponType = "blunt";
    public float projectileSpawnDelayFactor = 0.2f;

    //private AudioSource audiosource;
    Animator animator;

    GameObject nearestMob = null;
    float shortestDistance = Mathf.Infinity;
    float nextAnimationTime = 0f;

    public GameObject projectilePrefab;
    public Transform firePoint;


    //initial setup for stats and the attacking check routine
    void Start()
    {
        gameMasterScript = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        //gameMasterScript = GetComponent<GameMasterScript>();
        animator = GetComponent<Animator>();
        nextAnimationTime = Time.time + Random.Range(5f, 120f);
        currentHealth = maxHealth;
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);
        targetPosition = transform.position;
        //Debug.Log(targetPosition);
        //InvokeRepeating("Attack", 0f, attackSpeed);
        attackCountdown = 0f;
        attackCountdown = attackCountdown - Random.Range(0f, 0.1f);
        //audiosource = GetComponent<AudioSource>();
        //animator.speed = animationLength * attackSpeed;
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

        HandleIdleAnimation();
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

            float hitDelay = ((1f / attackSpeed) * projectileSpawnDelayFactor);
            Invoke("CreateProjectile", hitDelay);
            hitDelay = hitDelay + shortestDistance / projectileSpeed;

            attackCountdown = 1f / attackSpeed;
            //audiosource.PlayDelayed(hitDelay);
            AttackerScript mob = nearestMob.GetComponent<AttackerScript>();
            mob.Hit(baseDamage, impact, hitDelay, weaponType);
            animator.SetTrigger("firing");
        }

        return;

    }

    private void CreateProjectile()
    {
        GameObject projectileGameObject = GameObject.Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        ProjectileHomingScript projectileHomingScript = projectileGameObject.GetComponent<ProjectileHomingScript>();

        if (projectileHomingScript != null)
            projectileHomingScript.Seek(nearestMob, projectileSpeed);

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);

    }

    private void HandleIdleAnimation()
    {

        if (gameMasterScript.getBuildingPhase())
        {
            animator.SetBool("idle", true);
            if (Time.time > nextAnimationTime)
            {
                float randomNumber = Random.value;
                if (randomNumber > 0.60f)
                {
                    animator.SetTrigger("idleAnimation1");
                }

                else if (randomNumber > 0.30f)
                {
                    animator.SetTrigger("idleAnimation2");
                }

                else if (randomNumber > 0.20f)
                {
                    animator.SetTrigger("idleAnimation3");
                }

                else
                {
                    animator.SetTrigger("idleAnimation4");
                }
                nextAnimationTime = Time.time + Random.Range(30f, 120f);
            }
        }

        else
        {
            animator.SetBool("idle", false);
            nextAnimationTime = Time.time + Random.Range(5f, 15f);
        }


    }
}
