using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;



public class Packeges {

	public byte Index;
	public byte[] data;
	public byte[] pakage;

	public Packeges (PackegType pakagtype, byte[] data)
	{
		int pakagesize = data.Length + 1; 
		pakage = new byte[pakagesize];

		pakage[0] = (byte)pakagtype.GetHashCode();

		int i = 1;
		for(i=1;i<pakagesize;i++)
		{
			pakage [i] = data [i - 1];
		}
		//Index = pakagtype;

		//this.data = 
	}

	public int GetSize()
	{
		return pakage.Length;
	}

	public byte[] GetByte()
	{
		return pakage;
	}
		
}

public class Loadnetwork : MonoBehaviour {

	// Use this for initialization
	public static Loadnetwork instanse;
	[Header("Network Settings")]
	public string ServerIP = "127.0.0.1";
	public int ServerPort = 5500;
	public bool IsConnected = false;

	public TcpClient PlayerSocket;
	public NetworkStream myStream;
	public StreamReader myReader;
	public StreamWriter myWriter;
	private byte[] asyncBuff;
	private byte[] myBytes;
	public bool shouldHendleData;

    [Header("Game Settings")]
    public InputField login;
    public InputField password;
    public static int MaxClient = 100;

    

    public void Awake()
	{
		instanse = this;
	}

	void Start () {
		
	}

	void Update() {
        /*
		if (shouldHendleData)
		{
			HandleData(myBytes);
			shouldHendleData = false;
		}
        */
	}

	
	void ConnectGameServer()
	{

        Debug.Log ("Connect server");
		if (PlayerSocket != null) {
			if (PlayerSocket.Connected) return;
			if(IsConnected) return;	
		
			PlayerSocket.Close ();
			PlayerSocket = null;
		}

		PlayerSocket = new TcpClient ();
		PlayerSocket.ReceiveBufferSize = 4096;
		PlayerSocket.SendBufferSize = 4096;
		PlayerSocket.NoDelay = false;
		Array.Resize (ref asyncBuff, 8196);
		PlayerSocket.BeginConnect (ServerIP, ServerPort,new AsyncCallback(ConnectCallback), PlayerSocket);
		IsConnected = true;

	}

	void ConnectCallback(IAsyncResult result)
	{
		//MenuManager.instanse._menu = MenuManager.Menu.Home;
		Debug.Log ("Connect server ConnectCallback");
		if (PlayerSocket != null) {
			PlayerSocket.EndConnect (result);
			if (PlayerSocket.Connected == false) {
				IsConnected = false;
				return;
			} else {
				PlayerSocket.NoDelay = true;
				myStream = PlayerSocket.GetStream ();
				myStream.BeginRead (asyncBuff, 0, 8192, onRecerved, null);
			}
		}



	}

    public void MovePlayer(float x,float y,float z)
    {
        KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
        buffer.WriteInteger(4);
        buffer.WriteFloat(x);
        buffer.WriteFloat(y);
        buffer.WriteFloat(z);
        SendDataToServer(buffer.ToArray());
        buffer = null;
    }


	bool sentlogin = true;
    public void SendLoginToServer()
    {
        transform.GetComponent<NetworkLayerClient>().SendLogin();
        /*
        ConnectGameServer();
        KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
		buffer.WriteInteger(2);

		if (login.text == string.Empty)
		{
			Debug.Log("Please insert a username.");
			return;
		}

		if (password.text == string.Empty)
		{
			Debug.Log("Please insert a password.");
			return;
		}

		buffer.WriteString(login.text);
		buffer.WriteString(password.text);

		SendDataToServer(buffer.ToArray());
		buffer = null;
        */

    }

	public void SendDataToServer(byte[]data)
	{
        /*
		KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
        try
        {
            buffer.WriteBytes(data);
            myStream.Write(buffer.ToArray(), 0, buffer.ToArray().Length);
            buffer = null;
        } catch(Exception e)
        {
            Debug.LogError(e);
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }
		*/
	}


    void onRecerved(IAsyncResult result)
	{
		if(PlayerSocket != null)
		{
			if (PlayerSocket == null)
				return;

			int byteArray = myStream.EndRead(result);
			myBytes = null;
			Array.Resize(ref myBytes, byteArray);
			Buffer.BlockCopy(asyncBuff, 0, myBytes, 0, byteArray);

			if(byteArray == 0)
			{
				//Debug.Log("You got disconnected from the Server.");
				MenuManager.instance.Debuglog ("You got disconnected from the Server.");
				PlayerSocket.Close();
				return;
			}

			shouldHendleData = true;

			if (PlayerSocket == null)
				return;
			myStream.BeginRead(asyncBuff, 0, 8192, onRecerved, null);
		}
	}

  

	public void HandleData(byte[] data)
	{
		int packetnum;
		KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
		buffer.WriteBytes(data);
		packetnum = buffer.ReadInteger();
		buffer = null;
		if (packetnum == 0)
			return;

		HandleMessages(packetnum, data);
	}


	void HandleMessages(int packetNum, byte[] data)
	{
		switch (packetNum)
		{
		case 1:
			HandleIngame(packetNum, data);
			break;
        case 3:
                OtherPlayerConnected(data);
        break;
        case 4:
                OtherPlayerMovement(data);
         break;
         case 5:
                OtherPlayerDisconnect(data);
         break;
        }
	}




    void HandleIngame(int packetNum, byte[] data)
	{
		KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
		buffer.WriteBytes(data);
        /*
		int packetnum = buffer.ReadInteger();
		Player.instance.Username = buffer.ReadString();
		Player.instance.Password = buffer.ReadString();
		Player.instance.Map = buffer.ReadInteger();
		Player.instance.Access = buffer.ReadByte();
		Player.instance.FirstTime = buffer.ReadByte();
		Player.instance.Item = buffer.ReadInteger();
		Player.instance.otherplayercount = buffer.ReadInteger();
		if ((int)Player.instance.Access == 1) {
            
            if (Player.instance.otherplayercount > 0)
            {

                int othindex = 0, i=0;
                string othername = "";

                for (i=0;i< Player.instance.otherplayercount;i++)
                {
                    othindex = buffer.ReadInteger();
                    othername = buffer.ReadString();
                    //OtherPlayer.instance[othindex].Index = othindex;
                    //OtherPlayer.instance[othindex].Username = othername;
                    OtherPlayer nweoth = new OtherPlayer();
                    nweoth.Username = othername;
                    nweoth.Index = othindex;
                    OtherPlayer.addOthPlayer(nweoth, othindex);
                    Debug.Log("Player " + othername + " connected");
                }
            }

            Debug.Log("Server Send Ingaame my index "+ Player.instance.Item + "other player count "+ Player.instance.otherplayercount );
			MenuManager.instance.Debuglog ("Server connected. Loading game");
			LoadingGame ();
		} else {
			//Debug.Log("Server Send В доступе отказано");
			MenuManager.instance.Debuglog ("Server disconnect. Username not found");
		}

        //////////////

        */
		//Debug.Log("Server Send Access ");
		//Player.instance.Access = buffer.ReadByte();
		//Player.instance.FirstTime = buffer.ReadByte();
		//buffer = null;


	}

    void OtherPlayerDisconnect(byte[] data)
    {
        KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
        buffer.WriteBytes(data);
        /*
        int pakagenum = buffer.ReadInteger();
        int otherindex = buffer.ReadInteger();
        Debug.Log("Disconnect player " + OtherPlayer.instance[otherindex].Username+"index "+ otherindex);
        if (Player.instance.Item != otherindex)
        {
            MenuManager.instance.Debuglog("Disconnect player " + OtherPlayer.instance[otherindex].Username);
            Player.instance.otherplayercount = Player.instance.otherplayercount - 1;
            GameObject othe = GameObject.Find("OtherPlayer_" + otherindex + "(Clone)");
            OtherPlayer.desOthPlayer(otherindex);
            
            Destroy(othe);
        }
        */

     }

    void OtherPlayerMovement(byte[] data)
    {
        
        KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
        buffer.WriteBytes(data);
        int pakagenum = buffer.ReadInteger();
        int otherindex = buffer.ReadInteger();

        float x = buffer.ReadFloat();
        float y = buffer.ReadFloat();
        float z = buffer.ReadFloat();

        /*
        if (Player.instance.Item != otherindex) { 
            Rigidbody rb;
            //rb = OtherPlayer.instance[otherindex].GetComponent<Rigidbody>();
            Debug.Log("Player Movement index "+otherindex+ " x "+x+" y"+y);
            GameObject othe = GameObject.Find("OtherPlayer_" + otherindex+ "(Clone)");

            othe.transform.position = new Vector3(x, y, z);
            othe.transform.Rotate(0, x, 0);
            //othe.transform.Translate(0, 0, y, Space.World);


            //othe.transform.Translate()
            //* Time.deltaTime, Space.World
            //othe.transform.position = new Vector3(x, 0.0f, y);

            //rb = othe.GetComponent<Rigidbody>();
            //Vector3 movement = new Vector3(x, 0.0f, y);
            //rb.AddForce(movement * 6.0f);
        }
        */

    }

    void OtherPlayerConnected(byte[] data)
    {
        /*
        KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
        buffer.WriteBytes(data);
        int pakagenum = buffer.ReadInteger();
        string othname = buffer.ReadString();
        int othindex = buffer.ReadInteger();
        if (Player.instance.Item != othindex)
        {
            Player.instance.otherplayercount = Player.instance.otherplayercount + 1;
            OtherPlayer newother = new OtherPlayer();
            newother.Username = othname;
            newother.Index = othindex;
            OtherPlayer.addOthPlayer(newother, newother.Index);
            MenuManager.instance.Debuglog("Connected player " + othname);
        }
        */
    }

    void LoadingGame()
    {
        MenuManager.instance.ChangeMenu(3);
        //var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "terrain"));
        //var myLoadedAssetBundle = AssetBundle.LoadFromFile("/home/warlock/TrendCity2/Assets/StreamingAssets/LightingData.asset");
        ///home/warlock/TrendCity2/Assets/StreamingAssets

        //if (myLoadedAssetBundle == null)
        //{
        //	Debug.Log("Failed to load AssetBundle! "+Application.streamingAssetsPath);
        //	return;
        //}

        //var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("MyObject");
        GameObject prefab = Resources.Load("Terrain") as GameObject;
        Instantiate(prefab);
        GameObject prefab3 = GameObject.Find("PlayerGui").gameObject; //Resources.Load("PlayerGui") as GameObject;
                                                                      //  prefab3 = Instantiate(prefab3);
        //GameObject hotbar = GameObject.FindGameObjectWithTag("Hotbar");
        GameObject hotbar = prefab3.transform.GetChild(2).gameObject;
        //prefab3.transform.Find("Panel - Inventory(Clone)").gameObject;
        GameObject inventar = prefab3.transform.GetChild(1).gameObject;
        GameObject craftinv = prefab3.transform.GetChild(3).gameObject;
        prefab3.transform.GetChild(0).gameObject.SetActive(true);
        hotbar.SetActive(true);
        GameObject prefab2 = Resources.Load("First Person Controller") as GameObject;
        prefab2.GetComponent<PlayerIO>().Prefhotbar = hotbar;
        prefab2.GetComponent<PlayerIO>().Prefinv = inventar;
        prefab2.GetComponent<PlayerInventory>().inventory = inventar;
        prefab2.GetComponent<PlayerInventory>().craftSystem = craftinv;
        Instantiate(prefab2);
        //PlayerIO.isNetwork = true;

        MenuManager.instance.Debuglog("");

        //int i = 9;
        //for (i = 0; i < Player.instance.otherplayercount; i++)
        //{

        //}

    }

    void OnApplicationQuit()
    {
        // restore original trees
        //Server Disconnect
        KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
        buffer.WriteInteger(5);
        SendDataToServer(buffer.ToArray());
        buffer = null;
        // terrain.terrainData.treeInstances = _originalTrees;
    }
}

