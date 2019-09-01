export const PROPS = [
    propString("Type", "explosion", {
        label: "Type:",
    }),
    propDecimal("Radius", 5, {
        label: "Effect radius",
    }),
    propDecimal("Strength", 20, {
        label: "Effect strength",
    }),
    propBoolean("Prorate", true, {
        label: "Prorate by distance"
    })
];

export function onAction(actionMessage) {
    const center = getBoundsCenter();
    const affectedActors = overlapSphere(center, props.Radius);
    for (const actor of affectedActors) {
        const actorPos = getBoundsCenter(actor);
        let toActor = vec3sub(actorPos, center);
        let distance = vec3length(toActor);
        toActor = distance < 0.01 ? vec3y(1) : vec3normalized(toActor);
        let effect = props.Strength;
        if (props.Prorate)
            effect = interp(0, effect, props.Radius, 0, distance);
        send(actor, "Effect", {type: props.Type, strength: effect, direction: toActor})
    }
}

export function getCardStatus() {
    if (props.Prorate) {
        return {
            description: `Causes effect <color=yellow>${props.Type}</color> with inital strength <color=orange>${props.Strength}</color> decreasing in radius <color=orange>${props.Radius}</color>`
        }
    } else {
        return {
            description: `Causes effect <color=yellow>${props.Type}</color> with strength <color=orange>${props.Strength}</color> in radius <color=orange>${props.Radius}</color>`
        }
    }
}
