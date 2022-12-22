ResourceManager:LoadJson("ItemInfo", function(obj)
    local txt = obj
    itemList = Json.decode(txt.text)
    itemInfo = {}
    for key, value in pairs(itemList.info) do
        itemInfo[value.id] = value
    end
end)

