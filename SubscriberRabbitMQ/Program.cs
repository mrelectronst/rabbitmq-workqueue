
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rktcczxq:eBER7Ivsuu2oNFAD-RPxpEFEPZMu99W5@whale.rmq.cloudamqp.com/rktcczxq"); //write AMQP URL

using (var connection = factory.CreateConnection())
{
    var channel = connection.CreateModel();

    channel.QueueDeclare("workqueue-one", true, false, false);

    channel.BasicQos(0,20,false);

    var subscriber = new EventingBasicConsumer(channel);

    channel.BasicConsume("workqueue-one", false, subscriber);

    subscriber.Received += (object? sender, BasicDeliverEventArgs e) =>
    {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        Thread.Sleep(1000);

        Console.WriteLine($"Received Message : {message}");

        channel.BasicAck(e.DeliveryTag, false);
    };
}