---
title: "Multiplayer"
---

Multyplayer is built in the GameBuilder, and though it mostly works without any special considerations, there are 
things to take care of in multiplayer environment. The game can be running in a single-player or multiplayer mode, 
which can be checked with a call to `isInMultiplayerMode()`. Note that single player mode can still have multiple 
players by using virtual players, see [console](console.md) documentation. Likewise, multiplayer mode doesn't mean there
are more than one player currently.  

## Players and Actors

One critical point to understand in multiplayer is to distinguish between a player and an actor:

* `player` – a person at the computer, or more precisely an instance of a GameBuilder running
* `actor` – an active element of the scene, possibly with some cards attached

Player generates input events and is presented with the rendered scene. 
For an actor to receive these input events it should be _controlled by the player_. 
Only one actor can be controlled by the player at a time.
Normally, the `Player Panel` handles all the player-to-actor assignments, including multiplayer cases. 

* `getControllingPlayer` – tells which player controls an actor
* `setControllingPlayer` – assigns a player to the current actor
* `getPlayerControlledActor` – gets actor that is being controlled by the current player

## Player ID

Though both player and actor have identifiers and they look similar (GUID-like strings), they are different things.
An attempt to call `getPos(player)` when `player` is a player and not an actor will fail at runtime. 

* `getAllPlayers()` – returns an array of player IDs currently connected to the game.
* `getLocalPlayer()` – returns an ID of the local player. 
It is either a controlling player, or player for which `onLocalTick` is currently running.

## 

For more information see documentation from developers: [How does multiplayer work](https://docs.google.com/document/d/1EIlvn-RD0IxdvYHQxSOfWVU6LpZTe1XDLXA1PSqJd6A/edit)
