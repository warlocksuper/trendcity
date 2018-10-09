using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventarControler : MonoBehaviour {

	public static ItemMy[] Inventar = new ItemMy[35];
    public static InventarControler instance;

    //public Text[] invetartextcount = new Text[35];
    //public RawImage[] invetarimadgslot =  new RawImage[35];
    public GameObject[] slots = new GameObject[35];

    private void Awake()
    {
     
    }

    public enum ItemsType
	{
		ITEM,
		INSTRUMENT,
		BOX,
		QUEST
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        
	}

    public void addItem(string name, InventarControler.ItemsType typeitem)
    {
        Debug.Log("addItem name"+name);
    }

    void refreshOnGui(int index, string name, int count)
    {

        if(count <= 0)
        {
            return;
        }


        //GameObject slots;
        /*
        if(index < 9)
        {
            slots = GameObject.Find("Slot_inv_0" + (index +1));
        } else
        {
            slots = GameObject.Find("Slot_inv_" + (index+1));
        }
        */

        GameObject itemobject = null;

        if (slots[index].transform.childCount > 0)
        {
            itemobject = slots[index].transform.GetChild(0).gameObject;
        } else
        {

            itemobject = Instantiate(Resources.Load<GameObject>("items/item"), new Vector3(0,0), Quaternion.identity);
            itemobject.transform.parent = slots[index].transform;
            itemobject.transform.position = slots[index].transform.position;
            

        }

        itemobject.GetComponent<ItemObject>().SetItem(count, name);
        // itemobject =       slots.transform.Find("imginv");

        /*
        if (!PlayerIO.isNetwork) { 

        } else
        {
            //string findname = "Slot_0" + (index + 1);
            invetartextcount[index].enabled = true;
            invetartextcount[index].text = "" + count;

            RawImage invimg = Resources.Load("items/icon/" + Inventar[index].NameItem + ".png") as RawImage;

            invetarimadgslot[index].texture = invimg.texture;


            

            
        }
        */

        // GameObject countinv = inventpref.GetComponent<Text>();

        // countinv.text = "" + count;
        //GameObject inventpref =  GameObject.Find("pref_inventar");
    }
}

