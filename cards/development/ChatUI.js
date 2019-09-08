export const PROPS = [
    propNumber('Height', 20, {
        label: "Height (0-100%)"
    }),
    propNumber('Width', 30, {
        label: "Width (0-100%)"
    }),
    propNumber('MaxMsgs', 5, {
        label: "Max Lines"
    }),
    propDecimal("Opacity", 60, {
        label: "Max Opacity (0-100%)"
    })
];

export function onDrawScreen() {
    if (card.chatShow) {
        const screenHeight = uiGetScreenHeight();

        const height = screenHeight * props.Height * 0.01;
        const width = uiGetScreenWidth() * props.Width * 0.01;
        const opacity = 0.01 * props.Opacity * card.opacity;
        
        // Message history
        uiRect(0, screenHeight - height, width, height * 0.9, UiColor.BLACK, {opacity: opacity});
        uiText(10, screenHeight - height * 0.15, card.chatMsg || "", UiColor.BLACK, {opacity: opacity});
        
        // Input Box
        uiRect(0, screenHeight - height * 0.2, width, height * 0.9, UiColor.WHITE, {opacity: opacity});
        uiText(10, screenHeight - screenHeight * props.Height * 0.0098, card.msgString || "", UiColor.WHITE);
        
        // Overlay button to activate chat
        if (!card.keyInput)
            uiButton(0, screenHeight - height * 0.2, width, height * 0.9, "", 'ActivateChat', {opacity: 0});
    }
}

export function onActivateChat() {
    card.keyInput = true;
}

export function onInit() {
    card.chatShow = true;
    card.opacity = 1;
    card.chatMsg = "";
    card.keyInput = false;
    card.msgString = "";
    card.msgCount = 0;
}

const inputKeys = '0123456789abcdefghijklmnopqrstuvwxyz';

export function onKeyDown(msg) {
    log(`KeyDown: <color=yellow>${msg.keyName}</color>`);
    
    card.key = msg.keyName;
    if (!card.keyInput) {
        if (msg.keyName === KeyCode.Y) {
            card.chatMsg = "";
            card.keyInput = true;
        }
        return;
    }

    switch (msg.keyName) {
        case KeyCode.ENTER:
        case "return":
        case KeyCode.MOUSE:
            sendToAll("ChatMessage", {actor: getDisplayName(myself()), chat: card.chatMsg});
            card.chatMsg = "";
            card.keyInput = false;
            return;
        case KeyCode.SPACE:
            card.chatMsg += " ";
            return;
        default:
            if (inputKeys.indexOf(msg.keyName) !== -1)
                card.chatMsg += msg.keyName;
            break;
    }
    cooldown(0.05);
}

export function onChatMessage(msg) {
    if (card.msgCount >= props.MaxMsgs) 
        card.msgString = card.msgString.replace(/.+?\n/, "");
    else 
        card.msgCount++;
    card.msgString += `${msg.actor}: ${msg.chat}
`;
}