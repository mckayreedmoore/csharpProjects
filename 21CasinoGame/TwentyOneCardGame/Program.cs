﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Casino1;
using Casino1.TwentyOne;
using System.Data.SqlClient;
using System.Data;

namespace TwentyOneCardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Welcome to the Grand Hotel and Casino! \nWhat is your name?");
            string playerName = Console.ReadLine();

            if (playerName.ToLower() == "admin")
            {
                List<ExceptionEntity> Exceptions = ReadExceptions();
                foreach (var exception in Exceptions)
                {
                    Console.Write(exception.Id + " | ");
                    Console.Write(exception.ExceptionType + " | ");
                    Console.Write(exception.ExceptionMessage + " | ");
                    Console.Write(exception.TimeStamp + " | ");
                    Console.WriteLine();
                }
                Console.Read();
                return;
            }
            

            bool validAnswer = false;
            int bank = 0;
            while (!validAnswer)
            {
                Console.WriteLine("And How much money did you bring today?");
                validAnswer = int.TryParse(Console.ReadLine(), out bank);
                if (!validAnswer) Console.WriteLine("Please enter digits only, no decimals");
            }

            
            Console.WriteLine("Hello, {0}. Would you like to join a game of Twenty-One?", playerName);


            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" || answer == "yea" || answer == "y" || answer == "ya")
            {
                Player player = new Player(playerName, bank);
                player.Id = Guid.NewGuid();
                using (StreamWriter file = new StreamWriter(@"C:\Users\mckay\OneDrive\Documents\GitHub\C-Sharp Projects\Basic_C#_Programs\log.txt", true))
                {
                    file.WriteLine(player.Id);
                    
                }
                game game = new TwentyOneGame();
                game += player;
                player.isActivelyPlaying = true;
                while (player.isActivelyPlaying && player.Balance > 0)
                {
                    try
                    {
                        game.Play();
                    }
                    catch(FraudException ex)
                    {
                        Console.WriteLine(ex.Message);
                        UpdatedDbWithException(ex);
                        Console.ReadLine();
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred. Please contact your system administrator.");
                        UpdatedDbWithException(ex);
                        Console.ReadLine();
                        return;
                    }
                    
                }
                //game -= player;
                Console.WriteLine("Thank you for playing!");
            }
            Console.WriteLine("Feel free to look around the casino. bye for now!");
            Console.Read();
        }

        private static void UpdatedDbWithException(Exception ex)
        {
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwentyOneGame;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string queryString = @"INSERT INTO Exceptions (ExceptionType, ExceptionMessage, TimeStamp) VALUES (@ExceptionType, @ExceptionMessage, @TimeStamp)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@ExceptionType", SqlDbType.VarChar);
                command.Parameters.Add("@ExceptionMessage", SqlDbType.VarChar);
                command.Parameters.Add("@TimeStamp", SqlDbType.DateTime);

                command.Parameters["@ExceptionType"].Value = ex.GetType().ToString();
                command.Parameters["@ExceptionMessage"].Value = ex.Message;
                command.Parameters["@TimeStamp"].Value = DateTime.Now;

                connection.Open();
                 command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static List<ExceptionEntity> ReadExceptions()
        {
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwentyOneGame;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string queryString = @"Select Id, ExceptionType, ExceptionMessage, TimeStamp From Exceptions";

            List<ExceptionEntity> Exceptions = new List<ExceptionEntity>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ExceptionEntity exception = new ExceptionEntity();
                    exception.Id = Convert.ToInt32(reader["Id"]);
                    exception.ExceptionType = reader["ExceptionType"].ToString();
                    exception.ExceptionMessage = reader["ExceptionMessage"].ToString();
                    exception.TimeStamp = Convert.ToDateTime(reader["TimeStamp"]);
                    Exceptions.Add(exception);
                }
                connection.Close();
            }

            return Exceptions;
        }
    }
}
