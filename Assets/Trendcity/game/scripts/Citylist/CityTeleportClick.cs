using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CityTeleportClick : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData data)
    {
        //transform.parent.
        NetworkCity networkCity = transform.parent.GetComponent<CitySlot>().city;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIO>().onTeleport(""+networkCity.id);    
    }
}
