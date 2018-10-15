using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour {

	public InventarControler.ItemsType itemtype;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Transform playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

            if (Vector3.Distance(playerTrans.position, this.transform.position) < 2f)
            {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIO>().ShowQestWindows();
            }
        }
    }
}
