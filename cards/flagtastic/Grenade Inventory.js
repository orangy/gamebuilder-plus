export const PROPS = [
    propNumber('Y', 800, {
        label: "Vertical Position"
    }),
    propNumber('Size', 50, {
        label: "Size"
    }),
    propDecimal("EyeHeight", 1.2, {
        label: "Eye Height"
    }),
    propArray(4, index => [
        propString(`ItemName${index}`, "explosion", {label:`Grenade #${index+1}`}),
        propActor(`ItemProjectile${index}`, "", {label:`Projectile #${index+1}`, allowOffstageActors: true}),
        propColor(`ItemColor${index}`, "#ffffff", {label:`Color #${index+1}`})
    ])
];

function propArray(size, fn) {
    let props = [];
    for (let i = 0; i < size; i++) props.push(fn(i));
    return props;
}

const PADDING = UI_TEXT_LINE_HEIGHT / 2;

export function onResetGame() {
    delete mem.selectedGrenade;
    mem.grenades = [];
    for (let index = 0; index < 4; index++) {
        mem.grenades.push({
            name: props[`ItemName${index}`],
            projectile: props[`ItemProjectile${index}`],
            color: props[`ItemColor${index}`],
            amount: 0,
            speed: 30,
            cooldown: 0.2
        })
    }
}

export function onLocalTick() {
    if (getControllingPlayer() !== getLocalPlayer())
        return;

    const y = props.Y;
    const size = props.Size;
    const num = mem.grenades.length;

    const left = (uiGetScreenWidth() - num * size - (num - 1) * PADDING) / 2;
    mem.grenades.forEach((grenade, index) => {
        const x = left + index * (size + PADDING);
        if (index === mem.selectedGrenade)
            uiBorder(x, y, size, size, grenade.color, grenade.color);
        else
            uiRect(x, y, size, size, grenade.color, {style: RectStyle.BORDER});
        if (grenade.amount)
            uiText(x + size / 2, y + size / 2, grenade.amount + "", grenade.color, {center: true})
    });
}

export function onItemPickup(msg) {
    const grenade = mem.grenades.find(grenade => grenade.name === msg.item);
    if (grenade)
        grenade.amount += msg.amount || 0;
}

export function onPrimaryAction() {
    if (mem.selectedGrenade === undefined)
        return; // nothing selected

    const grenade = mem.grenades[mem.selectedGrenade];
    if (!grenade.amount)
        return; // no ammo

    const offsetFromCenter = getBoundsRadiusOuter() + getBoundsRadiusOuter(grenade.projectile) + 0.05;
    const origin = selfToWorldPos(vec3(0, props.EyeHeight, offsetFromCenter));
    const shootDirection = vec3normalized(getAimDirection());
    const rotation = new Quaternion();
    rotation.setFromUnitVectors(vec3z(), shootDirection);

    const velocity = vec3scale(shootDirection, grenade.speed);
    const shift = vec3scale(shootDirection, 1);
    const launched = clone(grenade.projectile, vec3add(origin, shift), rotation);
    push(launched, velocity);

    grenade.amount--;
    cooldown(grenade.cooldown);
    send(launched, "Launched", {owner: myself()});
}

export function onKeyDown(msg) {
    switch (msg.keyName) {
        case '1':
            mem.selectedGrenade = 0;
            break;
        case '2':
            mem.selectedGrenade = 1;
            break;
        case '3':
            mem.selectedGrenade = 2;
            break;
        case '4':
            mem.selectedGrenade = 3;
            break;
    }
}

// copy from game!
const XCORNER_SIZE = 10;
const YCORNER_SIZE = 12;

function uiBorder(x, y, w, h, color, colorHighlight) {
    uiRect(x, y, w, h, color, {style: RectStyle.BORDER});

    // top-left corner
    uiRect(x - 1, y - 1, XCORNER_SIZE, 2, colorHighlight, {style: RectStyle.FILLED});
    uiRect(x - 1, y - 1, 2, YCORNER_SIZE, colorHighlight, {style: RectStyle.FILLED});

    // top-right corner
    uiRect(x + w - XCORNER_SIZE + 2, y - 1, XCORNER_SIZE, 2, colorHighlight, {style: RectStyle.FILLED});
    uiRect(x + w, y - 1, 2, YCORNER_SIZE, colorHighlight, {style: RectStyle.FILLED});

    // bottom-right corner
    uiRect(x + w - XCORNER_SIZE + 2, y + h - 1, XCORNER_SIZE, 2, colorHighlight, {style: RectStyle.FILLED});
    uiRect(x + w, y + h - YCORNER_SIZE + 1, 2, YCORNER_SIZE, colorHighlight, {style: RectStyle.FILLED});

    // bottom-left corner
    uiRect(x - 1, y + h - 1, XCORNER_SIZE, 2, colorHighlight, {style: RectStyle.FILLED});
    uiRect(x - 1, y + h - YCORNER_SIZE + 1, 2, YCORNER_SIZE, colorHighlight, {style: RectStyle.FILLED});
}

export function getCardStatus() {
    return {
        description: `Handles inventory of grenades.`,
        debugText: ``
    }
}