using UnityEditor;
using System.IO;

public class AssetBundleBuilder: Editor {

   [MenuItem("Tools/CreatAssetBundle for Android")]

   static void CreatAssetBundle() {
      string path = "Assets/StreamingAssets";

      if(!Directory.Exists(path)) {
         Directory.CreateDirectory(path);
      }

      BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.Android);
      UnityEngine.Debug.Log("Android AssetBundle Finish!");
   }

   [MenuItem("Tools/CreatAssetBundle for Windows")]
   static void CreatPCAssetBundleForwINDOWS() {
      string path = "Assets/AssetBundles";

      if(!Directory.Exists(path)) {
         Directory.CreateDirectory(path);
      }

      BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
      UnityEngine.Debug.Log("Windows AssetBundle Finish!");
   }
}