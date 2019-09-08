export const PROPS = [
    propActorGroup("Recipient", "", {
        label: "Recipient",
        pickerPrompt: "Who should I send to?",
        allowOffstageActors: true
    }),
    propString("Notification", "Hello", {
        label: "Notification Text"
    }),
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
        sendToManyDelayed(props.Delay, targets, "Notify", {text: expandVariable(props.Notification)});
    } else {
        sendToMany(targets, "Notify", {text: props.Notification});
    }
}

function expandVariable(text, targetActor) {
    const actor = targetActor ? getCardTargetActor(targetActor) : myself();
    return text.replace(/\${([a-zA-Z]*)}/g, (match, variable) => getVar(variable, actor) || 0)
}

export function getCardStatus() {
    return {
        description: `Sends "<color=yellow>${props.Notification}</color>" notification to <color=green>${getActorGroupDescription(props.Recipient)}</color>`
    }
}
