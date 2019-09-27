export const PROPS = [
    propDecimal("OffsetX", 0),
    propDecimal("OffsetY", 1.5),
    propDecimal("OffsetZ", 0),
    propDecimal("FieldOfView", 60),
];

export function onInit() {
    card.yaw = null;  // null means "unset"
    card.pitch = 0;
}

export function onTick() {
    const target = getParent();
    if (!exists(target)) 
        return;

    if (getCameraActor(target) !== myself()) {
        setCameraActorPlease(target, myself());
    }
    
    // If yaw is unset (null), initialize with the target's yaw.
    if (card.yaw === null) {
        card.yaw = getYaw(target) || 0;
    }

    setPos(selfToWorldPos(vec3(props.OffsetX, props.OffsetY, props.OffsetZ), target));
    card.yaw += getLookAxes(target).x;
    card.pitch = Math.min(Math.max(card.pitch + getLookAxes(target).y, degToRad(-80)), degToRad(80));
    setYawPitchRoll(card.yaw, -card.pitch, 0);
    setCameraSettings({
        cursorActive: false,
        aimOrigin: getPos(),
        aimDir: getForward(),
        dontRenderActors: [target],
        fov: props.FieldOfView || 60,
    });
}

