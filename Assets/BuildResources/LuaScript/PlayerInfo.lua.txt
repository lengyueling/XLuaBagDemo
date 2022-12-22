PlayerInfo = {}
PlayerInfo.equips = {}
PlayerInfo.items = {}
PlayerInfo.gems = {}

function PlayerInfo:Init()
    table.insert(self.equips,{id = 1,num = 1})
    table.insert(self.equips,{id = 7,num = 1})
    table.insert(self.equips,{id = 2,num = 1})
    table.insert(self.items,{id = 3,num = 40})
    table.insert(self.items,{id = 4,num = 99})
    table.insert(self.gems,{id = 5,num = 50})
    table.insert(self.gems,{id = 6,num = 99})
end