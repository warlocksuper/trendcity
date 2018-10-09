using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUICraftMan : MonoBehaviour {

    // Use this for initialization
    public GameObject slotpref;
    BlueprintDatabase blueprintDatabase;
    public Transform Gripelement;
    public List<CraftOrder> craftOrders;

    // Update is called once per frame
    void Update () {
        /*
		if(!isCrafting)
        {
            foreach (var item in OrderList)
            {
                if(item.GetComponent<CraftSlot>().ordercount > 0)
                {
                    Gamelocal gamelocal = GameObject.Find("GameLocal").GetComponent<Gamelocal>();
                    gamelocal.StartCraft(verstack, item, OrderList);
                    isCrafting = true;
                    return;
                }
            }
            
        }
        */
	}

    public void UpdateOrderList(List<CraftOrder> craftOrders)
    {
        //Debug.Log("GUICraftMan Start");
        //blueprintDatabase = (BlueprintDatabase)Resources.Load("BlueprintDatabase");

        ClearGripelementList();
        foreach (var item in craftOrders)
        {
            GameObject newbluepr = Instantiate(slotpref, Gripelement, true);
            Image icon = newbluepr.transform.Find("Image").GetComponent<Image>();
            newbluepr.GetComponent<CraftSlot>().craftOrder = item;
            newbluepr.GetComponent<CraftSlot>().blueprint = item.blueprint;
            newbluepr.GetComponent<CraftSlot>().ordercount = item.count;
            icon.sprite = item.blueprint.finalItem.itemIcon;
            //OrderList.Add(newbluepr);
            //item.finalItem.itemIcon
        }
    }

    private void ClearGripelementList()
    {
        int y = 0;
        for(y=0;y<Gripelement.childCount;y++)
        {
            Destroy(Gripelement.GetChild(y).gameObject);
        }
    }
}
