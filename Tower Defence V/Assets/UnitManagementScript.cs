using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManagementScript : MonoBehaviour
{

    int personelCount = 0;
    public GameObject personelPrefab;

    Vector3 position1 = new Vector3(0f, 0f, 0f);
    Vector3 position2 = new Vector3(-2.5f, 0, 0);
    Vector3 position3 = new Vector3(-1.2811f, 0, 2.21893f);
    Vector3 position4 = new Vector3(-1.2811f, 0, -2.21893f);
    Vector3 position5 = new Vector3(2.5f, 0, 0);
    Vector3 position6 = new Vector3(1.2811f, 0, -2.21893f);
    Vector3 position7 = new Vector3(1.2811f, 0, 2.21893f);


    // Start is called before the first frame update
    void Start()
    {
        personelCount = this.transform.childCount;
        //Debug.Log(personelCount);
        //personelPrefab = this.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReplenishUnit()
    {
        Debug.Log("starting replenishment");
        int i = 0;
        while (i < personelCount)
        {
            //transform.GetChild(i).gameObject.SetActive(true);
            if (this.transform.GetChild(i).gameObject != null)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
            i++;
        }


        if (personelCount > 0)
            Instantiate(personelPrefab, this.transform.position + position1, personelPrefab.transform.rotation, this.transform);

        if (personelCount > 1)
            Instantiate(personelPrefab, this.transform.position + position2, personelPrefab.transform.rotation, this.transform);

        if (personelCount > 2)
            Instantiate(personelPrefab, this.transform.position + position3, personelPrefab.transform.rotation, this.transform);

        if (personelCount > 3)
            Instantiate(personelPrefab, this.transform.position + position4, personelPrefab.transform.rotation, this.transform);

        if (personelCount > 4)
            Instantiate(personelPrefab, this.transform.position + position5, personelPrefab.transform.rotation, this.transform);

        if (personelCount > 5)
            Instantiate(personelPrefab, this.transform.position + position6, personelPrefab.transform.rotation, this.transform);

        if (personelCount > 6)
            Instantiate(personelPrefab, this.transform.position + position7, personelPrefab.transform.rotation, this.transform);
    }
}
