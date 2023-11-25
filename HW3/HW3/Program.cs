int numOfAgents = 20;
Agent[] agents = new Agent[numOfAgents];
Thread[] threads = new Thread[numOfAgents];
Elevator elevator = new Elevator();
elevator.Begin();
for(int i = 0; i<numOfAgents; i++){
    agents[i] = new Agent(level: Random.Shared.Next(0,3), elevator: elevator, name: i.ToString());
    threads[i] = new Thread(agents[i].StartWorkDay);
}
foreach(Thread thread in threads) thread.Start();
foreach(Thread thread in threads) thread.Join();
elevator.Stop();
System.Console.WriteLine("Work day finished.");
System.Console.WriteLine("Press ENTER to exit.");
Console.ReadLine();