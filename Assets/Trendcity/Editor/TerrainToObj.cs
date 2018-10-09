// Create an AssetBundle for Windows.
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


[ExecuteInEditMode]
public class TerrainToObj : EditorWindow
{

	public static Terrain terrain;

	[MenuItem("Example/TerrainToObj")]
	static void Init()
	{
		//CustomTreeBrush window = (CustomTreeBrush)GetWindow (typeof(CustomTreeBrush));
		//terrain = (Terrain)EditorGUILayout.ObjectField (terrain, typeof(Terrain), true);
		//terrain=GameObject.Find("Terrain") as Terrain;
		//Convert();
		try
		{
			GameObject go = GameObject.Find("Terrain");
			if (go.GetComponent<Terrain>() == null)
			{
				EditorGUILayout.HelpBox("Your gameobject 'Terrain' does not have component 'Terrain'", MessageType.Warning);
			}
			else
			{
				//GUILayout.Label("World Builder", EditorStyles.boldLabel);
				//scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
				//worldGen.OnGUI();
				//EditorGUILayout.EndScrollView();
				terrain=go.GetComponent<Terrain>();
				Convert();
			}
		}
		catch
		{
			EditorGUILayout.HelpBox("Insert 'Terrain' on the scene", MessageType.Warning);
		}
	}
	void OnGUI()
	{
		
		//if(GUILayout.Button("Convert to objects"))
		//{
		//	Convert();
		//}

	}

	public static void Convert()
	{

		for (int i = 0; i < terrain.terrainData.treeInstances.Length; i++) {
			TreeInstance treeInstance = terrain.terrainData.treeInstances[i];
			GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);

			CapsuleCollider capsuleCollider = capsule.GetComponent<Collider>() as CapsuleCollider;
			capsuleCollider.center = new Vector3(0, 0, 0);
			capsuleCollider.height = 10.0f;
			capsuleCollider.radius = 1.01f;


			DestroyableTree tree = capsule.AddComponent<DestroyableTree>();
			tree.terrainIndex = i;

			capsule.transform.position = Vector3.Scale(treeInstance.position, terrain.terrainData.size);
			capsule.tag = "Tree";
			capsule.transform.parent = terrain.transform;
			capsule.GetComponent<Renderer>().enabled = false;
		}


	}

	static void BuildTO()
	{


	}
}

