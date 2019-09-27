export const PROPS = [
    propCardTargetActor("Target", {
        label: "Whose variable?"
    }),
    propString("Message", "I have ${gold} gold.", {}),
    propDecimal("OffsetAbove", 0, {
        label: "Offset Above"
    }),
    propNumber("TextSize", 1, {
        label: "Text Size"
    }),
    propDecimal("HideDelay", 0.5, {
        label: "Hide Delay\nContinuous mode: 0.5"
    })
];

const INITIAL_OFFSET_ABOVE = 1;
const INITIAL_SCALE = 6;

export function onAction(actionMessage) {
    const text = expandVariable(props.Message, "Target", actionMessage);

    // Create popup text if we don't have it yet.
    if (!card.popupTextActor || !exists(card.popupTextActor)) {
        let position = getPointAbove(getBoundsSize().y + INITIAL_OFFSET_ABOVE + props.OffsetAbove);
        card.popupTextActor = clone("builtin:PopupText", position);
        send(card.popupTextActor, "SetText", {text: text});
        const scale = Math.min(Math.max(INITIAL_SCALE + (props.TextSize || 0) * 0.5, 1), 12);
        setScaleUniformPlease(card.popupTextActor, scale);
        attachToParentPlease(card.popupTextActor, myself());
    }
    sendDelayed(props.HideDelay, myself(), "HideText");
}

export function onResetGame() {
    // Popup is a script clone, so it gets destroyed automatically.
    delete card.popupTextActor;
    delete card.popupHideTime;
}

export function onHideText() {
    if (card.popupTextActor) {
        destroySelfPlease(card.popupTextActor);
        delete card.popupTextActor;
    }
}

export function getCardStatus() {
    return {
        description: `Displays text popup "<color=orange>${props.Message}</color>" with variables from <color=green>${getCardTargetActorDescription("Target")}</color>`,
        debugText: `Expanded Text:

${expandVariable(props.Message, "Target")}`
    }
}

function expandVariable(text, targetActor, event) {
    const actor = targetActor ? getCardTargetActor(targetActor, event) : myself();
    return text.replace(/\${([a-zA-Z]*)}/g, (match, variable) => getVar(variable, actor) || 0)
}
