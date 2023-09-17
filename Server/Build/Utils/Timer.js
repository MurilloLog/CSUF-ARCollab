"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Timer {
    range = 60;
    si;
    constructor(run, range) {
        if (range !== undefined)
            this.range = range;
        if (run !== undefined)
            this.start(run);
    }
    start(run) {
        this.si = setInterval(() => {
            run();
        }, 1000 / this.range);
    }
    stop() {
        clearInterval(this.si);
    }
    runAfter(time, callback) {
        return new Promise((res, rej) => {
            let range = setInterval(() => {
                callback !== undefined ? callback() : null;
                res();
                clearInterval(range);
            }, 1000 * time);
        });
    }
}
exports.default = Timer;
//# sourceMappingURL=Timer.js.map