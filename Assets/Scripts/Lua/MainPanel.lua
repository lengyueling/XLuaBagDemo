BasePanel:subClass("MainPanel")
MainPanel.panelObj = nil
MainPanel.btnBag = nil

function MainPanel:Init()
    self.base.Init(self)
    if MainPanel.panelObj == nil then
        ResourceManager:LoadUI("MainPanel",function(obj)
            self.panelObj = GameObject.Instantiate(obj)
            self.panelObj.transform:SetParent(Canvas, false)
            self.panelObj:SetActive(true)
            
            self.btnBag = self.panelObj.transform:Find("BtnPanel").transform:Find("Bag"):GetComponent(typeof(Button))
            self.btnBag.onClick:AddListener(function()
                self:BtnBagClick()
            end)
        end)
    end

end

function MainPanel:BtnBagClick()
    BagPanel:ShowMe()
end