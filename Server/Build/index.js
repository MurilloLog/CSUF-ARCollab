"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const app = (0, express_1.default)();
// Server settings
app.set('port', 8080);
// Server start
app.listen(app.get('port'), () => {
    console.log("The app is listening the port server");
});
//# sourceMappingURL=index.js.map