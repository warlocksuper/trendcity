// Create an AssetBundle for Windows.
using UnityEngine;
using UnityEditor;

public class BuildAssetBundlesExample : MonoBehaviour
{
    [MenuItem("Example/Build Asset Bundles")]
    static void BuildABs()
    {
        // Put the bundles in a folder called "ABs" within the Assets folder.
         AssetBundleBuild[] buildMap = new AssetBundleBuild[2];

        buildMap[0].assetBundleName = "enemybundle";

        string[] enemyAssets = new string[1];
        enemyAssets[0] = "Assets/HowToLoadCurvePresets.png";

        buildMap[0].assetNames = enemyAssets;
        buildMap[1].assetBundleName = "herobundle";

        string[] heroAssets = new string[1];
        heroAssets[0] = "char_hero_beanMan";
        buildMap[1].assetNames = heroAssets;

	  BuildPipeline.BuildAssetBundles("Assets/ABs", buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

    }
}

