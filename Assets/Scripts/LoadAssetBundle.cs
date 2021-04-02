
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class LoadAssetBundle : MonoBehaviour
{

    private string cubePath = "AssetBundles/prefabs/cube.ab";


    private void Start()
    {
        //以下都是依赖于本地文件的加载方式，还有许多方式，具体查看AssetBundle.LoadFrom系列方法，这里不再做演示

        //LoadFromFileByAB(cubePath);
        //StartCoroutine("LoadFromMemoryAsyncByAB", cubePath);
        //LoadFromMemoryByAB(cubePath);


        //网络加载 WWW已经被UnityWebRequest代替，所以不再演示
        //将路径转化为Uri格式，具体内容可调试查看 
        //var uri = new Uri(@"D:\WorkData\UnityProject\AssetBundleStudy\AssetBundles\prefabs\cube.ab");
        //StartCoroutine("UnityWebRequestByAB",uri);

        //检查包以及获取包的依赖
        RelyONByAB("cube.ab");

        LoadFromFileByAB(cubePath);

        //关于卸载资源  当场景切换的时候就需要卸载一次资源，减少内存使用以及防止丢失
        //卸载所有资源，即便资源在使用
        //AssetBundle.UnloadAllAssetBundles(true);
        //卸载个别资源,即便资源在使用
        //Resources.UnloadAsset();
        //卸载没有在使用的资源
        //AssetBundle.UnloadAllAssetBundles(false);


    }

    //加载本地文件
    public void LoadFromFileByAB(string path)
    {
       //加载AB包
       AssetBundle ab= AssetBundle.LoadFromFile(path);
       //加载GameObject对象
       GameObject obj = ab.LoadAsset<GameObject>("Cube");
       Instantiate(obj);
    }

    //从内存中加载 异步加载
    IEnumerator  LoadFromMemoryAsyncByAB(string path)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path));
        //等待加载完成
        yield return request;
        //加载完毕后就可以使用了
        AssetBundle ab = request.assetBundle;
        //加载GameObject对象
        GameObject obj = ab.LoadAsset<GameObject>("Cube");
        Instantiate(obj);
    }

    //从内存中加载 同步加载
    private void LoadFromMemoryByAB(string path)
    {
        //加载完毕后就可以使用了
        AssetBundle ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(path));
        //加载GameObject对象
        GameObject obj = ab.LoadAsset<GameObject>("Cube");
        Instantiate(obj);
    }

    //本地文件路径或网络路径加载
    IEnumerator UnityWebRequestByAB(Uri uri)
    {
        //获取资源
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri);
        //等待与服务器通信
        yield return request.SendWebRequest();
        //取得内容
        //AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        //可以下载固定对象
        AssetBundle ab =(request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
        //保存到本地 需要路径 
        //File.WriteAllBytes();

        //加载GameObject对象 还有多种方式 具体查看文档
        GameObject obj = ab.LoadAsset<GameObject>("Cube");
        Instantiate(obj);
    }

    //查看某个包依赖于哪些包
    private void RelyONByAB(string abName)
    {
        AssetBundle manifestAB = AssetBundle.LoadFromFile("AssetBundles/AssetBundles");
        AssetBundleManifest manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //输出所有AB包名
        //foreach (string name in manifest.GetAllAssetBundles())
        //{
        //    Debug.Log(name);
        //}

        //查询某个包的依赖包 并加载
        string[] strs= manifest.GetAllDependencies(abName);
        foreach (string name in strs)
        {
            Debug.Log(name);
            AssetBundle.LoadFromFile("AssetBundles/" + name);
        }
    }
}
