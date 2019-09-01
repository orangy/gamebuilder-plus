export const PROPS = [
    propString("Type", "grenade", {
        label: "Type",
    }),
    propNumber("Amount", 1, {
        label: "Amount",
    }),
    propSound("CollectSound", Sounds.COLLECT, {
        label: "Collect Sound"
    }),
];

export function onAction(actionMessage) {
    if (!isVisible()) return;

    const target = (actionMessage && actionMessage.event) ? actionMessage.event.actor : null;
    if (!target)
        return;

    hide();
    send(target, "ItemPickup", {event: actionMessage.event, amount: props.Amount, item: props.Type});
    playSound(props.CollectSound);
}

export function getCardStatus() {
    return {
        description: `Collects <color=orange>${props.Amount}</color> item(s) of type "<color=yellow>${props.Type}</color>"`
    }
}
