using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PackegType
{
    NetworkRegistred = 1,
    NetworkLogin,
    NetworkAccess,
    GetCity,
    GetInventory,
    BuyItems,
    SellItems,
    GetMoney,
    CreateNewHome,
    CreateNewBlock,
    GetItemInventory,
    RemoveBlock,
    HarwestWood,
    RotateBlock,
    CraftItem,
    RemoveHome,
    SendChat
}


public class Crypt
{

    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
}

public class NetworkLayerClient : MonoBehaviour {

    private const int MAX_CONNECTION = 100;
    public int port = 5701;
    public string serverip = "10.0.0.110";
    public int hostID;
    public int webHostID;
    public NetworkCity citynetwork;
    private string networkConnections;

    private int reliableChannel;
    private int unreliableChannel;
    private int reliableFragChannel;
    private int unreliableFragChannel;
    public int connectionId;
    public int channelId;
    public bool isConnected = false;
    private byte error;
    private bool isStarted = false;
    public float connectionTime;
    public GameObject playerController;
    public GameObject playerGUI;
    public Inventory playerInventory;
    private PlayerIO playerIO;
    public int PlayerMoney = 0;
    private GameObject txmoney;
    private GameObject Txmanount;
    public int workercount=0;
    private bool isGame = false;
    public List<Home> homes;
    private ItemDataBaseList itemDatabase;
    // Use this for initialization
    void Start () {
        itemDatabase = (ItemDataBaseList)Resources.Load("ItemDatabase");
        NetworkTransport.Init();
        homes = new List<Home>();
    }
	

    public void connectserver()
    {
        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);
        reliableFragChannel = cc.AddChannel(QosType.ReliableFragmented);
        unreliableFragChannel = cc.AddChannel(QosType.UnreliableFragmented);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostID = NetworkTransport.AddHost(topo, 0);
        //webHostID = NetworkTransport.AddWebsocketHost(topo, 0);
        connectionId = NetworkTransport.Connect(hostID, serverip, port, 0, out error);
        connectionTime = Time.time;
        isConnected = true;
        //txmoney = GameObject.Find("PlayerGui").transform.Find("Txmoney").gameObject;
        //playerIO.isNetwork = true;
    }

    public void SendLogin()
    {
        if (!isConnected)
        {
            connectserver();
        }
        string pname = GameObject.Find("InputLogin").GetComponent<InputField>().text;

        if(pname == "")
        {
            Debug.Log("enter name");
            return;
        }

        string ppass = GameObject.Find("InputPassword").GetComponent<InputField>().text;

        if (ppass == "")
        {
            Debug.Log("enter password");
            return;
        }
        //string message = "NAMEAC|"+ pname+ "|"+ ppass;

        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.NetworkLogin);
        writer.Write(pname);
        string compname = pname + ":" + ppass + ":qazwsxedcrfv";
        writer.Write(Crypt.Md5Sum(compname));
        Send(writer, channelId, connectionId);

    }

	// Update is called once per frame
	void Update () {

        if (!isConnected)
            return;

        if (isGame)
        {
            if(txmoney == null)
            {
                txmoney = playerGUI.transform.Find("Txmoney").gameObject;
            }

            txmoney.GetComponent<Text>().text = "Денег " + PlayerMoney + "р";
            if(Txmanount == null)
            {
                Txmanount= playerGUI.transform.Find("Txmanount").gameObject;
            }
            Txmanount.GetComponent<Text>().text = "Людей " + workercount + "ч";
        }
        int recHostId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing: break;
            case NetworkEventType.ConnectEvent: break;
            case NetworkEventType.DataEvent:
                NetworkReader reader = new NetworkReader(recBuffer);
                // string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                PackegType pkgtype = (PackegType)reader.ReadInt32();
                
               // Debug.Log("Player " + connectionId + " pkg type " + pkgtype);

                switch (pkgtype)
                {
                    case PackegType.NetworkLogin:
                        ////////////// нет авторизации на сервере
                        break;
                    case PackegType.NetworkAccess:

                        Player.instance.playerName = reader.ReadString();
                        Player.instance.position = reader.ReadVector3();
                        Player.instance.Access = reader.ReadInt32();
                        Player.instance.homecity = reader.ReadInt32();
                        Player.instance.currentcity = reader.ReadInt32();
                        PlayerMoney = reader.ReadInt32();

                        


                        if (Player.instance.Access == 1)
                        {
                            //// загрузка сцены
                           // Debug.Log("send gameobgect =" + Player.instance.name);
                            LoadingGame(channelId, connectionId);
                        } else
                        {
                            MenuManager.instance.Debuglog("Server disconnect. Username not found");
                        }

                            // GameObject getplayer = reader.ReadGameObject();
                            
                        break;
                    case PackegType.GetCity:
                        citynetwork = reader.ReadMessage<NetworkCity>();
                        Debug.Log("get city id="+ citynetwork.id);
                        switch (citynetwork.tamplate)
                        {
                            case 1:   
                                GameObject prefab = Resources.Load("Terrain_1") as GameObject;
                                Instantiate(prefab);   
                            break;
                            default:
                            break;
                        }
                        GameObject.FindGameObjectWithTag("Player").transform.position = citynetwork.spawn;
                        break;
                    case PackegType.GetInventory:
                        InventoryNetwork inventoryNetwork = reader.ReadMessage<InventoryNetwork>();

                        Inventory inventory = playerInventory;
                        //inventory.deleteAllItems();

                        inventory.deleteAllItems();
                        

                          int az = 0;
                          for(az=0;az< inventoryNetwork.items.Count;az++)
                          {
                            if(! inventory.checkIfItemAllreadyExist(inventoryNetwork.items[az].itemId, inventoryNetwork.items[az].count))
                            { 
                               inventory.addItemToInventory(inventoryNetwork.items[az].itemId, inventoryNetwork.items[az].count);
                            }
                        }


                       // Debug.Log("getinventory="+ inventoryNetwork.items.Count);
                        break;
                    case PackegType.GetItemInventory:
                        
                        int itemid = reader.ReadInt32();
                        int itemcount = reader.ReadInt32();
                        Debug.Log("GetItemInventory itemid=" + itemid + "itemcount=" + itemcount);
                        Hotbar hotbar = GameObject.FindGameObjectWithTag("Hotbar").GetComponent<Hotbar>();
                        // Inventory inventory = GameObject.Find("PlayerGui").transform.GetChild(1).GetComponent<Inventory>();

                        if (playerInventory.SetIfItemAllreadyExist(itemid, itemcount))
                        {
                            playerInventory.updateItemIndex();
                            playerInventory.updateItemList();

                            if (itemcount < 1)
                            {
                                Item item = playerInventory.ItemsInInventory.Find(x => x.itemID == itemid);
                                GameObject itemobj = playerInventory.getItemGameObject(item);
                                if (itemobj.GetComponent<ConsumeItem>().duplication != null)
                                {
                                    GameObject dub = itemobj.GetComponent<ConsumeItem>().duplication;
                                    dub.GetComponent<ItemOnObject>().item.itemValue = itemcount;
                                    dub.transform.parent.parent.parent.GetComponent<Inventory>().updateItemList();
                                    Destroy(dub);
                                }
                                Destroy(itemobj);

                            }
                        }
                        else
                        {
                            playerInventory.addItemToInventory(itemid, itemcount);

                        }
                        
                                             
                        break;
                    case PackegType.GetMoney:
                        PlayerMoney = reader.ReadInt32();
                        break;
                    case PackegType.CreateNewHome:
                        Home NetworkHome = reader.ReadMessage<Home>();
                        GameObject newhome = new GameObject("Home_" + NetworkHome.idtable);
                        if(isDublicateHome(NetworkHome.position) == null)
                        {
                            NetworkHome.cityWorkers = new List<GameObject>();
                            homes.Add(NetworkHome);
                        }
                        newhome.transform.position = new Vector3(NetworkHome.position.x, NetworkHome.position.y, NetworkHome.position.z);
                        break;
                    case PackegType.CreateNewBlock:
                        NetworkBlock networkBlock = reader.ReadMessage<NetworkBlock>();
                        //Debug.Log("PackegType.CreateNewBlock block homeid=" + networkBlock.homeid);
                        onCreateNewBlock(networkBlock);

                        break;
                    case PackegType.CraftItem:
                        CraftNetwork craftNetwork = reader.ReadMessage<CraftNetwork>();
                        OnCraftItem(craftNetwork);
                        break;
                    case PackegType.RemoveHome:
                        int homeidrem = reader.ReadInt32();
                        GameObject remobj = GameObject.Find("Home_" + homeidrem);
                        if(remobj != null)
                        {
                            Destroy(remobj);
                        }
                        break;
                    case PackegType.SendChat:
                        string chattext = reader.ReadString();
                        MenuManager menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
                        menuManager.Debuglog(chattext);
                        break;
                    default:
                        break;
                }
                break;
            case NetworkEventType.DisconnectEvent:
                Cursor.visible = true;
                SceneManager.LoadScene(0);
                break;
            case NetworkEventType.BroadcastEvent:

                break;
        }
    }

    private void onCreateNewBlock(NetworkBlock networkBlock)
    {

        networkBlock.Instant(itemDatabase.itemList[networkBlock.typeid]);
        Home home = homes.Find(x => x.idtable == networkBlock.homeid);
        if (home.city != 0)
        {
            ItemStore newblock = isDublicateBlock(networkBlock.homeid, networkBlock.position);
            if (newblock == null)
            {
                //Debug.Log("onCreateNewBlock block homeid=" + networkBlock.homeid);
                home.itemList.Add(networkBlock.toItemStore());

            } else
            {
                newblock.gameObject = networkBlock.gameObject;
            }
        }
        
    }

    private void OnCraftItem(CraftNetwork craftNetwork)
    {
        //Home home = this.GetHomesInList(craftNetwork.homeid);
        //Debug.Log("Network OnCraftItem homeid="+ craftNetwork.homeid + " verstackid="+ craftNetwork.verstackid);
        // ItemStore itemStoreVerstack = home.itemList.Find(x => x.ItemID == craftNetwork.verstackid);
        string verstackname = "verstak_" + craftNetwork.verstackid;
        verstack verstack = GameObject.Find(verstackname).GetComponent<verstack>();  //itemStoreVerstack.gameObject.GetComponent<verstack>();
        verstack.FinalCraft(craftNetwork);

    }

    private void Send(NetworkWriter writer, int channelID, int connectionID)
    {
        NetworkTransport.Send(hostID, connectionID, channelID, writer.ToArray(), writer.ToArray().Length * sizeof(char), out error);
    }

    public Home GetHomesInList(int homeid)
    {
       return homes.Find(x => x.idtable == homeid);
    }

    public Home isDublicateHome(Vector3 position)
    {
        int i = 0;
        for(i=0;i<homes.Count;i++)
        {
            if(position == homes[i].position)
            {
                return homes[i];
            }
        }

        return null;
    }


    public ItemStore isDublicateBlock(int homeid, Vector3 position)
    {

        Home home = homes.Find(x => x.idtable == homeid);
        if (home == null)
        {
            return null;
        }
        else
        {
            List<ItemStore> itemlist = home.itemList;
          //  Debug.Log("isDublicateBlock block homeid=" + homeid);
            int i = 0;
            for (i = 0; i < itemlist.Count; i++)
            {
                if (position.x == itemlist[i].coordX && position.y == itemlist[i].coordY && position.z == itemlist[i].coordZ)
                {
                   // Debug.Log("isDublicateBlock block itemlist[i]; homeid=" + itemlist[i].HomeId);
                    return itemlist[i];
                }
                
            }
           // Debug.Log("isDublicateBlock block homeid=" + homeid);
        }
        return null;
    }


    void LoadingGame(int channelID, int connectionID)
    {
        MenuManager.instance.ChangeMenu(3);
        playerController.SetActive(true);
        PlayerIO playerIO = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIO>();
        playerIO.isNetwork = true;
        playerGUI.SetActive(true);

        

        txmoney = GameObject.Find("PlayerGui").transform.Find("Txmoney").gameObject;

        GetNetworkCity(channelID, connectionID);

        
        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.GetInventory);
        Send(writer, channelID, connectionID);
        isGame = true;
        // GameObject prefab2 = Resources.Load("First Person Controller") as GameObject;
        // Instantiate(prefab2);
    }

    void GetNetworkCity(int channelID, int connectionID)
    {
        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.GetCity);
        writer.Write(Player.instance.currentcity);
        Send(writer, channelID, connectionID);
    }
    
    public void buy_items(Item item,int count)
    {
        Debug.Log("Network buy item "+item.itemID+ " count" + count);
        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.BuyItems);
        writer.Write(item.itemID);
        writer.Write(count);
        Send(writer, channelId, connectionId);
    }

    public void SendChat(string test)
    {
        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.SendChat);
        writer.Write(test);
        Send(writer, channelId, connectionId);
    }

    public void CraftItem(int itemid,int count,int verstackid, int homeid)
    {
        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.CraftItem);
        writer.Write(itemid);
        writer.Write(count);
        writer.Write(verstackid);
        writer.Write(homeid);
        Send(writer, channelId, connectionId);
    }

    public void HarwestWood(int itemid,int count)
    {
        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.HarwestWood);
        writer.Write(itemid);
        writer.Write(count);
        Send(writer, channelId, connectionId);
    }


    public void RotateBlock(int itemid, Vector3 rotation)
    {
        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.RotateBlock);
        writer.Write(itemid);
        writer.Write(rotation);
        Send(writer, channelId, connectionId);
    }

    public void sell_items(Item item)
    {
        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.SellItems);
        writer.Write(item.itemID);
        writer.Write(item.itemValue);
        Send(writer, channelId, connectionId);
    }

    public void HomeListAdd(Vector3 position)
    {
        if (isDublicateHome(position) != null)
                return;


        Home newhome = new Home();
        newhome.position = position;
        homes.Add(newhome);
        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.CreateNewHome);
        writer.Write(citynetwork.id);
        writer.Write(position);
        Send(writer, channelId, connectionId);
    }

    public void RemoveHome(int homeid)
    {
        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.RemoveHome);
        writer.Write(homeid);
        Send(writer, channelId, connectionId);
    }

    public void RemoveBlock(int id, int homeid)
    {
        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.RemoveBlock);
        writer.Write(homeid);
        writer.Write(id);
        Send(writer, channelId, connectionId);


    }

    public void CreateNewBlock(Vector3 position, int homeid, int type)
    {
        ItemStore itemStore = isDublicateBlock(homeid, position);
        if (itemStore == null)
        {

        itemStore = new ItemStore();
        itemStore.coordX = position.x;
        itemStore.coordY = position.y;
        itemStore.coordZ = position.z;

        //homes[homeid].itemList.Add(itemStore);
        //homes.Find(x => x.idtable == homeid).itemList.Add(itemStore); /// Ошибка дублирования блока


        NetworkBlock networkBlock = new NetworkBlock();
        networkBlock.position = position;
        networkBlock.homeid = homeid;
        networkBlock.typeid = type;
        networkBlock.city = citynetwork.id;

        NetworkWriter writer = new NetworkWriter();
        writer.Write((int)PackegType.CreateNewBlock);

        networkBlock.Serialize(writer);
        Send(writer, channelId, connectionId);

        } else
        {
            Debug.Log("Этот блок уже сушесьвует "+ itemStore.ItemID);
        }

    }


    public void StartCraft(GameObject verstak, CraftOrder slotCraft)
    {
        //Debug.Log("Start Craft " + verstak.name);
        bool isfind = false;
        int homeid = verstak.GetComponent<ItemMy>().homeid;
        Home home = homes.Find(x => x.idtable == homeid);
        foreach (var item in home.cityWorkers)
        {
            if (!isfind)
            {
                if (!item.GetComponent<WorkerController>().isbusy)
                {
                    item.GetComponent<WorkerController>().isbusy = true;
                    item.GetComponent<WorkerController>().setCraft(verstak, slotCraft);
                    isfind = true;

                }
            }
        }
        if (!isfind)
        {
           MenuManager.instance.Debuglog("Нет свободных рабочих");
        }
    }


}


public class NetworkCity : MessageBase
{
    public int id;
    public int owner;
    public string name;
    public int tamplate;
    public Vector3 spawn;
    public int rating;



    // This method would be generated
    public override void Deserialize(NetworkReader reader)
    {
        id = reader.ReadInt32();
        owner = reader.ReadInt32();
        name = reader.ReadString();
        tamplate = reader.ReadInt32();
        spawn = reader.ReadVector3();
        rating = reader.ReadInt32();
    }

    // This method would be generated
    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(id);
        writer.Write(owner);
        writer.Write(name);
        writer.Write(tamplate);
        writer.Write(spawn);
        writer.Write(rating);
    }

}



public class InventoryNetwork : MessageBase
{
    public List<ItemNetwork> items;

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(items.Count);
        int i = 0;
        for (i = 0; i < items.Count; i++)
        {
            items[i].Serialize(writer);
        }

    }

    public override void Deserialize(NetworkReader reader)
    {
        items = new List<ItemNetwork>();
        int coune = reader.ReadInt32();
        int i = 0;
        for (i = 0; i < coune; i++)
        {
            items.Add(ItemNetwork.Deserialize(reader));
        }
    }

}


[System.Serializable]
public class ItemNetwork : MessageBase
{
    public int itemId;
    public int count;

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(itemId);
        writer.Write(count);
    }

    public static ItemNetwork Deserialize(NetworkReader reader)
    {
        ItemNetwork item = new ItemNetwork();
        item.itemId = reader.ReadInt32();
        item.count = reader.ReadInt32();
        return item;
    }
}