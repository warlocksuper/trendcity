using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        //GameObject hotbar = GameObject.Find("PlayerGui").transform.GetChild(2).gameObject; //GameObject.FindGameObjectWithTag("Hotbar");
        
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.E)) // || Input.GetMouseButtonDown(0)
        {
            //Ray ray = Camera.main.ScreenPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Vector3 fwd = transform.TransformDirection(Vector3.forward) * 10;
                      
         //   Debug.DrawRay(transform.position, fwd, Color.green);
            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.green);
            if (Physics.Raycast(ray, out hit, 2f))
            {


                //Debug.Log("Cleck on " + hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag == "Magazin")
                {
                    Debug.Log("Shop fount");
                    //HarvestWood2(hit);
                    GameObject Shop = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIO>().ShopSystem;
                    if(Shop.activeSelf) {
                        Shop.SetActive(false);
                    } else
                    {
                        Shop.SetActive(true);
                    }
                    
                    Debug.Log("Cleck on " + hit.collider.gameObject.tag);
                } else
                {
                    Debug.Log("Cleck on " + hit.collider.gameObject.tag);
                }
            }
        }
    }
}
