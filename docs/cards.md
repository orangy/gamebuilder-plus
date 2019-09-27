---
title: "Cards"
---

If you author cards outside of GameBuilder, you create two files:

* `CardName.js` – contains JavaScript code for the card
* `CardName.js.metaJson` – contains metadata for the GameBuilder

Consider using `jshotload` so that GameBuilder will monitor changes to these files and reload it automatically. 
See [console][console.md] commands for details. 

## JavaScript

Card is a plain JavaScript code, and it typically starts with declaration of properties:

```js
export const PROPS = [ 
    // properties definitions
];
```

Unfortunately, you cannot use other JavaScript code (other than GameBuilder API), so you have to copy and paste all the code you need into the card.  

See:

* [Properties](http://gamebuilder.area120.com/props.html)
* [Events](events.md)

## metaJson

```
{
  "cardSystemCardData": {
    "isCard": true,
    "title": "…",
    "description": "…",
    "categories": [ … ],
    "imageResourcePath": "…",
    "userProvidedId": "…"
  }
}
```

* `title` – title of the card, will be used in search
* `description` – extended description, default value 
if no [getCardStatus](http://gamebuilder.area120.com/getCardStatus.html) is not provided
* `categories` – an array of categories for the card. Each panel specifies which category it accepts.
  * Custom – fits into `Custom Panel`
  * Action – fits into an `Action` deck of panels like If/Then, Always, Health
  * Event – fits into the `If` deck of conditional panel
  * Move – fits into the `Movements` panel
  * Camera – fits into the `Camera` panel
  * Screen – fits into the `Screen` panel
* `imageResourcePath` – an icon for the card. 
  * `icon:name` – format used for [Material Icons](https://material.io/icons). Not all icons from this site are supported, experiment. 
  * `BuiltinAssets/CardImages/oscillate` – format used for built-in icons (you have to find their names from magic sources)
  
## getCardStatus 

Implement `getCardStatus` function to improve the card usability. It is used by GameBuilder to dynamically provide
information about card configuration to users without the need to open card properties.

Use the following colors as a convention:
* `green` for references to actors
* `yellow` for names, identifiers, types, categories – things that should match similar property on the other card
* `orange` for everything else – numbers, text, colors, etc. 

Consider the following example of a status function for a `Start Timer` card: 

```js
export function getCardStatus() {
    return {
        description: `Starts timer <color=yellow>${props.Name}</color> on <color=green>${getCardTargetActorDescription("Actor")}</color> for <color=orange>${props.Time}</color> seconds.`
    }
}
``` 

In the example above `Name` is yellow, because user will have to use the same name on another card (Timer Expired Event)*[]: 
`Actor` is a reference to an actor and thus green.
`Time` is a number that specifies how much time should pass before timer expired event is triggered, and thus orange.

See [API documentation](http://gamebuilder.area120.com/RuntimeCardStatus.html) on what else can you return from this function.
