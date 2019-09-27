export const PROPS = [
    propActorGroup("Recipient", "", {
        label: "Recipient",
        pickerPrompt: "Who should I send to?",
        allowOffstageActors: true
    }),
    propString("Notification", "Hello", {
        label: "Notification Text"
    }),
    propCardTargetActor("Target", {
        label: "Whose variables?"
    }),
    propDecimal("Delay", 0)
];

export function onAction(actionMessage) {
    const targets = getActorsInGroup(props.Recipient);
    if (!targets) {
        return;
    }

    const text = expandVariable(props.Notification, "Target", actionMessage); 
    if (props.Delay > 0) {
        sendToManyDelayed(props.Delay, targets, "Notify", {text: text});
    } else {
        sendToMany(targets, "Notify", {text: text});
    }
}

function expandVariable(text, targetActor, event) {
    const actor = targetActor ? getCardTargetActor(targetActor, event) : myself();
    return text.replace(/\${([a-zA-Z]*)}/g, (match, variable) => getVar(variable, actor) || 0)
}

export function getCardStatus() {
    return {
        description: `Sends "<color=yellow>${props.Notification}</color>" notification to <color=green>${getActorGroupDescription(props.Recipient)}</color>`
    }
}
