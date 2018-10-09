using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar_script : MonoBehaviour {

    public  Image[] img_slot1;
    public  RawImage[] img_slot1_img;
    public  Texture[] itemstexture1;

    private static Image[] img_slot;
    private static RawImage[] img_slot_img;
    private static Texture[] itemstexture;
    // Use this for initialization
    void Start () {

        img_slot = img_slot1;
        img_slot_img = img_slot1_img;

        itemstexture = itemstexture1;

        // img_slot1_img[1].texture = (Texture)itemstexture1[1];

        //img_slot1.color = new Color(0.216f, 0.129f, 0.0f, 0.65f);
        //img_slot1_img.IndexOf()
        // img_slot1_img.texture = 
        if (img_slot1_img.Length > 0)
        {
            for (int i = 0; i < 6; i++)
            {
                RawImage x = img_slot1_img[i];
                x.color = new Color(255, 255, 255, 255);
                if (img_slot1_img.Length > 0)
                {
                    x.texture = (Texture)itemstexture1[i];
                }


            }
        }


    }

    public void select_item(int index_bar)
    {

        img_slot[0].color = new Color(0.170f, 0.170f, 0.170f, 0.65f);
        img_slot[1].color = new Color(0.170f, 0.170f, 0.170f, 0.65f);
        img_slot[2].color = new Color(0.170f, 0.170f, 0.170f, 0.65f);
        img_slot[3].color = new Color(0.170f, 0.170f, 0.170f, 0.65f);
        img_slot[4].color = new Color(0.170f, 0.170f, 0.170f, 0.65f);
        img_slot[5].color = new Color(0.170f, 0.170f, 0.170f, 0.65f);
        img_slot[index_bar].color = new Color(0.216f, 0.129f, 0.0f, 0.65f);
        
      


    }

	// Update is called once per frame
	void Update () {
		
	}
}
