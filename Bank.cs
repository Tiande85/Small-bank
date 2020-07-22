using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Small_bank
{
    class Bank 
    {        
        const long accNumSeed = 301610000;     //the initial account number starting from 0 for the last digit
        long accNum;
        static int cusCount;

        /// <summary>
        /// AccNumGenerator function generates account number for new customers by geting the latest account number from CurrAccNum.txt
        /// and add 1 on it. 
        /// </summary>
        public long AccNumGenerator()
        {
            long currAccNum; 
            string accNumStr;
            currAccNum = long.Parse(File.ReadAllText("CurrAccNum.txt"));  //get latest accNum from CurrAccNum.txt and parse it to long
            accNum = currAccNum + 1;                                      //new accNum = latest accNum + 1
            accNumStr = accNum.ToString();                                //change new accNum to string
            File.WriteAllText("CurrAccNum.txt", accNumStr);               //overwrite new accNum to CurrAccNum.txt for later use
            File.AppendAllText("AccNum.txt", accNumStr + "\n");           //append the new accNum to AccNum.txt accNum collection
            return accNum;
        }

        /// <summary>
        /// CusCount function counts the total number of customers and re-write it to CusCount.txt when a new account is opened
        /// </summary>
        public void CusCount()
        {
            int currCusCount = int.Parse(File.ReadAllText("CusCount.txt")); //get latest cusCount from CusCount.txt
            cusCount = currCusCount + 1;                                    
            File.WriteAllText("CusCount.txt", cusCount.ToString());         //write the new cusCount
        }

        /// <summary>
        /// AccessAccount function allow users to open a new account or to access their account to make deposit/withdraw 
        /// </summary>
        public static void AccessOrOpenAccount()
        {
            Account user = new Account();
            Console.WriteLine("Welcome to my bank!\n" + "Enter \"open\" for opening a new account.\n" + "Enter \"deposit\" for making a deposit.\n" + "Enter \"withdraw\" for making a withdraw.");           
            string enter = Console.ReadLine();
            if (enter == "open")                                       //call OpenAccount function when user enter "open"
            {
                user.OpenAccount();
                //return;
            }
            else if (enter == "deposit" || enter == "withdraw")
            {
                user.LogIn();                                          //Call LogIn function to validate username/password, and get accNum
                while (enter == "deposit")                             //if a user enter "deposit" call deposit function
                {
                    user.MakeDeposit();
                    enter = Console.ReadLine();
                }
                while (enter == "withdraw")                            //if a user enter "withdraw" call withdraw function, for other entered values, get out the while loop
                {
                    user.MakeWithdraw();
                    enter = Console.ReadLine();
                }
            }            
        }
    }
}
