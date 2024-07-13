import * as net from "net"
import Timer from "./Utils/Timer"
import { OneVsOne } from "./MatchMaking/Room";
import mongoose from "mongoose";
import { mongo } from "./database";
import Player from "./models/Player";
import Objects from "./models/Objects";

const USERS_PER_ROOM = 5; // Set number of players for each room

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

// List of all available instructions
let actions = 
{
    SEARCH_ROOM: "SEARCH_ROOM",
    UPDATE_PLAYER_POSE: "UPDATE_PLAYER_POSE",
    UPDATE_OBJECT_POSE: "UPDATE_OBJECT_POSE"
};

// Creating rooms
let roomsOneVsOne: Map<string, OneVsOne> = new Map<string, OneVsOne>();
let rooms: Map<string, Array<IPlayer>> = new Map<string, Array<IPlayer>>(); // An array<IPlayer>'s room 

// Creating the local server
let server = net.createServer(socket =>
    {
        const uuid = require('uuid').v4;
        let id = uuid();
        let connected = false;
        
        const player = new Player();
        
        // Process to send an ID connection to the clients
        timer.runAfter(1).then(() => 
        {
            console.log(`${socket.remoteAddress} is trying to connect...`); // Remote IP
            console.log(`Device connected with id: ${id}`); // Assigned ID
            timer.runAfter(1).then(() =>
            {
                socket.write(Buffer.from(`id: ${id}`, "utf-8"));
                player._id = id;
                player.save(function (err: any) {
                    if (err) return handleError(err);
                    // Saved on Mongo DB!
                  });
            });
            connected = true;
        });

        /**** Process to receive data from any client ****/
        socket.on("data", async data =>
        {
            try
            {
                let jsonData = JSON.parse(data.toString());
                console.log("\n*********************************");
                console.log(jsonData);
                switch (jsonData.command)
                {
                    case actions.SEARCH_ROOM:
                        console.log(jsonData._id + " is searching room");
                        searchRoom.set(id, {id: id, conexion: socket});
                        playersPaired++;

                        if (playersPaired < USERS_PER_ROOM)
                            socket.write(Buffer.from(`{"command": "WAITING_PLAYER"}`, "utf-8"));
                        else
                        {
                            let roomPlayers: IPlayer[] = [];
                            searchRoom.forEach(player => {
                                roomPlayers.push(player);
                                if (roomPlayers.length === USERS_PER_ROOM) return;
                            });

                            const roomId = roomPlayers.map(player => player.id).join('-');
                            rooms.set(roomId, roomPlayers);

                            roomPlayers.forEach(player => {
                                player.conexion.write(Buffer.from(`{"command": "ROOM_CREATED", "roomId": "${roomId}"}`, "utf-8"));
                                searchRoom.delete(player.id);
                            });

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
                                    unixTimestamp: jsonData.unixTimestamp
                                },
                                {new: true});
                                console.log("Player updated");
                                console.log(user);
                            }
                            else
                            {
                                console.log("Failed");
                            }

                            players.forEach( (playerSocket) => {
                                if(playerSocket.id != jsonData._id)
                                    playerSocket.conexion.write(Buffer.from(data.toString(), "utf-8"));
                            });

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
                        
                    default:
                        console.log("The received command is not recognized");
                        break;
                }

                jsonData = "";
            }
            catch(dataSyncError)
            {
                //console.log("Error " + e.message);
                console.log(data.toString());
                console.log(dataSyncError);
            }

            async function deleteCollection()
            {
                const objects = await Objects.find();
                objects.forEach((_id: any) => {
                    const query = Objects.findByIdAndDelete(_id);
                })
            }
            if(playersOffline <= 0)
                deleteCollection();
        });

        socket.on('error', (error)=>
        {
            console.log(error.message);
        });

        /**** Process to close socket connection ****/
        socket.on("close", () => 
        {
            socket.end();
            players.forEach( (playerSocket) => {
                playerSocket.conexion.write(Buffer.from(`{"command": "PLAYER_OFFLINE"}`, "utf-8"));
            });
            console.log(`Connection with ${socket.remoteAddress} : ${socket.remotePort} closed.`);
            playersOffline--;
        });
});

const PORT = 8080;

/**** Process to starts the server ****/
server.listen(PORT, () =>
{
    console.log("Conecting to mongo database... ");
    mongoose.set('strictQuery', true);
    mongoose.connect(`mongodb:\/\/${databaseConfig.host}/${databaseConfig.db}`,{
        family: 4,
    });
    console.log("Successful connection.");
    console.log("Server is running on port: " + PORT);
    console.log("Waiting for players...");

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
            
            const roomId = roomPlayers.map(player => player.id).join('-');
            rooms.set(roomId, roomPlayers);
            
            roomPlayers.forEach( (player) => {
                player.conexion.write(Buffer.from(`{"command": "ROOM_CREATED", "roomId": "${roomId}"}`, "utf-8"));
                searchRoom.delete(player.id);
            });
            
            /*roomsOneVsOne.set(players[0].id + players[1].id, new OneVsOne(players[0], players[1]));
            console.log("Room One vs One created between players ID: " + players[0].id + " and " + players[1].id);
            // Deleting players for search room list
            searchRoom.delete(players[0].id);
            searchRoom.delete(players[1].id);*/
        }
    });
});

function handleError(err: any) {
    throw new Error("General error.");
}
