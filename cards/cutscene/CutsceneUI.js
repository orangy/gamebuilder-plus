export function onDrawCutscene(arg) {
    delete card.cutscene;
    card.cutscene = {
        height: arg.height,
        startTime: arg.startTime,
        duration: arg.duration,
        reverse: arg.reverse,
        opacity: arg.opacity,
    }
}

export function onDrawScreen() {
    const player = getControllingPlayer();
    if (player && player !== getLocalPlayer()) {
        return;
    }
    if (card.cutsceneStarted) {
        card.cutscene.fraction = min(1, (getTime() - card.cutscene.startTime) / card.cutscene.duration);
        if (card.cutscene.reverse) {
            if (card.cutscene.fraction >= 1) {
                card.cutsceneStarted = false;
            }
            card.cutscene.fraction = 1 + -1 * min(1, (getTime() - card.cutscene.startTime) / card.cutscene.duration);

        }
        card.height = card.cutscene.fraction * card.cutscene.height;
        card.opacity = min(1, 0.5 * card.cutscene.opacity + 0.5 * card.cutscene.fraction * card.cutscene.opacity);
        uiRect(0, 0, 1600, card.height, UiColor.BLACK, {opacity: card.opacity});
        uiRect(0, uiGetScreenHeight() - card.height, 1600, card.height, UiColor.BLACK, {opacity: card.opacity});
    }
}