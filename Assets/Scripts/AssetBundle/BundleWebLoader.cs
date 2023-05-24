using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class BundleWebLoader : MonoBehaviour
{
    public string bundleUrl = "http://localhost/assetbundles/testbundle";
    public string assetName = "BundledObject";

    // Start is called before the first frame update
    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            AssetBundle remoteAssetBundle = DownloadHandlerAssetBundle.GetContent(www);
            if (remoteAssetBundle == null)
            {
                Debug.LogWarning("Failed to download AssetBundle! " + bundleUrl);
                yield break;
            }
            GetComponent<MeshRenderer>().material = (remoteAssetBundle.LoadAsset<Material>(assetName));
            remoteAssetBundle.Unload(false);
        }
    }


}