using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public class TerrainDeform : MonoBehaviour {

	bool shouldDraw = false;
	public Terrain myTerrain;
	TerrainData tData;
	int xResolution;
	int zResolution;
	float[,] heights;
	public float speed = 0.01f;
	string speedString;
	private float groundHeight = 0.03f;
		// Use this for initialization
	void Start () {
		myTerrain=Terrain.activeTerrain;
		speedString = speed.ToString();
		tData = myTerrain.terrainData;
		xResolution = tData.heightmapWidth;
		zResolution = tData.heightmapHeight;
		heights = tData.GetHeights (0, 0, xResolution, zResolution);
	}

    // Update is called once per frame
    public int block_count = 0;
	void Update () {

		//Ray digRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Ray digRay = Camera.main.ScreenPointToRay(new Vector3(0.5f, 0.5f, 0f));

		RaycastHit hit;

		speed = Convert.ToSingle (speedString);
        GameObject hotbar = GameObject.FindGameObjectWithTag("Hotbar");
        if (Input.GetMouseButtonDown(0)) {
			
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast (digRay, out hit, 30f)) {
				if (hit.collider.tag == "Terrain") {
                    if( Input.GetKey(KeyCode.RightControl))
                    {
                        raiseTerrain(hit.point);
                    } 
                }
			}

 
        }


        if ( Input.GetMouseButtonDown(1) && Input.GetKey (KeyCode.RightControl)) {
			
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast (digRay, out hit, 10) && hit.collider.tag == "Terrain"){
				lowerTerrain(hit.point);
			}
		}
	}

	private void raiseTerrain(Vector3 point){
	

		int mouseX = (int)((point.x / tData.size.x) * xResolution);
		int mouseZ = (int)((point.z / tData.size.z) * zResolution);

		//float height = myTerrain.SampleHeight(new Vector3((int)point.x, 0, (int)point.y));

		//groundHeight = groundHeight / 550;
		//groundHeight = 0.03f;
		//float height2 = myTerrain.terrainData.GetHeight(mouseX,mouseX);
		//float y = heights [mouseX, mouseZ];
		//Debug.Log(groundHeight.ToString());
			float[,] modHeights = new float[1, 1];
		////////// Первая точка
		modHeights [0, 0] = groundHeight;
		//heights [mouseX, mouseZ] = groundHeight;
		tData.SetHeights (mouseX, mouseZ, modHeights);

		////////////////////// Вторая точка///////////////
		mouseX+=1;
		//heights [mouseX, mouseZ] = groundHeight;
		modHeights [0, 0] = groundHeight;
		tData.SetHeights (mouseX, mouseZ, modHeights);

		////////////////////// Третья точка ///////////
		mouseZ+=1;
		//heights [mouseX, mouseZ] = groundHeight;
		modHeights [0, 0] = groundHeight;
		tData.SetHeights (mouseX, mouseZ, modHeights);
		mouseX+=1;
		//heights [mouseX, mouseZ] = groundHeight;
		modHeights [0, 0] = groundHeight;
		tData.SetHeights (mouseX, mouseZ, modHeights);
		////////////////////// Четвертая точка ////////////////
		mouseZ+=1;
		//heights [mouseX, mouseZ] = groundHeight;
		modHeights [0, 0] = groundHeight;
		tData.SetHeights (mouseX, mouseZ, modHeights);
		mouseX+=1;
		//heights [mouseX, mouseZ] = groundHeight;
		modHeights [0, 0] = groundHeight;
		tData.SetHeights (mouseX, mouseZ, modHeights);
		//////////////////////////////////////////////
		/*
		Debug.Log (y);
		y -= 0.2f;
		if (y > tData.size.y)
			y = tData.size.y;
		modHeights [0, 0] = y;
		heights [mouseX, mouseZ] = y;
		tData.SetHeights (mouseX, mouseZ, modHeights);
        */
		//Debug.Log ("raiseTerrain");
		//heights = tData.GetHeights (mouseX, mouseZ, xResolution, zResolution);
		//heights[mouseX, mouseZ] -= 0.005f;
		//tData.SetHeights((int)point.x, (int)point.y, heights);

	}
	private void lowerTerrain(Vector3 point){

		/*
		int mouseX = (int)((point.x / tData.size.x) * xResolution);
		int mouseZ = (int)((point.z / tData.size.z) * zResolution);
		float[,] modHeights = new float[1, 1];
		float y = heights [mouseX, mouseZ];
		y -= (speed / 10) * Time.deltaTime;
		Debug.Log (y);
		if (y < 0.0f)
			y = 0.0f;
		modHeights [0, 0] = y;
		heights [mouseX, mouseZ] = y;
		tData.SetHeights (mouseX, mouseZ, modHeights);
	*/
		groundHeight = myTerrain.SampleHeight(point);
		groundHeight = groundHeight / 540;
		/*
		heights = tData.GetHeights ((int)point.x, (int)point.y, xResolution, zResolution);
		heights[(int)point.x, (int)point.y] -= 0.005f;
		tData.SetHeights((int)point.x, (int)point.y, heights);
		*/
	}

}
