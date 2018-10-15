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
            var fwd = transform.TransformDirection(Vector3.forward);
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2f))
            {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIO>().ShowQestWindows();
            }
        }
    }
}
