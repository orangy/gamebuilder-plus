export const PROPS = [
    propNumber('X', 100),
    propNumber('Y', 100),
    propColor('Color', '#ffff00'),
    propCardTargetActor("Target", {
        label: "Whose variables?"
    }),
    propString('Text', 'Gold: ${gold}'),
];

export function onDrawScreen() {
    card.text = expandVariable(props.Text, "Target");
    uiText(props.X, props.Y, card.text, props.Color || UiColor.WHITE);
}

export function getCardStatus() {
    return {
        description: `Displays text '<color=orange>${props.Text}</color>' with variables from <color=green>${getCardTargetActorDescription("Target")}</color>`,
        debugText: `Expanded Text:

${expandVariable(props.Text, "Target")}`
    }
}

function expandVariable(text, targetActor, event) {
    const actor = targetActor ? getCardTargetActor(targetActor, event) : myself();
    return text.replace(/\${([a-zA-Z]*)}/g, (match, variable) => getVar(variable, actor) || 0)
}
