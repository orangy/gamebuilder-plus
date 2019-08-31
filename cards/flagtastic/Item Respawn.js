export const PROPS = [
    propBoolean("Light", true),
    propNumber("LightRange", 80, {
        label: 'Light Range',
        requires: [requireTrue("Light")]
    }),
    propColor("LightColor", "#ffffff", {
        label: 'Light Color',
        requires: [requireTrue("Light")]
    }),
    propDecimal("RespawnTime", 20, {
        label: "Respawn Time (seconds):",
    }),
    propSound("RespawnSound", Sounds.HIT, {
        label: "Respawn Sound",
    }),
    propBoolean("Randomize", true, {
        label: "Randomize Spawn"
    })
];

export function onResetGame() {
    delete card.light;
    if (props.Randomize) {
        hide();
        setLight(0);
        let randomStart = THREE.Math.randFloat(props.RespawnTime / 2, props.RespawnTime);
        sendToSelfDelayed(randomStart, "Spawn");
    } else {
        show();
        if (props.Light) {
            setLight(props.LightRange, props.LightColor);
        } else {
            setLight(0);
        }
    }
}

export function onAction(actionMessage) {
    hide();
    sendToSelfDelayed(props.RespawnTime, "Spawn");
}

export function onSpawn() {
    if (isVisible()) return;
    
    card.light = 0;
    if (props.Light) {
        onLightUp();
    }
    show();
    playSound(props.RespawnSound);
}

export function onLightUp() {
    setLight(card.light, props.LightColor);
    card.light += 5;
    if (card.light < props.LightRange) {
        sendToSelfDelayed(0.05, "LightUp");
    } else {
        setLight(props.LightRange, props.LightColor);
        delete card.light;
    }
}

export function getCardStatus() {
    return {
        description: `Respawns item in <color=orange>${props.RespawnTime}</color> seconds`
    }
}
