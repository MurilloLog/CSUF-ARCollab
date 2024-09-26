import  {prop, getModelForClass, Severity, mongoose} from '@typegoose/typegoose'

class Draws {
    @prop({required: true, trim: true})
    _id: "string"
    
    @prop()
    playerCreator: "string"

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

    @prop()
    materialOption: "Decimal128"
}

const DrawingSchema = getModelForClass(Draws)
export default DrawingSchema