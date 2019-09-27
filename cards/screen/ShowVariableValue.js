export const PROPS = [
    propString('Text', 'Gold: ${gold}'),
    propNumber('X', 100),
    propNumber('Y', 100),
    propBoolean("Center", true),
    propBoolean("Bold", true),
    propColor('Color', '#bdc3c7', {
        label: "Color"
    }),
    propDecimal('TextScale', 1, {
        label: "Text Scale"
    }),
    propCardTargetActor("Target", {
        label: "Whose variables?"
    }),
    propBoolean('Advanced', false, {
        label: 'Advanced'
    }),
    propString("ConditionVariableName", 'state', {
        label: 'Condition Variable',
        requires: [requireTrue("Advanced")]
    }),
    propString("ConditionVariableValue", 'game', {
        label: 'Condition Value',
        requires: [requireTrue("Advanced")]
    })
];

export function onDrawScreen() {
    if (!conditionSatisfied()) return;

    const value = expandVariable(props.Text, "Target");
    let text = props.Bold ? `<b>${value}</b>` : value;
    let x = props.X;
    let y = props.Y;
    let textSize = props.TextScale * UI_DEFAULT_TEXT_SIZE;
    if (props.Center) {
        x -= uiGetTextWidth(value, textSize) / 2;
        y -= uiGetTextHeight(value, textSize) / 2;
    }
    uiText(x, y, text, props.Color, {textSize: textSize});
}

export function getCardStatus() {
    let debug = `Expanded Text:
${expandVariable(props.Text, "Target")}`;
    let description = `Displays text '<color=orange>${truncateWithEllipses(props.Text, 25)}</color>' with variables from <color=green>${getCardTargetActorDescription("Target")}</color>`;

    if (props.Advanced) {
        debug += `
        
Expected: 
${props.ConditionVariableName} is '${props.ConditionVariableValue}'

Actual: 
${props.ConditionVariableName} is '${getVar(props.ConditionVariableName)}'
`
        description += ` when <color=yellow>${props.ConditionVariableName}</color> == <color=orange>${props.ConditionVariableValue}</color>`
    }

    return {
        description: description,
        debugText: debug
    }
}

function truncateWithEllipses(text, max) {
    return text.substr(0, max - 1) + (text.length > max ? '...' : '');
}

function conditionSatisfied() {
    if (props.Advanced) {
        const variable = props.ConditionVariableName;
        const value = props.ConditionVariableValue;
        if (variable && value && getVar(variable) != value)
            return false;
    }
    return true;
}

function expandVariable(text, targetActor, event) {
    const actor = targetActor ? getCardTargetActor(targetActor, event) : myself();
    return text.replace(/\${([a-zA-Z]*)}/g, (match, variable) => getVar(variable, actor) || 0)
}
