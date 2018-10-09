﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Menu {
	MainMenu,
	NetworkMenu,
	HiddenAll
}

public class MenuManager : MonoBehaviour {

	public static MenuManager instance;

    public GameObject MainMenu;
    public GameObject NetworkMenu;
    public Canvas canvas;
    public Camera firstcamera;
    public Text debugingame;
    private float timeLeft = 30.0f;

    // Use this for initialization
    private void Awake()
	{
		instance = this;
	}


    // Update is called once per frame
    void Update()
    {
        if( timeLeft >= 0) { 
        timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                if (debugingame != null)
                {
                    debugingame.text = "";
                }
               // GameOver();
            }
        }
    }



    public void Loadscene()
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeMenu(int menu)
    {
		
        switch(menu)
        {
            case 1:
                MainMenu.SetActive(true);
                NetworkMenu.SetActive(false);
                break;
            case 2:
                MainMenu.SetActive(false);
                NetworkMenu.SetActive(true);
                GameObject.Find("NetworkManager").GetComponent<NetworkLayerClient>().connectserver();
                break;
	    case 3:  //hiddenall
		MainMenu.SetActive(false);
		NetworkMenu.SetActive(false);
                canvas.GetComponent<RawImage>().enabled = false;
                firstcamera.enabled = false;

                break;
        }
    }

    public void Debuglog(string text)
    {

	    debugingame.text = text;
        timeLeft = 3.0f;
    }

    public void QuiTScene()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

}