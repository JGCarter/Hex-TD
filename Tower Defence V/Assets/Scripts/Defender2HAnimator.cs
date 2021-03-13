using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender2HAnimator : MonoBehaviour
{

    const float locomotionAnimationSmoothTime = 0.1f;

    Rigidbody agent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speedPercent = agent.velocity.magnitude / 10;
        //Debug.Log(speedPercent);
        animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
    }

    protected virtual void OnAttack()
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
