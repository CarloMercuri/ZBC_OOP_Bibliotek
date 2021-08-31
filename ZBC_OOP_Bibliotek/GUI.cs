using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Bibliotek
{
    public static class GUI
    {
        private static string[] bookAsciiLeft;
        private static string[] bookAsciiRight;

        private static int pilesStartY = 15;

        private static int selectionPileStartX = 5;
        private static bool selectionPileIsEven;

        private static int basketPileStartX = 12;
        private static bool basketPileIsEven;

        public static void InitializeGUI()
        {
            InitializeAsciis();
        }

        private static void InitializeAsciis()
        {
            bookAsciiLeft = new string[]
            {
                @"   _____",
                @"  /    /|",
                @" /    //",
                @"(====|/",
            };

            bookAsciiRight = new string[]
            {
                @"   ______",
                @"  /     /|",
                @" /     //",
                @"(=====|/",
            };
        }

        public static void ClearSelectionPile()
        {

        }

        public static void DrawSelectionPile(List<Book> booksList)
        {
            ClearSelectionPile();
            selectionPileIsEven = false;

            for (int i = 0; i < booksList.Count; i++)
            {
                DrawBookOnSelectionPile(booksList[i], i);
                selectionPileIsEven = !selectionPileIsEven;
            }
        }

        private static void DrawBookOnSelectionPile(Book book, int index)
        {
            int x = selectionPileStartX;

            if (!selectionPileIsEven)
            {
                ConsoleTools.PrintArray(bookAsciiLeft, selectionPileStartX, pilesStartY - index, null, ConsoleColor.White);
            } 
            else
            {
                ConsoleTools.PrintArray(bookAsciiRight, selectionPileStartX + 1, pilesStartY - index, null, ConsoleColor.White);
            }

            
        }
    }
}
