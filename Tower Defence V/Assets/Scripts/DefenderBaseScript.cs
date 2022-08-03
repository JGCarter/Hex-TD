using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderBaseScript : MonoBehaviour
{
    protected GameMasterScript gameMasterScript;

    public Rigidbody rb;

    protected Vector3 targetPosition;

    //these are default values to be changed on a unit by unit basis
    public float maxHealth = 100f;
    public float currentHealth = 0f;
    public float force = 200000f;
    public float drag = 2f;
    public string targetMobTag = "Attacker";
    public float range = 3f;
    public float attackCountdown;
    public float attackSpeed = 0.5f;
    public float baseDamage = 40f;
    public string weaponType = "sword";
    public float hitDelay = 0.5f;
    public float impactMultiplyer = 20f;
    public GameObject targetMob;

    protected float nextAnimationTime = 0f;

    protected AudioSource audiosource;

    protected Animator animator;

    //sr

    protected virtual void Start()
    {
        gameMasterScript = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);
        targetPosition = transform.position;
        //Debug.Log(targetPosition);
        //InvokeRepeating("Attack", 0f, attackSpeed);
        attackCountdown = 1f / attackSpeed;
        audiosource = GetComponent<AudioSource>();
        nextAnimationTime = Time.time + Random.Range(5f, 120f);


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


    protected virtual void Update()
    {

        if (Vector3.Distance(transform.position, targetPosition) >= 0.3f)
        {
            //return;
            Vector3 dir = targetPosition - transform.position;
            rb.AddForce(dir.normalized * force * Time.deltaTime);
        }



        if (targetMob != null)
        {
            Vector3 direction = targetMob.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 2f * Time.deltaTime);
        }

        HandleIdleAnimation();
    }

    protected virtual void OnMouseOver()
    {
        gameObject.GetComponentInParent<TileScript>().OnMouseOver();
    }

    protected virtual void OnMouseExit()
    {
        gameObject.GetComponentInParent<TileScript>().OnMouseExit();
    }

    protected void OnMouseUp()
    {
        gameObject.GetComponentInParent<TileScript>().OnMouseUp();
    }




    protected virtual void Attack()
    {
        Vector3 impact = transform.forward * impactMultiplyer;

        GameObject[] mobs = GameObject.FindGameObjectsWithTag(targetMobTag);
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

        if (nearestMob != null && shortestDistance <= range)
        {
            targetMob = nearestMob;
            Vector3 direction = nearestMob.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.5f);

            attackCountdown = 1f / attackSpeed;
            //audiosource.Play();
            AttackerScript mob = nearestMob.GetComponent<AttackerScript>();
            mob.Hit(baseDamage, impact, hitDelay, weaponType);
            //animator.SetTrigger("attack");
            PlayAttackAnimation();

        }

        return;

    }

    protected virtual void PlayAttackAnimation()
    {
        float randomNumber = Random.value;
        if (randomNumber > 0)
        {
            animator.SetTrigger("attack");
        }

    }

    protected virtual void HandleIdleAnimation()
    {

    }



    public virtual void Hit(float damage)
    {

        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

}