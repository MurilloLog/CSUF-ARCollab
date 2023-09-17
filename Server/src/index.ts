import mongoose from "mongoose";
import Player from "./models/Player";
import { mongo } from "./database";

let databaseConfig = new mongo();

async function conectBD(){
    const db = await mongoose.connect(`mongodb:\/\/${databaseConfig.host}/${databaseConfig.db}`);
    console.log("DB is connected to", db.connection.db.databaseName);
}

conectBD();

async function executeQuery() {
    
    /*  DESCRIPCIÓN:
    En este bloque se crea un nuevo documento de manera manual. Empleando la función save()
    se inserta el documento con las específicaciones definidad previamente al uso de la función.
    Si el _id se proporciona, solo será posible guardarlo una unica vez, de lo contrario, se
    mostrará el error de duplicación de valor.
    En cambio, si el _id se asigna de manera automatica, se pueden guardar tantos documentos
    como sean necesarios.*/

    /*const player = new Player({
        _id: "05411j",
        
        position:
        {
            x: 2.5,
            y: 5.5,
            z: 8.7,
        },

        rotation:
        {
            x: 2.5,
            y: 5.5,
            z: 8.7,
            w: 5.2
        } });

        await player.save();
        console.log(player);
    */


    const player = await Player.find({_id: " New player ID"}, {_id: 1, position: 1})
    if(player.length != 0)
    {
        console.log("There is an object");
        const player = await Player.findByIdAndUpdate("New player ID", 
        {position: { x: 1.5, y: 2.5, z: 1},
        rotation: { x: 1, y: 0, z: 1}},
        {new: true});
        console.log("Player updated");
        console.log(player);
    }
    else
    {
        console.log("There isn't an object");
        const player = new Player({
            _id: "New player ID",
            position: { x: 0.5, y: 1.5, z: 0},
            rotation: { x: 0, y: 0, z: 1}
        });
        await player.save();
        console.log(player);
        

    }
    
}

executeQuery();