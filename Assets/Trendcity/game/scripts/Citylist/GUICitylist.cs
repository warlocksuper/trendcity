using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUICitylist : MonoBehaviour {

    public GameObject slotpref;
    public Transform Gripelement;
    private List<NetworkCity> networkCities;

    public void UpdateCityList(List<NetworkCity> networkCities)
    {
        ClearGripelementList();
        foreach (var item in networkCities)
        {
            GameObject newcity = Instantiate(slotpref, Gripelement, true);
            newcity.GetComponent<CitySlot>().city = item;
            newcity.transform.Find("txtCityname").GetComponent<Text>().text = item.name;
            newcity.transform.Find("txtPlayername").GetComponent<Text>().text = ""+item.owner;
            newcity.transform.Find("txtRating").GetComponent<Text>().text = "" + item.rating;
        }
    }

    private void ClearGripelementList()
    {
        int y = 0;
        for (y = 0; y < Gripelement.childCount; y++)
        {
            Destroy(Gripelement.GetChild(y).gameObject);
        }
    }
}
