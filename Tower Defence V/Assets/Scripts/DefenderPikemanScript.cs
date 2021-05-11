using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderPikemanScript : DefenderBaseScript
{
    //public Rigidbody rb;
    //public float speed = 2000f;

    //private Vector3 targetPosition;

    //public float maxHealth = 100f;
    //public float currentHealth = 0f;

    //public string targetMobTag = "Attacker";
    //public float range = 3f;
    //public float attackCountdown;
    //public float attackSpeed = 0.5f;
    // public float baseDamage = 40f;
    //public string weaponType = "sword";
    public float pikeLength;

    //private AudioSource audiosource;

    //Animator animator;

    protected override void Update()
    {

        if (Vector3.Distance(transform.position, targetPosition) >= 0.3f)
        {
            //return;
            Vector3 dir = targetPosition - transform.position;
            rb.AddForce(dir.normalized * speed * Time.deltaTime);
        }



        //if (targetMob != null)
        //{
        //    Vector3 direction = targetMob.transform.position - transform.position;
        //    Quaternion lookRotation = Quaternion.LookRotation(direction);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 2f * Time.deltaTime);
        //}

    }

    protected override void Attack()
    {
        Vector3 impact = transform.forward * 20;

        Vector3 attackPoint = transform.position + (transform.forward * pikeLength);

        GameObject[] mobs = GameObject.FindGameObjectsWithTag(targetMobTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestMob = null;
        foreach (GameObject mob in mobs)
        {
            float distanceToMob = Vector3.Distance(attackPoint, mob.transform.position);
            if (distanceToMob < shortestDistance)
            {
                shortestDistance = distanceToMob;
                nearestMob = mob;
            }
        }

        if (nearestMob != null && shortestDistance <= range)
        {
            targetMob = nearestMob;
            //Vector3 direction = nearestMob.transform.position - transform.position;
            //Quaternion lookRotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.5f);

            attackCountdown = 1f / attackSpeed;
            //audiosource.Play();
            AttackerScript mob = nearestMob.GetComponent<AttackerScript>();
            float hitDelay = 0.5f;
            mob.Hit(baseDamage, impact, hitDelay, weaponType);
            animator.SetTrigger("attack");

        }

        return;

    }
}
