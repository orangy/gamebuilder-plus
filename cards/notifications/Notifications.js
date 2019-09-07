export const PROPS = [
    propNumber("ExpirationTime", 5, {
        label: "Expiration Time (sec)"
    }),
    propNumber('X', 1270, {
        label: "Horizontal Position"
    }),
    propNumber('Y', 870, {
        label: "Vertical Position"
    }),
    propBoolean('BottomUp', true, {
        label: "Bottom-up"
    }),
    propColor('Color', '#2980b9', {
        label: "Color"
    }),
    propColor('ColorHighlight', '#1abc9c', {
        label: "Highlight Color"
    }),

    propBoolean("Background", false, {
        label: "Show Box"
    }),
    propNumber('Width', 300, {
        requires: [requireTrue("Background")]
    }),
    propNumber('Height', 200, {
        requires: [requireTrue("Background")]
    }),
    propColor('BackgroundColor', '#ffffff', {
        label: "Background Color",
        requires: [requireTrue("Background")]
    }),
    propDecimal('BackgroundOpacity', 0.1, {
        label: "Background Opacity",
        requires: [requireTrue("Background")]
    }),
];

const PADDING = UI_TEXT_LINE_HEIGHT / 2;
const ITEM_HEIGHT = UI_TEXT_LINE_HEIGHT + PADDING * 2;

export function onInit() {
    onResetGame()
}

export function onResetGame() {
    card.notifications = [];
}

export function onLocalTick() {
    if (card.notifications === undefined)
        return;
    
    const time = getTime();
    card.notifications = card.notifications.filter(n => n.expires > time);
}

export function onDrawScreen() {
    const player = getLocalPlayer();
    if (!card.notifications)
        return;

    if (props.Background) {
        uiRect(props.X, props.Y, props.Width, props.Height, props.BackgroundColor, {
            style: ShapeStyle.FILLED,
            opacity: props.BackgroundOpacity
        });
        uiRect(props.X, props.Y, props.Width, props.Height, props.Color, {style: ShapeStyle.BORDER});
    }

    const nickName = getPlayerNickName(player);
    let baseY = props.Y;
    if (props.BottomUp) {
        // Y value is bottom for this mode
        baseY -= card.notifications.length * ITEM_HEIGHT;
    }

    card.notifications.forEach((n, i) => {
        const text = n.text;
        const x = props.X;
        const y = baseY + i * ITEM_HEIGHT;
        let color = props.Color;
        if (text.indexOf(nickName) !== -1) color = props.ColorHighlight;
        uiText(x, y, text, color)
    });
}

export function onNotify(msg) {
    const expires = getTime() + props.ExpirationTime;
    card.notifications.push({text: msg.text, expires: expires})
}

export function getCardStatus() {
    return {
        description: `Show notifications for <color=orange>${props.ExpirationTime}</color> seconds`
    }
}