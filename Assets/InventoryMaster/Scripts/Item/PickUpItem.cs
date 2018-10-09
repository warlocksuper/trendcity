using UnityEngine;
using System.Collections;
public class PickUpItem : MonoBehaviour
{
    public Item item;
    private Inventory _inventory;
    private GameObject _player;
    private PlayerIO playerIO;
    private MenuManager menuManager;
    // Use this for initialization

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        playerIO = _player.GetComponent<PlayerIO>();
        if (_player != null)
            _inventory = _player.GetComponent<PlayerInventory>().inventory.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inventory != null && Input.GetKeyDown(KeyCode.E))
        {
            float distance = Vector3.Distance(this.gameObject.transform.position, _player.transform.position);

            if (distance <= 3)
            {
                
                if (playerIO.isNetwork)
                {
                    menuManager.Debuglog("Собранно " + item.itemName + " колличество " + item.itemValue);
                    //MenuManager.instance.Debuglog();
                    GameObject.Find("NetworkManager").GetComponent<NetworkLayerClient>().HarwestWood(item.itemID, item.itemValue);
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.Log("Собранно " + item.itemName + " колличество " + item.itemValue);
                    bool check = _inventory.checkIfItemAllreadyExist(item.itemID, item.itemValue);
                    if (check)
                        Destroy(this.gameObject);
                    else if (_inventory.ItemsInInventory.Count < (_inventory.width * _inventory.height))
                    {

                        _inventory.addItemToInventory(item.itemID, item.itemValue);
                        _inventory.updateItemList();
                        _inventory.stackableSettings();
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }

}