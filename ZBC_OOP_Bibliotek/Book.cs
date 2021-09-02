using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_OOP_Bibliotek
{
    public class Book
    {
        private string authorName;

        public string AuthorName
        {
            get { return authorName; }
            set { authorName = value; }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string publishDate;

        public string PublishDate
        {
            get { return publishDate; }
            set { publishDate = value; }
        }


        private DateTime loanExpiration;

        public DateTime LoanExpiration
        {
            get { return loanExpiration; }
            set { loanExpiration = value; }
        }


        public Book(string _author, string _title, string _publishDate)
        {
            authorName = _author;
            title = _title;
            publishDate = _publishDate;
        }

        public Book()
        {

        }

        public override string ToString()
        {
            return authorName + " - " + title;
        }


    }
}
