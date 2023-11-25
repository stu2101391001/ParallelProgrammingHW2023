class Agent{
    int level = 0;
    public int Level{get{return level;}private set{if(value >=0 && value<=2) level = value;}}
    public int Floor{get;set;} = 0;
    public string Name{get;set;}
    Elevator elevator;

    public Agent(int level, Elevator elevator, string name){
        Level = level;
        this.elevator = elevator;
        Name = name;
    }

    public void StartWorkDay(){
        while(true){
        int chance = Random.Shared.Next(101);
        if(chance < 70) Work();
        else if(chance < 90){
            if(elevator.Enter(this)){
                System.Console.WriteLine($"Agent {Name} is entering the elevator from floor {Floor}.");
                while(true){
                    int floor = Random.Shared.Next(0,4);
                    while(floor == Floor){
                        floor = Random.Shared.Next(0,4);
                    }
                    if(elevator.Exit(this, floor)) {
                        System.Console.WriteLine($"Agent {Name} has left the elevator at floor {Floor}.");
                        break;
                    }
                    System.Console.WriteLine($"Access to level {Floor} denied to agent {Name}.");
                }
            }
            else System.Console.WriteLine($"Access to elevator denied to agent {Name}.");
        }
        else if(Floor==0) {
            System.Console.WriteLine($"Agent {Name} is leaving for today."); 
            break;
            }
        else {
            elevator.Enter(this);
            elevator.Exit(this, 0);
            System.Console.WriteLine($"Agent {Name} is leaving for today.");
            break;
        }
        }

    }

    private void Work(){
        System.Console.WriteLine($"Agent {Name} is working.");
        Thread.Sleep(Random.Shared.Next(100, 401));
    }
}