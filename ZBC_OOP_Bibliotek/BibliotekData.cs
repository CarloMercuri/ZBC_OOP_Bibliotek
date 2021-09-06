using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Bibliotek
{
    public class BibliotekData
    {
        private List<Book> availableBooks;
        private Stack<Book> userChosenBooks;

        // Outside set is not allowed
        public List<Book> AvailableBooks
        {
            get { return availableBooks; }
        }

        // Outside set is not allowed
        public Stack<Book> UserChosenBooks
        {
            get { return userChosenBooks; }
        }

        /// <summary>
        /// Returns the count of the books available for loan
        /// </summary>
        /// <returns></returns>
        public int GetAvailableBooksCount()
        {
            return availableBooks.Count;
        }

        /// <summary>
        /// Removes a book from the available list at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAvailableAtIndex(int index)
        {
            availableBooks.RemoveAt(index);
        }

        /// <summary>
        /// Pushes the specified book to the user loan stack
        /// </summary>
        /// <param name="book"></param>
        public void PushToUserStack(Book book)
        {
            userChosenBooks.Push(book);
        }

        /// <summary>
        /// Returns a book from the available list at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Book GetAvailableBookAtIndex(int index)
        {
            return availableBooks[index];
        }

        /// <summary>
        /// Pops a book from the user stack
        /// </summary>
        /// <returns></returns>
        public Book PopFromStack()
        {
            return userChosenBooks.Pop();
        }

        /// <summary>
        /// Peeks at the user stack
        /// </summary>
        /// <returns></returns>
        public Book PeekAtStack()
        {
            return userChosenBooks.Peek();
        }

        /// <summary>
        /// Gets the number of items contained in the user stack.
        /// </summary>
        /// <returns></returns>
        public int GetUserStackCount()
        {
            return userChosenBooks.Count;
        }

        public BibliotekData()
        {
            availableBooks = new List<Book>();
            userChosenBooks = new Stack<Book>();
        }

        /// <summary>
        /// Adds a book to the available list.
        /// </summary>
        /// <param name="book"></param>
        public void AddBookToAvailableList(Book book)
        {
            availableBooks.Add(book);
        }
    }
}
