﻿using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace UnityMiMDatabaseConnect
{
    class Program
    {
        public static MySqlConnection connection;

        public static string user = "UnityUser";
        public static string password = "BKunjm3uCqjdBQpL";
        public static string database = "NeuralNetwork";
        public static string ip = "127.0.0.1";
        public static string port = "5456";

        static void Main(string[] args)
        {
            connection = new MySqlConnection(string.Format("Server={0}; Port={1}; database={2}; UID={3}; password={4}", ip, port, database, user, password));
            connection.Open();
            TcpListener server = new TcpListener(5455);
            server.Start();
            Console.WriteLine("Stareted server");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread t = new Thread(new ParameterizedThreadStart(Program.clientHandlerer));
                t.Start(client);
            }
        }
        static void clientHandlerer(Object obj)
        {
            Console.WriteLine("Client Connected");
            TcpClient client = (TcpClient)obj;
            StreamReader clientReader = new StreamReader(client.GetStream());
            StreamWriter clientWriter = new StreamWriter(client.GetStream());
            
            while (true&&client.Connected)
            {
                string text = clientReader.ReadLine();
                Console.WriteLine("Running non Query: "+text);
                if (text.Length > 6)
                {
                    MySqlCommand cmd = new MySqlCommand(text, connection);
                    clientWriter.WriteLine(cmd.ExecuteNonQuery());
                    clientWriter.Flush();
                }
                else
                {
                    client.Close();
                    break;
                }
            }
            connection.Close();
            clientReader.Close();
            clientWriter.Close();
            client.Close();
        }
    }
}