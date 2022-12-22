using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

/// <summary>
/// 打包工具类
/// </summary>
public class BuildTools : Editor
{
    [MenuItem("Tools/Build/Build Windows Bundle")]
    static void BundleWindowsBuild()
    {
        Build(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Tools/Build/Build Android Bundle")]
    static void BundleAndroidBuild()
    {
        Build(BuildTarget.Android);
    }

    [MenuItem("Tools/Build/Build IOS Bundle")]
    static void BundleIOSBuild()
    {
        Build(BuildTarget.iOS);
    }

    /// <summary>
    /// 打包
    /// </summary>
    /// <param name="target">打包平台</param>
    static void Build(BuildTarget target)
    {
        //所有需要被打ab包的资源列表
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();

        //文件信息列表（filelist）
        List<string> bundleInfos = new List<string>();

        //获取所有文件，递归查找所有子目录
        string[] files = Directory.GetFiles(PathUtil.BuildResourcesPath, "*", SearchOption.AllDirectories);
        
        for (int i = 0; i < files.Length; i++)
        {
            //排除.meta文件
            if (files[i].EndsWith(".meta"))
            {
                continue;
            }
            //实例化每个要打包的对象
            AssetBundleBuild assetBundle = new AssetBundleBuild();

            string fileName = PathUtil.GetStandardPath(files[i]);
            Debug.Log("file:" + fileName);
            //要打包的资源名
            string assetName = PathUtil.GetUnityPath(fileName);
            //获取要打包的资源名字给打包对象
            assetBundle.assetNames = new string[] { assetName };
            string bundleName = fileName.Replace(PathUtil.BuildResourcesPath, "").ToLower();
            //设置打包后资源的名字
            assetBundle.assetBundleName = bundleName + ".ab";
            assetBundleBuilds.Add(assetBundle);

            //添加文件资源依赖信息到filelist中
            List<string> dependenceInfo = GetDependence(assetName);
            string bundleInfo = assetName + "|" + bundleName + ".ab";
            if (dependenceInfo.Count > 0)
            {
                bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);
            }
            bundleInfos.Add(bundleInfo);
        }
        
        //如果存在该文件夹则把他递归删除
        if (Directory.Exists(PathUtil.BundleOutPath))
        {
            Directory.Delete(PathUtil.BundleOutPath, true);
        }
        Directory.CreateDirectory(PathUtil.BundleOutPath);
        //打包Bundle，目标目录，压缩方式，目标平台
        BuildPipeline.BuildAssetBundles(PathUtil.BundleOutPath,assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
        //写入文件信息，依赖关系
        File.WriteAllLines(PathUtil.BundleOutPath + "/" + AppConst.FileListName, bundleInfos);
        //刷新Asset面板
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取依赖文件列表
    /// </summary>
    /// <param name="curFile"></param>
    /// <returns></returns>
    static List<string> GetDependence(string curFile)
    {
        List<string> dependence = new List<string>();
        string[] files = AssetDatabase.GetDependencies(curFile);
        //排除.cs文件和当前文件本身
        dependence = files.Where(file => !file.EndsWith(".cs") && !file.Equals(curFile)).ToList();
        return dependence;
    }
}
