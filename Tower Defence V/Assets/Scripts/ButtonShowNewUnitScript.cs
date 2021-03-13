using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonShowNewUnitScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}
    public GameObject originTile;
    public GameObject upgradeUnit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        originTile.GetComponent<TileScript>().ShowNewUnit(upgradeUnit);
    }

    public void OnPointerExit(PointerEventData eventData)

    {
        originTile.GetComponent<TileScript>().HideNewUnit();
    }
}
