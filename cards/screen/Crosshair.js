export const PROPS = [
    propNumber("Size", 20, {
        label: "Size of the crosshair"
    }),
    propEnum('Style', 'CROSS', [
        {value: 'CROSS', label: 'Cross: like a + sign'},
        {value: 'DIAGONAL', label: 'Diagonal: like an X'},
        {value: 'SQUARE', label: 'Square: a square with ticks'},
        {value: 'CIRCLE', label: 'Circle: a circlet with ticks'},
        {value: 'IMAGE', label: 'Image: any custom image'},

    ]),
    propColor("Color", "#d35400", {
        requires: [requireNotEqual("Style", "IMAGE")]
    }),
    propImage("Image", {
        requires: [requireEqual("Style", "IMAGE")]
    })
];

const CROSSHAIR_GAP = 0.25; // fraction of SIZE

export function onDrawScreen() {
    switch (props.Style) {
        case 'CROSS':
            DrawCross();
            break;
        case 'DIAGONAL':
            DrawDiagonal();
            break;
        case 'SQUARE':
            DrawSquare();
            break;
        case 'CIRCLE':
            DrawCircle();
            break;
        case 'IMAGE':
            DrawImage();
            break;
    }
}

function DrawCross() {
    const cx = uiGetScreenWidth() / 2;
    const cy = uiGetScreenHeight() / 2;

    const size = props.Size;
    const color = props.Color;
    const gap = size * CROSSHAIR_GAP;
    uiLine(cx, cy - size, cx, cy - gap, color, {style: ShapeStyle.FILLED});
    uiLine(cx, cy + gap, cx, cy + size, color, {style: ShapeStyle.FILLED});
    uiLine(cx - size, cy, cx - gap, cy, color, {style: ShapeStyle.FILLED});
    uiLine(cx + gap, cy, cx + size, cy, color, {style: ShapeStyle.FILLED});
}

function DrawDiagonal() {
    const cx = uiGetScreenWidth() / 2;
    const cy = uiGetScreenHeight() / 2;

    const size = props.Size;
    const color = props.Color;
    const gap = size * CROSSHAIR_GAP;
    uiLine(cx - size, cy - size, cx - gap, cy - gap, color, {style: ShapeStyle.FILLED});
    uiLine(cx - size, cy + size, cx - gap, cy + gap, color, {style: ShapeStyle.FILLED});
    uiLine(cx + size, cy - size, cx + gap, cy - gap, color, {style: ShapeStyle.FILLED});
    uiLine(cx + size, cy + size, cx + gap, cy + gap, color, {style: ShapeStyle.FILLED});
}

function DrawSquare() {
    const cx = uiGetScreenWidth() / 2;
    const cy = uiGetScreenHeight() / 2;

    DrawCross();

    const size = props.Size;
    const color = props.Color;
    const gap = size * CROSSHAIR_GAP;
    const side = (size - gap) / 2 + gap;
    uiRect(cx - side, cy - side, side * 2, side * 2, color, {style: ShapeStyle.BORDER})
}

function DrawCircle() {
    const cx = uiGetScreenWidth() / 2;
    const cy = uiGetScreenHeight() / 2;

    DrawCross();

    const size = props.Size;
    const color = props.Color;
    const gap = size * CROSSHAIR_GAP;
    const radius = (size - gap) / 2 + gap;
    uiCircle(cx, cy, radius, color, {style: ShapeStyle.BORDER})
}

function DrawImage() {
    const cx = uiGetScreenWidth() / 2;
    const cy = uiGetScreenHeight() / 2;

    const size = props.Size;
    uiImage(cx - size, cy - size, props.Image, size * 2, size * 2)
}

export function getCardStatus() {
    return {
        description: `Draws a "<color=orange>${props.Style}</color>" crosshair`
    }
}
