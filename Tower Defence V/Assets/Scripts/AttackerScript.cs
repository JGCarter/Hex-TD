using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerScript : MonoBehaviour
{

    public GameObject gameMaster;
    GameMasterScript gameMasterScript;

    public Rigidbody rb;
    public float force = 1000f;
    public float drag = 1.2f;
    public float maxHealth = 100f;
    public float currentHealth = 0f;
    public int value;
    protected bool movementDisabled = false;

    public string targetMobTag = "Defender";

    public float range = 3f;
    public float attackCountdown;
    public float attackSpeed = 0.5f;
    public float baseDamage = 10f;

    protected Transform target;
    protected int wavepointIndex = 0;
    protected Collider[] fractureCollider;
    protected Rigidbody[] velocityArray;
    protected Vector3 randomDirection;

    protected AudioSource audioSource;

    public AudioClip coins;

    public AudioClip[] hurtBySword;
    public AudioClip[] hurtByArrow;

    public AudioClip[] deathBySword;
    public AudioClip[] deathByArrow;

    public AudioClip[] hurtByBlunt;
    public AudioClip[] deathByBlunt;



    protected Animator animator;

    public GameObject fracturedVersion;

    protected virtual void Start()
    {
        gameMaster = GameObject.Find("GameMaster");
        gameMasterScript = gameMaster.GetComponent<GameMasterScript>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
        target = WaypointsScript.points[0];
        currentHealth = maxHealth;
        attackCountdown = 1f / attackSpeed;

        audioSource = GetComponent<AudioSource>();


        StartCoroutine(AttackCheck());

    }


    protected virtual void Update()
    {

        if ((gameObject.tag != "Dead") && (!movementDisabled))
        {
            Vector3 dir = target.position - transform.position;
            rb.AddForce(dir.normalized * force * Time.deltaTime);

            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 1f);

            if (Vector3.Distance(transform.position, target.position) <= 3f)
            {
                GetNextWaypoint();
            }
        }

    }



    protected virtual void GetNextWaypoint()
    {
        if (wavepointIndex >= WaypointsScript.points.Length - 1)
        {
            gameMasterScript.SubtractLives(1);
            gameMasterScript.mobsRemaining--;
            Destroy(gameObject);
            return;
        }

        wavepointIndex++;
        target = WaypointsScript.points[wavepointIndex];
    }



    protected virtual IEnumerator AttackCheck()
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



    protected virtual void Attack()
    {
        movementDisabled = true;
        GameObject[] mobs = GameObject.FindGameObjectsWithTag(targetMobTag);
        float shortestDistanceSqared = Mathf.Infinity;
        GameObject nearestMob = null;
        foreach (GameObject mob in mobs)
        {
            float distanceToMobSqaured = Vector3.SqrMagnitude(mob.transform.position - transform.position);
            if (distanceToMobSqaured < shortestDistanceSqared)
            {
                shortestDistanceSqared = distanceToMobSqaured;
                nearestMob = mob;
            }
        }

        float shortestDistance = Mathf.Sqrt(shortestDistanceSqared);

        if (nearestMob != null && shortestDistance <= range)
        {
            attackCountdown = 1f / attackSpeed;
            //audiosource.Play();
            DefenderBaseScript mob = nearestMob.GetComponent<DefenderBaseScript>();
            mob.Hit(baseDamage);
            PlayAttackAnimation();


        }
        else
        {
            movementDisabled = false;
        }
        return;

    }



    public virtual void Hit(float damage, Vector3 impact, float hitDelay, string weaponType)
    {
        currentHealth = currentHealth - damage;
        //string weaponType = "sword";

        if (currentHealth <= 0)     //If the unit is killed
        {
            gameObject.tag = "MarkedForDeath";
            baseDamage = 0;
            StartCoroutine(DeathSound(weaponType, hitDelay));
            StartCoroutine(DeathRagdoll(impact, hitDelay));

        }

        else        //If the unit is hurt but not dead
        {
            StartCoroutine(HurtSound(weaponType, hitDelay));
            StartCoroutine(SimulateImpact(impact, hitDelay, weaponType));
        }
    }



    protected virtual IEnumerator SimulateImpact(Vector3 impact, float hitDelay, string weaponType)
    {
        yield return new WaitForSeconds(hitDelay);

        if (weaponType == "spear")
        {
            rb.velocity = Vector3.zero;
        }
        rb.AddForce(impact * 1000);
    }



    private IEnumerator Death(Vector3 impact, float hitDelay)
    {

        yield return new WaitForSeconds(hitDelay);
        GameObject fracturedMob = Instantiate(fracturedVersion, transform.position, transform.rotation);

        //Vector3 dir = target.position - transform.position;

        Vector3 velocity = rb.velocity;
        velocityArray = fracturedMob.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in velocityArray)
        {
            randomDirection = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f) + 0.2f, Random.Range(0f, 1f));
            rigidbody.velocity = velocity + impact + randomDirection * 1;
        }

        Destroy(gameObject);


        Destroy(fracturedMob, 2f);

    }



    protected virtual IEnumerator DeathRagdoll(Vector3 impact, float hitDelay)
    {
        yield return new WaitForSeconds(hitDelay);
        //Debug.Log(impact);

        gameObject.tag = "Dead";


        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }


        Vector3 bodyVelocity = GetComponent<Rigidbody>().velocity;

        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.velocity = bodyVelocity;
            rb.isKinematic = false;
            //if (impact.y < 0)
            //{
            //    impact.y = 0f;
            //}
            rb.AddForce(impact * 100);
        }


        GetComponent<Animator>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;

        gameObject.layer = 11;
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            child.GetComponent<Transform>().gameObject.layer = 11;
        }

        gameMasterScript.AddGold(value);
        gameMasterScript.mobsRemaining--;

        yield return new WaitForSeconds(10);
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<PlayerAnimator>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 50f);

    }



    public virtual IEnumerator DeathSound(string weaponType, float hitDelay)
    {
        yield return new WaitForSeconds(hitDelay - 0.2f);

        if (weaponType == "sword")
        {
            audioSource.PlayOneShot(deathBySword[Random.Range(0, deathBySword.Length)]);
        }

        else if (weaponType == "spear")
        {
            audioSource.PlayOneShot(deathBySword[Random.Range(0, deathBySword.Length)]);
        }

        else if (weaponType == "arrow")
        {
            audioSource.PlayOneShot(deathByArrow[Random.Range(0, deathByArrow.Length)]);
        }

        else if (weaponType == "blunt")
        {
            audioSource.PlayOneShot(deathByBlunt[Random.Range(0, deathByBlunt.Length)]);
        }

        audioSource.PlayOneShot(coins);

    }

    public virtual IEnumerator HurtSound(string weaponType, float hitDelay)
    {
        yield return new WaitForSeconds(hitDelay - 0.2f);

        if (weaponType == "sword")
        {
            audioSource.PlayOneShot(hurtBySword[Random.Range(0, hurtBySword.Length)]);
        }

        else if (weaponType == "spear")
        {
            audioSource.PlayOneShot(hurtBySword[Random.Range(0, hurtBySword.Length)]);
        }

        else if (weaponType == "arrow")
        {
            audioSource.PlayOneShot(hurtByArrow[Random.Range(0, hurtByArrow.Length)]);
        }

        else if (weaponType == "blunt")
        {
            audioSource.PlayOneShot(hurtByBlunt[Random.Range(0, hurtByBlunt.Length)]);
        }

    }

    protected virtual void PlayAttackAnimation()
    {
        float randomNumber = Random.value;
        if (randomNumber > 0)
        {
            animator.SetTrigger("attack");
        }

    }


}
