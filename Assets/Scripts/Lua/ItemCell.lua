Object:subClass("ItemCell")
ItemCell.obj = nil
ItemCell.icon = nil
ItemCell.num = nil

function ItemCell:Init(father, data)
    ResourceManager:LoadUI("ItemCell",function(obj)
        self.obj = GameObject.Instantiate(obj)
        self.obj.transform:SetParent(father, false)

        local itemData = itemInfo[data.id]

        self.num = self.obj.transform:Find("Num"):GetComponent(typeof(Text))
        self.num.text = data.num

        self.icon = self.obj.transform:Find("Icon"):GetComponent(typeof(Image))
        local strs = string.split(itemData.icon,"_")
        ResourceManager:LoadSprite("Icon.spriteatlas",function(obj)
            spriteAtlas = obj
            self.icon.sprite = spriteAtlas:GetSprite(strs[2]) 
        end)
    end)
end

function ItemCell:Destroy()
    GameObject.Destroy(self.obj)
    self.obj = nil
end


