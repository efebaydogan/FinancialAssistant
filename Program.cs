using System;
using System.Data;
using System.Data.SqlClient;

namespace FinancialAssistant
{

    internal class Program
    {
        static void Main(string[] args)
        {
            UserInformation UserInformation = new UserInformation();
            Bills Bills = new Bills();
            Subscriptions subscriptions = new Subscriptions();

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
                MainMenu();
            }

            Console.ReadKey();

            //functions
            void MainMenu()
            {
                Console.Clear();
                if (UserInformation.isLogin == true)
                {
                    int actionNumber = 0;

                    Console.Clear();
                    Console.WriteLine("What action do you want to do?");
                    Console.WriteLine("1 - Bills \n2 - Subscriptions \n3 - Income and Expense");

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
                        case 3:
                            Console.Clear();
                            IncomeExpenseFunction();
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid value");
                            Console.ReadKey();
                            MainMenu();
                            break;
                    }
                }
            }
            void SignUpFunction()
            {
                try
                {
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

                    string signup = "INSERT INTO UserInformation (firstName,secondName,email,birthday,password) VALUES (@fn,@sn,@e,@bd,@pw)";
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

                Console.WriteLine("Welcome! \nHere you can do your bill actions. \n1 - Show Bills\n2 - Add Bills\n3 - Update Bills");
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

                    Console.WriteLine("Press any key to go back to main menu.");
                    Console.ReadKey();
                    MainMenu();
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

                        Console.WriteLine("Press any key to go back to main menu.");
                        Console.ReadKey();
                        MainMenu();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                else if (billInput == "3")
                {

                    Console.WriteLine("Which bill do you want to update? : ");
                    Bills.name = Console.ReadLine();
                    Console.WriteLine("Set the price");
                    Bills.cost = Convert.ToInt32(Console.ReadLine());

                    string updateBills = "Update BillTable set cost = @costparameter where billName = @nameparameter"; //Cost and billName are my columns name.
                    SqlCommand cmdUpdate = new SqlCommand(updateBills, connect);

                    cmdUpdate.Parameters.AddWithValue("@costparameter", Bills.cost);
                    cmdUpdate.Parameters.AddWithValue("@nameparameter", Bills.name);

                    Console.WriteLine("Successful!");
                    cmdUpdate.ExecuteNonQuery();
                    connect.Close();

                    Console.WriteLine("Press any key to go back to main menu.");
                    Console.ReadKey();
                    MainMenu();

                }
            }
            void SubscribeFunction()
            {
                Console.Clear();
                string subscriptionInput;

                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                Console.WriteLine("Welcome! \nHere you can do your subscription actions. \n1 - Show Subscriptions\n2 - Add Subscription\n3 - Update Subscriptions");
                subscriptionInput = Console.ReadLine();

                if (subscriptionInput == "1")
                {
                    Console.Clear();

                    try
                    {
                        string showSubscriptions = "select * from SubscriptionTable";
                        SqlCommand cmdssubscriptions = new SqlCommand(showSubscriptions, connect);

                        dr = cmdssubscriptions.ExecuteReader();

                        while (dr.Read())
                        {
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                Console.WriteLine(dr[i].ToString());
                            }
                        }
                        dr.Close();
                        cmdssubscriptions.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    connect.Close();

                    Console.WriteLine("Press any key to go back to main menu.");
                    Console.ReadKey();
                    MainMenu();
                }

                if (subscriptionInput == "2")
                {
                    Console.Clear();

                    Console.WriteLine("Write your subscription's name : ");
                    subscriptions.name = Console.ReadLine();
                    Console.WriteLine("Write how many months you subscribe : ");
                    subscriptions.months = Convert.ToInt16(Console.ReadLine());
                    Console.WriteLine("Write your subscription's price : ");
                    subscriptions.price = Convert.ToInt16(Console.ReadLine());

                    try
                    {
                        string addSubscription = "Insert into SubscriptionTable (subscriptionName, months, price) values (@sn,@m,@p)";
                        SqlCommand cmdaSubscription = new SqlCommand(addSubscription, connect);

                        cmdaSubscription.Parameters.AddWithValue("@sn", subscriptions.name);
                        cmdaSubscription.Parameters.AddWithValue("@m", subscriptions.months);
                        cmdaSubscription.Parameters.AddWithValue("@p", subscriptions.price);

                        Console.WriteLine("Successful!");
                        cmdaSubscription.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    connect.Close();

                    Console.WriteLine("Press any key to go back to main menu.");
                    Console.ReadKey();
                    MainMenu();
                }

                if (subscriptionInput == "3")
                {

                    string updateInput;
                    Console.Clear();

                    Console.WriteLine("Which subscription do you want to update : ");
                    subscriptions.name = Console.ReadLine();
                    Console.WriteLine("What do you want want to update about " + subscriptions.name + "\n1 - The price, 2 - The month");
                    updateInput = Console.ReadLine();

                    if (updateInput == "1")
                    {
                        Console.WriteLine("Set the price : ");
                        subscriptions.price = Convert.ToInt16(Console.ReadLine());

                        string updatepSubscription = "Update SubscriptionTable set price = @p where subscriptionName = @sn";
                        SqlCommand cmduSubscription = new SqlCommand(updatepSubscription, connect);

                        cmduSubscription.Parameters.AddWithValue("@p", subscriptions.price);
                        cmduSubscription.Parameters.AddWithValue("@sn", subscriptions.name);

                        Console.WriteLine("Successful!");

                        cmduSubscription.ExecuteNonQuery();
                        connect.Close();

                        Console.WriteLine("Press any key to go back to main menu.");
                        Console.ReadKey();
                        MainMenu();
                    }

                    if (updateInput == "2")
                    {
                        Console.WriteLine("How many months do you want to update of your " + subscriptions.name + " subscription");
                        subscriptions.months = Convert.ToInt16(Console.ReadLine());

                        string updatemSubscription = "Update SubscriptionTable set months = @m where subscriptionName = @sn";
                        SqlCommand cmduSubscription = new SqlCommand(updatemSubscription, connect);

                        cmduSubscription.Parameters.AddWithValue("@m", subscriptions.months);
                        cmduSubscription.Parameters.AddWithValue("@sn", subscriptions.name);

                        Console.WriteLine("Successful!");

                        cmduSubscription.ExecuteNonQuery();
                        connect.Close();

                        Console.WriteLine("Press any key to go back to main menu.");
                        Console.ReadKey();
                        MainMenu();
                    }
                }
            }
            void IncomeExpenseFunction()
            {
                //Expense Section
                Console.WriteLine("Here you can see your expense and income.");

                //We need to send bill and subscription datas to ExpenseTable.Then we calculate the exoense.

                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                //We insert the datas to our expense table but also we check is there same data in our expense table.
                string sendbExpense = "Insert into ExpenseTable (expenseName,expenseAmount) Select billName , cost from BillTable Where not exists (Select 1 from ExpenseTable where ExpenseTable.expenseName = BillTable.billName and ExpenseTable.expenseAmount = BillTable.cost)";
                string sendsExpense = "Insert Into ExpenseTable (expenseName,expenseAmount) Select subscriptionName,price from SubscriptionTable where not exists (Select 1 from ExpenseTable where ExpenseTable.expenseName = subscriptionName and ExpenseTable.expenseAmount = SubscriptionTable.price)";
                SqlCommand cmdsExpense = new SqlCommand(sendbExpense, connect);
                SqlCommand cmdsExpense2 = new SqlCommand(sendsExpense, connect);

                cmdsExpense.ExecuteNonQuery();
                cmdsExpense2.ExecuteNonQuery();

                //We send datas.Now we will calculate expense.

                string calExpense = "Select sum(expenseAmount) from ExpenseTable";
                SqlCommand cmdcExpense = new SqlCommand(calExpense, connect);
                object calculate = cmdcExpense.ExecuteScalar();
                int total1 = Convert.ToInt32(calculate);
                cmdcExpense.ExecuteScalar();

                Console.WriteLine("Let's see your expense : " + total1);



                //Income Section
                //We will check is there any data in IncomeTable.
                string checkNull = "select count(*) from IncomeTable ";
                SqlCommand cmdcNull = new SqlCommand(checkNull, connect);
                int rowCount = Convert.ToInt32(cmdcNull.ExecuteScalar());

                if (rowCount > 0)
                {
                    string calIncome = "Select sum(incomeAmount) from IncomeTable";
                    SqlCommand cmdcIncome = new SqlCommand(calIncome, connect);
                    int total2 = Convert.ToInt32(cmdcIncome.ExecuteScalar());
                    Console.WriteLine("And your income is : " + total2);
                }

                else
                {

                    Console.WriteLine("There is no income data in the table.You need to enter.");

                    Console.WriteLine("Enter the income name.");
                    UserInformation.incomeName = Console.ReadLine();
                    Console.WriteLine("Enter the income amount.");
                    UserInformation.incomeAmount = Console.ReadLine();

                    string insertIncome = "Insert into IncomeTable(incomeName,incomeAmount) values (@in,@ia)";
                    SqlCommand cmdiIncome = new SqlCommand(@insertIncome, connect);

                    cmdiIncome.Parameters.AddWithValue("@in", UserInformation.incomeName);
                    cmdiIncome.Parameters.AddWithValue("@ia", UserInformation.incomeAmount);
                    cmdiIncome.ExecuteNonQuery();
                    Console.WriteLine("Successful.");
                }

                connect.Close();
                Console.WriteLine("Press any key to go back to main menu.");
                Console.ReadKey();
                MainMenu();

            }
        }
    }
}
