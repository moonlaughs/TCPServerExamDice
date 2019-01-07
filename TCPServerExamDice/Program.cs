using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TCPServerExamDice
{
    class Program
    {
        private static readonly int PORT = 7000;

        static void Main(string[] args)
        {
            IPAddress localAddress = IPAddress.Loopback;
            TcpListener serverSocket = new TcpListener(localAddress, PORT);

            serverSocket.Start();

            Console.WriteLine("TCP Server running on port number: " + PORT);

            while (true)
            {
                try
                {
                    TcpClient client = serverSocket.AcceptTcpClient();
                    Console.WriteLine("Incoming client");
                    Task.Run((() => DoComunicate(client)));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private static void DoComunicate(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);

            while (true)
            {
                ClassLibraryExamDice.DiceRoll obj = new ClassLibraryExamDice.DiceRoll();

                string request = reader.ReadLine();

                if (request != null)
                {
                    Console.WriteLine("Request: " + request);

                    string response = null;
                    string[] myArray = request.Split(',');

                    if (request.Split(',').Length == 3)
                    {
                        int number = Convert.ToInt32(myArray[2]);
                        int guess = Convert.ToInt32(myArray[1]);
                        response = "Name: " + myArray[0] + ", result: "+ obj.ResultMethod(guess, number).ToString();

                        Console.WriteLine("Responce: " + response);
                        writer.WriteLine(response + "\n ");
                        Console.WriteLine();
                        writer.WriteLine();

                        
                        //if (myArray[2] == "1")
                        //{
                        //    int number = Convert.ToInt32(myArray[2]);
                        //    int guess = Convert.ToInt32(myArray[1]);
                        //    response = obj.ResultMethod(guess, number).ToString();

                        //    Console.WriteLine("Responce: " + response);
                        //    writer.WriteLine(response + "\n ");
                        //    Console.WriteLine();
                        //    writer.WriteLine();

                        //}
                        //if (myArray[2] == "2")
                        //{
                        //    int number = Convert.ToInt32(myArray[2]);
                        //    int guess = Convert.ToInt32(myArray[1]);
                        //    response = obj.ResultMethod(guess, number).ToString();

                        //    Console.WriteLine("Responce: " + response);
                        //    writer.WriteLine(response + "\n ");
                        //    Console.WriteLine();
                        //    writer.WriteLine();
                        //}
                        //if (request != "Dice1" && request != "Dice2")
                        //{
                        //    Console.WriteLine("Available actions: Dice1/Dice2");
                        //    writer.WriteLine("No such action available");
                        //    Console.WriteLine();
                        //    writer.WriteLine();
                        //}

                    }
                    if (request == "STOP")
                    {
                        break;
                    }
                    else
                    {
                        if (request.Split(',').Length != 3)
                        {
                            Console.WriteLine("No such action available");
                            Console.WriteLine();
                            writer.WriteLine("No such action available");
                            writer.WriteLine();
                        }
                    }
                    writer.Flush();
                }
                
            }
            client.Close();
            Console.WriteLine("Client disconnected.\nWaiting...");
        }
    }
}
