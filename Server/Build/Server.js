"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const net = __importStar(require("net"));
const Timer_1 = __importDefault(require("./Utils/Timer"));
const Room_1 = require("./MatchMaking/Room");
let timer = new Timer_1.default();
// List of all players searching for a room
let searchRoom = new Map();
// List of all available instructions
let actions = {
    SEARCH_ROOM: "SEARCH_ROOM",
    UPDATE_PLAYER_POSE: "UPDATE_PLAYER_POSE"
};
// Making rooms
let roomsOneVsOne = new Map();
// Making the web server
let server = net.createServer(socket => {
    const uuid = require('uuid').v4;
    let id = uuid();
    let connected = false;
    // Process to send a message to clients
    timer.runAfter(1).then(() => {
        console.log(`${socket.remoteAddress} is connected`); // Remote IP
        connected = true;
        socket.write(Buffer.from("Device connected with ", "utf-8")); // Sending a message to socket
        timer.runAfter(1).then(() => {
            socket.write(Buffer.from(`id: ${id}`, "utf-8"));
        });
    });
    /**** Process to receive data from clients ****/
    socket.on("data", data => {
        let jsonData = JSON.parse(data.toString());
        switch (jsonData.command) {
            case actions.SEARCH_ROOM:
                console.log(`${id} is searching room`);
                searchRoom.set(id, { id: id, conexion: socket });
                break;
            case actions.UPDATE_PLAYER_POSE:
                console.log("\nPlayer " + jsonData.playerID + " update pose to:");
                console.log("Position: " + "<" + jsonData.Px + ", " + jsonData.Py + ", " + jsonData.Pz + ">");
                console.log("Rotation: " + "<" + jsonData.Rx + ", " + jsonData.Ry + ", " + jsonData.Rz + ">");
                break;
            default:
                console.log("The received command is not recognized");
                break;
        }
    });
    /**** Process to close room ****/
    socket.on("close", () => {
        console.log("The client has disconnected");
    });
});
const PORT = 8080;
/**** Process to start server ****/
server.listen(PORT, () => {
    console.log("Server is running on port: " + PORT);
    console.log("Waiting for players...");
    let searchTimer = new Timer_1.default(() => {
        if (searchRoom.size >= 2) {
            // List of players
            let players = [];
            searchRoom.forEach(j => {
                players.push(j);
            });
            roomsOneVsOne.set(players[0].id + players[1].id, new Room_1.OneVsOne(players[0], players[1]));
            console.log("Room One vs One created between players ID: " + players[0].id + " and " + players[1].id);
            // Deleting players for search room list
            searchRoom.delete(players[0].id);
            searchRoom.delete(players[1].id);
        }
    });
});
//# sourceMappingURL=Server.js.map