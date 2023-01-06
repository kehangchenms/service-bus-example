# AsbMassNetCoreQueue

This solution consists of three projects,
- AsbMassNetCoreQueue - Sender
- AsbMassNetCoreQueue.Consumer - Consumer
- AsbMassNetCoreQueue.Contracts - Shared classes

## Compile & Run

1. Build the projects: `dotnet build`
2. Run Sender: `dotnet run --project AsbMassNetCoreQueue/AsbMassNetCoreQueue.Sender.csproj`
3. Run Consumer: `dotnet run --project AsbMassNetCoreQueue.Consumer/AsbMassNetCoreQueue.Consumer.csproj`

## Note

Due to the issue of port duplication issue, I can not run both projects in the same time and hopefully it is an easy fix for you guys.  To get message through,

1. Start the "Sender": `dotnet run --project AsbMassNetCoreQueue/AsbMassNetCoreQueue.Sender.csproj`
2. Send request to the "Sender" with: `curl -v -X POST https://localhost:5001/api/orders`
3. Go to Azure portal, you can see the "Incoming Messages" in the "tsys-msg" queue of the "SweetBus" namespace
3. After sending messages to the queue, stop "Sender" 
4. Start the "Consumer":  `dotnet run --project AsbMassNetCoreQueue.Consumer/AsbMassNetCoreQueue.Consumer.csproj`
5. Go to Azure portal You can see the "Outgoing Messages" for the "tsys-msg" queue in "SweetBus" namespace increase.

Also, I am not sure what happed to those two namespaces (DevTest00 and DevTestDisRecov00), I cannot create queue in them.

I will fix the issue of duplicated port issue for these two projects tomorrow and also add pub/sub example.