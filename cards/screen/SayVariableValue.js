export const PROPS = [
    propCardTargetActor("Target", {
        label: "Whose variable?"
    }),
    propString("Message", "I have #varGold gold."),
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
    const str = props.Message.replace(/#var([a-zA-Z]*)/g, varReplace);
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

function varReplace(match, variable) {
    return getVar(variable, card.target) || 0;
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
