BasePanel:subClass("BagPanel")

BagPanel.panelObj = nil
BagPanel.btnClose = nil
BagPanel.togEquip = nil
BagPanel.togItem = nil
BagPanel.togGem = nil
BagPanel.svBag = nil
BagPanel.content = nil
BagPanel.items = {}

function BagPanel:Init()
    if self.isInit then
       return 
    end
    self.base.Init(self)
    if BagPanel.panelObj == nil then
        ResourceManager:LoadUI("BagPanel",function(obj)
            self.panelObj = GameObject.Instantiate(obj)
            self.panelObj.transform:SetParent(Canvas, false)
            self.panelObj:SetActive(true)

            local panel = self.panelObj.transform:Find("Panel")

            self.btnClose = panel.transform:Find("Close"):GetComponent(typeof(Button))
            self.btnClose.onClick:AddListener(function()
                self:BtnCloseClick()
            end)

            self.togEquip = panel.transform:Find("Equip"):GetComponent(typeof(Toggle))
            self.togEquip.onValueChanged:AddListener(function(value)
                if value == true then
                    self:ChangeType(1)
                end
            end)

            self.togItem = panel.transform:Find("Item"):GetComponent(typeof(Toggle))
            self.togItem.onValueChanged:AddListener(function(value)
                if value == true then
                    self:ChangeType(2)
                end
            end)

            self.togGem = panel.transform:Find("Gem"):GetComponent(typeof(Toggle))
            self.togGem.onValueChanged:AddListener(function(value)
                if value == true then
                    self:ChangeType(3)
                end
            end)

            self.svBag = panel.transform:Find("Scroll View"):GetComponent(typeof(ScrollRect))
            self.content = self.svBag.transform:Find("Viewport").transform:Find("Content")

            self.isInit = true
            BagPanel:ChangeType(2)
        end)
    end

end

function BagPanel:ShowMe()
    self.base.ShowMe(self)
    if self.isInit then
        self.panelObj:SetActive(true)
        BagPanel:ChangeType(2)
    end
    
    
end

function BagPanel:HideMe()
    self.base.HideMe(self)
end

function BagPanel:BtnCloseClick()
    BagPanel:HideMe()
end

--1:equip 2:item 3:gem
function BagPanel:ChangeType(type)
    for key, value in pairs(BagPanel.items) do
        self.items[key]:Destroy()
    end

    self.items = {}

    local nowItems = nil
    if type == 1 then
        nowItems = PlayerInfo.equips
    elseif type == 2 then
        nowItems = PlayerInfo.items
    elseif type == 3 then
        nowItems = PlayerInfo.gems
    end

    for key, value in pairs(nowItems) do
        local grid = ItemCell:new()
        grid:Init(self.content, nowItems[key])
        table.insert(self.items, grid)
    end
end