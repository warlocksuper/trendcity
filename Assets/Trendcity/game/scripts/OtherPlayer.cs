using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour {

	public static OtherPlayer[] instance;
	public int MaxClient = 100;

	private void Awake()
	{
		instance = new OtherPlayer[MaxClient];
		int i = 0;
		for(i=0;i<MaxClient;i++)
		{
			instance [i] = new OtherPlayer (); 
		}
		//instance = this;
	}

    public static void desOthPlayer(int index)
    {
        instance[index] = new OtherPlayer();
    }

    public static void addOthPlayer(OtherPlayer other, int index)
    {
        instance[index] = other;
        instance[index].Index = index;
        instance[index].preb = Resources.Load("othreplater") as GameObject;

        OtherPlayer othe = instance[index].preb.GetComponent<OtherPlayer>();
        othe.Username = other.Username;
        othe.Index = index;
        othe.preb = instance[index].preb;
        Vector3 oplayer = new Vector3(1682.9f, 37.047f, 876);
        instance[index].preb.transform.SetPositionAndRotation(oplayer, Quaternion.identity);
        instance[index].preb.name = "OtherPlayer_" + index;
        Instantiate(instance[index].preb);



        /*
if (Player.instance.otherplayercount > 0) {
GameObject prefab4 = Resources.Load("othreplater") as GameObject;
OtherPlayer othe = prefab4.GetComponent<OtherPlayer>();
othe.Username = "Iser";
//prefab4.GetComponent<OtherPlayer>().Username = "test";
//prefab4.transform.position.x = 1682.9f;
Vector3 oplayer = new Vector3(1682.9f, 37.047f, 876);
prefab4.transform.SetPositionAndRotation(oplayer, Quaternion.identity);
Instantiate(prefab4);
}
*/

    }
    //General
    public string Username;
	public int Index;
    public GameObject preb;

}
