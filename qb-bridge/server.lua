local QBCore = exports['qb-core']:GetCoreObject()

RegisterNetEvent('QBBridge:GetPlayerData', function(source,key)
    print(source)
    local PlayerData = QBCore.Functions.GetPlayer(tonumber(source))
    TriggerEvent("returnQbJobFromQbCore",tostring(PlayerData.PlayerData.job.name),tostring(key))
end)

RegisterNetEvent('QBCore:ToggleDuty', function()
    local src = source
    local Player = QBCore.Functions.GetPlayer(src)
    if not Player then return end
    if Player.PlayerData.job.onduty then
        TriggerEvent("cs:engine:server:duty:tracker",tostring(Player.PlayerData.id),false)
    else
        TriggerEvent('cs:engine:server:duty:tracker',tostring(Player.PlayerData.id),true)
    end
end)

RegisterNetEvent('cs:engine:client:qbcore:checkplayerdata', function(source)
    print"bridge triger"
    TriggerEvent('cs:engine:server:qbcore:checkplayerdata',json.encode(QBCore.Player.CheckPlayerData(nil, source)))
end)