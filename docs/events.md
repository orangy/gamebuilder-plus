Events are functions that are called by the engine on the card and/or panels. 
They need to be `export function` and generally follow the following pattern:

```js
export function onEventName(msg) {
  // your code here
}
```

## User input events 
These events are called on a card on the player-controllable actor when player input happens
`hasPlayerInput` can be called to check if current actor has player input. 

* `onKeyDown(KeyMessage)` – called once on key press
* `onKeyHeld(KeyMessage)` - called each frame when the key is being held
* `onMouseUp()` – called when primary mouse button was just released
* `onMouseDown()` – called when primary mouse button was just pressed 
* `onMouseHeld()` – called each frame when primary mouse button is held
* `onActorClicked` – called when this actor was just clicked
