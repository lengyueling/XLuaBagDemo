Object:subClass("BasePanel")
BasePanel.isInit = false
BasePanel.panelObj = nil

function BasePanel:Init()
end

function BasePanel:ShowMe()
    self:Init()
end

function BasePanel:HideMe()
    self.panelObj:SetActive(false)
end