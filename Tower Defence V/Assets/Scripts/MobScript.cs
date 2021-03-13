using UnityEngine;
using System.Collections;


public class MobScript : MonoBehaviour
{

    public Rigidbody rb;
    public float speed = 2000f;

    private Transform target;
    private int wavepointIndex = 0;
    private Collider[] fractureCollider;
    private Rigidbody[] velocityArray;
    private Vector3 randomDirection;


    public GameObject fracturedVersion;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = WaypointsScript.points[0];
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        //transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        rb.AddForce(dir.normalized * speed * Time.deltaTime);
        rb.drag = 2;

        if (Vector3.Distance(transform.position, target.position) <= 3f)
        {
            GetNextWaypoint();
        }

    }

    void GetNextWaypoint()
    {
        if(wavepointIndex >= WaypointsScript.points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        wavepointIndex++;
        target = WaypointsScript.points[wavepointIndex];
    }

    public void Fracture()
    {
        GameObject fracturedMob =  Instantiate(fracturedVersion, transform.position, transform.rotation);

        //Vector3 dir = target.position - transform.position;

        Vector3 velocity = rb.velocity;
        velocityArray = fracturedMob.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in velocityArray)
        {
            randomDirection = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f) + 0.2f, Random.Range(0f, 1f));
            rigidbody.velocity = velocity + randomDirection * 5;
        }

        //fractureCollider = fracturedMob.GetComponentsInChildren<MeshCollider>();

    
        //StartCoroutine(RigidBodyDiableRoutine());
        //IEnumerator RigidBodyDiableRoutine()
        //{
        //    yield return new WaitForSeconds(1);
        //    Debug.Log("Coruotine started");

        //    foreach (MeshCollider collider in fractureCollider)
        //    {
        //        collider.enabled = false;
        //    }

        //}
        Destroy(gameObject);


        Destroy(fracturedMob, 2f);



    }


}
