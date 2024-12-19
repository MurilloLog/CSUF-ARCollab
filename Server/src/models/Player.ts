import  {prop, getModelForClass, Severity, mongoose} from '@typegoose/typegoose'

class Player {
    @prop({required: true, trim: true})
    _id: string

    @prop({ allowMixed: Severity.ALLOW, type: () => mongoose.Schema.Types.Mixed })
    position:
    {
        x: "Decimal128",
        y: "Decimal128",
        z: "Decimal128"
    }

    @prop({ allowMixed: Severity.ALLOW, type: () => mongoose.Schema.Types.Mixed })
    rotation:
    {
        x: "Decimal128",
        y: "Decimal128",
        z: "Decimal128",
        w: "Decimal128"
    }
}

const PlayerSchema = getModelForClass(Player)
export default PlayerSchema