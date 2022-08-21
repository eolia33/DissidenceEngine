[Installation]

- ensure [cs] après qb-core

- éditer qb-core/server/player.lua à la ligne 190 et rajouter :
  TriggerEvent("cs:engine:server:playerdata:update", self.PlayerData.cid,json.encode(self.PlayerData));

- éditer qb-core/shared/items.lua et ajouter à la ligne 354
    ['balise'] = {['name'] = 'balise',['label'] = 'Balise', ['weight'] = 700,['type'] = 'item',['image'] = 'balise.png', ['unique'] = true,
    ['useable'] = true, ['shouldClose'] = true,    ['combinable'] = nil,   ['description'] = 'Une balise, très utile pour retrouver rapidement ses amis.'},

- éditer qb-smallresources/server/consumables et rajouter : 
QBCore.Functions.CreateUseableItem("balise", function(source)
    TriggerClientEvent("cs:engine:client:tracker:open", source)
end)

[Working]

- Tracker
  - Fréquence protégées
  - Duty On / Off
  - Notifications
  - Notifications On / Off
  - NUI
  - Changement de couleur live
  
- Shot Fire
  - Zones configurées dans le fichier zone.json.
  - Probabilité par zone, taille de cercle par zone, par période du jour. 
  - Découpage de la carte a 1300 entre le nord et le sud
  - Marge d'erreur réglée par zone dans la position du cercle de tir. 
  - Previent police ou sherif en fonction de la zone, et l'autre entité si personne en service.
  
 - Bridge
  - Récupératioin des données joueur en live depuis qb-core et upload vers le client (stocké dans un dictionnaire player dans le server)
  
 [To do]
 - Sauvegarder les param balise en bdd  
 - Vérifier si il ni a pas de bugs
 - Mettre une animation lorsque l'on sort la balise, peut être un props à voir
