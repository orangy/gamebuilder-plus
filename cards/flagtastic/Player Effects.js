export const PROPS = [
    propNumber('Y', 800, {
        label: "Vertical Position"
    }),
    propArray(4, index => [
        propString(`EffectName${index}`, "explosion", {label: `Effect #${index + 1}`}),
        propColor(`EffectColor${index}`, "#ffffff", {label: `Color #${index + 1}`}),
        propDecimal(`EffectMaxTime${index}`, 10, {label: `Max Time #${index + 1}`}),
        propEnum(`EffectKind${index}`, `DURABLE`, [
            {value: 'DURABLE', label: 'Durable'},
            {value: 'INSTANT', label: 'Instant'}
        ], {label: `Kind #${index + 1}`})
    ])
];

function propArray(size, fn) {
    let props = [];
    for (let i = 0; i < size; i++) props.push(fn(i));
    return props;
}

export function onResetGame() {
    mem.effects = []; // effects should be available to other cards
}

const EFFECT_ITEM_HEIGHT = 50;
const EFFECT_MAX_TIME = 10;

export function onEffect(msg) {
    log(`Effect: ${msg.type}`);
    for (let index = 0; index < 4; index++) {
        if (msg.type === props[`EffectName${index}`]) {
            if (props[`EffectKind${index}`] === 'DURABLE') {
                AddEffect(msg.type, msg.strength, props[`EffectMaxTime${index}`], props[`EffectColor${index}`]);
            } else {
                ExecuteEffect(msg.type, msg.strength, msg.direction);
            }
            return;
        }
    }
}

function ExecuteEffect(type, strength, direction) {
    // TODO: Convert to message and separate explosion card
    if (type === 'explosion')
        ExplosionEffect(strength, direction)
}

function ExplosionEffect(strength, direction) {
    const push = vec3scale(direction, strength);
    // Horizontal (XZ plane) velocity is added to throttle
    mem.externalVelocity = vec3(push.x, 0, push.z);
    // Vertical part is added as a force like a jump
    addVelocity(vec3(0, push.y, 0));
}

function AddEffect(type, strength, maxTime, color) {
    const time = getTime();
    const existing = mem.effects.find(e => e.type === type);
    if (!existing) {
        mem.effects.push({type: type, expires: time + strength, color: color})
    } else {
        const timeLeft = existing.expires - time;
        existing.expires = time + clamp(timeLeft + strength, 0, maxTime);
    }
}

export function onTick() {
    // Remove expired effects
    const time = getTime();
    if (mem.effects) {
        mem.effects = mem.effects.filter(n => n.expires > time);
    }
}

export function onDrawScreen() {
    const time = getTime();
    mem.effects.forEach((effect, i) => {
        const x = uiGetScreenWidth() / 2;
        const y = props.Y - (i + 1) * EFFECT_ITEM_HEIGHT;
        let color = effect.color;
        const timeLeft = effect.expires - time;
        const m1 = Math.floor(timeLeft / 600);
        const m2 = Math.floor((timeLeft % 600) / 60);
        const s1 = Math.floor((timeLeft % 60) / 10);
        const s2 = Math.floor((timeLeft % 10));
        uiText(x, y, `${effect.type} ${m1}${m2}:${s1}${s2}`, color, {center: true})
    });
}


