export const PROPS = [
    propCardTargetActor("Target", {
        label: "Send to:"
    }),
    propDecimal("Height", 220),
    propDecimal("Opacity", 0.9),
    propDecimal("Duration", 2),
    propDecimal("Delay", 0),
    propBoolean("Hide")
];

export function onAction(actionMessage) {
    const target = getCardTargetActor("Target", actionMessage);
    if (!target) {
        return;
    }
    sendDelayed(props.Delay, target, "DrawCutscene", {
        height: props.Height,
        opacity: props.Opacity,
        duration: props.Duration,
        reverse: props.Hide,
        startTime: getTime()
    });
}