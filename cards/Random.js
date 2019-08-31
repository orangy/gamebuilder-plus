export const PROPS = [
    propDecimal("Minimum", 1),
    propDecimal("Maximum", 10),
    propString("Name", "random"),
    propEnum("Operation", "MESSAGE_SELF", [
        {value: 'MESSAGE_SELF', label: 'Send Message to Self'},
        {value: 'MESSAGE', label: 'Send Message to Actor'},
        {value: 'VARIABLE', label: 'Set Variable'},
    ]),
    propActor("Actor", "", {
        label: "Send To:",
        requires: [requireEqual("Operation", "MESSAGE")]
    })
];

export function getCardErrorMessage() {
    if (props.Operation === 'MESSAGE' && !exists(props.Actor)) {
        return "NEED ACTOR TO SEND MESSAGE TO. Click card to fix.";
    }
}

export function onAction(actionMessage) {
    const min = props.Minimum;
    const max = props.Maximum;
    const value = randBetween(min, max)
    switch (props.Operation) {
        case "MESSAGE":
            if (exists(props.Actor))
                send(props.Actor, props.Name, {amount: value, event: actionMessage.event});
            break;
        case "MESSAGE_SELF":
            sendToSelf(props.Name, {amount: value, event: actionMessage.event});
            break;
        case "VARIABLE":
            setVar(props.Name, value);
            break;
    }
}
