[![.NET](https://github.com/aimenux/ServiceBusDemo/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/aimenux/ServiceBusDemo/actions/workflows/ci.yml)

# ServiceBusDemo
```
Using various ways to send and receive messages to/from azure service bus
```

> In this repo, i m using various ways in order to send and receive messages to/from [azure service bus](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-overview).
>
> :one: `Example01` : use worker template with [ServiceBusClient](https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusclient)
>
> :two: `Example02` : use worker template with [ServiceBusReceiver](https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusreceiver) and [ServiceBusSender](https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebussender)
>
> :three: `Example03` : use worker template with [ServiceBusProcessor](https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusprocessor)
>
> :four: `Example04` : use worker template with [MassTransit](https://masstransit-project.com)
>
> :five: `Example05` : use worker template with [Wolverine](https://wolverinefx.net)
>
> :six: `Example06` : use worker template with [Rebus](https://github.com/rebus-org/Rebus)
>
> :seven: `Example07` : use worker template with [SlimMessageBus](https://github.com/zarusz/SlimMessageBus)
> 

**`Tools`** : net 8.0, servicebus, masstransit, wolverine, rebus, slim-message-bus
