using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender2HScript : DefenderBaseScript

{


    protected override void Attack()
    {
        Vector3 impact = transform.forward * 30;

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

            attackCountdown = 1f / attackSpeed;
            //audiosource.Play();
            BaseAttackerScript mob = nearestMob.GetComponent<BaseAttackerScript>();
            float hitDelay = 0.6f;
            mob.Hit(baseDamage, impact, hitDelay, weaponType);
            PlayRandomAnimation();

            StartCoroutine(AOEDamage());

            IEnumerator AOEDamage()
            {
                yield return new WaitForSeconds(hitDelay);              

                foreach (GameObject mob2 in mobs)
                {
                    if (mob2 != null)
                    {
                        float distanceToMob = Vector3.Distance(transform.position, mob2.transform.position);
                        if (distanceToMob < range && mob2 != nearestMob)
                        {
                            BaseAttackerScript mobScript = mob2.GetComponent<BaseAttackerScript>();
                            mobScript.Hit(baseDamage, impact, 0f, weaponType);
                        }
                    }
                }

                //rb.AddForce(impact * 1000);
            }


        }

        return;

    }





    private void PlayRandomAnimation()
    {
        if (Random.value > 0.5)
        {
            animator.SetTrigger("attack1");
        }

        else
        {
            animator.SetTrigger("attack2");
        }

    }

}