using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyableTree : MonoBehaviour {
	public int terrainIndex;

	public void Delete() {
		Terrain terrain = Terrain.activeTerrain;

		List<TreeInstance> trees = new List<TreeInstance>(terrain.terrainData.treeInstances);
		trees[terrainIndex] = new TreeInstance();
		terrain.terrainData.treeInstances = trees.ToArray();

		Destroy(gameObject);
	}
}