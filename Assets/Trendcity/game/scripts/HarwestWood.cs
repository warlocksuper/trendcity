using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarwestWood : MonoBehaviour {
	public int harvestTreeDistance;    
	private Terrain terrain;            // Derived from hit...GetComponent<Terrain>
	private RaycastHit hit;                // For hit. methods
	private Transform myTransform;
	public GameObject LogSpawn;
	private GameObject felledTree;        // Prefab to spawn at terrain tree loc for TIIIIIIMBER!
    private PlayerIO playerIO;

    public List<RestoreWood> restoreWoods;
    // Use this for initialization
    void Start () {
        restoreWoods = new List<RestoreWood>();
        myTransform = transform;
		terrain = Terrain.activeTerrain;
        playerIO = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIO>();
	}
	
	// Update is called once per frame
	void Update () {


        int iw = 0;
        for(iw=0;iw< restoreWoods.Count;iw++)
        {
            if (restoreWoods[iw].timeLeft >= 0)
            {
                restoreWoods[iw].timeLeft -= Time.deltaTime;
                if (restoreWoods[iw].timeLeft < 0)
                {
                    Instantiate(restoreWoods[iw].felledTree, restoreWoods[iw].position, Quaternion.identity);
                    restoreWoods.Remove(restoreWoods[iw]);
                }
            }
        }

		Ray ray = Camera.main.ScreenPointToRay(new Vector3(0.5f, 0.5f, 0f));
		var fwd = transform.TransformDirection(Vector3.forward);

		if (Input.GetMouseButtonDown (0)) {
            //Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            

			if (Physics.Raycast (transform.position, fwd, out hit, 2f)) {
				
				//Debug.Log ("Capsule fount out");
				if (hit.collider.gameObject.tag == "Tree") {
                    if(playerIO.isNetwork)
                    {
                        HarvestWood2Network(hit);
                    } else
                    {
                        HarvestWood2(hit);
                    }
					
				} else
                {
                    Debug.Log("Cleck on " + hit.collider.gameObject.tag);
                }
			}
		}

		if (Input.GetKeyDown (KeyCode.E)) {

            
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
            RaycastHit hit;

            if (Physics.Raycast (ray, out hit, 2f)) {
                //Debug.Log ("Capsule fount out");
                if (hit.collider.gameObject.tag == "ITEM") {
					Debug.Log ("Harvest Item");
					HarvestITEM (hit);
					//HarvestWood2 (hit);
				} 
			}
		}
	}

	private void HarvestITEM(RaycastHit hit) {
		GameObject item = hit.collider.gameObject;
		
        var itemsname = item.name.Remove(item.name.IndexOf('('), item.name.Length - item.name.IndexOf('('));
        Debug.Log("Harvest " + itemsname);
        switch (itemsname)
        {
            case "Log":
                /*
                if (PlayerIO.isNetwork)
                {
                    MenuManager.instance.Debuglog("Собранны бревна");
                } else
                {
                    Gamelocal.instance.Debuglog("Собранны бревна");
                }
                */
                Debug.Log("Собранны бревна");
                //InventarControler.instance.addItem(itemsname, InventarControler.ItemsType.ITEM);
            break;
            
        }

        //Destroy(item);
	}


    private void HarvestWood2Network(RaycastHit hit)
    {
        GameObject tree = hit.collider.gameObject;
        tree.GetComponent<Rigidbody>().AddForce(transform.forward / 2);
        tree.GetComponent<MeshCollider>().enabled = false;
        tree.GetComponent<Rigidbody>().isKinematic = false;
        // tree.GetComponent<DestroyableTree>().Delete();
        Vector3 treePos = tree.transform.position;


        Destroy(tree, 5);
        //Vector3 position = treePos + Vector3(0,0,0);
        //Instantiate(logs, tree.transform.position + Vector3(0,0,0) + position, Quaternion.identity);

        Vector3 positionleg1 = treePos;
        Vector3 positionleg2 = treePos;
        positionleg1.x += 1;
        positionleg2.x += 2;
        positionleg2.y += 1;

        LogSpawn.AddComponent<PickUpItem>();
        Item item2 = new Item
        {
            itemID = 26,
            itemValue = 1
        };

        LogSpawn.GetComponent<PickUpItem>().item = item2;

        Instantiate(LogSpawn, treePos, Quaternion.identity);
        Instantiate(LogSpawn, positionleg1, Quaternion.identity);
        Instantiate(LogSpawn, positionleg2, Quaternion.identity);
        terrain = Terrain.activeTerrain;
        TreePrototype proto = terrain.terrainData.treePrototypes[1];
        restoreWoods.Add(new RestoreWood(hit.collider.gameObject.transform.position, proto));

    }

    private void HarvestWood2(RaycastHit hit) {
		int prototipeindex = -1;

		GameObject tree = hit.collider.gameObject;
		Vector3 treePos = tree.transform.position;
        
        int terrainindex = tree.GetComponent<DestroyableTree> ().terrainIndex;
		prototipeindex = terrain.terrainData.treeInstances[terrainindex].prototypeIndex;

		tree.GetComponent<DestroyableTree> ().Delete ();

		TreePrototype proto = terrain.terrainData.treePrototypes [prototipeindex];
		felledTree = proto.prefab;

		GameObject fellTree = Instantiate(felledTree,treePos,Quaternion.identity) as GameObject;
		fellTree.GetComponent<Rigidbody>().AddForce(transform.forward/2);
		fellTree.GetComponent<Rigidbody>().isKinematic = false;


		Destroy(fellTree,5);
		//Vector3 position = treePos + Vector3(0,0,0);
		//Instantiate(logs, tree.transform.position + Vector3(0,0,0) + position, Quaternion.identity);

		Vector3 positionleg1 = treePos;
		Vector3 positionleg2 = treePos;
		positionleg1.x += 1;
		positionleg2.x += 2;
		positionleg2.y += 1;

        LogSpawn.AddComponent<PickUpItem>();
        Item item2 = new Item
        {
            itemID = 26,
            itemValue = 1
        };

        LogSpawn.GetComponent<PickUpItem>().item = item2;

        Instantiate(LogSpawn, treePos , Quaternion.identity);
		Instantiate(LogSpawn, positionleg1 , Quaternion.identity);
		Instantiate(LogSpawn, positionleg2 , Quaternion.identity);

    }

}

[System.Serializable]
public class RestoreWood
{
    public float timeLeft = 100.0f;
    public Vector3 position;
    public GameObject felledTree;

    public RestoreWood(Vector3 position, TreePrototype proto)
    {

        felledTree = proto.prefab;
        felledTree.GetComponent<Rigidbody>().isKinematic = true;
        felledTree.tag = "Tree";
        this.position = position;
    }
}