using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XLua;

public static class AddXluaFeature
{
    //实现为系统类添加[CSharpCallLua]和[LuaCallCSharp]特性
    [CSharpCallLua]
    public static List<Type> csharpCallLuaList = new List<Type>()
    {
        //将需要添加特性的类放入list中,再手动生成Xlua代码即可
        typeof(UnityAction<float>),
        typeof(UnityAction<bool>), 
    };
    [LuaCallCSharp]
    public static List<Type> luaCallCsharpList = new List<Type>()
    {
        //将需要添加特性的类放入list中,再手动生成Xlua代码即可
        typeof(GameObject),
        typeof(Rigidbody),
    };
}

