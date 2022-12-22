using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作为所有manager的入口
/// </summary>
public class Manager : MonoBehaviour
{
    private static ResourceManager _resource;
    public static ResourceManager Resource
    {
        get { return _resource; }
    }

    private static LuaManager _lua;
    public static LuaManager Lua
    {
        get { return _lua; }
    }

    private void Awake()
    {
        _resource = this.gameObject.AddComponent<ResourceManager>();
        _lua = this.gameObject.AddComponent<LuaManager>();
    }
}