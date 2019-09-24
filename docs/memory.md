---
title: "Memory"
---

Actors have a place to store their data:

* `card` - private per card instance
  * JS code in this card instance will see and can modify the value
  * Not visible to other cards or actors, even if the card is of the same type on same actor
  * Serialized to scene file and sent over network
  * Useful for storing card-specific state
* `temp` - private per card instance, can be discarded at any time 
  * JS code in this card instance will see and can modify the value
  * Not visible to other cards or actors, even if the card is of the same type on same actor
  * Can be discarded at any time without any notice.
  * Useful for caches. 
* `mem` - private per actor
  * Any JS code on this actor will see and can modify the value
  * Not visible to other actors
  * Serialized to scene file and sent over network
  * Useful for storing actor data that can be used and modified by several cards
* `getVar` and `setVar` – public per actor
  * Any JS code in scene can see and can modify the value
  * Visible to every card on every actor
  * Serialized to scene file and sent over network
  * Useful for storing public actor data that can be accessed by multiple actors

Examples:

```js
export function onInit() {
    mem.answer = 42; // actor's state
    card.value = "Hello!"; // card's state
    setVar("health", 100); // public actor's state
}

export function onCollision(msg) {
    if (temp.cachedValue === undefined)
        temp.cachedValue = calculateValue(); // cache expensive values
        
    setVarPlease(msg.actor, "health", getVar("health", msg.actor) - 10); // ask another actor to modify its variable
}
```  