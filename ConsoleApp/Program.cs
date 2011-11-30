using System;
using System.Collections.Generic;
using TicketDomain;
using WorkflowRulesEngine;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            var rulesEngine = new RulesEngine<TicketHolder>();
            var ruleSet = rulesEngine.LoadRuleSetFromFile();

            Console.WriteLine("Press space to modify rules, otherwise press any key to execute the rules...");

            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                ruleSet = rulesEngine.LaunchRulesDialog(ruleSet);
            }

            var ticketHolders = GetTicketHolders();

            Console.WriteLine("Before the rules are applied");
            DisplayTicketHolders(ticketHolders);

            ticketHolders.ForEach(ticketHolder => rulesEngine.ProcessRuleSet(ticketHolder, ruleSet));

            Console.WriteLine("After the rules are applied");
            DisplayTicketHolders(ticketHolders);

            Console.ReadKey();
        }

        private static void DisplayTicketHolders(List<TicketHolder> ticketHolders)
        {
            ticketHolders.ForEach(ticketHolder =>
                                      {
                                          var message = string.Format(
                                                                      "Age: {0}, Fare: ${1}, Is Employee: {2}",
                                                                      GetAge(ticketHolder.DateOfBirth),
                                                                      ticketHolder.Fare,
                                                                      ticketHolder.IsEmployee ? "Yes" : "No");
                                          Console.WriteLine(message);
                                      });
        }

        private static int GetAge(DateTime dateOfBirth)
        {
            var now = DateTime.Today;
            var age = now.Year - dateOfBirth.Year;

            if (dateOfBirth > now.AddYears(-age)) 
                age--;
            return age;
        }

        private static List<TicketHolder> GetTicketHolders()
        {
            return new List<TicketHolder>
                       {
                           new TicketHolder
                               {
                                   DateOfBirth = DateTime.Today.AddYears(-72),
                                   IsEmployee = true
                               },
                           new TicketHolder
                               {
                                   DateOfBirth = DateTime.Today.AddYears(-16)
                               },
                           new TicketHolder
                               {
                                   DateOfBirth = DateTime.Today.AddYears(-66)
                               },
                           new TicketHolder
                               {
                                   DateOfBirth = DateTime.Today.AddYears(-20),
                               },
                           new TicketHolder
                               {
                                   DateOfBirth = DateTime.Today.AddYears(-20),
                                   IsEmployee = true
                               },
                           new TicketHolder
                               {
                                   DateOfBirth = DateTime.Today.AddYears(-17),
                                   IsEmployee = true
                               },
                           new TicketHolder
                               {
                                   DateOfBirth = DateTime.Today.AddYears(-32),
                                   IsEmployee = true
                               }
                       };
        }
    }
}
