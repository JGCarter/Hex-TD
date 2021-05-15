using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHomingScript : MonoBehaviour
{
    public GameObject target;

    public GameObject impactEffect;
    public float projectileSpeed = 1f;
    Vector3 aimedTarget;
    Vector3 direction;

    public void Seek(GameObject _target, float _projectileSpeed)
    {
        target = _target;
        projectileSpeed = _projectileSpeed;

        Rigidbody targetRidgidbody = target.GetComponent<Rigidbody>();
        aimedTarget = target.transform.position + targetRidgidbody.velocity * (12f / projectileSpeed) + new Vector3(0f, 3f, 0f);

        projectileSpeed = _projectileSpeed;

        direction = aimedTarget - transform.position;
        transform.LookAt(target.transform);
        transform.Rotate(0f, 90f, 0f);
    }


    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            Debug.Log("error, target died before projectile could reach");
            return;
        }

        direction = aimedTarget - transform.position;

        float distanceThisFrame = projectileSpeed * Time.deltaTime;
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

    }


    void HitTarget()
    {
        //GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        //Destroy(effectInstance, 2f);

        Destroy(gameObject);
    }

}
