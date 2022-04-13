
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rktcczxq:eBER7Ivsuu2oNFAD-RPxpEFEPZMu99W5@whale.rmq.cloudamqp.com/rktcczxq"); //write AMQP URL

string messageRead;

do
{
    Console.Write("Please write your message for 50 times: ");
    messageRead = Console.ReadLine();
    if (messageRead != string.Empty && messageRead != null)
    {
        PublishMessage(messageRead);
    }
    else
    {
        Environment.Exit(0);
    }

} while (messageRead != null);

void PublishMessage(string message)
{
    try
    {
        using (var connection = factory.CreateConnection())
        {
            var channel = connection.CreateModel();

            channel.QueueDeclare("workqueue-one", true, false, false);

            Enumerable.Range(0, 40).ToList().ForEach(x =>
            {
                var messageBody = Encoding.UTF8.GetBytes(message+$"_{x}");

                channel.BasicPublish(string.Empty, "workqueue-one", null, messageBody);

                Console.WriteLine($"'{message}-{x}' is sended");
            });

            Console.WriteLine("All messages are sended");
        }
    }
    catch (Exception ex) { Console.WriteLine(ex.ToString()); }


}


