using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour {

    public Blueprint blueprint;
    public int ordercount;
    public CraftOrder craftOrder;
    // Use this for initialization

	// Update is called once per frame
	void Update () {
        transform.GetChild(1).GetComponent<InputField>().text = ordercount.ToString();
        //Text inputcount = transform.GetChild(1).Find("Text").GetComponent<Text>();
        //inputcount.text = ordercount.ToString();
        //Debug.Log("CraftSlot child name "+ inputcount.name);
    }

    public void changekeybord()
    {
        ordercount = Convert.ToInt32(transform.GetChild(1).GetComponent<InputField>().text);
        craftOrder.count = ordercount;
    }
}
