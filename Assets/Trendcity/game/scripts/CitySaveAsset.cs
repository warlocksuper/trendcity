
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


[System.Serializable]
public class CitySaveAsset : ScriptableObject
{
    [SerializeField]
    public List<ItemStore> itemList = new List<ItemStore>();
    public Gamelocal gameLocal;
    public int playermoney = 0;
    public int citycount = 0;
    //string gamefile = "game.save";

    public ItemStore getItemByID(int id)
    {
        return itemList[id];
        
    }

    public void addItem(Item item,Vector3 p)
    {
        gameLocal = GameObject.Find("GameLocal").GetComponent<Gamelocal>();
        
        ItemStore itemmy = new ItemStore();
        itemmy.ItemID = item.itemID;
        itemmy.coordX = p.x;
        itemmy.coordY = p.y;
        itemmy.coordZ = p.z;
        itemmy.HomeId = item.homeid;
        Debug.Log("addItem (itemList[i].coordX=" + itemmy.coordX + " (itemList[i].coordY" + itemmy.coordY + " (itemList[i].coordZ" + itemmy.coordZ+ " item.homeid="+ item.homeid);
        if (item.homeid > 0)
        {
            gameLocal.city.HomeList[item.homeid-1].itemList.Add(itemmy);
        }
        itemList.Add(itemmy);
        gameLocal.itemList.Add(itemmy);
        gameLocal.SaveGame();
    }

    public void removeItem(GameObject remove_block)
    {
        Vector3 p = remove_block.transform.position;
        int homeid = (remove_block.GetComponent<ItemMy>().homeid-1);

        gameLocal = GameObject.Find("GameLocal").GetComponent<Gamelocal>();
       // Debug.Log("remove block  homeid=" + homeid );
        Home curhome = gameLocal.city.HomeList[homeid];

        if (curhome.itemList.Count > 0)
        {
            Debug.Log("remove block  homeid="+ homeid+ "  curhome.itemList.Count="+ curhome.itemList.Count + " position cur "+p);
            for (int i = 0; i < curhome.itemList.Count; i++)
            {
                Vector3 newp = new Vector3(curhome.itemList[i].coordX, curhome.itemList[i].coordY, curhome.itemList[i].coordZ);
                if (newp == p)
                {
                    Debug.Log("remove block  homeid=" + homeid + "  curhome.itemList.Count=" + curhome.itemList.Count + " position cur " + p);
                    //itemList.RemoveAt(i);
                    curhome.itemList.RemoveAt(i);
                    gameLocal.SaveGame();
                }
                
            }
        }
    }

    public void rotateItem(Vector3 p,Vector3 rotation,int homeid)
    {
        gameLocal = GameObject.Find("GameLocal").GetComponent<Gamelocal>();
        Home curhome = gameLocal.city.HomeList[homeid];
        if (curhome.itemList.Count > 0)
        {
            for (int i = 0; i < curhome.itemList.Count; i++)
            {
                Vector3 newp = new Vector3(curhome.itemList[i].coordX, curhome.itemList[i].coordY, curhome.itemList[i].coordZ);
                if (newp == p)
                {
                    curhome.itemList[i].rotation = new Vector3Serializer(rotation.x, rotation.y, rotation.z);
                    gameLocal.SaveGame();
                }

            }
        }
    }

    public void test()
    {

        WWW www = new WWW("http://www.ya.ru/1.jpg");

        
    }

    /*
    [MenuItem("Assets/Create/CitySaveAsset")]
    public static void CreateAsset ()
    {
        ScriptableObjectUtility.CreateAsset<CitySaveAsset> ();
    }
    */

}