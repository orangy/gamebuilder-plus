


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
