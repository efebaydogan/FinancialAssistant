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
            Bills Bills = new Bills();

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
                int actionNumber = 0;

                Console.Clear();
                LoginFunction();

                if (UserInformation.isLogin == true)
                {
                    Console.Clear();
                    Console.WriteLine("What action do you want to do?");
                    Console.WriteLine("1 - Bills \n2 - Subscriptions");

                    actionNumber = Convert.ToInt16(Console.ReadLine());

                    switch (actionNumber)
                    {
                        case 1:
                            Console.Clear();
                            BillsFunction();
                            break;

                        case 2:
                            Console.Clear();
                            SubscribeFunction();
                            break;
                    }

                }
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

                    string signup = "(Your Connection String)";
                    //In the first parantheses,you should write your columns name in your sql table.Second paranteses include our parametres.You can write what do you want with @.
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

                string login = "Select * from UserInformation where email=@e and password=@pw";
                //email and password are my columns' name.
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
                    UserInformation.isLogin = true;
                }
                else
                {
                    Console.WriteLine("Your information is incorrect! You cannot log in.");
                }
                connect.Close();

            }
            void BillsFunction()
            {

                Console.Clear();
                string billInput;

                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                Console.WriteLine("Welcome! \nHere you can see your bills,or add new bills.What do you want to do? \n1 - Show Bills\n2 - Add Bills\n3 - Update Bills");
                billInput = Console.ReadLine();

                if (billInput == "1")
                {
                    Console.Clear();

                    try
                    {

                        Console.WriteLine("Of Course!");

                        string showbills = "Select * from BillTable";
                        SqlCommand cmdsbills = new SqlCommand(showbills, connect);

                        dr = cmdsbills.ExecuteReader();

                        while (dr.Read())
                        {
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                Console.WriteLine(dr[i].ToString() + "\t");
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    connect.Close();
                }

                else if (billInput == "2")
                {
                    try
                    {

                        Console.WriteLine("Write the Your bill's name : ");
                        Bills.name = Console.ReadLine();
                        Console.WriteLine("Write the Your bill's cost : ");
                        Bills.cost = Convert.ToInt32(Console.ReadLine());

                        string addBills = "insert into BillTable (billName,cost) VALUES (@bn,@c)";
                        SqlCommand cmdbill = new SqlCommand(addBills, connect);

                        cmdbill.Parameters.AddWithValue("@bn", Bills.name);
                        cmdbill.Parameters.AddWithValue("@c", Bills.cost);

                        Console.WriteLine("Successful!");

                        cmdbill.ExecuteNonQuery();
                        connect.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                else if(billInput == "3")
                {

                    Console.WriteLine("Which bill do you want to update? : ");
                    Bills.name = Console.ReadLine();
                    Console.WriteLine("Set the price");
                    Bills.cost = Convert.ToInt32(Console.ReadLine());

                    string updateBills = "Update BillTable set cost = @costparameter where billName = @nameparameter"; //Cost and billName are my columns name.
                    SqlCommand cmdUpdate = new SqlCommand(updateBills,connect);

                    cmdUpdate.Parameters.AddWithValue("@costparameter", Bills.cost);
                    cmdUpdate.Parameters.AddWithValue("@nameparameter", Bills.name);

                    Console.WriteLine("Successful!");
                    cmdUpdate.ExecuteNonQuery();
                    connect.Close();

                }
            }

            void SubscribeFunction()
            {
                Console.WriteLine("subscribe");
            }
        }
    }
}