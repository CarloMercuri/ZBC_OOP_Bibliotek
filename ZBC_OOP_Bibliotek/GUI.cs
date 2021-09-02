using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Bibliotek
{
    public static class GUI
    {
        private static string[] titleAscii;


        /// <summary>
        /// Sets up the gui
        /// </summary>
        public static void InitializeGUI()
        {
            InitializeAsciis();
            DrawTitle();
            ConsoleTools.SetWarningOptions(2, 11, 50);
        }

        /// <summary>
        /// Initializes the asciis for this program
        /// </summary>
        private static void InitializeAsciis()
        {
            titleAscii = new string[]
            {
                @"   ______           __      __       __  ",
                @"  / ____/___ ______/ /___  / /____  / /__",
                @" / /   / __ `/ ___/ / __ \/ __/ _ \/ //_/",
                @"/ /___/ /_/ / /  / / /_/ / /_/  __/ ,<   ",
                @"\____/\__,_/_/  /_/\____/\__/\___/_/|_|  ",
            };
        }


        /// <summary>
        /// Draws the Carlotek title
        /// </summary>
        private static void DrawTitle()
        {
            ConsoleTools.PrintArray(titleAscii, 50, 1, null, ConsoleColor.White);
        }

        /// <summary>
        /// Clears the menu
        /// </summary>
        public static void ClearSelectionMenu()
        {
            for (int i = 13; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("                                                          ");
            }
        }

        /// <summary>
        /// Clears the list of the loaned objects
        /// </summary>
        public static void ClearLoanedStack()
        {
            int y = 15;

            Console.SetCursorPosition(90, y);

            for (int i = 0; i < 24; i++)
            {
                Console.Write("                                                                               ");
                Console.SetCursorPosition(90, y + i);
            }
        }

        /// <summary>
        /// Draws the loaned stack
        /// </summary>
        public static void DrawLoanedStack()
        {
            Console.SetCursorPosition(90, 12);
            Console.Write("Books you want to loan:");


            ClearLoanedStack();

            int y = 26 - BibliotekLogic.UserChosenBooks.Count; // last item always at line 26, so we start accordingly
            int x = 90;

            Console.SetCursorPosition(x, y);

            // Print them downwards
            foreach(Book b in BibliotekLogic.UserChosenBooks)
            {
                Console.Write(b.ToString());
                y++;
                Console.SetCursorPosition(x, y);
            }
        }

        /// <summary>
        /// Draws the aviable books selection
        /// </summary>
        public static void DrawSelectionMenu()
        {
            // Clear it every time
            ClearSelectionMenu();

            // Copy it to an array
            string[] books = new string[BibliotekLogic.AviableBooks.Count];

            for (int i = 0; i < BibliotekLogic.AviableBooks.Count; i++)
            {
                // Add an index to the left STARTING AT 1. Have to offset it later when selecting it
                string newName = BibliotekLogic.AviableBooks[i].ToString().Insert(0, $"{i + 1} - ");
                books[i] = newName;
            }

            ConsoleTools.PrintLine("Select book(s) you wish to loan: ", 2, 13, ConsoleColor.White);

            // After printing the array, we are going to place the cursor at the end of the previous printline
            (int Left, int Top) curLoc = Console.GetCursorPosition();

            // Print the array            
            ConsoleTools.PrintArray(books, 2, 15, null, ConsoleColor.White);

            // Place the cursor as explained before
            Console.SetCursorPosition(curLoc.Left + 1, curLoc.Top);

        }
    }
}
