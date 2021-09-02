using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Bibliotek
{
    public static class BibliotekLogic
    {
        private static List<Book> aviableBooks;
        private static Stack<Book> userChosenBooks;

        // Outside set is not allowed
        public static List<Book> AviableBooks
        {
            get { return aviableBooks; }
        }

        // Outside set is not allowed
        public static Stack<Book> UserChosenBooks
        {
            get { return userChosenBooks; }
        }

        // Current state of the logic
        private static BibliotekStatus CurrentState {get; set; }

        /// <summary>
        /// Initial settings
        /// </summary>
        public static void InitializeBibliotek()
        {
            CurrentState = BibliotekStatus.ChoosingFromLoan;

            aviableBooks = new List<Book>();
            userChosenBooks = new Stack<Book>();

            // Fill up the list

            aviableBooks.Add(new Book("Isaac Asimov", "Prelude to Foundation", "1988"));
            aviableBooks.Add(new Book("Isaac Asimov", "Foundation", "1951"));
            aviableBooks.Add(new Book("Isaac Asimov", "Foundation and Empire", "1952"));
            aviableBooks.Add(new Book("Isaac Asimov", "Second Foundation", "1953"));
            aviableBooks.Add(new Book("Isaac Asimov", "Foundation's Edge", "1982"));
            aviableBooks.Add(new Book("Isaac Asimov", "Foundation and Earth", "1986"));
            aviableBooks.Add(new Book("Isaac Asimov", "I, Robot", "1950"));
            aviableBooks.Add(new Book("James Dashner", "The Maze Runner", "2009"));
            aviableBooks.Add(new Book("Harry Harrison", "Death World", "1964"));
            aviableBooks.Add(new Book("Roger MacBride Allen", "The Ring of Charon", "1990"));
            aviableBooks.Add(new Book("Roger MacBride Allen", "The Shattered Sphere", "1995"));
        }

        /// <summary>
        /// Start the main logic
        /// </summary>
        public static void StartMainLogic()
        {
            // It's going to be sequential

            // Choosing loan

            while(CurrentState == BibliotekStatus.ChoosingFromLoan)
            {
                // Get an input
                string userInput = Console.ReadLine();


                if(userInput.ToLower().Equals("f"))
                {
                    // If it's possible, go to checkout
                    if(userChosenBooks.Count > 0)
                    {
                        // By doing this, we skip this while loop
                        CurrentState = BibliotekStatus.AtCheckout;
                        continue;
                    } else
                    {
                        ConsoleTools.ShowWarning("You didn't choose any book yet!", ConsoleColor.Red);
                    }
                }

                // Back to top of while loop if the input is not valid
                if (!ConsoleTools.IsInputOnlyDigits(userInput) || userInput == "")
                {
                    // Redrawing the whole thing to clear the user input. Lazy way
                    GUI.DrawSelectionMenu();
                    continue;
                }

                int index = Int32.Parse(userInput);

                // We incremented the displayed index in the GUI by 1 so it didn't start at 0,
                // so we need to decrease it here to compensate
                index--;

                // Ofc
                if (index < 0) index = 0;
               

                // If it's valid in the list
                if(aviableBooks.Count > index)
                {
                    LoanBook(index);
                }

                // Update visuals
                GUI.DrawLoanedStack();
                GUI.DrawSelectionMenu();

                // If it was the last item, set the state to checkout and skip while loop
                if(aviableBooks.Count <= 0)
                {
                    CurrentState = BibliotekStatus.AtCheckout;
                    continue;
                }
               
            }

            // CHECKOUT
            GUI.ClearSelectionMenu();

            do
            {
                // Clearing the previous line
                ConsoleTools.PrintLine($"                                                                              ", 2, 13, ConsoleColor.White);

                // Asking if they want to loan the next book in like with Peek()
                ConsoleTools.PrintLine($"Do you want to loan {BibliotekLogic.userChosenBooks.Peek().ToString()}? y/n", 2, 13, ConsoleColor.White);

                // Get the user to choose yes or not
                ConsoleKey key = ConsoleTools.GetUserChoice(ConsoleKey.Y, ConsoleKey.N, false);

                ConsoleTools.PrintLine("                                                             ", 2, 15, ConsoleColor.White);

                // going to pop it regardless if the user wants to loan it or not
                Book b = userChosenBooks.Pop();

                if(key == ConsoleKey.Y)
                {
                    ConsoleTools.PrintLine($"Loaned {b.ToString()} !", 2, 15, ConsoleColor.White);

                } else
                {
                    ConsoleTools.PrintLine($"Discarded {b.ToString()} !", 2, 15, ConsoleColor.White);
                }

                GUI.DrawLoanedStack();
                


            } while (userChosenBooks.Count > 0); // Quit when there's no more left

            Console.ReadKey();

        }

        /// <summary>
        /// Removes the book from the aviable list and pushes it into the chosen stack
        /// </summary>
        /// <param name="index"></param>
        public static void LoanBook(int index)
        {
            // shouldn't happen, but hey
            if(aviableBooks[index] == null)
            {
                return;
            }

            userChosenBooks.Push(aviableBooks[index]);

            aviableBooks.RemoveAt(index);
        }
    }
}
