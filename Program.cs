using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

namespace Small_bank
{
    class Program
    {
        static void Main(string[] args)
        {
            Bank.AccessOrOpenAccount();
        }

        /// <summary>
        ///RunSourceData function can not only be run once. because if runing more than once, 
        ///all passwords and usernames will be loaded twice, which will cause dictonary out of index!
        ///You can also open an account without running it.
        /// </summary>
        static void RunSourceDate()
        {
            Account Jack = new Account("Tiande", "Wu", "adewutiande", "Adewu66310571!", 50000);
            Account John = new Account("John", "McLaren", "Mj1234", "Mj66310571!", 40000);
            Account Kavin = new Account("Han", "Carry", "bh11111", "Bh66310571!", 20000);
            Account Mike = new Account("Mike", "Tyson", "mk11111", "Mks11111111!", 200000);

            Jack.WriteAccInfo();
            John.WriteAccInfo();
            Kavin.WriteAccInfo();
            Mike.WriteAccInfo();
        }
    }    
}
