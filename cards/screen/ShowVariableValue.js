export const PROPS = [
    propNumber('X', 100),
    propNumber('Y', 100),
    propColor('Color', '#ffff00'),
    propCardTargetActor("Target", {
        label: "Whose variables?"
    }),
    propString('Text', 'Gold: #vargold'),
];

export function onDrawScreen() {
    card.text = props.Text.replace(/#var([a-zA-Z]*)/g, varReplace);
    uiText(props.X, props.Y, card.text, props.Color || UiColor.WHITE);
}

function varReplace(match, variable) {
    return getVar(variable, getCardTargetActor("Target")) || 0;
}

export function getCardStatus() {
    return {
        description: `Displays text '<color=orange>${props.Text}</color>' with variables from <color=green>${getCardTargetActorDescription("Target")}</color>`,
        debugText: `Expanded Text:

${props.Text.replace(/#var([a-zA-Z]*)/g, varReplace)}`
    }
}
