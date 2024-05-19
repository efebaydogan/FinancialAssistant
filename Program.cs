using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Remoting;
using System.Diagnostics;

namespace FinancialAssistant
{

    internal class Program
    {
        static void Main(string[] args)
        {
            UserInformation UserInformation = new UserInformation();

            //SQL Connection
            string conString = "Your Connection String";
            SqlConnection connect = new SqlConnection(conString);
            SqlDataReader dr; //For check our information to login.

            Console.WriteLine("Hello! \nWelcome to your Financial Assistant.");
            Console.WriteLine("If you are new here you need to sign up or you can just login.");
            Console.WriteLine("What you want to do? \n1 - Signup \n2 - Login\n");

            string userInput = Console.ReadLine();

            if (userInput == "1")
            {
                Console.Clear();
                SignUpFunction();
            }

            else if (userInput == "2")
            {
                Console.Clear();
                LoginFunction();
            }

            Console.ReadKey();

            //functions
            void SignUpFunction()
            {
                try
                {
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

                    string signup = "INSERT INTO UserInformation (firstName,secondName,email,birthday,password) VALUES (@fn,@sn,@e,@bd,@pw)";//In the first parantheses,you should write your columns name in your sql table.Second paranteses include our parametres.You can write what do you want with @.
                    SqlCommand cmdsignup = new SqlCommand(signup, connect);

                    Console.WriteLine("Welcome to the signup panel.You need to enter the required information.");

                    Console.WriteLine("First, What is your name ? :");
                    UserInformation.firstName = Console.ReadLine();
                    Console.WriteLine("What is your second name ? :");
                    UserInformation.secondName = Console.ReadLine();
                    Console.WriteLine("What is your email ? :");
                    UserInformation.email = Console.ReadLine();
                    Console.WriteLine("When is your birthday(YYYY-MM-DD) ? :");
                    UserInformation.birthday = Console.ReadLine();
                    Console.WriteLine("Set a password. :");
                    UserInformation.password = Console.ReadLine();

                    cmdsignup.Parameters.AddWithValue("@fn", UserInformation.firstName);
                    cmdsignup.Parameters.AddWithValue("@sn", UserInformation.secondName);
                    cmdsignup.Parameters.AddWithValue("@e", UserInformation.email);
                    cmdsignup.Parameters.AddWithValue("@pw", UserInformation.password);
                    cmdsignup.Parameters.AddWithValue("@bd", UserInformation.birthday);

                    cmdsignup.ExecuteNonQuery();
                    connect.Close();

                    Console.WriteLine("Tamamlandı");
                    Console.ReadKey();
                }

                catch (Exception error)
                {
                    Console.WriteLine(error.Message);
                }
            }
            void LoginFunction()
            {

                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                string login = "Select * from UserInformation where email=@e and password=@pw"; //email and password are my columns' name.
                SqlCommand cmdlogin = new SqlCommand(login, connect);

                Console.WriteLine("Welcome to the login panel.You need to enter the required information.");
                Console.WriteLine("First,what's your email? :");
                UserInformation.email = Console.ReadLine();
                Console.WriteLine("What's your password? :");
                UserInformation.password = Console.ReadLine();

                cmdlogin.Parameters.AddWithValue("@e", UserInformation.email);
                cmdlogin.Parameters.AddWithValue("@pw", UserInformation.password);

                dr = cmdlogin.ExecuteReader();

                if (dr.Read())
                {
                    Console.Write("Your information is correct!\n You can log in successfully.");
                }
                else
                {
                    Console.WriteLine("Your information is incorrect!\n You cannot log in.");
                }
                connect.Close();

            }
        }
    }
}