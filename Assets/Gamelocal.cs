using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using UnityEngine.UI;
using UnityEngine.AI;

public class Gamelocal : MonoBehaviour {

    public static Gamelocal instance;
    public int PlayerMoney=0;
    public List<GameObject> cityWorkers;
    private ItemDataBaseList itemDatabase;
    public List<ItemStore> itemList;
    public PlayerGameSetting playerSetting;
    public City city;

    public NavMeshSurface meshSurface;

    string gamefile = "game.save.xml";
    string playerfile = "player.dat";
    string citysave = "city.dat";
    // Use this for initialization
    public Text debugingame;
    private float timeLeft = 30.0f;
    private float orenda = 100.0f;
    private GameObject txmoney;
    private GameObject txcitycount;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }


    void Start () {
        itemDatabase = (ItemDataBaseList)Resources.Load("ItemDatabase");
        debugingame = GameObject.Find("Debug_game").GetComponent<Text>();
        txmoney = GameObject.Find("PlayerGui").transform.Find("Txmoney").gameObject;
        txcitycount = GameObject.Find("PlayerGui").transform.Find("Txmanount").gameObject;
        //  CitySaveAsset save = (CitySaveAsset)Resources.Load("CitySaveAsset");
        if (File.Exists(gamefile)) {


           // itemList = ReadFromXmlFile<List<ItemStore>>(gamefile);

            //itemList = ReadFromBinaryFile<List<ItemStore>>("game.save");

          //  loadGame();
        }
        if (File.Exists(playerfile)) {
            //  playerSetting = ReadFromXmlFile<PlayerGameSetting>(playerfile);
            playerSetting =  ReadFromBinaryFile< PlayerGameSetting >(playerfile);
            PlayerMoney = playerSetting.money;
            Inventory invent = GameObject.Find("PlayerGui").transform.GetChild(1).GetComponent<Inventory>();
              int i;
               for(i=0;i< playerSetting.ItemsInInventory.Count;i++)
               {
                   invent.addItemToInventory(playerSetting.ItemsInInventory[i].ItemID, playerSetting.ItemsInInventory[i].cout);
              }

            //GameObject.Find("PlayerGui").transform.GetChild(1).GetComponent<Inventory>().ItemsInInventory;
        }
        if (File.Exists(citysave))
        {
            city = ReadFromBinaryFile<City>(citysave);
            loadGame();
        } else
        {
            city = new City();
        }

        //meshSurface.AddData();
       // NavMeshSurface meshSurface = new NavMeshSurface();
       
        //NavMeshData navdata = new NavMeshData();
       // meshSurface.BuildNavMesh();

}
	
	// Update is called once per frame
	void Update () {
		if(Cursor.visible == false)
        {
            Cursor.visible = true;
        }

        if (cityWorkers.Count > 0)
        {
            txcitycount.GetComponent<Text>().text = "Людей: " + cityWorkers.Count + "ч";
        }
        txmoney.GetComponent<Text>().text = "Денег "+PlayerMoney+"р";

        if (timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                if (debugingame != null)
                {
                    debugingame.text = "";
                }
            }
        }

        if (orenda >= 0)
        {
            orenda -= Time.deltaTime;
            if (orenda < 0)
            {
                //PlayerMoney += cityWorkers.Count;
                if(cityWorkers.Count > 0)
                {
                    
                    foreach (var home in city.HomeList)
                    {
                        PlayerMoney += home.itemList.Count / 100;
                    }
       
                }

                if (cityWorkers.Count > 0)
                {
                    foreach (var item in cityWorkers)
                    {
                        if(item.GetComponent<WorkerController>().isbusy)
                        {
                            PlayerMoney -= 3;
                            if (PlayerMoney < 0)
                            {
                                item.GetComponent<WorkerController>().isbusy = false;
                                item.GetComponent<WorkerController>().bankrot();
                                PlayerMoney = 0;
                            }

                        }
                    }
                }
                orenda = 100.0f;

                playerSetting = new PlayerGameSetting();
                playerSetting.money = PlayerMoney;

                List<Item> ItemsInInventory = GameObject.Find("PlayerGui").transform.GetChild(1).GetComponent<Inventory>().ItemsInInventory;
                int i = 0;
                for (i = 0; i < ItemsInInventory.Count; i++)
                {
                    if (ItemsInInventory[i].itemValue > 0) { 
                        ItemStore itemStore = new ItemStore();
                        itemStore.ItemID = ItemsInInventory[i].itemID;
                        itemStore.cout = ItemsInInventory[i].itemValue;
                        playerSetting.ItemsInInventory.Add(itemStore);
                    }
                }
                
                //Debug.Log("Count inventory "+ ItemsInInventory.Count);
                //playerSetting.ItemsInInventory=GameObject.Find("PlayerGui").transform.GetChild(1).GetComponent<Inventory>().ItemsInInventory;
                // WriteToXmlFile<PlayerGameSetting>(playerfile, playerSetting);
                WriteToBinaryFile<PlayerGameSetting>(playerfile, playerSetting, false);
            }
        }


    }

    public void loadGame()
    {

        //save.addItem(item, newblock.transform.position);
        if (city.HomeList.Count > 0)
        {
            for (int y = 0; y < city.HomeList.Count; y++)
            {
                if (city.HomeList[y].itemList.Count > 0)
                {
                    GameObject newhome = new GameObject("Home_" + y);
                    //newhome.AddComponent<LocalNavMeshBuilder>();
                    //Instantiate(newhome, city.HomeList[y].position, Quaternion.identity);
                    for (int i = 0; i < city.HomeList[y].itemList.Count; i++)
                    {
                        ItemStore loaditem = city.HomeList[y].itemList[i];
                        //Debug.Log("loadGame (itemList[i].coordX=" + itemList[i].coordX + " (itemList[i].coordY"+ itemList[i].coordY+ " (itemList[i].coordZ"+ itemList[i].coordZ+ " itemList[i].ItemID"+ itemList[i].ItemID);
                        Vector3 newp = new Vector3(loaditem.coordX, loaditem.coordY, loaditem.coordZ);

                        //List<Item> items = Resources.Load
                        Item item = itemDatabase.itemList[loaditem.ItemID];
                        GameObject newblock = Instantiate(item.itemModel, newp, Quaternion.identity);
                        newblock.AddComponent<ItemMy>();
                        newblock.GetComponent<ItemMy>().Item = item;
                        newblock.GetComponent<ItemMy>().homeid = loaditem.HomeId;
                        newblock.transform.parent = newhome.transform;
                        
                        if (loaditem.rotation != null)
                        {
                            Vector3 rotat = new Vector3(loaditem.rotation.x, loaditem.rotation.y, loaditem.rotation.z);
                            Quaternion target = Quaternion.Euler(rotat);
                            newblock.transform.rotation = target;
                        }
                    }
                }
            }
        }
    }

    public void SaveGame()
    {
       // WriteToXmlFile<List<ItemStore>>("game.save.xml", itemList);
        WriteToBinaryFile<City>(citysave, city, false);
        //WriteToBinaryFile<List<ItemStore>>("game.save", itemList);
    }

    /// <summary>
    /// Writes the given object instance to an XML file.
    /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
    /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
    /// <para>Object type must have a parameterless constructor.</para>
    /// </summary>
    /// <typeparam name="T">The type of object being written to the file.</typeparam>
    /// <param name="filePath">The file path to write the object instance to.</param>
    /// <param name="objectToWrite">The object instance to write to the file.</param>
    /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
    public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
    {
        TextWriter writer = null;
        try
        {
            var serializer = new XmlSerializer(typeof(T));
            writer = new StreamWriter(filePath, append);
            serializer.Serialize(writer, objectToWrite);
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }
    }

    /// <summary>
    /// Reads an object instance from an XML file.
    /// <para>Object type must have a parameterless constructor.</para>
    /// </summary>
    /// <typeparam name="T">The type of object to read from the file.</typeparam>
    /// <param name="filePath">The file path to read the object instance from.</param>
    /// <returns>Returns a new instance of the object read from the XML file.</returns>
    public static T ReadFromXmlFile<T>(string filePath) where T : new()
    {
        TextReader reader = null;
        try
        {
            var serializer = new XmlSerializer(typeof(T));
            reader = new StreamReader(filePath);
            return (T)serializer.Deserialize(reader);
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
    }

    /// <summary>
    /// Writes the given object instance to a binary file.
    /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
    /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
    /// </summary>
    /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
    /// <param name="filePath">The file path to write the object instance to.</param>
    /// <param name="objectToWrite">The object instance to write to the XML file.</param>
    /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
    {
        using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }

    /// <summary>
    /// Reads an object instance from a binary file.
    /// </summary>
    /// <typeparam name="T">The type of object to read from the XML.</typeparam>
    /// <param name="filePath">The file path to read the object instance from.</param>
    /// <returns>Returns a new instance of the object read from the binary file.</returns>
    public static T ReadFromBinaryFile<T>(string filePath)
    {
        using (Stream stream = File.Open(filePath, FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (T)binaryFormatter.Deserialize(stream);
        }
    }

    public void Debuglog(string text)
    {

        debugingame.text = text;
        timeLeft = 3.0f;
    }

    public void AddWorker(GameObject obj)
    {
        cityWorkers.Add(obj);
    }

    public void RemoveWorker(GameObject obj)
    {
        cityWorkers.Remove(obj);
    }

    public void StartCraft(GameObject verstak, CraftOrder slotCraft)
    {
        //Debug.Log("Start Craft " + verstak.name);
        bool isfind = false;
        foreach (var item in cityWorkers)
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
            Debuglog("Нет свободных рабочих");
        }
    }
}

[System.Serializable]
public class PlayerGameSetting
{
    public int money;
    public List<ItemStore> ItemsInInventory = new List<ItemStore>();
}