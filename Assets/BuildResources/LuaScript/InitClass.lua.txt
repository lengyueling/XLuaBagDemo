--面向对象相关
require("Object")
--字符串拆分
require("SplitTools")
--Json解析
Json = require("JsonUtility")

--Unity相关
GameObject = CS.UnityEngine.GameObject
Resources = CS.UnityEngine.Resources
Transform = CS.UnityEngine.Transform
RectTransform = CS.UnityEngine.RectTransform
TextAsset = CS.UnityEngine.TextAsset
--图集对象类
SpriteAtlas = CS.UnityEngine.U2D.SpriteAtlas

Vector3 = CS.UnityEngine.Vector3
Vector2 = CS.UnityEngine.Vector2

--UI相关
UI = CS.UnityEngine.UI
Image = UI.Image
Text = UI.Text
Button = UI.Button
Toggle = UI.Toggle
ScrollRect = UI.ScrollRect
Canvas = GameObject.Find("Canvas").transform
UIBehaviour = CS.UnityEngine.EventSystems.UIBehaviour

--C#脚本相关
ResourceManager = CS.Manager.Resource
