
function expandVariable(text, targetActor) {
    /* 
    TODO: return (props.VariableText === "$PLAYER") ? getPlayerNickName() : (getValue(name)).toString();
    */
    const actor = targetActor ? getCardTargetActor(targetActor) : myself();
    return text.replace(/\${([a-zA-Z]*)}/g, (match, variable) => getVar(variable, actor) || 0)
}

function format(number) {
    if (number === undefined)
        return "";
    if (Math.abs(number) < 0.01) return "0.00";
    return number.toFixed(2);
}

function vectorToText(v) {
    if (v === undefined)
        return "(---)";
    return "(" + format(v.x) + ", " + format(v.y) + ", " + format(v.z) + ")";
}
