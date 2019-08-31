export const PROPS = [
    propDecimal("JumpHeight", 1.8, {
        label: "Jump Height:",
    }),
    propDecimal("DoubleJumpHeight", 3.0, {
        label: "Double Jump Height:",
    }),
    propDecimal("Speed", 15, {
        label: "Running Speed:",
    }),
    propDecimal("WalkSpeed", 6, {
        label: "Walking Speed:",
    }),
];

const PRE_COYOTTE_TIME = 0.1;
const POST_COYOTTE_TIME = 0.1;
const DOUBLE_JUMP_TIME = 0.35;
const GROUND_GAP = 0.2; // how far down from player to look for in-air ground checks
const SPEED_CHECK = 0.1; // how fast user should move for in-air ground checks to trigger

/*
 * Swift movements are designed to be active and fast and allow for some tricks
 * 
 * Fluid Jumps: Jumping is allowed to miss exact ground contact by PRE_COYOTTE_TIME & POST_COYOTTE_TIME time to stimulate bouncing moves.
 * Double Jumps: If you manage to jump within DOUBLE_JUMP_TIME, jump velocity is increased.
 * 
 * Fast Moves: Movements are fast by default, but you can walk if holding SHIFT (opposite to default)
 * Air Control: While airborne you can control yourself by pressing forward (W) and aiming. Experiment :)
 */

export function onResetGame() {
    card.groundedTimestamp = getTime();
    delete card.jumpRequestTimestamp;
}

export function onJump() {
    // Do not jump when frozen
    if (mem.effects && mem.effects.find(e => e.type === "freeze"))
        return;

    card.jumpRequestTimestamp = getTime();
    //log("[" + card.jumpRequestTimestamp + "] jump request")
}

function touchingGround() {
    if (isGrounded()) {
        card.groundDistance = 666;
        return true;
    }

    const radius = getBoundsRadiusInner();
    const direction = getWorldThrottle();
    const horizontalDirection = vec3(direction.x, 0, direction.z);
    let castShift;
    if (horizontalDirection.manhattanLength() < SPEED_CHECK) {
        castShift = vec3(0, 0, 0);
    } else {
        castShift = horizontalDirection.normalize()
    }

    const bottomCenter = getPos(); // bottom-center, subject to `renderableOffset`
    let hit = castAdvanced(
        vec3add(bottomCenter, vec3scale(castShift, radius)), // origin
        getDown(), // look down
        GROUND_GAP, // distance
        0, // radius (0 means ray cast)
        CastMode.CLOSEST, // find closes entity
        true, false, true // actors, terrain, but not self
    );


    if (hit) {
        card.groundDistance = hit.distance;
        const actor = hit.actor;
        return !actor || isSolid(actor); // terrain or solid actor
    } else
        delete card.groundDistance;
    return false;
}

export function onActiveTick() {
    const velocity = mem.externalVelocity || vec3zero();
    if (velocity.length() < 0.01)
        delete mem.externalVelocity;
    else
        mem.externalVelocity.multiplyScalar(0.8);

    if (touchingGround()) {
        if (!card.groundedTimestamp) {
            //    log("grounded: " + card.groundDistance + " [Y=" + getPos().y + "]");
        }

        card.groundedTimestamp = getTime();
    }

    // We allow for some early jump by reacting to jump key in PRE_COYOTTE_TIME time when eventually grounded
    // and some late jump by allowing to jump in POST_COYOTTE_TIME time since ground

    if (card.jumpRequestTimestamp && card.groundedTimestamp) {
        const timeSinceJumpRequest = getTime() - card.jumpRequestTimestamp;
        const timeSinceGrounded = getTime() - card.groundedTimestamp;

        if (timeSinceJumpRequest < PRE_COYOTTE_TIME) {
            // can react to jump when grounded slightly after actual jump button
            if (timeSinceGrounded < POST_COYOTTE_TIME) {
                const timeSinceLastJump = getTime() - card.jumpTimestamp;

                /*
                    log("[" + card.jumpRequestTimestamp + "] jump");
                    log("    timeSinceGrounded:" + timeSinceGrounded);
                    log("    timeSinceJumpRequest:" + timeSinceJumpRequest);
                    const dbl = timeSinceLastJump < DOUBLE_JUMP_TIME ? "[DOUBLE]" : "";
                    log("    timeSinceLastJump:" + timeSinceLastJump + " " + dbl);
                */

                const gravity = 9.81 + 50;
                const jumpHeight = timeSinceLastJump < DOUBLE_JUMP_TIME ? props.DoubleJumpHeight : props.JumpHeight;
                let jumpSpeed = Math.sqrt(jumpHeight * 2 * gravity);
                
                // Clamp maximum vertical speed to the jumpSpeed
                const verticalVelocity = getVelocity().y;
                if (verticalVelocity > 0) jumpSpeed = jumpSpeed - verticalVelocity;
                
                const deltaVelocity = vec3(0, jumpSpeed, 0);
                addVelocity(deltaVelocity);
                card.jumpTimestamp = getTime();

                // consume this jump request, so that another keyboard hit is needed to jump again
                delete card.jumpRequestTimestamp;

                // consume "was on ground" so that one cannot jump off the same ground during coyotte time
                delete card.groundedTimestamp;
            } else {
                //log("    timeSinceGrounded:" + timeSinceGrounded + " [expired]");
            }
        } else {
            //log("[" + card.jumpRequestTimestamp + "] jump expired");
            delete card.jumpRequestTimestamp; // expired 
        }
    } else {
        if (!card.groundedTimestamp) {
            //log("no ground");
        }
    }

    const freeze = mem.effects && mem.effects.find(e => e.type === "freeze");
    const slow = mem.effects && mem.effects.find(e => e.type === "slow");
    const confused = mem.effects && mem.effects.find(e => e.type === "confusion");

    if (freeze) {
        moveGlobal(vec3zero());
    } else {
        const speed = isSprinting() || slow ? props.WalkSpeed : props.Speed;

        const throttle = getWorldThrottle();
        throttle.y = 0; // drop vertical velocity
        throttle.multiplyScalar(speed);

        const external = mem.externalVelocity || vec3zero();
        moveGlobal(vec3add(external, throttle));
    }

    lookDir(getAimDirection());
    if (confused) {
        const randomX = randBetween(-1, +1);
        const randomY = randBetween(-1, +1);
        const amplitude = randBetween(0, 0.3);
        requestCameraOffset(vec3(randomX * amplitude, randomY * amplitude, 0), myself());
    }
}

