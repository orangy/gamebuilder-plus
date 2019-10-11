export const PROPS = [
    propDecimal("Speed", 2.0, {
        label: "Speed"
    }),
    propDecimal("Threshold", 1.0, {
        label: "Threshold"
    }),
    propBoolean('RequirePressed', true, {
        label: "Only when mouse is pressed"
    })
];

export function onTick() {
    if (props.RequirePressed && !mouseIsPressed()) {
        move(vec3zero()); // stop
        return;
    }

    const pointUnderMouse = getTerrainPointUnderMouse();
    if (pointUnderMouse && getDistanceTo(pointUnderMouse) > props.Threshold) {
        moveToward(pointUnderMouse, props.Speed);
    } else {
        move(vec3zero());// stop
    }
}

export function getCardStatus() {
    // <color=green>actor</color>
    // <color=orange>value</color>
    // <color=yellow>identifier</color>
    let when = `Always moves towards mouse pointer`;
    if (props.RequirePressed)
        when = `Moves towards mouse pointer when mouse button is pressed`;
    return {
        description: `${when}, speed <color=orange>${props.Speed}</color>. Do not move if distance is less than <color=orange>${props.Threshold}</color>`
    };
}

