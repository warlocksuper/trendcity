using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public class Player : MessageBase
{
  //  public static Player instance;

    private void Awake()
    {
    //    instance = this;
    }

    //General
    public int connectionID;
    public string playerName;
    public string playerpass;
    public Vector3 position;
    public int Access;
    public int homecity;
    public int currentcity;
    public int money;
    public int lodka;
}
