import  {prop, getModelForClass, Severity, mongoose} from '@typegoose/typegoose'

class Objects {
    @prop({required: true, trim: true})
    _id: "string"
    
    @prop()
    playerCreator: "string"

    @prop()
    playerEditor: "string"

    @prop()
    objectMesh: "string"

    @prop()
    IsSelected: "Boolean"

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

    @prop({ allowMixed: Severity.ALLOW, type: () => mongoose.Schema.Types.Mixed })
    scale:
    {
        x: "Decimal128",
        y: "Decimal128",
        z: "Decimal128"
    }
}

const ObjectSchema = getModelForClass(Objects)
export default ObjectSchema