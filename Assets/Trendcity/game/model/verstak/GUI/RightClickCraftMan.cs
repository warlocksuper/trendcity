using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickCraftMan : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData data)
    {
        //transform.parent.
            transform.parent.GetComponent<CraftSlot>().ordercount++;
            transform.parent.GetComponent<CraftSlot>().craftOrder.count++;
    }
}
