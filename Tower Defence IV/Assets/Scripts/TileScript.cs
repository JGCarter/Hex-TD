using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class TileScript : MonoBehaviour
{

    //public GameObject archerUnit;
    public GameObject currentUnit;
    [SerializeField]
    GameObject upgradeChoice1;
    [SerializeField]
    GameObject upgradeChoice2;
    [SerializeField]
    GameObject upgradeChoice3;

    GameObject ghostUnit;
    public GameObject rangeShower;
    GameObject tempRangeShower;


    public Button upgradeButtonTemplate;
    private Button upgradeButtonTop;
    private Button upgradeButtonLeft;
    private Button upgradeButtonRight;
    public float buttonDistance = 180;

    private GameObject canvas;
    public GameMasterScript gameMasterScript;
    public Camera mainCamera;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        currentUnit = Instantiate(currentUnit);
    }

    //   // Update is called once per frame
    //   void Update()
    //   {

    //   }

    //Highlight the tile
    public void OnMouseOver()
    {
        if (gameMasterScript.upgrading == false)
        {
            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        }
    }

    public void OnMouseExit()
    {
        GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }

    public void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && (!gameMasterScript.upgrading))   //if the UI is not intercepting the click
        {
            gameMasterScript.PlayUIClick();
            //Debug.Log("Tile Clicked");

            StartCoroutine(RightClickListener());


            canvas = GameObject.FindGameObjectWithTag("UICanvas");
            float canvasXScale = canvas.GetComponent<RectTransform>().localScale.x;
            float canvasYScale = canvas.GetComponent<RectTransform>().localScale.y;


            upgradeButtonTop = Instantiate(upgradeButtonTemplate, canvas.transform);
            upgradeButtonTop.transform.position = mainCamera.WorldToScreenPoint(gameObject.transform.position) + new Vector3(0f, buttonDistance * canvasYScale, 0f);

            upgradeButtonLeft = Instantiate(upgradeButtonTemplate, canvas.transform);
            upgradeButtonLeft.transform.position = mainCamera.WorldToScreenPoint(gameObject.transform.position) + new Vector3((-buttonDistance * 0.866f * canvasXScale), (buttonDistance * 0.866f * 0.5f) * canvasYScale, 0f);

            upgradeButtonRight = Instantiate(upgradeButtonTemplate, canvas.transform);
            upgradeButtonRight.transform.position = mainCamera.WorldToScreenPoint(gameObject.transform.position) + new Vector3((buttonDistance * 0.866f * canvasXScale), (buttonDistance * 0.866f * 0.5f) * canvasYScale, 0f);

            UpgradeUnitScript upgradeScript = currentUnit.GetComponent<UpgradeUnitScript>();

            upgradeChoice1 = upgradeScript.upgrade1;
            if (upgradeChoice1 != null)
            {
                UpgradeUnitScript upgrade1NameFetcher = upgradeChoice1.GetComponent<UpgradeUnitScript>();
                upgradeButtonTop.GetComponentInChildren<Text>().text = upgrade1NameFetcher.unitName;
                upgradeButtonTop.GetComponentsInChildren<Text>()[1].text = (upgrade1NameFetcher.unitCost - upgradeScript.unitCost).ToString();
                upgradeButtonTop.onClick.AddListener(delegate { TaskOnClick(upgradeChoice1); });
                upgradeButtonTop.GetComponent<ButtonShowNewUnitScript>().originTile = gameObject;
                upgradeButtonTop.GetComponent<ButtonShowNewUnitScript>().upgradeUnit = upgradeChoice1;
            }
            else
            {
                upgradeButtonTop.GetComponentInChildren<Text>().text = "no unit found";
                upgradeButtonTop.gameObject.SetActive(false);
            }

            upgradeChoice2 = upgradeScript.upgrade2;
            if (upgradeChoice2 != null)
            {
                UpgradeUnitScript upgrade2NameFetcher = upgradeChoice2.GetComponent<UpgradeUnitScript>();
                upgradeButtonLeft.GetComponentInChildren<Text>().text = upgrade2NameFetcher.unitName;
                upgradeButtonLeft.GetComponentsInChildren<Text>()[1].text = (upgrade2NameFetcher.unitCost - upgradeScript.unitCost).ToString();
                upgradeButtonLeft.onClick.AddListener(delegate { TaskOnClick(upgradeChoice2); });
                upgradeButtonLeft.GetComponent<ButtonShowNewUnitScript>().originTile = gameObject;
                upgradeButtonLeft.GetComponent<ButtonShowNewUnitScript>().upgradeUnit = upgradeChoice2;
            }
            else
            {
                upgradeButtonLeft.GetComponentInChildren<Text>().text = "no unit found";
                upgradeButtonLeft.gameObject.SetActive(false);
            }

            upgradeChoice3 = upgradeScript.upgrade3;
            if (upgradeChoice3 != null)
            {
                UpgradeUnitScript upgrade3NameFetcher = upgradeChoice3.GetComponent<UpgradeUnitScript>();
                upgradeButtonRight.GetComponentInChildren<Text>().text = upgrade3NameFetcher.unitName;
                upgradeButtonRight.GetComponentsInChildren<Text>()[1].text = (upgrade3NameFetcher.unitCost - upgradeScript.unitCost).ToString();
                upgradeButtonRight.onClick.AddListener(delegate { TaskOnClick(upgradeChoice3); });
                upgradeButtonRight.GetComponent<ButtonShowNewUnitScript>().originTile = gameObject;
                upgradeButtonRight.GetComponent<ButtonShowNewUnitScript>().upgradeUnit = upgradeChoice3;
            }
            else
            {
                Debug.Log("button disabled");
                upgradeButtonRight.GetComponentInChildren<Text>().text = "no unit found";
                upgradeButtonRight.gameObject.SetActive(false);
            }


            gameMasterScript.SetUpgradeingValue(true);
        }

    }

    void TaskOnClick(GameObject chosenUpgrade)
    {
        Debug.Log("Button Clicked");
        gameMasterScript.PlayUIClick();

        int cost = chosenUpgrade.GetComponent<UpgradeUnitScript>().unitCost - currentUnit.GetComponent<UpgradeUnitScript>().unitCost;

        if (gameMasterScript.SubtractGold(cost))
        {
            Destroy(currentUnit);

            currentUnit = Instantiate(chosenUpgrade, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z), Quaternion.identity);

            currentUnit.transform.SetParent(gameObject.transform);


            //clean up
            StopAllCoroutines();

            upgradeChoice1 = null;
            upgradeChoice2 = null;
            upgradeChoice3 = null;
            Destroy(upgradeButtonTop.gameObject);
            Destroy(upgradeButtonLeft.gameObject);
            Destroy(upgradeButtonRight.gameObject);
            Destroy(ghostUnit);
            Destroy(tempRangeShower);

            gameMasterScript.SetUpgradeingValue(false);
            gameMasterScript.ChangePrecheckingUpgradeValue(false);
        }

    }



    public void ShowNewUnit(GameObject fakeUpgrade)
    {
        //Debug.Log("Showing new unit");
        foreach (MeshRenderer meshRenderer in currentUnit.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in currentUnit.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            skinnedMeshRenderer.enabled = false;
        }

        ghostUnit = Instantiate(fakeUpgrade, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z), Quaternion.identity);
        float range = ghostUnit.GetComponent<UpgradeUnitScript>().range * 2;

        foreach (MonoBehaviour script in ghostUnit.GetComponentsInChildren<MonoBehaviour>())
        {
            script.enabled = false;
        }
        foreach (Collider collider in ghostUnit.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rb in ghostUnit.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }
        //Debug.Log(range);
        tempRangeShower = Instantiate(rangeShower);
        tempRangeShower.transform.localScale = tempRangeShower.transform.localScale * range;
        tempRangeShower.transform.position = ghostUnit.transform.position;

        gameMasterScript.ChangePrecheckingUpgradeValue(true);

    }

    public void HideNewUnit()
    {
        //Debug.Log("Hiding new unit");
        foreach (MeshRenderer meshRenderer in currentUnit.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = true;
        }
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in currentUnit.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            skinnedMeshRenderer.enabled = true;
        }

        gameMasterScript.ChangePrecheckingUpgradeValue(false);

        Destroy(ghostUnit);
        Destroy(tempRangeShower);
    }

    private IEnumerator RightClickListener()
    {
        while (!Input.GetMouseButtonDown(1))
        {
            //Debug.Log("listener running");
            yield return 0;          
        }

        Debug.Log("force closing");

        upgradeChoice1 = null;
        upgradeChoice2 = null;
        upgradeChoice3 = null;
        Destroy(upgradeButtonTop.gameObject);
        Destroy(upgradeButtonLeft.gameObject);
        Destroy(upgradeButtonRight.gameObject);
        Destroy(ghostUnit);
        Destroy(tempRangeShower);

        gameMasterScript.SetUpgradeingValue(false);
        gameMasterScript.ChangePrecheckingUpgradeValue(false);
        yield break;


    }

}
