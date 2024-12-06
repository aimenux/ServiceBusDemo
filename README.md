[![.NET](https://github.com/aimenux/ServiceBusDemo/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/aimenux/ServiceBusDemo/actions/workflows/ci.yml)

# ServiceBusDemo
```
Playing with service bus
```

> In this repo, i m using [service bus](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-overview) in order to send and receive messages.
>
> :one: `Example01` : use worker template with [ServiceBusClient](https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusclient)
>
> :two: `Example02` : use worker template with [ServiceBusReceiver](https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebussender) and [ServiceBusSender](https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusreceiver)
>
> :three: `Example03` : use worker template with [ServiceBusProcessor](https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusprocessor)
>
> To run the demo, type the following commands :
> - `dotnet run --project .\src\Example01`
> - `dotnet run --project .\src\Example02`
> - `dotnet run --project .\src\Example03`
>

**`Tools`** : net 8.0, servicebus
