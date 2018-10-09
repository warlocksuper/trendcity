using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour {

    public string Itemname;
    public int itemcount;

    public void SetItem(int count,string name)
    {
       // invetartextcount[index].enabled = true;
       // invetartextcount[index].text = "" + count;

        string pathtoimg = "items/icon/" + name;
        Texture2D invimg = Resources.Load<Texture2D>(pathtoimg);

        GetComponentInChildren<Text>().text = count.ToString();
        GetComponentInChildren<RawImage>().texture = invimg;
        //invetarimadgslot[index].texture = invimg;
        //invetarimadgslot[index].color = new Color(255, 255, 255, 255);
    }
}
