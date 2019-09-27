export const PROPS = [
    propString('Caption', 'Health'),
    propEnum('Style', 'SOLID', [
        {value: 'SOLID', label: 'Solid color bar'},
        {value: 'STRIPES', label: 'Color bar divided into stripes'},
        {value: 'CIRCLE', label: 'Circle filled from bottom'},
    ]),
    propString('Variable', "health", {
        label: "Variable"
    }),
    propString('VariableMax', "startingHealth", {
        label: "Variable (max)"
    }),
    propString('VariableText', "${health}/${startingHealth}", {
        label: "Text with variables",
    }),
    propImage("Icon", {
        label: "Icon"
    }),
    propNumber("IconSize", UI_TEXT_LINE_HEIGHT, {
        label: "Icon Size",
    }),
    propNumber('X', 50, {
        label: "Horizontal Position"
    }),
    propNumber('Y', 100, {
        label: "Vertical Position"
    }),
    propNumber('Width', 200),
    propNumber('Height', UI_TEXT_LINE_HEIGHT),

    propNumber("StripeSize", 10, {
        label: "Stripe Size",
        requires: [requireEqual("Style", "STRIPES")]
    }),
    propColor('Color', '#ffffff', {
        label: "Color"
    }),
    propColor('BackgroundColor', '#ffffff', {
        label: "Background Color"
    }),
    propDecimal('BackgroundOpacity', 0.5, {
        label: "Background Opacity"
    }),
    propColor('FillColorHigh', '#00ff00', {
        label: "Fill Color (75% or more)"
    }),
    propColor('FillColorMedium', '#ffff00', {
        label: "Fill Color (25% or more)"
    }),
    propColor('FillColorLow', '#ff0000', {
        label: "Fill Color (less then 25%)"
    }),
];

const PADDING = UI_TEXT_LINE_HEIGHT / 2;
const INSET = 2;

export function onDrawScreen() {
    const player = getControllingPlayer();
    if (player && player !== getLocalPlayer()) {
        return;
    }

    const value = getVar("isDead") ? 0 : (getVar(props.Variable) || mem[props.Variable] || 0);
    const max = Math.max(1, getVar(props.VariableMax) || mem[props.VariableMax] || 0);
    const fraction = Math.min(Math.max(value / max, 0), 1);
    const fillColor = fraction < 0.25 ? props.FillColorLow : fraction < 0.75 ? props.FillColorMedium : props.FillColorHigh;

    let text = expandVariable(props.VariableText);
    switch (props.Style) {
        case 'SOLID':
            drawRectangleFrame(fraction, fillColor, text);
            fillSolid(fraction, fillColor);
            break;
        case 'STRIPES':
            drawRectangleFrame(fraction, fillColor, text);
            fillStripes(fraction, fillColor);
            break;
        case 'CIRCLE':
            drawCircleFrame(fraction, fillColor, text)
    }

}

function getValue(name) {
    let value = getVar(name);
    if (value !== undefined)
        return value;
    value = mem[name];
    if (value !== undefined)
        return value;
    return "";
}

function expandVariable(text, targetActor, event) {
    const actor = targetActor ? getCardTargetActor(targetActor, event) : myself();
    return text.replace(/\${([a-zA-Z]*)}/g, (match, variable) => getVar(variable, actor) || 0)
}

function fillSolid(fraction, fillColor) {
    const width = props.Width - 2 * INSET;
    const height = props.Height - 2 * INSET;
    uiRect(props.X + INSET, props.Y + INSET, width * fraction, height, fillColor);
}

function fillStripes(fraction, fillColor) {
    const width = props.Width - 2 * INSET;
    const height = props.Height - 2 * INSET;
    const fillWidth = width * fraction;
    const stripeStep = props.StripeSize + INSET;
    const numStripes = Math.floor(fillWidth / stripeStep);
    for (let i = 0; i < numStripes; i++) {
        uiRect(props.X + INSET + i * stripeStep, props.Y + INSET, props.StripeSize, height, fillColor);
    }
}

function drawRectangleFrame(fraction, fillColor, text) {
    uiRect(props.X, props.Y, props.Width, props.Height, props.BackgroundColor, {
        style: ShapeStyle.FILLED,
        opacity: props.BackgroundOpacity
    });
    uiRect(props.X, props.Y, props.Width, props.Height, props.Color, {style: ShapeStyle.BORDER});

    const caption = props.Caption;
    if (caption) {
        uiText(props.X, props.Y - uiGetTextHeight(caption) - PADDING, caption, props.Color);
    }

    const textWidth = uiGetTextWidth(text);
    uiText(props.X + props.Width - textWidth, props.Y + props.Height + PADDING, text, props.Color);
    if (props.Icon) {
        uiImage(props.X - props.IconSize - PADDING, props.Y + props.Height / 2 - props.IconSize / 2, props.Icon, props.IconSize, props.IconSize)
    }
}

function drawCircleFrame(fraction, fillColor, text) {
    const radius = props.Width / 2;
    const size = props.Width - 2 * INSET;
    const innerRadius = size / 2;
    const fillSize = size * fraction;

    uiCircle(props.X + radius, props.Y + radius, radius, props.Color, {style: ShapeStyle.BORDER});
    uiCircle(props.X + radius, props.Y + radius, radius, props.BackgroundColor, {
        style: ShapeStyle.FILLED,
        opacity: props.BackgroundOpacity
    });

    for (var y = size; y > size - fillSize; y--) {
        // x^2 + y^2 = r^2
        const p = (y - innerRadius);
        const x = Math.sqrt(innerRadius * innerRadius - p * p);
        uiLine(props.X + radius - x + INSET, props.Y + y + INSET, props.X + radius + x - INSET, props.Y + y + INSET, fillColor)
    }

    uiText(props.X + radius, props.Y + props.Width + PADDING, text, props.Color, {center: true});

    const caption = props.Caption;
    if (caption) {
        uiText(props.X + radius, props.Y - uiGetTextHeight(caption), caption, props.Color, {center: true});
    }
}
