import {IPlayer} from "../Server";

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

    /**** Constructor for mode OneVsOne ****/
    constructor(type: {player1:IPlayer; player2:IPlayer});

    /**** Constructor for mode TwoVsTwo ****/
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
}