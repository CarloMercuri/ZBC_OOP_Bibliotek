using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Bibliotek
{
    public class GUI
    {

        // Console size hack, makes it so you cannot resize it

        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        private string[] titleAscii;

        private BibliotekLogic _logic;

        // warnings
        private int warningX = 0;
        private int warningY = 0;
        private int warningMaxLenght = 20;
        private int warningLastLenght = 0;


        /// <summary>
        /// Sets up the gui
        /// </summary>
        public void InitializeGUI()
        {
            // Size and lock the console. No scrolling, no resizing
            Console.SetWindowSize(180, 40);
            Console.SetBufferSize(180, 40);
            Console.Title = "Bibliotek";
            LockConsole();

            // UI Initialization
            InitializeAsciis();
            DrawTitle();
            SetWarningOptions(2, 11, 50);

            _logic = new BibliotekLogic();
            _logic.InitializeBibliotek();

            DrawSelectionMenu();

            StartMainLogic();
        }

        /// <summary>
        /// Start the main logic
        /// </summary>
        public void StartMainLogic()
        {
            // It's going to be sequential

            // Choosing loan

            while (_logic.CurrentState == BibliotekStatus.ChoosingFromLoan)
            {
                // Get an input
                string userInput = Console.ReadLine();


                if (userInput.ToLower().Equals("f"))
                {
                    // If it's possible, go to checkout
                    if (_logic.GetUserStackCount() > 0)
                    {
                        // By doing this, we skip this while loop
                        _logic.CurrentState = BibliotekStatus.AtCheckout;
                        continue;
                    }
                    else
                    {
                        ShowWarning("You didn't choose any book yet!", ConsoleColor.Red);
                    }
                }

                // Back to top of while loop if the input is not valid
                if (!IsInputOnlyDigits(userInput) || userInput == "")
                {
                    // Redrawing the whole thing to clear the user input. Lazy way
                    DrawSelectionMenu();
                    continue;
                }

                int index = Int32.Parse(userInput);

                // We incremented the displayed index in the GUI by 1 so it didn't start at 0,
                // so we need to decrease it here to compensate
                index--;

                // Ofc
                if (index < 0) index = 0;


                // If it's valid in the list
                if (_logic.GetAvailableBooksCount() > index)
                {
                    _logic.LoanBook(index);
                }

                // Update visuals
                DrawLoanedStack();
                DrawSelectionMenu();

                // If it was the last item, set the state to checkout and skip while loop
                if (_logic.GetAvailableBooksCount() <= 0)
                {
                    _logic.CurrentState = BibliotekStatus.AtCheckout;
                    continue;
                }

            }

            // CHECKOUT
            ClearSelectionMenu();

            do
            {
                // Clearing the previous line
                PrintLine($"                                                                              ", 2, 13, ConsoleColor.White);

                // Asking if they want to loan the next book in like with Peek()
                PrintLine($"Do you want to loan {_logic.PeekAtStack().ToString()}? y/n", 2, 13, ConsoleColor.White);

                // Get the user to choose yes or not
                ConsoleKey key = GetUserChoice(ConsoleKey.Y, ConsoleKey.N, false);

                PrintLine("                                                             ", 2, 15, ConsoleColor.White);

                // going to pop it regardless if the user wants to loan it or not
                Book b = _logic.PopFromStack();

                if (key == ConsoleKey.Y)
                {
                    PrintLine($"Loaned {b.ToString()} !", 2, 15, ConsoleColor.White);

                }
                else
                {
                    PrintLine($"Discarded {b.ToString()} !", 2, 15, ConsoleColor.White);
                }

                DrawLoanedStack();



            } while (_logic.GetUserStackCount() > 0); // Quit when there's no more left

            Console.ReadKey();

        }

        /// <summary>
        /// Initializes the asciis for this program
        /// </summary>
        private void InitializeAsciis()
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
        private void DrawTitle()
        {
            PrintArray(titleAscii, 50, 1, null, ConsoleColor.White);
        }

        /// <summary>
        /// Clears the menu
        /// </summary>
        public void ClearSelectionMenu()
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
        public void ClearLoanedStack()
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
        public void DrawLoanedStack()
        {
            Console.SetCursorPosition(90, 12);
            Console.Write("Books you want to loan:");


            ClearLoanedStack();

            int y = 26 - _logic.GetUserStackCount(); // last item always at line 26, so we start accordingly
            int x = 90;

            Console.SetCursorPosition(x, y);

            // Print them downwards
            foreach(Book b in _logic.GetUserStack())
            {
                Console.Write(b.ToString());
                y++;
                Console.SetCursorPosition(x, y);
            }
        }

        /// <summary>
        /// Draws the aviable books selection
        /// </summary>
        public void DrawSelectionMenu()
        {
            // Clear it every time
            ClearSelectionMenu();

            int availableCount = _logic.GetAvailableBooksCount();

            // Copy it to an array
            string[] books = new string[availableCount];

            for (int i = 0; i < availableCount; i++)
            {
                // Add an index to the left STARTING AT 1. Have to offset it later when selecting it
                //string newName = _logic.AviableBooks[i].ToString().Insert(0, $"{i + 1} - ");
                string newName = _logic.GetAvailableBookAtIndex(i).ToString().Insert(0, $"{i + 1} - ");
                books[i] = newName;
            }

            PrintLine("Select book(s) you wish to loan: ", 2, 13, ConsoleColor.White);

            // After printing the array, we are going to place the cursor at the end of the previous printline
            (int Left, int Top) curLoc = Console.GetCursorPosition();

            // Print the array            
            PrintArray(books, 2, 15, null, ConsoleColor.White);

            // Place the cursor as explained before
            Console.SetCursorPosition(curLoc.Left + 1, curLoc.Top);

        }

        /// <summary>
        /// Makes it so you cannot resize or maximize it
        /// </summary>
        private void LockConsole()
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
        }

        /////////////////     TOOLS   /////////////////////
        ///

        /// <summary>
        /// Asks the user to choose between two keys, and keeps asking until the input is valid
        /// </summary>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ConsoleKey GetUserChoice(ConsoleKey k1, ConsoleKey k2, bool displayError, string message = "")
        {
            while (true)
            {
                if (message != "")
                {
                    Console.WriteLine(message);
                }


                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == k1 || key.Key == k2)
                {
                    return key.Key;
                }
                else
                {
                    if (displayError)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid choice.");
                    }
                }
            }
        }

        /// <summary>
        /// Clears the last warning
        /// </summary>
        public void ClearWarning()
        {
            Console.SetCursorPosition(warningX, warningY);
            // Create an empty string of the lenght of the last warning
            string clearString = new string(' ', warningLastLenght);
            Console.Write(clearString);
        }

        public void ShowWarning(string warning, ConsoleColor color)
        {
            ClearWarning();
            Console.SetCursorPosition(warningX, warningY);

            // Enforce max lenght
            if (warning.Length > warningMaxLenght)
            {
                warning = warning.Substring(0, warningMaxLenght);
            }
            Console.ForegroundColor = color;
            Console.Write(warning);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void SetWarningOptions(int location_x, int location_y, int maxLenght)
        {
            warningX = location_x;
            warningY = location_y;
            warningMaxLenght = maxLenght;
        }

        /// <summary>
        /// Asks the user to input a double, and keeps asking until the input is valid
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="maxDecimals"></param>
        /// <returns></returns>
        public double GetUserInputDouble(string phrase, int maxDecimals = -1)
        {
            string userInput = "";

            while (true)
            {
                Console.WriteLine(phrase);


                userInput = Console.ReadLine();

                // Empty input (only pressed enter for example)
                if (userInput.Length <= 0)
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }

                // Check that it only contains numbers
                if (!IsInputDouble(userInput))
                {
                    Console.WriteLine("Invalid input: can only contain numbers and one comma or dot");
                    continue;
                }
                else
                {
                    break;
                }

                // If asked to, removes excess decimals
                if (maxDecimals != -1)
                {
                    // Count the numbers after the comma.
                    int dIndex = userInput.IndexOf(',');

                    if (dIndex >= 0)
                    {
                        // Limit to a few decimals, first of all its pointless, second also avoid
                        // too big numbers and consequent errors
                        if (userInput.Length - dIndex - 1 > 5)
                        {
                            // Just fix it instead of giving error
                            userInput = userInput.Remove(dIndex + 6);
                        }
                    }
                }


            }

            return double.Parse(userInput);
        }

        /// <summary>
        /// Prints a single line to the specified location, with the specified color
        /// </summary>
        /// <param name="line"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void PrintLine(string line, int x, int y, ConsoleColor color)
        {
            if (color == ConsoleColor.Black)
            {
                color = ConsoleColor.White;
            }

            Console.ForegroundColor = color;

            Console.SetCursorPosition(x, y);

            Console.Write(line);
        }


        /// <summary>
        /// Prints the given string array to the console in the specified location, skipping over eventual blacklisted characters. Blacklist can be null.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="blacklist"></param>
        public void PrintArray(string[] array, int x, int y, List<char> blacklist, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            if (color == ConsoleColor.Black)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            for (int i = 0; i < array.Length; i++)
            {
                Console.SetCursorPosition(x, y + i);

                for (int j = 0; j < array[i].Length; j++)
                {
                    if (blacklist != null && blacklist.Contains(array[i][j]))
                    {
                        // If we need to skip this, we'll increment the cursor position manually by 1
                        var pos = Console.GetCursorPosition();

                        Console.SetCursorPosition(pos.Left + 1, pos.Top);
                    }
                    else
                    {
                        Console.Write(array[i][j]);
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Gray;

        }

        /// <summary>
        /// Checks that the input conforms to a Double
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool IsInputDouble(string input)
        {
            input = input.Replace('.', ',');

            foreach (char c in input)
            {
                // Also accept commas
                if (c == ',')
                {
                    continue;
                }

                // check that it's a number (unicode)
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Requests the user to enter an integer with the corresponding request string, and
        /// makes sure the input is sanitized
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public int GetUserInputInteger(string phrase = "")
        {
            string userInput = "";

            while (true)
            {
                if (phrase != "")
                {
                    Console.WriteLine(phrase);
                }

                userInput = Console.ReadLine();

                // Empty input (only pressed enter for example)
                if (userInput.Length <= 0)
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }

                // Check that it only contains numbers
                if (!IsInputOnlyDigits(userInput))
                {
                    Console.WriteLine("Invalid input: must only contain numbers");
                    continue;
                }
                else
                {
                    break;
                }
            }

            return int.Parse(userInput);
        }

        /// <summary>
        /// Returns true if the string only contains digits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsInputOnlyDigits(string input)
        {
            foreach (char c in input)
            {
                // check that it's a number (unicode)
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

    }
}
