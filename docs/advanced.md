
## Meta JSON

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
  * `icon:name` – format used for [Material Icons](https://material.io/resources/icons)
  * `BuiltinAssets/CardImages/oscillate` – format used for built-in icons (you have to know their names)
  
 

 