export const PROPS = [
    propCardTargetActor("Target", {
        label: "Whose variable?"
    }),
    propString("Message", "I have ${gold} gold."),
    propDecimal("OffsetAbove", 0),
    propNumber("TextSize", 1),
    propDecimal("HideDelay", 0.5, {label: "Hide Delay\nContinuous mode: 0.5"})
];

const HIDE_DELAY_SECONDS = 0.5;
const INITIAL_OFFSET_ABOVE = 1;
const INITIAL_SCALE = 6;

export function onAction(actionMessage) {
    // We change the variable
    card.target = getCardTargetActor("Target", actionMessage);
    const str = expandVariable(props.Message, "Target");
    // Create popup text if we don't have it yet.
    if (!card.popupTextActor || !exists(card.popupTextActor)) {
        card.popupTextActor = clone("builtin:PopupText",
            getPointAbove(getBoundsSize().y + INITIAL_OFFSET_ABOVE + props.OffsetAbove));
        send(card.popupTextActor, "SetText", {text: str});
        const scale = Math.min(Math.max(INITIAL_SCALE + (props.TextSize || 0) * 0.5, 1), 12);
        send(card.popupTextActor, "SetScale", scale);
        send(card.popupTextActor, "SetParent", {parent: myself()});
    }
    card.popupHideTime = getTime() + HIDE_DELAY_SECONDS;
}

export function onResetGame() {
    // Popup is a script clone, so it gets destroyed automatically.
    delete card.popupTextActor;
    delete card.popupHideTime;
}

export function onTick() {
    if (card.popupHideTime && getTime() > card.popupHideTime) {
        // Hide the popup.
        send(card.popupTextActor, "Destroy");
        delete card.popupTextActor;
        delete card.popupHideTime;
    }
}

function expandVariable(text, targetActor) {
    const actor = targetActor ? getCardTargetActor(targetActor) : myself();
    return text.replace(/\${([a-zA-Z]*)}/g, (match, variable) => getVar(variable, actor) || 0)
}
