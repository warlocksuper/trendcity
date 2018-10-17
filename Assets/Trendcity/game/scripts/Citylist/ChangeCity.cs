using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCity : MonoBehaviour {

    // Use this for initialization
    private NetworkLayerClient networkLayer;
    private GameObject player;
    private List<NetworkCity> networkCities;
    void Start () {
        List<NetworkCity> networkCities = new List<NetworkCity>();
        networkLayer = GameObject.Find("NetworkManager").GetComponent<NetworkLayerClient>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
            RaycastHit hit;

            if (Physics.Raycast (ray, out hit, 2f))
            {
                if(hit.collider.name== "board_OK")
                {
                    networkLayer.GetCityList();
                    player.GetComponent<PlayerIO>().CitylistWindow.SetActive(true);
                }

            }
        }
    }

    public void ShowCityList(List<NetworkCity> networkCities)
    {
        this.networkCities = networkCities;
        gameObject.GetComponent<GUICitylist>().UpdateCityList(networkCities);
    }
    public void CloseCityList()
    {
        this.gameObject.SetActive(false);
    }
}
