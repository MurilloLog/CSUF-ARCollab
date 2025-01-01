import * as net from "net"
import Timer from "./Utils/Timer"
import mongoose from "mongoose";
import crypto from "crypto";
import { mongo } from "./database";
import Player from "./models/Player";
import Objects from "./models/Objects";
import Drawings from "./models/Draws";
import { Room } from "./MatchMaking/Room";

const USERS_PER_ROOM = 2; // Set number of players for each room

let timer: Timer = new Timer();
let players: Array<IPlayer> = [];
let databaseConfig = new mongo();

export interface IPlayer
{
    id: string;
    conexion: net.Socket;
}

// List of all players searching for a room
let searchRoom: Map<String, IPlayer> = new Map<String, IPlayer>();
let playersPaired = 0;
let playersOffline = 0;
let roomIdCounter = 0;

// List of all available instructions
let actions = 
{
    SEARCH_ROOM: "SEARCH_ROOM",
    UPDATE_PLAYER_POSE: "UPDATE_PLAYER_POSE",
    UPDATE_OBJECT_POSE: "UPDATE_OBJECT_POSE",
    DRAWING: "DRAWING"
};

// Creating rooms
let rooms: Map<string, Room> = new Map<string, Room>(); // An array<IPlayer>'s room 

// Hash ID Generator
function generateId(remoteAddress: string): string {
    const timestamp = Date.now().toString();
    const random = Math.random().toString();
    const data = `${remoteAddress}-${timestamp}-${random}`;
    const hash = crypto.createHash('sha256').update(data).digest('hex');
    return hash.substring(0, 8); // Use only 8 characters
}

// Creating the local server
let server = net.createServer((socket) => {
        let remoteAddress = socket.remoteAddress || "unknown";
        let id = generateId(remoteAddress);
        let buffer = "";
        let connected = false;
        
        const player = new Player();
        
        // Process to send an ID connection to the clients
        timer.runAfter(1).then(() => {
            console.log(`${socket.remoteAddress} is trying to connect...`); // Remote IP
            console.log(`Device connected with id: ${id}`); // Assigned ID
            timer.runAfter(1).then(() => {
                socket.write(Buffer.from(`id: ${id}`, "utf8"));
                player._id = id;
                player.save(function (err: any) {
                    if (err) return handleError(err);
                    // Saved on Mongo DB!
                });
            });
            connected = true;
        });

        socket.setEncoding("utf8");

        /**** Process to receive data from any client ****/
        socket.on("data", async (data) => {
            buffer += data;
            let delimiter = "|";
            let messages = buffer.split(delimiter);
            buffer = messages.pop() || "";
            
            for (let message of messages) {
                try {
                    let jsonData = JSON.parse(message.trim());
                    console.log("\n*********************************");
                    console.log(jsonData);
                    
                    switch (jsonData.command) {
                        case actions.SEARCH_ROOM:
                            console.log(jsonData._id + " is searching room");
                            searchRoom.set(id, {id: id, conexion: socket});
                            playersPaired++;

                            if (playersPaired < USERS_PER_ROOM) {
                                socket.write(Buffer.from(`{"command": "WAITING_PLAYER"}`, "utf-8"));
                            } else {
                                let roomPlayers: IPlayer[] = [];
                                searchRoom.forEach(player => {
                                    roomPlayers.push(player);
                                    if (roomPlayers.length === USERS_PER_ROOM) return;
                                });

                                function generateIncrementalRoomId(): string {
                                    roomIdCounter++;
                                    return `R${roomIdCounter}`;
                                }

                                //const roomId = roomPlayers.map(player => player.id).join('-');
                                const roomId = generateIncrementalRoomId();
                                const playerInRoom = new Room(roomId, roomPlayers);
                                rooms.set(roomId, playerInRoom);

                                const currentMsgRoom = rooms.get(roomId);
                                if (currentMsgRoom)
                                    roomPlayers.forEach(player => {
                                        player.conexion.write(Buffer.from(`{"command": "ROOM_CREATED", "roomId": "${roomId}"}`, "utf8"));
                                        searchRoom.delete(player.id);
                                    });

                                console.log(`Room creada con ID: "${roomId}"`);

                                playersPaired = 0;
                                playersOffline += USERS_PER_ROOM;
                            }
                            break;

                        case actions.UPDATE_PLAYER_POSE:
                            try
                            {
                                const users = await Player.find({_id: jsonData._id}, {_id: 1})
                                if(users.length != 0)
                                {
                                    console.log("The player exists in DB");
                                    const user = await Player.findByIdAndUpdate(jsonData._id, 
                                    {
                                        position: jsonData.position,
                                        rotation: jsonData.rotation,
                                    },
                                    {new: true});
                                    console.log("Player updated");
                                    console.log(user);
                                }
                                else
                                {
                                    console.log("Failed");
                                }

                                const currentMsgRoom = rooms.get(jsonData.roomId);
                                if(currentMsgRoom)
                                    currentMsgRoom.updateRoomStateOnEvent(jsonData, jsonData._id);
                                else
                                    console.log(`Room with ID ${jsonData.roomId} not found`);

                                console.log("\nPlayer " + jsonData._id + " update pose.");
                            }
                            catch(dataSaveError)
                            {
                                console.log("Update Player Error: Error updating player pose");
                                console.log(dataSaveError);
                            }
                            break;

                        case actions.UPDATE_OBJECT_POSE:
                            try
                            {
                                //const object = new objectModel();
                                const objects = await Objects.find({_id: jsonData._id}, {_id: 1})
                                if(objects.length != 0)
                                {
                                    console.log("The ID exist in DB");
                                    const objects = await Objects.findByIdAndUpdate(jsonData._id, 
                                    {
                                        position: jsonData.position,
                                        rotation: jsonData.rotation,
                                        scale   : jsonData.scale,
                                        playerEditor: jsonData.playerEditor,
                                        IsSelected: jsonData.IsSelected
                                    },
                                    {new: true});
                                    console.log("Object updated");
                                    console.log(objects);
                                }
                                else
                                {
                                    console.log("The ID doesn't exist");
                                    const objects = new Objects(
                                    {
                                        _id: jsonData._id,
                                        playerCreator: jsonData.playerCreator,
                                        playerEditor : jsonData.playerEditor,
                                        objectMesh   : jsonData.objectMesh,
                                        IsSelected   : jsonData.IsSelected,
                                        position     : jsonData.position,
                                        rotation     : jsonData.rotation,
                                        scale        : jsonData.scale
                                    });
                                    await objects.save();
                                    console.log("Object saved");
                                    console.log(objects);
                                }
                                
                                players.forEach( (playerSocket) =>
                                {
                                    if(playerSocket.id != jsonData.playerEditor)
                                        playerSocket.conexion.write(Buffer.from(data.toString(), "utf-8"));
                                });
                            }
                            catch(dataSaveError)
                            {
                                console.log("Update Object Error: Error saving object");
                                console.log(String(dataSaveError));
                            }
                            break;
                        
                        case actions.DRAWING:
                            try
                            {
                                /*const drawings = await Drawings.find({_id: jsonData._id}, {_id: 1})
                                if(drawings.length != 0)
                                {
                                    console.log("The ID exist in DB");
                                    const drawings = await Drawings.findByIdAndUpdate(jsonData._id, 
                                    {
                                        playerCreator: jsonData.playerCreator,
                                        position: jsonData.position,
                                        rotation: jsonData.rotation,
                                        materialOption: jsonData.materialOption
                                    },
                                    {new: true});
                                    console.log("Drawing updated");
                                    //console.log(drawings);
                                }
                                else
                                {
                                    console.log("The ID doesn't exist");
                                    const drawings = new Drawings(
                                    {
                                        _id: jsonData._id,
                                        playerCreator: jsonData.playerCreator,
                                        position     : jsonData.position,
                                        rotation     : jsonData.rotation,
                                        materialOption: jsonData.materialOption
                                    });
                                    await drawings.save();
                                    console.log("Drawing saved");
                                    //console.log(drawings);
                                }*/
                                
                                const currentMsgRoom = rooms.get(jsonData.roomId);
                                if(currentMsgRoom)
                                    currentMsgRoom.updateRoomStateOnEvent(jsonData, jsonData.playerCreator);
                                else
                                    console.log(`Room with ID ${jsonData.roomId} not found`);
                            }
                            catch(dataSaveError)
                            {
                                console.log("Update Drawing Error: Error saving object");
                                console.log(String(dataSaveError));
                            }
                            break;

                        default:
                            console.log("The received command is not recognized");
                            break;
                    }

                    jsonData = "";
                }
                catch(dataSyncError)
                {
                    console.log("Error processing message:");
                    console.log(message.trim());
                    console.log(dataSyncError);
                }
            }

            // When all players are disconected, delete the collection
            async function deleteCollection()
            {
                const objects = await Objects.find();
                objects.forEach((_id: any) => {
                    const query = Objects.findByIdAndDelete(_id);
                })
            }
            if(playersOffline >= 2)
            {
                deleteCollection();
                console.log(`The collection was deleted`);
            }
        });

        socket.on('error', (error)=>
        {
            console.log(error.message);
        });

        /**** Process to close socket connection ****/
        socket.on("close", () => {
            socket.end();
            players.forEach( (playerSocket) => {
                playerSocket.conexion.write(Buffer.from(`{"command": "PLAYER_OFFLINE"}`, "utf-8"));
            });
            console.log(`Connection with ${socket.remoteAddress} : ${socket.remotePort} closed.`);
            playersOffline--;
            playersPaired--;
        });
});

const PORT = 8080;

/**** Process to starts the server ****/
server.listen(PORT, () =>
{
    console.log("Connecting to MongoDB... ");
    mongoose.set('strictQuery', true);
    mongoose.connect(`mongodb:\/\/${databaseConfig.host}/${databaseConfig.db}`,{
        family: 4,
    });

    console.log("Successful connection.");
    console.log("Server is running on port: " + PORT);
    console.log("Waiting for connections...");

    let searchTimer: Timer = new Timer(() => 
    {
        if(searchRoom.size >= USERS_PER_ROOM)
        {
            let roomPlayers: IPlayer[] = []
            searchRoom.forEach(player => 
            {
                roomPlayers.push(player);
                if(roomPlayers.length === USERS_PER_ROOM) 
                    return;
            });
            
            //const roomId = roomPlayers.map(player => player.id).join('-');
            function generateIncrementalRoomId(): string {
                roomIdCounter++;
                return `R${roomIdCounter}`;
            }

            let roomId = generateIncrementalRoomId();
            const room = new Room(roomId, roomPlayers);
            rooms.set(roomId, room);
            
            roomPlayers.forEach( (player) => {
                player.conexion.write(Buffer.from(`{"command": "ROOM_CREATED", "roomId": "${roomId}"}`, "utf-8"));
                console.log(`Room creada con ID: "${roomId}"`);
                searchRoom.delete(player.id);
            });
        }
    });
});

function handleError(err: any) {
    throw new Error("General error.");
}
