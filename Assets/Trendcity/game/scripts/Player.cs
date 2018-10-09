using UnityEngine;
using System.Collections;

[System.Serializable]
public class Player : MonoBehaviour
{
    public static Player instance;

    private void Awake()
    {
        instance = this;
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
}
