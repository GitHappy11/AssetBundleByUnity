using UnityEditor;
using System.IO;
using UnityEngine;
public class AssetBundlesEditor
{

   static string outPath = "AssetBundles";

    [MenuItem("Tools/BuildAssetBundles")]
   static void BuildAllAssetBundles()
    {
        BuildAssetBundles(outPath,BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows64);
    }

    // 打包AssetBundles 如果将预制体和依赖的材质打包在不同包里，则预制体会丢失材质 ，如果打包在相同的包中则不会丢失
    private static void BuildAssetBundles(string path, BuildAssetBundleOptions options, BuildTarget platform)
    {
        // 提示信息
        string msg = "当前平台：{0}\n输出路径：{1}";
        //补充提示信息
        msg = string.Format(msg, platform.ToString(), path);
        //创建提示窗口
        if (EditorUtility.DisplayDialog("打包信息", msg, "确定", "取消"))
        {
            //检测路径是否存在，如果不不存在则创建
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            //打包进度条
            //EditorUtility.DisplayProgressBar("信息", "正在打包资源包", 0f);
            //开始打包
            BuildPipeline.BuildAssetBundles(path, options, platform);
            //刷新编辑器资源
            AssetDatabase.Refresh();
            //tip弹窗提示
            EditorUtility.DisplayDialog("提示", "打包AssetBundle完毕", "确定");
        }
    }

}
