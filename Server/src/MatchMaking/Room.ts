import {IPlayer} from "../Server";
import * as net from "net";

export class Room 
{
    private roomId: string;
    private players: IPlayer[];

    constructor(roomId: string, players: IPlayer[] = []) { 
        this.roomId = roomId;
        this.players = players;
    }

    /**
     * Updates the state of the room when an event is triggered.
     * Sends data to all players except the one who triggered the update.
     * @param data - Data to send to other players.
     * @param playerIdWhoMadeUpdate - ID of the player who triggered the update.
     */
    public updateRoomStateOnEvent(data: any, playerIdWhoMadeUpdate: string): void {
        try {
            // Timestamp T3: the server is about to send the message to the client
            data.T3 = Date.now();

            const message = JSON.stringify(data);
            const bufferMessage = message;

            this.players.forEach(player => {
                // Skip the player who made the update
                if (player.id !== playerIdWhoMadeUpdate) {
                    try {
                        if (player.conexion && !player.conexion.destroyed) {
                            player.conexion.write(bufferMessage, "utf8");
                        } else {
                            console.log(`Player connection ${player.id} is invalid or destroyed.`);
                        }
                    } catch (writeError) {
                        console.error(`Error sending data to player ${player.id}:`, writeError);
                    }
                }
            });
        } catch (serializationError) {
            console.error("Error serializing data for updateRoomStateOnEvent:", serializationError);
        }
    }

    /**
     * Updates the state of the room when an event is triggered. It's used for testing purposes.
     * Sends data to all players including the one who triggered the update.
     * @param data - Data to send to other players.
     * @param playerIdWhoMadeUpdate - ID of the player who triggered the update.
     */
    public updateRoomStateOnEventTester(data: any, playerIdWhoMadeUpdate: string): void {
        try {
            // Timestamp T3: the server is about to send the message to the client
            data.T3 = Date.now();

            const message = JSON.stringify(data);
            const bufferMessage = message;

            this.players.forEach(player => {
                try {
                        if (player.conexion && !player.conexion.destroyed) {
                            player.conexion.write(bufferMessage, "utf8");
                        } else {
                            console.log(`Player connection ${player.id} is invalid or destroyed.`);
                        }
                    } catch (writeError) {
                        console.error(`Error sending data to player ${player.id}:`, writeError);
                    }
            });
        } catch (serializationError) {
            console.error("Error serializing data for updateRoomStateOnEvent:", serializationError);
        }
    }

    /**
     * Returns a player and removes them from the room.
     * @param playerId - ID of the player to return.
     */
    public returnPlayer(playerId: string): void {
        this.removePlayer(playerId);
    }

    /**
     * Adds a player to the room.
     * @param player - Player object to add.
     */
    public addPlayer(player: IPlayer): void {
        if (!this.players.find(p => p.id === player.id)) {
            this.players.push(player);
        } else {
            console.warn(`Player ${player.id} is already in the room.`);
        }
    }

    /**
     * Removes a player from the room.
     * @param playerId - ID of the player to remove.
     */
    public removePlayer(playerId: string): void {
        const initialLength = this.players.length;
        this.players = this.players.filter(player => player.id !== playerId);
        if (this.players.length < initialLength) {
            console.log(`Player ${playerId} removed from room ${this.roomId}.`);
        } else {
            console.warn(`Player ${playerId} not found in room ${this.roomId}.`);
        }
    }
}