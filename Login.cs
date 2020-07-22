using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Small_bank
{
    /// <summary>
    /// Login class provides users functions to login to their account 
    /// </summary>
    class Login
    {
        Dictionary<string, string> UnAcc = new Dictionary<string, string>();  //a dictionary consisting of username-accountNum pairs
        Dictionary<string, string> PwAcc = new Dictionary<string, string>();  //a dictionary consisting of password-accountNum pairs
        
        private string[] username = File.ReadAllLines("AllUsername.txt");     //a string array store all customers' usernames
        private string[] password = File.ReadAllLines("AllPasswords.txt");    //a string array store all customers' passwords  
        private string[] accNum = File.ReadAllLines("AccNum.txt");            //a string array store all account numbers

        private string userAccNum;

        /// <summary>
        /// LoadDicts function load username-accNum pairs and password-accNum pairs to their relevent dictionary
        /// </summary>
        public void LoadDicts()     
        {
            for (int i = 0; i < accNum.Length; i++)
            {
                UnAcc.Add(username[i], accNum[i]);
                PwAcc.Add(password[i], accNum[i]);
            }
        }

        /// <summary>
        /// LogIn function allow user to input their username/password and matchs with password-accNum and username-accNum pairs
        /// in two dictionaries, if the two matched account number are equal, return the matched account number.
        /// Otherwise, asking for username/password again.
        /// </summary>
        /// <returns></returns>
        public string LogIn()
        {
            Console.Write("Username : ");
            string inputUsername = Console.ReadLine();
            Console.Write("Password : ");
            string inputPassword = Console.ReadLine();
            Console.WriteLine();
            LoadDicts();                                                                       //load the two dictionaries
            while ((!UnAcc.ContainsKey(inputUsername) || !PwAcc.ContainsKey(inputPassword)) || //if user input username/password doesn't exist or the two matched accNum are different, asking for new input
                UnAcc[inputUsername] != PwAcc[inputPassword])
            {
                Console.Write("Re-enter Username : ");
                inputUsername = Console.ReadLine();
                Console.Write("Re-enter Password : ");
                inputPassword = Console.ReadLine();
                Console.WriteLine();
            }
            userAccNum = PwAcc[inputPassword];
            return userAccNum;      //return the matched account number
        }
    }
}
