public enum GameMode
{
    /// <summary>
    /// 读取编辑器模式下Asset内的资源
    /// 加载的不是assetbundle
    /// </summary>
    EditorMode,
    /// <summary>
    /// 读取streamingAssetsPath下的资源
    /// 加载的是打包后的bundle
    /// </summary>
    PackageBundle,
    /// <summary>
    /// 读取服务器上资源
    /// 加载的是打包后的bundle
    /// </summary>
    UpdateMode
}

public class AppConst
{
    public const string BundleExtension = ".ab";
    public const string FileListName = "filelist.txt";
    /// <summary>
    /// 当前游戏模式，默认为编辑器模式
    /// </summary>
    public static GameMode GameMode = GameMode.EditorMode;
    public static bool OpenLog = false;
    /// <summary>
    /// 资源更新服务器目录 
    /// </summary>
    public const string ResourcesUrl = "http://127.0.0.1:10888/AssetBundles/";
}
