using UnityEngine;
using System.Collections;

public class AttackerClubmanScript : AttackerScript
{

    protected override void PlayAttackAnimation()
    {
        float randomNumber = Random.value;
        if (randomNumber > 0.66)
        {
            animator.SetTrigger("attack1");
        }

        else if (randomNumber > 0.33)
        {
            animator.SetTrigger("attack2");
        }

        else
        {
            animator.SetTrigger("attack3");
        }

    }
}




