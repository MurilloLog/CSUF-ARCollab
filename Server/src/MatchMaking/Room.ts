import {IPlayer} from "../Server";
import * as net from "net";

export class Room 
{
    constructor(private roomId: string, private players: IPlayer[]) { }

    public updateRoomStateOnEvent(data: any, playerIdWhoMadeUpdate: string) {
        // Aqui se procesa la actualizacion cuando un jugador envia datos o cambia el estado del sistema
        this.players.forEach(player => {
            // Enviar actualizaciones a todos los jugadores en la sala excepto al jugador de referencia
            if (player.id !== playerIdWhoMadeUpdate) {
                player.conexion.write(Buffer.from(JSON.stringify(data), "utf-8"));
            }
        });
    }

    returnPlayer(playerId: string) {
        this.players = this.players.filter(player => player.id !== playerId);
    }

    addPlayer(player: IPlayer) {
        this.players.push(player);
    }

    removePlayer(playerId: string) {
        this.players = this.players.filter(player => player.id !== playerId);
    }
}

/* Previous code
interface RoomConfig
{
    mode: "1VS1" | "2VS2";
}

function isOneVsOne(
    players: { player1: IPlayer; player2: IPlayer} | Map<String, IPlayer>): players is 
    {
        player1: IPlayer;
        player2: IPlayer
    }
    {
        return <Map<String, IPlayer>>players == undefined;
    }

class Room
{
    private config: RoomConfig;
    private player1: IPlayer | null = null;
    private player2: IPlayer | null = null;
    private players: Map<String, IPlayer> | null = null;

    /**** Constructor for mode OneVsOne **** /
    constructor(type: {player1:IPlayer; player2:IPlayer});

    /**** Constructor for mode TwoVsTwo **** /
    constructor(type: Map<String, IPlayer>);

    constructor(type: |{ player1: IPlayer; player2: IPlayer;} | Map<String, IPlayer>)
    {
        if(isOneVsOne(type))
        {
            this.config = {mode: "1VS1"};
            this.player1 = type.player1;
            this.player2 = type.player2;
        }
        else
        {
            this.config = {mode: "2VS2"};
            this.players = type;
        }
    }
}

export class OneVsOne extends Room
{
    constructor(player1: IPlayer, player2: IPlayer)
    {
        super({player1: player1, player2: player2})
    }
}*/