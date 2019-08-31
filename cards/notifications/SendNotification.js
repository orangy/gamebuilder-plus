export const PROPS = [
    propActorGroup("Recipient", "", {
        label: "Recipient",
        pickerPrompt: "Who should I send to?",
        allowOffstageActors: true
    }),
    propString("Notification", "Hello"),
    propDecimal("Delay", 0)
];

/**
 * @param {GActionMessage} actionMessage
 */
export function onAction(actionMessage) {
    const targets = getActorsInGroup(props.Recipient);
    if (!targets) {
        return;
    }
    if (props.Delay > 0) {
        sendToManyDelayed(props.Delay, targets, "Notify", {text: props.Notification});
    } else {
        sendToMany(targets, "Notify", {text: props.Notification});
    }
}

export function getCardStatus() {
    return {
        description: `Sends "<color=yellow>${props.Notification}</color>" notification to <color=green>${getActorGroupDescription(props.Recipient)}</color>`
    }
}
