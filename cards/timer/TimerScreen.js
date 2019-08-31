export const PROPS = [
    propString("Name", "game", {
        label: "Timer Name",
    }),
    propNumber('X', 800, {
        label: "Horizontal Position"
    }),
    propNumber('Y', 60, {
        label: "Vertical Position"
    }),
    propBoolean("Center", true),
    propBoolean("Bold", true),
    propColor('Color', '#bdc3c7', {
        label: "Color"
    }),
    propNumber('TextSize', 60, {
        label: "Text Size"
    }),
    propBoolean("Center", true),
    propCardTargetActor("Actor", {
        label: "Actor",
        pickerPrompt: "Which actor's timer to draw?",
        allowOffstageActors: true,
    }),
    propBoolean("Alert", false),
    propColor('AlertColor', '#c0392b', {
        label: "Alert Color",
        requires: [requireTrue("Alert")]
    }),
    propDecimal("AlertTime", 10, {
        label: "Alert Time (sec)",
        requires: [requireTrue("Alert")]
    })
];

export function onDrawScreen() {
    const endTime = getVar(props.Name + "_timerDeadline", getCardTargetActor("Actor"));
    if (!endTime)
        return; // no timer, no drawing

    let timerLeft = endTime - getTime();
    if (timerLeft < 0)
        timerLeft = 0;

    let color = props.Color;
    if (props.Alert && timerLeft < props.AlertTime)
        color = props.AlertColor;
    const m1 = Math.floor(timerLeft / 600);
    const m2 = Math.floor((timerLeft % 600) / 60);
    const s1 = Math.floor((timerLeft % 60) / 10);
    const s2 = Math.floor((timerLeft % 10));

    let value = `${m1}${m2}:${s1}${s2}`;
    let text = props.Bold ? `<b>${value}</b>` : value;
    uiText(props.X, props.Y, text, color, {textSize: props.TextSize, center: props.Center});
}

export function getCardStatus() {
    return {
        description: `Displays value of timer <color=yellow>${props.Name}</color> on <color=green>${getCardTargetActorDescription("Actor")}</color>`
    }
}
