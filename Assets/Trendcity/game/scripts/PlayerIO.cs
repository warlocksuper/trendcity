using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIO : MonoBehaviour {


    public GameObject Prefinv;
    public GameObject Prefhotbar;
    public GameObject craftSystem;
    public GameObject ShopSystem;
    public GameObject cMenu;
    public GameObject CraftMan;
    public GameObject Chatinput;

    public byte selectedInventory = 0;
    private bool inventar_isActive = false;
    public bool isNetwork = false;
    // Use this for initialization
	void Start () {
        Cursor.visible = false;
        
	}
	
    
	// Update is called once per frame
	void Update () {
        if (isNetwork)
        {

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                float moveHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f; 
                float moveVertical = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
                //Debug.Log("Player local move x "+moveHorizontal+" y "+moveVertical + "position x " +transform.position.x + "position y "+ transform.position.z);
                // Loadnetwork.instanse.MovePlayer(moveHorizontal, moveVertical);
               // Loadnetwork.instanse.MovePlayer(transform.position.x, transform.position.y, transform.position.z);
            }

        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (Chatinput.activeSelf)
            {
                if (isNetwork)
                {
                    NetworkLayerClient networkLayerClient = GameObject.Find("NetworkManager").GetComponent<NetworkLayerClient>();
                    string text=Chatinput.GetComponentInChildren<InputField>().text;
                    networkLayerClient.SendChat(text);
                }
                Chatinput.GetComponentInChildren<InputField>().text = "";
                Chatinput.SetActive(false);

            } else
            {
                Chatinput.SetActive(true);

                
            }
        }

            if (Input.GetKeyDown(KeyCode.Escape))
        {
            Prefinv.SetActive(false);
            Chatinput.SetActive(false);

            if (cMenu.activeSelf)
            {
                cMenu.SetActive(false);
            } else
            {
                cMenu.SetActive(true);
            }

        }

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (!lockMovement())
        {
            Player.GetComponent<MouseLook>().enabled = true;
            Player.transform.GetChild(1).GetComponent<MouseLook>().enabled = true;
            Player.GetComponent<CharacterMotor>().enabled = true;
            Cursor.visible = false;

        } else
        {
            Player.GetComponent<MouseLook>().enabled = false;
            Player.GetComponent<CharacterMotor>().enabled = false;
            Cursor.visible = true;
            Player.transform.GetChild(1).GetComponent<MouseLook>().enabled = false;
        }

}

    bool lockMovement()
    {
        if (Prefinv != null && Prefinv.activeSelf)
            return true;
        else if (craftSystem != null && craftSystem.activeSelf)
            return true;
        else if (ShopSystem != null && ShopSystem.activeSelf)
            return true;
        else if (cMenu != null && cMenu.activeSelf)
            return true;
        else if (CraftMan != null && CraftMan.activeSelf)
            return true;
        else if (Chatinput != null && Chatinput.activeSelf)
            return true;
        else
        {
            
            return false;
        }
    }


}
