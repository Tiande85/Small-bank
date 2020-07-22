using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Small_bank
{
    /// <summary>
    /// Delegate associated with ValueEvent, which raised when login user making a deposit or withdraw
    /// Delegate accociated with OutputEvent, which raised after current balance is changed due to deposit or withdraw
    /// </summary>
    public delegate void DepositWithDrawEvent(string name, string transType, int amount);    
    public delegate void OutPutCurrentBal(string name, int currbal);

    /// <summary>
    /// Account class give customer functions to open an bank account and make deposit or withdraw. 
    /// It also provides password/user validation and withdraw check abilities.
    /// </summary>
    class Account : Bank
    {
        private string firstname;
        private string lastname;
        private string username;
        private string password;
        private int currentBalance;
        private int deposit;
        private int withdraw;
        private long accNum;
        private string[] accInfo;                                          //an array to store customer account information from a txt file

        private StreamWriter write;

        private static DateTime currDateTime = DateTime.Now;               //a system datetime tracker, which will show at the top of customre account information
        private string[] currDTFormat = currDateTime.GetDateTimeFormats();         //an array give customer an prefered format of datetime presentation. 

        public event DepositWithDrawEvent ValueEvent;                      //event ValueEvent raised when login user making a deposit or withdraw
        public event OutPutCurrentBal OutputEvent;                         //event OutputEvent raised after current balance is changed due to deposit or withdraw

        MyEvent loadEvent = new MyEvent();

        /// <summary>
        /// Username property provides a input username validation of Account class constructor 
        /// </summary>
        public string Username
        {
            get { return username; }
            set
            {
                int flag = 1;
                while (flag == 1)
                {
                    int check = 0;
                    while (value.Length < 5 || value.Length > 16)                      //username must have 5 - 16 length
                    {
                        Console.WriteLine($"{firstname}, please enter 6 to 16 character username again!");
                        value = Console.ReadLine();
                    }
                    foreach (char x in value)
                    {
                        if (char.IsPunctuation(x))                                     //username must not contain punctuation
                        {
                            check++;
                        }
                    }
                    if (check == 0)
                    {
                        username = value;
                        File.AppendAllText("AllUsername.txt", username + "\n");
                        flag = 0;
                    }
                    else
                    {
                        Console.WriteLine($"{firstname}, please enter username again, no Punctuation!");
                        value = Console.ReadLine();
                    }
                }
            }
        }

        /// <summary>
        /// Password property provides a input password validation from Account class constructor
        /// </summary>
        public string Password
        {
            get { return password; }
            set
            {
                int x = 1;                                                             //using an flag to indicate the time the while loop should stop
                while (x == 1)
                {
                    byte[] passwordCheck = new byte[] { 0, 0, 0, 0 };                  //an array to hold the count for lower, upper, digit, and punctuation in a password 
                    if (value.Length < 10 || value.Length > 16)                        //a password must have 10 - 16 length, otherwise keeping asking for user input
                    {
                        Console.WriteLine("Enter a password with at least 1 upper, lower, special characters, and numbers. 10 - 16 characters : ");
                        value = Console.ReadLine();
                        continue;
                    }
                    else
                    {
                        for (int i = 0; i < value.Length; i++)                         //use for loop to count the number of occurrance of each type of characters, and stored in an array
                        {
                            if (char.IsLower(value[i]))
                                passwordCheck[0]++;
                            else if (char.IsUpper(value[i]))
                                passwordCheck[1]++;
                            else if (char.IsPunctuation(value[i]))
                                passwordCheck[2]++;
                            else if (char.IsDigit(value[i]))
                                passwordCheck[3]++;
                        }
                    }
                    if (!passwordCheck.Contains<byte>(0))                               //check if the array contains a 0 count for any type of characters, if there is a 0, keeping asking for user input
                    {
                        password = value;
                        File.AppendAllText("AllPasswords.txt", password + "\n");
                        x = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Deposit property provides a deposit validation from Account class constructor 
        /// </summary>
        public int Deposit
        {
            get { return deposit; }
            set
            {
                if (value < 0)                    //if input deposit is smaller than 0, set deposit to 0
                {
                    deposit = 0;
                }
                else
                    deposit = value;
            }
        }

        /// <summary>
        /// InitialDeposit property allow new customer to set their initial balance, and pass the value to field deposit
        /// </summary>
        public int InitialDeposit
        {
            get { return deposit; }
            set
            {
                while (value < 1500)              //initial deposit must be bigger or equal to $1500
                {
                    Console.WriteLine("Your first deposit must bigger than $1500, re-enter a value: ");
                    value = int.Parse(Console.ReadLine());
                }
                deposit = value;
            }
        }

        /// <summary>
        /// MakeDeposit function is independent of any user input information from Account class constructor used for opening an bank account.
        /// It allows user login to their account page with password/username to make deposit. After deposit is made,
        /// updated user account information will be re-write into the same txt file (with the file name of "account_number.txt").
        /// </summary>
        public void MakeDeposit()
        {
            accInfo = File.ReadAllLines($"{accNum}.txt");    //after user login and the relevent user account number was validated, open the relevent account txt file and store all account information into an string array
            Console.WriteLine($"{accInfo[4]}, enter your deposit amount : ");     //rais a prompt to ask usr enter an ammount for deposit
            Deposit = int.Parse(Console.ReadLine());                              //store the user input deposit ammount in to Deposit property  
            ValueEvent = new DepositWithDrawEvent(loadEvent.ChangeCurrentBal);    //refer ChangeCurrentBal functioin in MyEvent class to ValueEvent
            ValueEvent?.Invoke(accInfo[4], "deposit", deposit);                   //invoke ValueEvent to output a deposit prompt
            CurrentBalance = int.Parse(accInfo[16]) + deposit;                    //change user current balance by summing the original current balance to user input deposit
            if (deposit != 0)                                                     //if user input deposit is not 0, refer the OutputAccBal function from MyEvent class to OutputEvent, and invoke the event
            {
                OutputEvent = new OutPutCurrentBal(loadEvent.OutputAccBal);
                OutputEvent?.Invoke(accInfo[4], currentBalance);
            }
            Deposit = int.Parse(accInfo[12]) + deposit;                           //get user total deposit ammount by summing all deposit value 
            accInfo[0] = currDTFormat[15];                                        //re-assign datetime to account info array 
            accInfo[12] = deposit.ToString();                                     //re-assign total deposit value 
            accInfo[16] = currentBalance.ToString();                              //re-assign current balance 
            File.WriteAllLines($"{accNum}.txt", accInfo);                         //re-write all account info from arry to the account txt file
        }

        /// <summary>
        /// Withdraw property provides a withdraw validation from Account class constructor 
        /// </summary>
        public int Withdraw
        {
            get { return withdraw; }
            set
            {
                if (value < 0 || value > currentBalance)   //when user input withdraw small than 0 or bigger than current balance, set withdraw to 0
                {
                    withdraw = 0;
                }
                else
                    withdraw = value;
            }
        }

        /// <summary>
        /// MakeWithdraw function is independent of any user input information from Account class constructor used for opening an bank account.
        /// It allows user login to their account page with password/username to make withdraw. After withdraw is made,
        /// updated user account information will be re-write into the same txt file (with the file name of "account_number.txt").
        /// </summary>
        public void MakeWithdraw()
        {
            accInfo = File.ReadAllLines($"{accNum}.txt");   //after user login and the user account number was validated, store all account information into an string array
            CurrentBalance = int.Parse(accInfo[16]);        //assign current balance value from array before user input the withdraw ammount
            Console.WriteLine($"{accInfo[4]}, enter your withdraw amount : ");
            Withdraw = int.Parse(Console.ReadLine());                              //get user input withdraw 
            ValueEvent = new DepositWithDrawEvent(loadEvent.ChangeCurrentBal);     
            ValueEvent?.Invoke(accInfo[4], "withdraw", withdraw);                  //rais enent
            CurrentBalance = int.Parse(accInfo[16]) - withdraw;                    //change user current balance   
            if (withdraw != 0)                                                     //if withdraw is not 0, rais event
            {
                OutputEvent = new OutPutCurrentBal(loadEvent.OutputAccBal);
                OutputEvent?.Invoke(accInfo[4], currentBalance);
            }
            Withdraw = int.Parse(accInfo[14]) + withdraw;                          //get the total withdraw
            accInfo[0] = currDTFormat[15];                                         //update the array                       
            accInfo[14] = withdraw.ToString();
            accInfo[16] = currentBalance.ToString();
            File.WriteAllLines($"{accNum}.txt", accInfo);                          //re-write account info to its relevent txt file
        }

        /// <summary>
        /// CurrentBalance property provides a CurrentBalance validation from Account class constructor 
        /// </summary>
        private int CurrentBalance
        {
            get { return currentBalance; }
            set
            {
                if (value > 0)                
                {
                    currentBalance = value;
                }
                else                           //current balance must be at least 0 amount
                    currentBalance = 0;
            }
        }

        /// <summary>
        /// Login function creates a Login class instance, and assign the validated account number to string accNum
        /// </summary>
        public void LogIn()               
        {
            Login myLogin = new Login();
            accNum = long.Parse(myLogin.LogIn());     //LogIn function in Login class returns a string
        }

        /// <summary>
        /// Default constructor is only used for user login function to make deposit/withdraw
        /// </summary>
        public Account()
        {

        }

        /// <summary>
        /// This consturctor is only used for initialize an customer bank account 
        /// </summary>
        public Account(string firstname, string lastname, string username, string passowrd, int deposit)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            Username = username;
            Password = passowrd;
            Deposit = deposit;
            Withdraw = 0;
            CurrentBalance = deposit;        //set initial current balance equal to initial deposit
            accNum = AccNumGenerator();      //assign a new customer an account number
            CusCount();                      //count the total customer number
        }

        /// <summary>
        /// OpenAccount function allow new user to oper a new account
        /// </summary>
        public void OpenAccount() 
        {
            Console.WriteLine("Enter your firstname : ");
            firstname = Console.ReadLine();
            Console.WriteLine("Enter your lastname : ");
            lastname = Console.ReadLine();
            Console.WriteLine("Enter your username : ");
            Username = Console.ReadLine();
            Console.WriteLine("Enter your password : ");
            Password = Console.ReadLine();
            Console.WriteLine("Enter your intital deposit : ");
            InitialDeposit = int.Parse(Console.ReadLine());       
            Withdraw = 0;                                         //customer can not have withdraw when opening an account
            CurrentBalance = deposit;                             //initial balance is equal to initial deposit
            accNum = AccNumGenerator();
            CusCount();
            WriteAccInfo();
        }

        /// <summary>
        /// WriteAccInfo function write all customer and account information after a new customer opening an account
        /// </summary>
        public void WriteAccInfo()
        {
            write = new StreamWriter($"{accNum}.txt");
            write.WriteLine(currDTFormat[15]);
            write.WriteLine("Account number : ");
            write.WriteLine($"{accNum}");
            write.WriteLine("Fistname : ");
            write.WriteLine($"{firstname}");
            write.WriteLine("Lastname : ");
            write.WriteLine($"{lastname}");
            write.WriteLine("Username : ");
            write.WriteLine($"{username}");
            write.WriteLine("Password : ");
            write.WriteLine($"{password}");
            write.WriteLine("Deposit : ");
            write.WriteLine($"{deposit}");
            write.WriteLine("Withdraw : ");
            write.WriteLine($"{withdraw}");
            write.WriteLine("Current balance : ");
            write.WriteLine($"{currentBalance}");
            write.Close();
        }
    }
}
