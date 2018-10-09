using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTerrain : MonoBehaviour {

	public Terrain terrain;
	private TreeInstance[] _originalTrees;

	void Start () {
		terrain = GetComponent<Terrain>();

		// backup original terrain trees
		_originalTrees = terrain.terrainData.treeInstances;
	}

	void OnApplicationQuit() {
		// restore original trees
		terrain.terrainData.treeInstances = _originalTrees;
	}
}


