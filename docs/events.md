---
title: "Events"
---

Events are functions that are called by the engine on the card and/or panels. 
They need to be `export function` and generally follow the following pattern:

```js
export function onEventName(msg) {
  // your code here
}
```

## Initialization events

* `onInit` – called when card is added to the actor or when the game is reset
* `onResetGame` – called when the game is reset

> NOTE: These events are not guaranteed to be sent in order. For example, 
> when game is being reset,  `onResetGame` can be called before `onInit`

## Multiplayer events

These events are about players (humans at computers), not actors in scene. 
For more information see [mutliplayer](multiplayer.md) 

* `onPlayerJoined(PlayerJoinedMessage)` – called when player has joined the game 
* `onPlayerLeft(PlayerLeftMessage)` – called when player has left the game

> NOTE: `onPlayerJoined` is not called for a player in a single player game. 
> Use `getAllPlayers` function to initialize the state in this case.

See also virtual multiplayer [console commands](console.md) 

## User input events 
These events are called on a card on the player-controllable actor when player input happens
`hasPlayerInput` can be called to check if current actor has player input. 

* `onKeyDown(KeyMessage)` – called once on key press
* `onKeyHeld(KeyMessage)` - called each frame when the key is being held
* `onMouseUp()` – called when primary mouse button was just released
* `onMouseDown()` – called when primary mouse button was just pressed 
* `onMouseHeld()` – called each frame when primary mouse button is held
* `onActorClicked` – called when this actor was just clicked
