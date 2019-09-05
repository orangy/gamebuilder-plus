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

export function onLocalTick() {
    if (card.chatShow) {
        uiRect(0, uiGetScreenHeight() - uiGetScreenHeight() * props.Height * 0.01, uiGetScreenWidth() * 0.01 * props.Width, uiGetScreenHeight() * props.Height * 0.01 * 0.9, UiColor.BLACK, {opacity: 0.01 * props.Opacity * card.opacity})
        uiRect(0, uiGetScreenHeight() - uiGetScreenHeight() * props.Height * 0.01 * 0.2, uiGetScreenWidth() * 0.01 * props.Width, uiGetScreenHeight() * props.Height * 0.01 * 0.9, UiColor.WHITE, {opacity: 0.01 * props.Opacity * card.opacity})
        if (!card.keyInput)
            uiButton(0, uiGetScreenHeight() - uiGetScreenHeight() * props.Height * 0.01 * 0.2, uiGetScreenWidth() * props.Width * 0.01, uiGetScreenHeight() * props.Height * 0.01 * 0.9, "", 'ChatActive', {opacity: 0});
        uiText(10, uiGetScreenHeight() - uiGetScreenHeight() * props.Height * 0.01 * 0.15, card.chatMsg || "", UiColor.BLACK, {opacity: 0.01 * props.Opacity * card.opacity})
        uiText(10, uiGetScreenHeight() - uiGetScreenHeight() * props.Height * 0.0098, card.msgString || "", UiColor.WHITE);

    }
}

export function onChatActive() {
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

export function onKeyDown(msg) {
    card.key = msg.keyName;
    if (!card.keyInput) {
        if (msg.keyName === KeyCode.Y) {
            card.chatMsg = "";
            card.keyInput = true;
            return;
        }
        return;
    }

    switch (msg.keyName) {
        case KeyCode.MOUSE: {
            sendToAll("ChatMessage", {actor: getDisplayName(), chat: card.chatMsg});
            card.chatMsg = "";
            card.keyInput = false;
            return;
        }
        case KeyCode.SPACE:
            card.chatMsg += " ";
            return;
        case KeyCode.NUMBER_0:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.NUMBER_1:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.NUMBER_2:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.NUMBER_3:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.NUMBER_4:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.NUMBER_5:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.NUMBER_6:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.NUMBER_7:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.NUMBER_8:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.NUMBER_9:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.A:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.B:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.C:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.D:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.E:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.F:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.G:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.H:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.I:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.J:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.K:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.L:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.M:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.N:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.O:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.P:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.Q:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.R:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.S:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.T:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.U:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.V:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.W:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.X:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.Y:
            card.chatMsg += msg.keyName;
            return;
        case KeyCode.Z:
            card.chatMsg += msg.keyName;
            return;
    }
    cooldown(0.05);
}

export function onChatMessage(msg) {
    if (card.msgCount >= props.MaxMsgs) card.msgString = card.msgString.replace(/.+?\n/, "");
    else card.msgCount++;
    card.msgString += msg.actor + ": " + msg.chat + "\n";

}