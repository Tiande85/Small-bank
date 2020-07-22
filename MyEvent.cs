using System;
using System.Collections.Generic;
using System.Text;

namespace Small_bank
{
    /// <summary>
    /// MyEvent class has two simple function associated with two events, raisen when proper signal is sent
    /// </summary>
    class MyEvent
    { 
        public void ChangeCurrentBal(string name, string tranType, int amount)  //associated with ValueEvent, raisen after customers enter an withdraw/deposit ammount
        {
            Console.WriteLine($"{name} made a {tranType} of ${amount}");
        }

        public void OutputAccBal(string name, int currbal)
        {
            Console.WriteLine($"{name}, your current balance : {currbal}");    // associated with OutputEvent, raisen if user make non zero deposit/withdraw
        }
    }
}
