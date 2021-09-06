using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Bibliotek
{
    public class BibliotekLogic
    {
        BibliotekData _data;
        GUI _gui;

        // Current state of the logic
        private BibliotekStatus CurrentState {get; set; }

        /// <summary>
        /// Initial settings
        /// </summary>
        public void InitializeBibliotek(GUI gui)
        {
            _gui = gui;
            _data = new BibliotekData();
            CurrentState = BibliotekStatus.ChoosingFromLoan;

            // Fill up the list

            _data.AddBookToAvailableList(new Book("Isaac Asimov", "Prelude to Foundation", "1988"));
            _data.AddBookToAvailableList(new Book("Isaac Asimov", "Foundation", "1951"));
            _data.AddBookToAvailableList(new Book("Isaac Asimov", "Foundation and Empire", "1952"));
            _data.AddBookToAvailableList(new Book("Isaac Asimov", "Second Foundation", "1953"));
            _data.AddBookToAvailableList(new Book("Isaac Asimov", "Foundation's Edge", "1982"));
            _data.AddBookToAvailableList(new Book("Isaac Asimov", "Foundation and Earth", "1986"));
            _data.AddBookToAvailableList(new Book("Isaac Asimov", "I, Robot", "1950"));
            _data.AddBookToAvailableList(new Book("James Dashner", "The Maze Runner", "2009"));
            _data.AddBookToAvailableList(new Book("Harry Harrison", "Death World", "1964"));
            _data.AddBookToAvailableList(new Book("Roger MacBride Allen", "The Ring of Charon", "1990"));
            _data.AddBookToAvailableList(new Book("Roger MacBride Allen", "The Shattered Sphere", "1995"));
        }

        /// <summary>
        /// Returns the number of books in the user stack
        /// </summary>
        /// <returns></returns>
        public int GetUserStackCount()
        {
            return _data.GetUserStackCount();
        }

        /// <summary>
        /// Returns the number of books in the available list
        /// </summary>
        /// <returns></returns>
        public int GetAvailableBooksCount()
        {
            return _data.GetAvailableBooksCount();
        }

        /// <summary>
        /// Returns a book at the specified index from the available list
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Book GetAvailableBookAtIndex(int index)
        {
            return _data.AvailableBooks[index];
        }

        /// <summary>
        /// Returns the user stack
        /// </summary>
        /// <returns></returns>
        public Stack<Book> GetUserStack()
        {
            return _data.UserChosenBooks;
        }

        /// <summary>
        /// Start the main logic
        /// </summary>
        public void StartMainLogic()
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
                    if(_data.GetUserStackCount() > 0)
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
                    _gui.DrawSelectionMenu();
                    continue;
                }

                int index = Int32.Parse(userInput);

                // We incremented the displayed index in the GUI by 1 so it didn't start at 0,
                // so we need to decrease it here to compensate
                index--;

                // Ofc
                if (index < 0) index = 0;
               

                // If it's valid in the list
                if(_data.GetAvailableBooksCount() > index)
                {
                    LoanBook(index);
                }

                // Update visuals
                _gui.DrawLoanedStack();
                _gui.DrawSelectionMenu();

                // If it was the last item, set the state to checkout and skip while loop
                if(_data.GetAvailableBooksCount() <= 0)
                {
                    CurrentState = BibliotekStatus.AtCheckout;
                    continue;
                }
               
            }

            // CHECKOUT
            _gui.ClearSelectionMenu();

            do
            {
                // Clearing the previous line
                ConsoleTools.PrintLine($"                                                                              ", 2, 13, ConsoleColor.White);

                // Asking if they want to loan the next book in like with Peek()
                ConsoleTools.PrintLine($"Do you want to loan {_data.PeekAtStack().ToString()}? y/n", 2, 13, ConsoleColor.White);

                // Get the user to choose yes or not
                ConsoleKey key = ConsoleTools.GetUserChoice(ConsoleKey.Y, ConsoleKey.N, false);

                ConsoleTools.PrintLine("                                                             ", 2, 15, ConsoleColor.White);

                // going to pop it regardless if the user wants to loan it or not
                Book b = _data.PopFromStack();

                if(key == ConsoleKey.Y)
                {
                    ConsoleTools.PrintLine($"Loaned {b.ToString()} !", 2, 15, ConsoleColor.White);

                } else
                {
                    ConsoleTools.PrintLine($"Discarded {b.ToString()} !", 2, 15, ConsoleColor.White);
                }

                _gui.DrawLoanedStack();
                


            } while (_data.GetUserStackCount() > 0); // Quit when there's no more left

            Console.ReadKey();

        }

        /// <summary>
        /// Removes the book from the aviable list and pushes it into the chosen stack
        /// </summary>
        /// <param name="index"></param>
        public void LoanBook(int index)
        {
            _data.PushToUserStack(_data.GetAvailableBookAtIndex(index));
            _data.RemoveAvailableAtIndex(index);
        }
    }
}
