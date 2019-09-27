export const PROPS = [
    propActor("Player", "", {
        label: "Player Offstage Prototype",
        pickerPrompt: "What actor to spawn as a player?",
        allowOffstageActors: true
    }),
    propString("Tag", "spawn", {
        label: "Spawn Place Tag",
    }),
];

function selectSpawnActor() {
    const players = getAllPlayers();
    const spawns = getActorsWithTag(props.Tag);
    if (players.length === 0 || spawns.length === 0)
        return undefined;

    const selected = spawns.reduce((current, spawn) => {
        // Sum of distances from this spawn point to all players
        let totalDistance = players.reduce(function (total, player) {
            const playerInfo = mem.players[player];
            if (playerInfo && playerInfo.actor)
                return total + getDistanceBetween(spawn, playerInfo.actor);
            else
                return total;
        }, 0);
        let currentDistance = current.distance || 0;
        return currentDistance > totalDistance ? current : {distance: totalDistance, actor: spawn};
    }, {});

    return selected.actor || spawns[0];
}

function spawnPlayer(playerId) {
    const nickname = getPlayerNickName(playerId);
    log(`Player "${playerId}" joined: ${nickname}`);

    const info = mem.players[playerId] || {};
    info.id = playerId;
    info.nickname = nickname;
    mem.players[playerId] = info;

    const spawnPoint = selectSpawnActor();
    let pos = getPos(spawnPoint);
    const playerActor = clone(props.Player, pos);
    setControllingPlayerPlease(playerActor, info.id);
    info.actor = playerActor;
}

export function onInit() {
    // init will be called when scene is loaded, by clones can still be there along with mem.players, 
    // so create actors only for players that don't have them
    if (mem.players) {
        const players = getAllPlayers();
        players.forEach(function (player) {
            const playerInfo = mem.players[player];
            if (!playerInfo || !playerInfo.actor)
                spawnPlayer(player);
        })
    }
}

export function onResetGame() {
    mem.players = {}; // forget everything, clones are already destroyed
    const players = getAllPlayers();
    players.forEach(player => spawnPlayer(player))
}

export function onPlayerJoined(msg) {
    spawnPlayer(msg.playerId);
}

export function onPlayerLeft(msg) {
    const playerId = msg.playerId;
    const info = mem.players[playerId] || {};
    log(`Player "${playerId}" left: ${info.nickname}`);
    delete mem.players[playerId];
    if (info.actor) {
        destroySelfPlease(info.actor);
    }
}

export function getCardStatus() {
    const player = getDisplayName(props.Player);
    return {
        description: `Spawns <color=green>${player}</color> for connected user at one of <color=yellow>${props.Tag}</color> points.`
    }
}
