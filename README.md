# Business Chat

## Requirements
* Docker
* Dotnet core 3.1
* RabbitMQ

## Setup Manually
First, we need to have an instance of a RabbitMQ server running, for this example, we are using all the default values:
 * UserName: guest,
 * Password: guest,
 * Port: 5672
 
We can easily start an instance, by running the following command with docker
```
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

After that, we can compile our application
```
cd BusinessChat
dotnet build

```

Second, we start the boot that will be in charge of solving our stock requests
```
cd BusinessChat
 cd BusinessChat.StooqWorker
 dotnet run
```


Finally, we start the web application
```
cd BusinessChat
cd BusinessChat.Webapp/
dotnet run
```
 
The webapp will start at localhost: 5001

## How to test the bot?
Send the following command in the chat:
```
/stock=aapl.us
```
