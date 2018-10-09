using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Block : MonoBehaviour {

    private PlayerIO playerIO;
    private NetworkLayerClient networkLayer;
    private GameObject hotbar;
    // Use this for initialization
    private void Awake()
    {
        playerIO = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIO>();
        if(playerIO.isNetwork)
        {
            networkLayer = GameObject.Find("NetworkManager").GetComponent<NetworkLayerClient>();
        }
        hotbar = playerIO.Prefhotbar;
    }

    void Start () {
        playerIO = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIO>();
        if (playerIO.isNetwork)
        {
            networkLayer = GameObject.Find("NetworkManager").GetComponent<NetworkLayerClient>();
        }
        hotbar = playerIO.Prefhotbar;//GameObject.Find("PlayerGui").transform.GetChild(2).gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        int SelectIndex = hotbar.GetComponent<Hotbar>().SelectIndex;//hotbar.GetComponent<Hotbar>().SelectIndex;
        RaycastHit hit;
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        ///////Удаление блока

        if (Input.GetMouseButtonDown(1))
        {

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            GameObject inventar = GameObject.Find("PlayerGui").transform.GetChild(1).gameObject;

            Color col = Color.red;
            if (Physics.Raycast(ray)) col = Color.green;

            Debug.DrawRay(ray.origin, ray.direction * 100, col);

            if (Physics.Raycast(ray, out hit, 10f))
            {
                string tag = hit.collider.tag;
                if (tag == "Block" || tag == "Door" || tag == "homeblock")
                {
                    ItemMy itemMy = hit.collider.gameObject.GetComponent<ItemMy>();
                    Item item = itemMy.Item;
                    if (playerIO.isNetwork)
                    {
                        //Vector3 newpos = CreatePosition(hit);
                        ///ItemMy itemMy = hit.collider.GetComponent<ItemMy>();
                        ///
                       //item
                        Home homecur = networkLayer.GetHomesInList(itemMy.homeid);
                        homecur.RemoveBlock(hit.collider.gameObject.transform.position);
                        //Debug.Log("Удаленние блока дом №" + homecur.idtable + " номер блока " + itemMy.id);
                        
                        if(tag == "homeblock")
                        {
                            networkLayer.RemoveHome(itemMy.homeid);
                        } else
                        {
                            networkLayer.RemoveBlock(itemMy.id, itemMy.homeid);
                        }
                        Destroy(hit.collider.gameObject);
                    } else
                    {
                        if (!inventar.GetComponent<Inventory>().checkIfItemAllreadyExist(item.itemID, 1))
                        {
                            inventar.GetComponent<Inventory>().addItemToInventory(item.itemID);
                        }

                        CitySaveAsset save = (CitySaveAsset)Resources.Load("CitySaveAsset");
                        save.removeItem(hit.collider.gameObject);
                        Destroy(hit.collider.gameObject);
                    }
                        

                    //save.addItem(item, newblock.transform.position);
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && SelectIndex != -1)
        {
            
            GameObject slot = hotbar.transform.GetChild(1).GetChild(SelectIndex).gameObject;
            

            if (slot.transform.childCount > 0)
            {
                GameObject invitem = slot.transform.GetChild(0).gameObject;
                Item item = invitem.GetComponent<ItemOnObject>().item;
                
                
                    // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                    
                    if (Physics.Raycast(ray, out hit))
                        {
                    if (item.itemType == ItemType.homeblock && hit.collider.tag == "Terrain") {
                        if (playerIO.isNetwork)
                        {
                            networkLayer.HomeListAdd(CreatePosition(hit));
                        } else { 
                            Gamelocal gamelocal = GameObject.Find("GameLocal").GetComponent<Gamelocal>();
                            gamelocal.city.HomeList.Add(new Home());
                            create_new_obj(item, hit, gamelocal.city.HomeList.Count);
                        }

                    }
                    if (item.itemType == ItemType.Block )
                    {
                        if (hit.collider.tag == "homeblock" || hit.collider.tag == "Block")
                            {
                            if (playerIO.isNetwork)
                            {
                                Vector3 newpos = CreatePosition(hit);
                                ItemMy itemMy = hit.collider.GetComponent<ItemMy>();
                                int homeid = itemMy.homeid;
                                networkLayer.CreateNewBlock(newpos, homeid, item.itemID);
                            } else
                            {
                                int homeid = hit.collider.GetComponent<ItemMy>().homeid;
                                create_new_obj(item, hit, homeid);
                                if (hotbar.transform.GetChild(1).GetChild(SelectIndex).childCount != 0 && hotbar.transform.GetChild(1).GetChild(SelectIndex).GetChild(0).GetComponent<ItemOnObject>().item.itemType != ItemType.UFPS_Ammo)
                                {
                                    if (hotbar.transform.GetChild(1).GetChild(SelectIndex).GetChild(0).GetComponent<ConsumeItem>().duplication != null && hotbar.transform.GetChild(1).GetChild(SelectIndex).GetChild(0).GetComponent<ItemOnObject>().item.maxStack == 1)
                                    {
                                        Destroy(hotbar.transform.GetChild(1).GetChild(SelectIndex).GetChild(0).GetComponent<ConsumeItem>().duplication);
                                    }
                                    hotbar.transform.GetChild(1).GetChild(SelectIndex).GetChild(0).GetComponent<ConsumeItem>().consumeIt();
                                }
                            }



                        }
                        }
                        // create_new_obj(Item item,)
                    }
                //item.itemModel;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.tag == "Block" || hit.collider.tag == "Door")
                {

                    Vector3 currentp = hit.collider.transform.localEulerAngles;
                    //float currenty;
                    if (isShiftKeyDown)
                    {
                        currentp.x += 90f;

                        //Debug.Log("Разворачиваем обьект shift текущий угол" + currentp.x);
                    } else
                    {
                        currentp.y += 90f;
                        //Debug.Log("Разворачиваем обьект текущий угол" + currentp.y);
                    }
                    
                    Quaternion target = Quaternion.Euler(currentp);
                    hit.collider.transform.rotation = target;
                    int homeid = hit.collider.transform.GetComponent<ItemMy>().homeid - 1;
                    if (playerIO.isNetwork)
                    {
                        Debug.Log("Разворачиваем обьект текущий угол x " + hit.collider.transform.localEulerAngles.x + "угол y " + hit.collider.transform.localEulerAngles.y);
                        ItemMy rotblock = hit.collider.gameObject.GetComponent<ItemMy>();
                        networkLayer.RotateBlock(rotblock.id, hit.collider.transform.localEulerAngles);
                    } else {
                        CitySaveAsset save = (CitySaveAsset)Resources.Load("CitySaveAsset");
                        save.rotateItem(hit.collider.transform.position, hit.collider.transform.localEulerAngles, homeid);
                    }
                        //RotateBlock


                }
            }

        }

        if (Input.GetKeyDown(KeyCode.R) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Block")
                {
                   // Debug.Log("Разворачиваем обьект текущий угол" + hit.collider.transform.localEulerAngles.y);
                    float currenty = hit.collider.transform.localEulerAngles.z;
                    currenty += 90f;
                    Quaternion target = Quaternion.Euler(0, 0, currenty);
                    hit.collider.transform.rotation = target;
                    int homeid = hit.collider.transform.GetComponent<ItemMy>().homeid - 1;
                    if (playerIO.isNetwork)
                    {
                        ItemMy rotblock = hit.collider.gameObject.GetComponent<ItemMy>();
                        networkLayer.RotateBlock(rotblock.id, hit.collider.transform.localEulerAngles);
                    }
                    else {
                        CitySaveAsset save = (CitySaveAsset)Resources.Load("CitySaveAsset");
                        save.rotateItem(hit.collider.transform.position, hit.collider.transform.localEulerAngles, homeid);
                    }


                }
            }

        }

    }


    void create_new_obj(Item item, RaycastHit hit,int homeid)
    {
        Vector3 p = hit.point;
        Vector3 newpos = CreatePosition(hit);
        Gamelocal gamelocal = GameObject.Find("GameLocal").GetComponent<Gamelocal>();
        if(hit.collider.tag == "Terrain")
        {
            gamelocal.city.HomeList[homeid - 1].position = new Vector3Serializer(newpos.x, newpos.y, newpos.z);
        }

        GameObject newblock = Instantiate(item.itemModel, newpos, Quaternion.identity);
        item.homeid = homeid;
        newblock.transform.parent = hit.collider.transform.parent;
        newblock.AddComponent<ItemMy>();
        newblock.GetComponent<ItemMy>().Item = item;
        newblock.GetComponent<ItemMy>().homeid = homeid;
       // if (hit.collider.tag == "Terrain")
       // {   
       //     p = newblock.transform.position;
       //     p.y += 0.5f;
      //      newblock.transform.position = p;
      //  }

        
        //gamelocal.city.HomeList[homeid].itemList.Add()

        CitySaveAsset save = (CitySaveAsset)Resources.Load("CitySaveAsset");
        save.addItem(item, newblock.transform.position);

    }

    public Vector3 CreatePosition(RaycastHit hit)
    {
        Debug.Log("CreatePosition position " + hit.point);
        Vector3 p = hit.point;
        Vector3 newpos = new Vector3();
        switch (hit.collider.tag)
        {
            case "Terrain":
                newpos = new Vector3(Mathf.FloorToInt(p.x), Mathf.FloorToInt(p.y), Mathf.FloorToInt(p.z));
                newpos.y += 0.5f;
                break;
            case "homeblock":
            case "Block":
                GameObject colob = hit.collider.gameObject;
                Vector3 min = hit.collider.bounds.min;
                Vector3 max = hit.collider.bounds.max;
                newpos = colob.transform.position;

                if (hit.point.x == min.x)
                {
                   // Debug.Log("left");
                    newpos.x -= 1f;
                }
                if (hit.point.x == max.x)
                {
                    //Debug.Log("right");
                    newpos.x += 1f;
                }
                if (hit.point.y == min.y)
                {
                    //Debug.Log("bottom");
                    newpos.y -= 1f;
                }
                if (hit.point.y == max.y)
                {
                    //Debug.Log("top");
                    newpos.y += 1f;
                }
                if (hit.point.z == min.z)
                {
                    //Debug.Log("front");
                    newpos.z -= 1f;
                }
                if (hit.point.z == max.z)
                {
                    //Debug.Log("back");
                    newpos.z += 1f;
                }
                break;
        }
        return newpos;
    }
}


[System.Serializable]
public class NetworkBlock : MessageBase
{

    public int id;
    public int owner;
    public int homeid;
    public int typeid;
    public int city;
    public Vector3 position;
    public Vector3 rotation;
    public GameObject gameObject;

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(id);
        writer.Write(owner);
        writer.Write(homeid);
        writer.Write(typeid);
        writer.Write(city);
        writer.Write(position);
        writer.Write(rotation);
        //base.Serialize (writer);
    }

    public override void Deserialize(NetworkReader reader)
    {
        id = reader.ReadInt32();
        owner = reader.ReadInt32();
        homeid = reader.ReadInt32();
        typeid = reader.ReadInt32();
        city = reader.ReadInt32();
        position = reader.ReadVector3();
        rotation = reader.ReadVector3();
    }


    public void Instant(Item item)
    {
        //ItemStore loaditem = city.HomeList[y].itemList[i];
        //Debug.Log("loadGame (itemList[i].coordX=" + itemList[i].coordX + " (itemList[i].coordY"+ itemList[i].coordY+ " (itemList[i].coordZ"+ itemList[i].coordZ+ " itemList[i].ItemID"+ itemList[i].ItemID);
        //Vector3 newp = new Vector3(loaditem.coordX, loaditem.coordY, loaditem.coordZ);

        //List<Item> items = Resources.Load
        //Item item = itemDatabase.itemList[loaditem.ItemID];
        //Debug.Log("instans network block positionX="+position.x+" positionY "+position.y+" positionZ="+position.z);
       
        gameObject = GameObject.Instantiate(item.itemModel, position, Quaternion.identity);
        GameObject parhome = GameObject.Find("Home_" + this.homeid);
        gameObject.name = item.itemModel.name + "_"+ this.id;
        gameObject.AddComponent<ItemMy>();
        gameObject.GetComponent<ItemMy>().id = this.id;
        gameObject.GetComponent<ItemMy>().Item = item;
        gameObject.GetComponent<ItemMy>().homeid = this.homeid;
        gameObject.transform.parent = parhome.transform;
        if (this.rotation != null)
        {
            Vector3 rotat = new Vector3(rotation.x, rotation.y, rotation.z);
            Quaternion target = Quaternion.Euler(rotat);
            gameObject.transform.rotation = target;
        }

    }


    public ItemStore toItemStore()
    {
        ItemStore itemStore = new ItemStore();
        itemStore.coordX = this.position.x;
        itemStore.coordY = this.position.y;
        itemStore.coordZ = this.position.z;
        itemStore.gameObject = this.gameObject;
        itemStore.cout = 1;
        itemStore.HomeId = this.homeid;
        itemStore.ItemID = this.typeid;
        itemStore.rotation = this.rotation;

        return itemStore;
    }

}
