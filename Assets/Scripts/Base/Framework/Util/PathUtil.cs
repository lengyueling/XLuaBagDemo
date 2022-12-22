using UnityEngine;

/// <summary>
/// 磁盘路径相关的工具类
/// 既方便使用，还减少GC
/// </summary>
public class PathUtil
{
    /// <summary>
    /// 根目录
    /// </summary>
    public static readonly string AssetsPath = Application.dataPath;

    /// <summary>
    /// 需要打Bundle的目录
    /// </summary>
    public static readonly string BuildResourcesPath = AssetsPath + "/BuildResources/";

    /// <summary>
    /// Bundle输出目录
    /// 不一定是streamingAssets
    /// 可以在这里更改
    /// </summary>
    public static readonly string BundleOutPath = Application.streamingAssetsPath;

    /// <summary>
    /// 只读目录
    /// </summary>
    public static readonly string ReadPath = Application.streamingAssetsPath;

    /// <summary>
    /// 可读写目录
    /// </summary>
    public static readonly string ReadWritePath = Application.persistentDataPath;

    /// <summary>
    /// Lua脚本打包路径
    /// </summary>
    public static readonly string BuildLuaPath = BuildResourcesPath + "/LuaScript/";

    /// <summary>
    /// Lua编写路径
    /// </summary>
    public static readonly string LuaPath = AssetsPath + "/Scripts/Lua/";

    /// <summary>
    /// Bundle资源路径
    /// 更新模式下返回可读写的目录sersistentDataPath
    /// 其他模式返回只读目录streamingAssetsPath
    /// </summary>
    public static string BundleResourcePath
    {
        get
        {
            //更新模式（分包模式）下，ReadPath是空的
            if (AppConst.GameMode == GameMode.UpdateMode)
            {
                //返回可读写的目录sersistentDataPath
                return ReadWritePath;
            }
            //返回只读目录streamingAssetsPath
            return ReadPath;
        }
    }

    /// <summary>
    /// 获取Unity相对路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }
        return path.Substring(path.IndexOf("Assets"));
    }

    /// <summary>
    /// 获取标准路径
    /// \ 变为 /
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetStandardPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }
        return path.Trim().Replace("\\", "/");
    }

    /// <summary>
    /// 获取Asset中Lua文件路径
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetLuaPath(string name)
    {
        return string.Format("Assets/BuildResources/LuaScript/{0}.lua.txt", name);
    }

    /// <summary>
    /// 获取Asset中UI文件路径
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetUIPath(string name)
    {
        return string.Format("Assets/BuildResources/UI/Prefab/{0}.prefab", name);
    }

    /// <summary>
    /// 获取音乐路径
    /// 后缀需要手动传递
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetMusicPath(string name)
    {
        return string.Format("Assets/BuildResources/Audio/Music/{0}", name);
    }

    /// <summary>
    /// 获取音效路径
    /// 后缀需要手动传递
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetSoundPath(string name)
    {
        return string.Format("Assets/BuildResources/Audio/Sound/{0}", name);
    }

    public static string GetEffectPath(string name)
    {
        return string.Format("Assets/BuildResources/Effect/Prefab/{0}.prefab", name);
    }

    public static string GetModelPath(string name)
    {
        return string.Format("Assets/BuildResources/Model/Prefab/{0}.prefab", name);
    }

    /// <summary>
    /// 获取ui图片路径
    /// 后缀需要手动传递
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetSpritePath(string name)
    {
        return string.Format("Assets/BuildResources/UI/Sprite/{0}", name);
    }

    public static string GetScenePath(string name)
    {
        return string.Format("Assets/BuildResources/Scene/{0}.unity", name);
    }

    public static string GetJsonPath(string name)
    {
        return string.Format("Assets/BuildResources/Json/{0}.txt", name);
    }
}
