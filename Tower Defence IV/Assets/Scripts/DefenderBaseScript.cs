using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderBaseScript : MonoBehaviour
{

    public Rigidbody rb;
    public float speed = 2000f;

    protected Vector3 targetPosition;

    public float maxHealth = 100f;
    public float currentHealth = 0f;

    public string targetMobTag = "Attacker";
    public float range = 3f;
    public float attackCountdown;
    public float attackSpeed = 0.5f;
    public float baseDamage = 40f;
    public string weaponType = "sword";
    public GameObject targetMob;

    protected AudioSource audiosource;

    protected Animator animator;

    //sr

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        rb.drag = 1;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);
        targetPosition = transform.position;
        //Debug.Log(targetPosition);
        //InvokeRepeating("Attack", 0f, attackSpeed);
        attackCountdown = 1f / attackSpeed;
        audiosource = GetComponent<AudioSource>();

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
            rb.AddForce(dir.normalized * speed * Time.deltaTime);
        }



        if (targetMob != null)
        {
            Vector3 direction = targetMob.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 2f * Time.deltaTime);
        }

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
        Vector3 impact = transform.forward * 20;

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
            BaseAttackerScript mob = nearestMob.GetComponent<BaseAttackerScript>();
            float hitDelay = 0.5f;
            mob.Hit(baseDamage, impact, hitDelay, weaponType);
            animator.SetTrigger("attack");

        }

        return;

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
        Destroy(gameObject);
    }

}