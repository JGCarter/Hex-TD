using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
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
        animator.SetTrigger("attack");
    }

}
