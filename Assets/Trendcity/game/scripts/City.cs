using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine.Networking;

[System.Serializable] 
public class City
{
    public string name;
    public int playerid;
    [SerializeField]
    public List<Home> HomeList = new List<Home>();
}


[System.Serializable]
public class Home : MessageBase
{
    public int idtable;
    public int owner;
    public int city;
    public string name;
    public int homeprice;
    public Vector3Serializer position;
    [SerializeField]
    public List<ItemStore> itemList = new List<ItemStore>();
    public List<GameObject> cityWorkers;

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(idtable);
        writer.Write(owner);
        writer.Write(city);
        writer.Write(position);
    }

    public override void Deserialize(NetworkReader reader)
    {
        idtable = reader.ReadInt32();
        owner = reader.ReadInt32();
        city = reader.ReadInt32();
        position = reader.ReadVector3();
    }

    public void RemoveBlock(Vector3 position)
    {
        
        int i = 0;
        for (i = 0; i < itemList.Count; i++)
        {
            if (position.x == itemList[i].coordX && position.y == itemList[i].coordY && position.z == itemList[i].coordZ)
            {
                itemList.Remove(itemList[i]);
            }
        }
    }

}

[System.Serializable]
public class ItemStore
{
    public int ItemID;
    //public Vector3 coord;
    [SerializeField]
    public GameObject gameObject;
    public float coordX;
    public float coordY;
    public float coordZ;
    [SerializeField]
    public Vector3Serializer rotation;
    public int cout;
    public int HomeId;
}

public class ItemMy : MonoBehaviour
{
    public int id;
    public Item Item;
    public Vector3 coord;
    public int homeid; 
}