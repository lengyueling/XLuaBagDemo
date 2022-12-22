using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Object = UnityEngine.Object;

/// <summary>
/// 资源热更新操作类
/// </summary>
public class HotUpdate : MonoBehaviour
{
    /// <summary>
    /// 只读目录filelist的文件数据
    /// </summary>
    byte[] m_ReadPathFileListData;
    /// <summary>
    /// 从服务器下载的filelist文件数据
    /// </summary>
    byte[] m_ServerFileListData;

    /// <summary>
    /// filelist中的文件信息
    /// </summary>
    internal class DownFileInfo
    {
        public string url;
        public string fileName;
        public DownloadHandler fileData;
    }

    /// <summary>
    /// 下载单个文件
    /// </summary>
    /// <param name="info"></param>
    /// <param name="Complete"></param>
    /// <returns></returns>
    IEnumerator DownLoadFile(DownFileInfo info, Action<DownFileInfo> Complete)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(info.url);
        yield return webRequest.SendWebRequest();
        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("下载文件出错：" + info.url);
            yield break;
        }
        info.fileData = webRequest.downloadHandler;
        //执行回调函数
        Complete?.Invoke(info);
        //释放UnityWebRequest对象
        webRequest.Dispose();
    }

    /// <summary>
    /// 下载多个文件
    /// </summary>
    /// <param name="infos"></param>
    /// <param name="Complete"></param>
    /// <param name="DownLoadAllComplete"></param>
    /// <returns></returns>
    IEnumerator DownLoadFile(List<DownFileInfo> infos, Action<DownFileInfo> Complete, Action DownLoadAllComplete)
    {
        foreach (DownFileInfo info in infos)
        {
            yield return DownLoadFile(info, Complete);
        }
        DownLoadAllComplete?.Invoke();
    }

    /// <summary>
    /// 获取filelist文件信息
    /// </summary>
    /// <param name="fileData"></param>
    /// <returns></returns>
    private List<DownFileInfo> GetFileList(string fileData, string path)
    {
        string content = fileData.Trim().Replace("\r", "");
        //分割获取filelist内的多个文件
        string[] files = content.Split('\n');
        //每一个文件一个downFileInfos，储存filelist中的文件信息，要将获取的信息储存进来
        List<DownFileInfo> downFileInfos = new List<DownFileInfo>(files.Length);
        for (int i = 0; i < files.Length; i++)
        {
            //分割获取文件的多个部分（文件路径|bundle名|依赖文件列表）
            string[] info = files[i].Split('|');
            DownFileInfo fileInfo = new DownFileInfo();
            fileInfo.fileName = info[1];
            fileInfo.url = Path.Combine(path, info[1]);
            downFileInfos.Add(fileInfo);
        }
        return downFileInfos;
    }

    private void Start()
    {
        if (AppConst.GameMode == GameMode.EditorMode)
        {
            EnterGame();
        }
        else
        {
            if (IsFirstInstall())
            {
                ReleaseResources();
            }
            else
            {
                CheckUpdate();
            }
        }
    }

    /// <summary>
    /// 是否初次安装
    /// 或者说是否是分包模式还是整包模式
    /// 只读目录存在filelist且可读写目录没有filelist
    /// </summary>
    /// <returns></returns>
    private bool IsFirstInstall()
    {
        //判断只读目录是否存在版本文件
        bool isExistsReadPath = FileUtil.IsExists(Path.Combine(PathUtil.ReadPath, AppConst.FileListName));

        //判断可读写目录是否存在版本文件
        bool isExistsReadWritePath = FileUtil.IsExists(Path.Combine(PathUtil.ReadWritePath, AppConst.FileListName));

        return isExistsReadPath && !isExistsReadWritePath;
    }

    /// <summary>
    /// 释放资源
    /// 下载只读文件夹中的filelist
    /// </summary>
    private void ReleaseResources()
    {
        string url = Path.Combine(PathUtil.ReadPath, AppConst.FileListName);
        DownFileInfo info = new DownFileInfo();
        info.url = url;
        StartCoroutine(DownLoadFile(info, OnDownLoadReadPathFileListComplete));
    }

    /// <summary>
    /// 解析文件信息
    /// 根据filelist下载热更新资源到可读写目录
    /// </summary>
    /// <param name="file"></param>
    private void OnDownLoadReadPathFileListComplete(DownFileInfo file)
    {
        //将只读目录的filelist数据赋值，供之后可读写目录使用
        m_ReadPathFileListData = file.fileData.data;
        List<DownFileInfo> fileInfos = GetFileList(file.fileData.text, PathUtil.ReadPath);
        StartCoroutine(DownLoadFile(fileInfos, OnReleaseFileComplete, OnReleaseAllFileComplete));
    }

    /// <summary>
    /// 下载所有热更新资源到可读写目录后后
    /// 在可读写目录写入只读目录的filelist
    /// </summary>
    private void OnReleaseAllFileComplete()
    {
        //在可读写目录写入只读目录的filelist
        FileUtil.WriteFile(Path.Combine(PathUtil.ReadWritePath, AppConst.FileListName), m_ReadPathFileListData);
        CheckUpdate();
    }

    /// <summary>
    /// 每下载一个热更新资源
    /// 写入对应的热更新资源到可读写文件夹
    /// </summary>
    /// <param name="obj"></param>
    private void OnReleaseFileComplete(DownFileInfo fileInfo)
    {
        Debug.Log("OnReleaseFileComplete:" + fileInfo.url);
        string writeFile = Path.Combine(PathUtil.ReadWritePath, fileInfo.fileName);
        FileUtil.WriteFile(writeFile, fileInfo.fileData.data);
    }

    /// <summary>
    /// 通过filelist检查版本更新
    /// </summary>
    private void CheckUpdate()
    {
        string url = Path.Combine(AppConst.ResourcesUrl, AppConst.FileListName);
        DownFileInfo info = new DownFileInfo();
        info.url = url;
        StartCoroutine(DownLoadFile(info, OnDownLoadServerFileListComplete));
    }

    /// <summary>
    /// 下载完成了所有的热更新文件后
    /// 检查对比现有本地目录和服务器目录filelist的不同
    /// 根据filelist下载还未下载的资源
    /// </summary>
    /// <param name="file"></param>
    private void OnDownLoadServerFileListComplete(DownFileInfo file)
    {
        m_ServerFileListData = file.fileData.data;
        List<DownFileInfo> fileInfos = GetFileList(file.fileData.text, AppConst.ResourcesUrl);
        List<DownFileInfo> downListFiles = new List<DownFileInfo>();
        for (int i = 0; i < fileInfos.Count; i++)
        {
            //通过课读写路径+文件名拼接获取本地路径，判断文件是否存在，若不存在则加入到需要加载资源的列表
            string localFile = Path.Combine(PathUtil.ReadWritePath, fileInfos[i].fileName);
            if (!FileUtil.IsExists(localFile))
            {
                fileInfos[i].url = Path.Combine(AppConst.ResourcesUrl, fileInfos[i].fileName);
                downListFiles.Add(fileInfos[i]);
            }
        }
        //加载资源的列表长度>0说明还有服务器资源没有被下载，下载资源
        if (downListFiles.Count > 0)
        {
            StartCoroutine(DownLoadFile(fileInfos, OnUpdateFileComplete, OnUpdateAllFileComplete));
        }
        else
        {
            EnterGame();
        }
    }

    /// <summary>
    /// 更新所有热更新资源后
    /// 写入新的filelist
    /// </summary>
    private void OnUpdateAllFileComplete()
    {
        //更新完成后，需要把最新的filelist文件下载到本地ReadWritePath
        FileUtil.WriteFile(Path.Combine(PathUtil.ReadWritePath, AppConst.FileListName), m_ServerFileListData);
        EnterGame();
    }

    /// <summary>
    /// 每更新一个热更新资源
    /// 写入对应的热更新资源
    /// </summary>
    /// <param name="file"></param>
    private void OnUpdateFileComplete(DownFileInfo file)
    {
        Debug.Log("OnUpdateFileComplete:" + file.url);
        string writeFile = Path.Combine(PathUtil.ReadWritePath, file.fileName);
        FileUtil.WriteFile(writeFile, file.fileData.data);
    }

    /// <summary>
    /// 开始游戏
    /// 开始解析文件 加载资源
    /// 如果不是编辑器模式，此时已经在可读写文件/只读目录获取了filelist
    /// </summary>
    private void EnterGame()
    {
        Manager.Resource.ParseVersonFile();

        Manager.Lua.Init(()=>
        {
            Manager.Lua.DoLuaFile("Main");
        });
    }
}