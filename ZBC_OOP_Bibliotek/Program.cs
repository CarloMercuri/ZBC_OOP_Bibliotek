using System;
using System.Collections.Generic;

namespace ZBC_OOP_Bibliotek
{
    class Program
    {
        static void Main(string[] args)
        {
            GUI.InitializeGUI();

            List<Book> list = new List<Book>();

            list.Add(new Book());
            list.Add(new Book());
            list.Add(new Book());
            list.Add(new Book());
            list.Add(new Book());
            list.Add(new Book());
            list.Add(new Book());   

            GUI.DrawSelectionPile(list);

           Console.ReadKey();
        }
    }
}


