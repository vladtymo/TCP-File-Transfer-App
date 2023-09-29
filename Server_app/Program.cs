using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Shared;
using System.Text.Json;

namespace Server_app
{
    class Program
    {
        static IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
        static int port = 7777;
        static void Main(string[] args)
        {
            IPEndPoint localEndPoint = new IPEndPoint(iPAddress, port);
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // створюємо екземпляр сервера вказуючи кінцеву точку для приєднання
            TcpListener server = new TcpListener(localEndPoint);
            // запускаємо прослуховування вказаної кінцевої точки
            server.Start(10);

            while (true)
            {
                try
                {
                    Console.WriteLine("\tWaiting for file...");
                    // отримуємо зв'язок з клієнтом
                    TcpClient client = server.AcceptTcpClient(); // waiting...

                    // отримуємо дані від клієнта
                    // та десеріалізуємо об'єкт
                    var info = (FileTransferInfo?)JsonSerializer.Deserialize(client.GetStream(), typeof(FileTransferInfo));
                    //BinaryFormatter serializer = new BinaryFormatter();
                    //var info = (FileTransferInfo)serializer.Deserialize(client.GetStream());

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Got a file: {info.Name} from {client.Client.RemoteEndPoint}");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Saving...");

                    if (!Directory.Exists("Files"))
                        Directory.CreateDirectory("Files");

                    // зберігаємо отриманий файл на сервері
                    //using (FileStream fs = new FileStream($"Files/{info.Name}", FileMode.Create, FileAccess.Write))
                    //{
                    //    fs.Write(info.Data, 0, info.Data.Length);
                    //}
                    File.WriteAllBytes($"Files/{info.Name}", info.Data);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Saved!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.ResetColor();
                }
            }

            // зупиняємо роботу сервера
            server?.Stop();
        }
    }
}