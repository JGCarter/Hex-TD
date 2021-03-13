using UnityEngine;
using System.Collections;


public class DefenderScript : MonoBehaviour
{

    public Rigidbody rb;
    public float speed = 2000f;

    private Vector3 targetPosition;
    public string mobTag = "mob";
    public float range = 3f;
    public float attackCountdown;
    public float attackSpeed = 0.3f;
    private Material material;
    private AudioSource audiosource;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 1;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);
        targetPosition = transform.position;
        Debug.Log(targetPosition);
        //InvokeRepeating("Attack", 0f, attackSpeed);
        material = GetComponent<Renderer>().material;
        attackCountdown = 1f / attackSpeed;
        audiosource = GetComponent<AudioSource>();

        StartCoroutine(AttackCheck());

        IEnumerator AttackCheck()
        {
            while (true)
            {
                if (attackCountdown <= 0)
                {
                    material.color = Color.green;
                    Attack();
                }

                attackCountdown -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }

    }


    void Update()
    {

        if (Vector3.Distance(transform.position, targetPosition) <= 0.3f)
        {
            return;
        }
        Vector3 dir = targetPosition - transform.position;

        rb.AddForce(dir.normalized * speed * Time.deltaTime);

    }



    void Attack()
    {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag(mobTag);
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
            attackCountdown = 1f / attackSpeed;
            material.color = Color.red;
            audiosource.Play();
            MobScript mob = nearestMob.GetComponent<MobScript>();
            mob.Fracture();
        }

        return;

    }

}
