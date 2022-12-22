using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 文件操作相关工具类
/// </summary>
public class FileUtil
{
    /// <summary>
    /// 文件是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsExists(string path)
    {
        FileInfo file = new FileInfo(path);
        return file.Exists;
    }

    /// <summary>
    /// 写入文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static void WriteFile(string path, byte[]data)
    {
        //获取标准路径
        path = PathUtil.GetStandardPath(path);
        //获取文件夹路径
        string dir = path.Substring(0, path.LastIndexOf("/"));
        //如果不存在该文件夹则创建一个
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        FileInfo file = new FileInfo(path);
        //如果文件存在则删除（无法覆盖写入）
        if (file.Exists)
        {
            file.Delete();
        }
        //写入文件
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
        catch(IOException e)
        {
            Debug.LogError(e.Message);
        }
    }
}