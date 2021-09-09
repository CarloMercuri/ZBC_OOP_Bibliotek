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

        private BibliotekStatus currentState;

        public BibliotekStatus CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }


        /// <summary>
        /// Initial settings
        /// </summary>
        public void InitializeBibliotek()
        {
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

        public Book PeekAtStack()
        {
            return _data.PeekAtStack();
        }

        /// <summary>
        /// Pops a book from the user stack
        /// </summary>
        /// <returns></returns>
        public Book PopFromStack()
        {
            return _data.PopFromStack();
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
