"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.OneVsOne = void 0;
function isOneVsOne(players) {
    return players == undefined;
}
class Room {
    config;
    player1 = null;
    player2 = null;
    players = null;
    constructor(type) {
        if (isOneVsOne(type)) {
            this.config = { mode: "1VS1" };
            this.player1 = type.player1;
            this.player2 = type.player2;
        }
        else {
            this.config = { mode: "2VS2" };
            this.players = type;
        }
    }
}
class OneVsOne extends Room {
    constructor(player1, player2) {
        super({ player1: player1, player2: player2 });
    }
}
exports.OneVsOne = OneVsOne;
//# sourceMappingURL=Room.js.map