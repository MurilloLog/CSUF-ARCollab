export default class Timer
{
    private range: number = 60;
    private si: any;

    constructor();
    constructor(run: Function);
    constructor(run: Function, range: number);
    constructor(run?: Function, range?: number)
    {
        if(range !== undefined)
            this.range = range;
        if(run !== undefined)
            this.start(run);
    }

    private start(run: Function)
    {
        this.si = setInterval(() =>
        {
            run();
        }, 1000 / this.range);
    }

    stop()
    {
        clearInterval(this.si);
    }

    runAfter(time: number, callback?: Function): Promise<void>
    {
        return new Promise((res, rej) => 
        {
            let range = setInterval(() =>
            {
                callback !== undefined ? callback() : null;
                res();
                clearInterval(range);
            }, 1000 * time);
        });
    }
}