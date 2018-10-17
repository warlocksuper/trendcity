using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class verstack : MonoBehaviour {

    // Use this for initialization
    private Transform playerTrans = null;
    public bool iscrafting = false;
    private GameObject Worker;
    private Text guitext;
    private Inventory playerinventory;
    private ObjectNameView textonobj;
    public float timeLeft = 0;
    private Blueprint blueprint;
    private Gamelocal gamelocal;
    private PlayerIO playerIO;
    public List<CraftOrder> craftOrders;
    public CraftOrder Craftslot;
    BlueprintDatabase blueprintDatabase;
    private NetworkLayerClient networkLayer;


    private void Awake()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        playerIO = playerTrans.GetComponent<PlayerIO>();
        playerinventory = GameObject.Find("PlayerGui").transform.GetChild(1).GetComponent<Inventory>();
        textonobj = this.GetComponent<ObjectNameView>();
        if (playerIO.isNetwork) {
            networkLayer = GameObject.Find("NetworkManager").GetComponent<NetworkLayerClient>();
        } else {
            gamelocal = GameObject.Find("GameLocal").GetComponent<Gamelocal>();
        }
            
        blueprintDatabase = (BlueprintDatabase)Resources.Load("BlueprintDatabase");
        craftOrders = new List<CraftOrder>();
        foreach (var item in blueprintDatabase.blueprints)
        {
            CraftOrder neworder = new CraftOrder(item, 0);
            neworder.count = 0;
            neworder.blueprint = item;
            craftOrders.Add(neworder);
        }
    }
    // Update is called once per frame
    void Update() {

            if (!iscrafting)
            {
            int money = 0;
            if (playerIO.isNetwork)
            {
               // money= Player.instance.money;
                money = networkLayer.player.money;
            } else
            {
                money = gamelocal.PlayerMoney;
            }
                if (money > 0)
                {
                    bool isNotfind = true;
                    foreach (var item in craftOrders)
                    {
                        if (isNotfind)
                        {
                            if (item.count > 0)
                            {
                            if (playerIO.isNetwork)
                            {
                                networkLayer.StartCraft(gameObject, item);
                            }  else {
                                //Gamelocal gamelocal = GameObject.Find("GameLocal").GetComponent<Gamelocal>();
                                gamelocal.StartCraft(gameObject, item);

                            }

                            isNotfind = false;
                                iscrafting = true;
                                timeLeft = -5;
                                return;
                            }
                        }
                    }
                }
            }
       
        if (Input.GetKeyDown(KeyCode.E)) //&& !doorGo
        {
            if (Vector3.Distance(playerTrans.position, this.transform.position) < 2f)
            {
               // playerTrans.gameObject.GetComponent<PlayerIO>().CraftMan.GetComponent<GUICraftMan>().verstack = gameObject;
                playerTrans.gameObject.GetComponent<PlayerIO>().CraftMan.GetComponent<GUICraftMan>().UpdateOrderList(craftOrders);
                playerTrans.gameObject.GetComponent<PlayerIO>().CraftMan.SetActive(true);
            }
        }

            if (iscrafting && timeLeft >= 0)
            {
                textonobj = this.GetComponent<ObjectNameView>();
                timeLeft -= Time.deltaTime;
                if (timeLeft < 0)
                {
                    textonobj.text = "";
                    blueprint = Craftslot.blueprint;
                    if (!playerIO.isNetwork) {
                        if (!playerinventory.checkIfItemAllreadyExist(blueprint.finalItem.itemID, blueprint.amountOfFinalItem))
                        {
                            playerinventory.addItemToInventory(blueprint.finalItem.itemID, blueprint.amountOfFinalItem);
                        }
                    removeresource(blueprint);
                    }
                        // Craftslot.GetComponent<CraftSlot>().ordercount--;
                    Craftslot.count--; 
                    if(Craftslot.count <=0)
                    {
                    iscrafting = false;
                    Worker.GetComponent<WorkerController>().isbusy = false;
                    playerIO.CraftMan.GetComponent<GUICraftMan>().UpdateOrderList(craftOrders);
                }
                //else
                  //  {
                  //      int tjisid = this.gameObject.GetComponent<ItemMy>().id;
                  //      int homeid = this.gameObject.GetComponent<ItemMy>().homeid;
                   //     networkLayer.CraftItem(blueprint.finalItem.itemID, 1, tjisid, homeid);
                 //  }

                    // playerTrans.gameObject.GetComponent<PlayerIO>().CraftMan.GetComponent<GUICraftMan>().isCrafting = false;
                }
                else
                {
                    int stlefttime = (int)timeLeft;
                    textonobj.text = "<b>Создание " + blueprint.finalItem.itemName + " осталось  " + stlefttime.ToString() + " сек</b>";
                }

            }

    }

    public void StartCraft(GameObject worker, CraftOrder slot)
    {
        Worker = worker;
        Craftslot = slot;
        blueprint = slot.blueprint;
        Debug.Log("Start Craft verstack");
        if (check_ingrdients(blueprint))
        {
            iscrafting = true;
            timeLeft = blueprint.timeToCraft;
            int tjisid = this.gameObject.GetComponent<ItemMy>().id;
            int homeid = this.gameObject.GetComponent<ItemMy>().homeid;
            networkLayer.CraftItem(blueprint.finalItem.itemID, 1, tjisid, homeid);
            //networkLayer.CraftItem(blueprint.finalItem.itemID, Craftslot.count, tjisid, homeid);
            //Debug.Log("Start start Craft verstack");
        }
        else
        {
            if (playerIO.isNetwork)
            {
                MenuManager.instance.Debuglog("Нехватает ресурсов");
            } else
            {
                gamelocal = GameObject.Find("GameLocal").GetComponent<Gamelocal>();
                gamelocal.Debuglog("Нехватает ресурсов");
            }
                
            Worker.GetComponent<WorkerController>().isbusy = false;
            Debug.Log("Нехватает ресурсов");
            iscrafting = false;
            
           
        }
        //Item finalitem = Craftslot.GetComponent<CraftSlot>();

    }

    public bool check_ingrdients(Blueprint blueprint)
    {
        int counting = blueprint.ingredients.Count;
        int i = 0;
        for (i = 0; i < counting; i++)
        {
            playerinventory = GameObject.Find("PlayerGui").transform.GetChild(1).GetComponent<Inventory>();
            if (!playerinventory.check_ingrdient(blueprint.ingredients[i], blueprint.amount[i]))
            {
                return false;
            }
        }
        
        return true;
    }

    public void removeresource(Blueprint blueprint)
    {
        playerinventory = GameObject.Find("PlayerGui").transform.GetChild(1).GetComponent<Inventory>();
        int i = 0;
        for (i=0;i< blueprint.ingredients.Count;i++)
        {
            foreach (var item in playerinventory.ItemsInInventory)
            {
                if (item.itemID == blueprint.ingredients[i])
                {
                    item.itemValue -= blueprint.amount[i];
                    if (item.itemValue <= 0)
                    {
                        playerinventory.deleteItem(item);
                    }
                    
                }
            }

        }
    }

    public void FinalCraft(CraftNetwork craftNetwork)
    {
        Worker.GetComponent<WorkerController>().isbusy = false;
        iscrafting = false;
        /// Хз что с этим делать
        //  CraftOrder craftOrder = craftOrders.Find(x => x.blueprint.finalItem.itemID == craftNetwork.finalitem);
        //  if(craftOrder.count > 0)
        //  {
        //       craftOrder.count = craftNetwork.count;
        ///  }
    }

}

[System.Serializable]
public class CraftOrder 
{
    public Blueprint blueprint;
    public int count;

    public CraftOrder(Blueprint blueprint, int count)
    {
        this.blueprint = blueprint;
        this.count = count;
    }
}


[System.Serializable]
public class CraftNetwork : MessageBase
{
    public int finalitem;
    public int finalitemcount;
    public float timefull;
    public float timecount;
    public int count;
    public Player player;
    public int verstackid;
    public int homeid;


    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(finalitem);
        writer.Write(count);
        writer.Write(verstackid);
        writer.Write(homeid);
        //base.Serialize (writer);
    }

    public override void Deserialize(NetworkReader reader)
    {
        finalitem = reader.ReadInt32();
        count = reader.ReadInt32();
        verstackid = reader.ReadInt32();
        homeid = reader.ReadInt32();
    }

}