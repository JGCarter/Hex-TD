using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderClubmanScript : DefenderBaseScript
{
    protected override void PlayAttackAnimation()
    {
        float randomNumber = Random.value;
        if (randomNumber > 0.75)
        {
            animator.SetTrigger("attack1");
        }

        else if (randomNumber > 0.5)
        {
            animator.SetTrigger("attack2");
        }

        else if (randomNumber > 0.25)
        {
            animator.SetTrigger("attack3");
        }

        else
        {
            animator.SetTrigger("attack4");
        }

    }

    protected override void HandleIdleAnimation()
    {

        if (gameMasterScript.getBuildingPhase())
        {
            animator.SetBool("idle", true);
            if (Time.time > nextAnimationTime)
            {
                float randomNumber = Random.value;
                if (randomNumber > 0.75)
                {
                    animator.SetTrigger("idleAnimation1");
                }

                else if (randomNumber > 0.5)
                {
                    animator.SetTrigger("idleAnimation2");
                }

                else if (randomNumber > 0.25)
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
