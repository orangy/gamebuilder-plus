---
title: "Console"
---

Toggle console by using `~` key. 

# Logging

Use `log("…")` function in JavaScript to output text to console. 
You can use formatting in messages, e.g. `<color=yellow>text</color>`.

# Cards

* `exportcards` – saves customized cards to a directory.
* `importcards` – loads cards from a directory.
* `defaultcards` – toggles creation of default cards on new panels, such as Collision + Red Flash for an If/Then panel.
* `jshotload` – see [Hot reloading cards](https://docs.google.com/document/d/1M1vw78aksyPDO7NIbomyylIKYWs1KOnbL882bY256XM/edit).
* `findjs` – find a text in all custom JavaScript cards.

# Virtual Multiplayer

When you don't have anyone to play with and you want to test multiplayer features, you can use `virtual players`.

* `vpjoin` – join and switch to a virtual player.
* `vpleave` – leave current virtual player.
* `vpswitch #` – switch to a virtual player by number.
* `vpnick <nick>` – set current virtual player's nickname.
* `vplist` – list all virtual players.
* `playmodeonly t|f` – set mode to play only (t) or play & edit (f). 

# Map

* `load` – load a VOOS file (opens dialog to choose file).
* `save` – saves currently opened VOOS file.
* `reload` – discards all changes and reloads current file. 
* `setground #` – sets ground terrain type to #. You need to know [codes](https://docs.google.com/document/d/1RovaMCZhEgnWolxAHeB_v4ZzW8ROQjIIOJnayKDupYg/edit). 
* `terrainreplace` – replace one kind of terrain with another. E.g. `terrainreplace Grass Stone`

# Actors

* `speccol` – enables speculative collision on selected actors. If something flies too fast and goes through terrain – try it!
* `nospeccol` – disables speculative collision on selected actors.
* `actors <optional-text>` – lists all actors, or if optional-text is given actors having this text in their display name. 

# UI

* `toggleui` – show or hide all the UI. Useful for screenshot.
* `togglehud` – show or hide HUD part of the UI.
* `toggleeditavatar` – show or hide red transparent robot in edit mode.

# Misc

* `clearpolycache` – resets the cache of downloaded models, in case something sticks incorrectly.
* `clear` – clears the console.
* `quit` – quit the application.
* `help` – lists all console commands along with their descriptions.