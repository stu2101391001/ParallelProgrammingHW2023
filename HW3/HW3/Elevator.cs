class Elevator{
    /*
    Legend:
        Floors:
        0 - G
        1 - S
        2 - T1
        3 - T2
        Agent Levels:
        0 - Confidential
        1 - Secret
        2 - Top-Secret
    */
    int floor = 0;
    int changeFloor = 0;
    Semaphore free = new Semaphore(initialCount:1, maximumCount:1);
    ManualResetEvent off = new ManualResetEvent(false);
    ManualResetEvent arrived = new ManualResetEvent(false);
    ManualResetEvent move = new ManualResetEvent(false);
    Thread thread;

    public Elevator(){
        thread = new Thread(Start);
    }

    void Start(){
        while(!off.WaitOne(0)){
            move.WaitOne();
            ChangeFloor(changeFloor);
        }
    }
    private void ChangeFloor(int numOfFloors){
        move.Reset();
        Thread.Sleep(numOfFloors * 1000);
        arrived.Set();
    }

    public bool Enter(Agent agent){
        if(agent.Level == 0) return false;
        free.WaitOne();
        changeFloor =  Math.Abs(agent.Floor - floor);
        arrived.Reset();
        move.Set();
        arrived.WaitOne();
        return true;
    }
    public bool Exit(Agent agent, int exitFloor){
        changeFloor = Math.Abs(exitFloor - floor);
        arrived.Reset();
        move.Set();
        arrived.WaitOne();
        agent.Floor = exitFloor;
        if(agent.Level == 2 || exitFloor!=3){
            free.Release();
            return true;
        }
        return false;
    }

    public void Begin(){
        thread.Start();
    }
    public void Stop(){
        off.Set();
        changeFloor = 0;
        move.Set();
        thread.Join();
    }
}