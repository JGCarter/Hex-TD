using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesScript : MonoBehaviour
{

    GameMasterScript gameMasterScript;

    public AudioSource[] audioSource;
    public AudioClip lightningAudioClip;
    public GameObject lightningGameObject;
    public GameObject pointLightGameObject;
    public GameObject postPrecessingVolumeObject;
    public PostProcessingEffectsScript postProcessingEffectsScript;

    public bool explosionAbility = true;
    public GameObject explosionPrefab;


    public float range = 5f;
    public float baseDamage = 200f;
    public Vector3 impact = Vector3.zero;
    public string weaponType = "none";
    public string targetTag = "Attacker";

    // Start is called before the first frame update
    void Start()
    {
        gameMasterScript = GetComponent<GameMasterScript>();
        audioSource = GetComponents<AudioSource>();
        postPrecessingVolumeObject = GameObject.FindGameObjectWithTag("PostProsessing");
        postProcessingEffectsScript = postPrecessingVolumeObject.GetComponent<PostProcessingEffectsScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameMasterScript.getBuildingPhase())
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(raycast, out RaycastHit hitPoint, 1000f);
                transform.position = hitPoint.point;
                audioSource[1].PlayOneShot(lightningAudioClip);
                Instantiate(lightningGameObject, transform);
                GameObject thisFlash = Instantiate(pointLightGameObject, transform);
                Destroy(thisFlash, 0.2f);
                postProcessingEffectsScript.darkenScreenEffect(0.5f);

                //AudioSource.PlayClipAtPoint(lightningAudioClip, hitPoint.point);
                Debug.Log(hitPoint.point);
                GameObject[] mobs = GameObject.FindGameObjectsWithTag(targetTag);
                foreach (GameObject mob in mobs)
                {
                    if (mob != null)
                    {
                        float distanceToMob = Vector3.Distance(hitPoint.point, mob.transform.position);
                        if (distanceToMob < range)
                        {
                            AttackerScript mobScript = mob.GetComponent<AttackerScript>();
                            impact = (mob.transform.position + new Vector3(0, 3, 0) - hitPoint.point);
                            impact = impact / (impact.magnitude * impact.magnitude);
                            impact = impact * 100;
                            mobScript.Hit(baseDamage / (distanceToMob + 1), impact, 0f, weaponType);
                        }
                    }
                }

            }

            if (Input.GetMouseButtonUp(1))
            {
                Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(raycast, out RaycastHit hitPoint, 1000f);
                transform.position = hitPoint.point;
                //Instantiate(lightningGameObject, transform);
                GameObject thisExplosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
                Destroy(thisExplosion, 10f);
                postProcessingEffectsScript.darkenScreenEffect(0.5f);

                //AudioSource.PlayClipAtPoint(lightningAudioClip, hitPoint.point);
                //Debug.Log(hitPoint.point);
                GameObject[] mobs = GameObject.FindGameObjectsWithTag(targetTag);
                foreach (GameObject mob in mobs)
                {
                    if (mob != null)
                    {
                        float distanceToMob = Vector3.Distance(hitPoint.point, mob.transform.position);
                        if (distanceToMob < range)
                        {
                            AttackerScript mobScript = mob.GetComponent<AttackerScript>();
                            impact = (mob.transform.position + new Vector3(0, 3, 0) - hitPoint.point);
                            impact = impact / (impact.magnitude * impact.magnitude);
                            impact = impact * 100;
                            mobScript.Hit(baseDamage / (distanceToMob + 1), impact, 0f, weaponType);
                        }
                    }
                }

            }



        }
    }
}
