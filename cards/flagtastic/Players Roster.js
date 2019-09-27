export const PROPS = [
    propNumber('X', 1270, {
        label: "Horizontal Position"
    }),
    propNumber('Y', 60, {
        label: "Vertical Position"
    }),
    propNumber('Width', 300),

    propColor('Color', '#ecf0f1', {
        label: "Color"
    }),
    propColor('BackgroundColor', '#2c3e50', {
        label: "Background"
    }),
    propColor('BackgroundColorAlt', '#34495e', {
        label: "Alternate Background"
    }),
    propColor('BackgroundColorHighlight', '#2980b9', {
        label: "Highlight Background"
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

/*
    Players roster is stored in the Game's actor memory as card.players[id]
    Use "Players States" card to manage it
    
    Each item has `state`, `stateText`, `stateWeight` and `score` properties
        state is identifier of a state, e.g. "warmup", "spectate", etc
        stateText is text of a state, e.g. "Warming Up", "Spectating", etc
        stateWeight is *negative* number for sorting players when they don't have scores
        score is number of points
        nickname is nickname
 */
const PADDING = UI_TEXT_LINE_HEIGHT / 2;
const SCORE_ITEM_HEIGHT = UI_TEXT_LINE_HEIGHT + PADDING * 2;

const SCORE_LABEL_WIDTH = 150;
const SCORE_BORDER_COLOR = colorFromHex("#8e44ad");
const SCORE_BORDER_HCOLOR = colorFromHex("#9b59b6");

const XCORNER_SIZE = 10;
const YCORNER_SIZE = 12;

export function onResetGame() {
    if (card.players === undefined) return; // no players joined

    for (let key in card.players) {
        delete card.players[key].score;
    }
}

export function onPlayerJoined(msg) {
    if (card.players === undefined) {
        card.players = {}; // first player activated. we can't rely on reset/init, because the events are out of order
    }

    const playerId = msg.playerId;
    const nickname = getPlayerNickName(playerId);
    log(`Player "${playerId}" joined: ${nickname}`);

    const info = card.players[playerId] || {};
    info.nickname = nickname;
    card.players[playerId] = info;
}

export function onPlayerLeft(msg) {
    if (card.players === undefined) return; // no players joined
    const playerId = msg.playerId;
    const info = card.players[playerId] || {};
    log(`Player "${playerId}" left: ${info.nickname}`);
    delete card.players[playerId];
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

export function onPointScored(msg) {
    if (card.players === undefined)
        return; // no players joined

    if (!conditionSatisfied())
        return;

    const info = card.players[msg.player];
    info.score = (info.score || 0) + (msg.amount || 0);
}

export function onDrawScreen() {
    if (card.players === undefined)
        return; // nothing to draw, no players data

    const playerId = getLocalPlayer();

    const playerIds = getAllPlayers();
    const numPlayers = playerIds.length;

    playerIds.sort((ida, idb) => {
        let scoreA = getScoreSortingOrder(card.players[ida]);
        let scoreB = getScoreSortingOrder(card.players[idb]);
        return scoreB - scoreA;
    });

    const x = props.X;
    const y = props.Y;
    const w = props.Width;

    uiBorder(x - 1, y - 1, w + 2, SCORE_ITEM_HEIGHT * numPlayers + 2, SCORE_BORDER_COLOR, SCORE_BORDER_HCOLOR);

    playerIds.forEach((id, index) => {
        let bgColor;
        if (id === playerId) bgColor = props.BackgroundColorHighlight;
        else if (index % 2 === 0) bgColor = props.BackgroundColor;
        else bgColor = props.BackgroundColorAlt;

        uiRect(x, y + SCORE_ITEM_HEIGHT * index, w, SCORE_ITEM_HEIGHT, bgColor, {
            opacity: 0.3,
            style: RectStyle.FILLED
        });

        let nickname = getPlayerNickName(id);
        if (!nickname || nickname.length === 0)
            nickname = "Player";

        const label = nickname;
        const info = card.players[id];

        const score = getScoreDisplayText(info);
        const scoreWidth = uiGetTextWidth(label);
        uiText(x + PADDING + SCORE_LABEL_WIDTH - scoreWidth, y + SCORE_ITEM_HEIGHT * index + PADDING, label, props.Color, {});
        uiText(x + PADDING + SCORE_LABEL_WIDTH + PADDING, y + SCORE_ITEM_HEIGHT * index + PADDING, score, props.Color, {});
    });
}

function getScoreSortingOrder(info) {
    if (info === undefined)
        return -4; // not yet registered
    if (info.score !== undefined)
        return info.score; // if score is there, display it
    return info.stateWeight || -4;
}

function getScoreDisplayText(info) {
    if (info === undefined)
        return "";
    if (info.score !== undefined)
        return info.score; // if score is there, display it
    return info.stateText || "";
}

function uiBorder(x, y, w, h, color, colorHighlight) {
    uiRect(x, y, w, h, color, {style: RectStyle.BORDER});

    // top-left corner
    uiRect(x - 1, y - 1, XCORNER_SIZE, 2, colorHighlight, {style: RectStyle.FILLED});
    uiRect(x - 1, y - 1, 2, YCORNER_SIZE, colorHighlight, {style: RectStyle.FILLED});

    // top-right corner
    uiRect(x + w - XCORNER_SIZE + 1, y - 1, XCORNER_SIZE, 2, colorHighlight, {style: RectStyle.FILLED});
    uiRect(x + w - 1, y - 1, 2, YCORNER_SIZE, colorHighlight, {style: RectStyle.FILLED});

    // bottom-right corner
    uiRect(x + w - XCORNER_SIZE + 1, y + h - 1, XCORNER_SIZE, 2, colorHighlight, {style: RectStyle.FILLED});
    uiRect(x + w - 1, y + h - YCORNER_SIZE + 1, 2, YCORNER_SIZE, colorHighlight, {style: RectStyle.FILLED});

    // bottom-left corner
    uiRect(x - 1, y + h - 1, XCORNER_SIZE, 2, colorHighlight, {style: RectStyle.FILLED});
    uiRect(x - 1, y + h - YCORNER_SIZE + 1, 2, YCORNER_SIZE, colorHighlight, {style: RectStyle.FILLED});
}
