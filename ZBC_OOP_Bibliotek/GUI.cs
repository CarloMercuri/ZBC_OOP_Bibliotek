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
            ConsoleTools.SetWarningOptions(2, 11, 50);

            _logic = new BibliotekLogic();
            _logic.InitializeBibliotek(this);

            DrawSelectionMenu();

            _logic.StartMainLogic();
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
            ConsoleTools.PrintArray(titleAscii, 50, 1, null, ConsoleColor.White);
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

            ConsoleTools.PrintLine("Select book(s) you wish to loan: ", 2, 13, ConsoleColor.White);

            // After printing the array, we are going to place the cursor at the end of the previous printline
            (int Left, int Top) curLoc = Console.GetCursorPosition();

            // Print the array            
            ConsoleTools.PrintArray(books, 2, 15, null, ConsoleColor.White);

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
    }
}
