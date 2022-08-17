local QBCore = exports['qb-core']:GetCoreObject()

RegisterNetEvent('QBCore:ToggleDuty', function(source)
    local src = source
    local Player = QBCore.Functions.GetPlayer(src)
    if not Player then return end
    if Player.PlayerData.job.onduty then
        TriggerEvent("cs:engine:server:duty:tracker",tostring(Player.PlayerData.id),false)
    else
        TriggerEvent('cs:engine:server:duty:tracker',tostring(Player.PlayerData.id),true)
    end
end)

RegisterNetEvent('cs:engine:client:qbcore:checkplayerdata', function(pid)
    local Player = QBCore.Functions.GetPlayer(pid)
    TriggerEvent("cs:engine:server:playerdata:update",pid,json.encode(Player))
end)