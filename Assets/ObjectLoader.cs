using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ObjectLoader : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    StartCoroutine(LoadCustomCustomBundlesAsync());
  }

  // Update is called once per frame
  void Update()
  {

  }

  private IEnumerator LoadCustomCustomBundlesAsync() // Special thanks to https://github.com/THE-GREAT-OVERLORD-OF-ALL-CHEESE/Custom-Scenario-Assets/ for this code
  {
    DirectoryInfo info = new DirectoryInfo(Directory.GetCurrentDirectory());
    Debug.Log("Searching " + Directory.GetCurrentDirectory() + " for .nbda custom weapons");
    foreach (FileInfo file in info.GetFiles("*.assets", SearchOption.AllDirectories))
    {
      Debug.Log("Found .nbda " + file.FullName);
      StartCoroutine(loadIndicatorAsync(file));
    }
    yield break;
  }

  private IEnumerator loadIndicatorAsync(FileInfo file) //thank you NotBDArmory github
  {
    AssetBundleCreateRequest a = AssetBundle.LoadFromFileAsync(file.FullName);
    yield return a;
    AssetBundle bundle = a.assetBundle;
    AssetBundleRequest handler = bundle.LoadAssetAsync("AlignmentIndicator.prefab");
    yield return handler;
    if (handler.asset == null)
    {
      Debug.Log("Couldn't find alignment indicator");
    }
    //Instantiate(handler.asset);
    GameObject AlignmentIndicator = handler.asset as GameObject;


    //0.13, 0.575, 6.125
    //25,0,0
    yield break;
  }
  GameObject makeObject(GameObject g)
  {
    GameObject gw = g;
    DontDestroyOnLoad(gw);
    return gw;
  }
}
