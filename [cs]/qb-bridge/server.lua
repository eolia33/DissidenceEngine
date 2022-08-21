local QBCore = exports['qb-core']:GetCoreObject()

RegisterNetEvent('QBCore:ToggleDuty', function(source)
    local src = source
    local Player = QBCore.Functions.GetPlayer(src)
    if not Player then return end
    if Player.PlayerData.job.onduty then
        TriggerEvent("cs:engine:server:duty:tracker",tostring(Player.PlayerData.source),false)
    else
        TriggerEvent('cs:engine:server:duty:tracker',tostring(Player.PlayerData.source),true)
    end
end)

