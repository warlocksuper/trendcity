using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeftClickCraftMan : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData data)
    {
        //transform.parent.
        if(transform.parent.GetComponent<CraftSlot>().ordercount > 0)
        {
            transform.parent.GetComponent<CraftSlot>().ordercount--;
            transform.parent.GetComponent<CraftSlot>().craftOrder.count--;
        }
    }
}
