export const PROPS = [
    propActorGroup("Actors", "@ANY", {
        label: "Collide with what:",
        pickerPrompt: "When I collide with what?",
    }),
    propDecimal("CollisionRadius", 1, {
        label: "Radius of the collision check:",
    }),
    propBoolean("IgnoreHidden", false, {
        label: "Ignore hidden actors"
    }),
    propBoolean("ActiveHidden", false, {
        label: "Active when hidden"
    })
];

export function onResetGame() {
    delete card.triggeredEvent;
}

export function onTick() {
    if (!props.ActiveHidden && !isVisible()) return;
    
    const hit = overlapSphere(getPos(), props.CollisionRadius);
    const hitActor = hit.find(actor => isActorInGroup(actor, props.Actors) && (isVisible(actor) || !props.IgnoreHidden));
    if (!hitActor)
        return;

    card.triggeredEvent = {actor: hitActor};
}

export function onCheck() {
    let event = card.triggeredEvent;
    delete card.triggeredEvent;
    return event;
}

export function getCardStatus() {
    const groupName = getActorGroupDescription(props.Actors, true);
    return {
        description: `When <color=green>${groupName}</color> is within radius of <color=orange>${props.CollisionRadius}</color>.`
    }
}
