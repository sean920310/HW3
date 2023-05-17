using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class BundleWebLoader : MonoBehaviour
{
    public string bundleUrl = "http://localhost/assetbundles/testbundle";
    public string assetName = "BundledObject";

    [SerializeField] MeshRenderer meshRenderer;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        using (WWW web = new WWW(bundleUrl))
        {
            yield return web;
            AssetBundle remoteAssetBundle = web.assetBundle;
            if (remoteAssetBundle == null)
            {
                Debug.LogWarning("Failed to download AssetBundle! " + bundleUrl);
                yield break;
            }
            meshRenderer.material = (remoteAssetBundle.LoadAsset<Material>(assetName));
            remoteAssetBundle.Unload(false);
        }
    }


}