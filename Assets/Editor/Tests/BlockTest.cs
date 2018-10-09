using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BlockTest
{

  //  [Test]
  //  public void NewTestScriptSimplePasses() {
        // Use the Assert class to test conditions.
  //  }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator Block_Create_new_block() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        RaycastHit hit = new RaycastHit();
        Block block = new Block();
        hit.collider.tag = "Block";
        Vector3 newpos = block.CreatePosition(hit);
        Assert.AreEqual(8, 8);
        yield return null;
    }
    [UnityTest]
    public IEnumerator Block_Network_Create_new_block()
    {
        GameObject NetworkManager = GameObject.Find("NetworkManager");//new GameObject("NetworkManager");
       // NetworkManager.AddComponent<NetworkLayerClient>();
        NetworkLayerClient networkLayer = NetworkManager.GetComponent<NetworkLayerClient>();
        Home home = new Home();
        home.city = 1;
        home.idtable = 255;
        networkLayer.citynetwork = new NetworkCity();
        // networkLayer.HomeListAdd(new Vector3());
        Vector3 position = new Vector3();
        if (networkLayer.isDublicateHome(position) != null)
            yield return null;

        Home newhome = new Home();
        newhome.position = position;
        networkLayer.homes.Add(newhome);

        networkLayer.citynetwork.id = 1;
        Assert.AreEqual(5701, networkLayer.port); /// Проверка подшрузился ли NetworkManager
        Assert.AreEqual(1, networkLayer.homes);    // Проверка на добавление дома




        ItemStore itemStore = networkLayer.isDublicateBlock(home.idtable, position);
        if (itemStore == null)
        {

            itemStore = new ItemStore();
            itemStore.coordX = position.x;
            itemStore.coordY = position.y;
            itemStore.coordZ = position.z;

            //homes[homeid].itemList.Add(itemStore);
           // networkLayer.homes.Find(x => x.idtable == home.idtable).itemList.Add(itemStore); //Причина дублирования юлоков

            NetworkBlock networkBlock = new NetworkBlock();
            networkBlock.position = position;

        }
        else
        {
            Debug.Log("Этот блок уже сушесьвует " + itemStore.ItemID);
        }


        yield return null;
    }
}
