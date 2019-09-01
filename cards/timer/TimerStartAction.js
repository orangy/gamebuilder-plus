export const PROPS = [
    propString("Name", "game", {
        label: "Timer Name",
    }),
    propDecimal("Time", 60, {
        label: "Timer (sec)"
    }),
    propCardTargetActor("Actor", {
        label: "Actor",
        pickerPrompt: "Which actor's timer to start?",
        allowOffstageActors: true,
    })
];

export function onAction() {
    const target = getCardTargetActor("Actor")
    const deadline = getTime() + props.Time;
    const variable = props.Name + "_timerDeadline";
    if (target === myself())
        setVar(variable, deadline);
    else
        setVarPlease(target, variable, deadline);
}

export function onResetGame() {
    const target = getCardTargetActor("Actor");
    const variable = props.Name + "_timerDeadline";
    if (target === myself())
        deleteVar(variable);
    else
        deleteVarPlease(target, variable)
}

export function getCardStatus() {
    return {
        description: `Starts timer <color=yellow>${props.Name}</color> on <color=green>${getCardTargetActorDescription("Actor")}</color> for <color=orange>${props.Time}</color> seconds.`
    }
}
