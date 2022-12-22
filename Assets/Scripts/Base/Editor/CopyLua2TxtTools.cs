using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CopyLua2TxtTools : Editor
{
    [MenuItem("Tools/CopyLua2Txt")]
    public static void CopyLua2Txt()
    {
        if(!Directory.Exists(PathUtil.LuaPath))
        {
            return;
        }
        if(!Directory.Exists(PathUtil.BuildLuaPath))
        {
            Directory.CreateDirectory(PathUtil.BuildLuaPath);
        }
        else
        {
            string[] oldFiles = Directory.GetFiles(PathUtil.BuildLuaPath, "*.txt", SearchOption.AllDirectories);
            for(int i = 0; i < oldFiles.Length; i++)
            {
                File.Delete(oldFiles[i]);
            }
        }

        string[] files = Directory.GetFiles(PathUtil.LuaPath,"*.lua", SearchOption.AllDirectories);
        string fileName;
        for (int i = 0; i < files.Length; i++)
        {
            fileName = PathUtil.BuildLuaPath + files[i].Substring(files[i].LastIndexOf("/")+1) + ".txt";
            File.Copy(files[i], fileName);
        }   
        AssetDatabase.Refresh();
    }
}
