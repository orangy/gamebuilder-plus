export const PROPS = [
    propString("Name", "game", {
        label: "Timer Name",
    }),
    propCardTargetActor("Actor", {
        label: "Actor",
        pickerPrompt: "Which actor's timer to check?",
        allowOffstageActors: true,
    }),
];

export function onCheck() {
    const target = getCardTargetActor("Actor");
    const variable = props.Name + "_timerDeadline";
    const deadline = getVar(variable, target);
    const expired = deadline !== undefined && deadline < getTime();
    if (expired) {
        if (target === myself())
            deleteVar(variable);
        else
            deleteVarPlease(target, variable)
    }
    return expired;
}

export function getCardStatus() {
    return {
        description: `Fires when timer <color=orange>${props.Name}</color> on <color=yellow>${getCardTargetActorDescription("Actor")}</color> expires.`
    }
}
