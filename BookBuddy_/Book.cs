using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBuddy_
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }

        public Book(int id, string title, string author, string genre, string description, bool isRead)
        {
            Id = id;
            Title = title;
            Author = author;
            Genre = genre;
            Description = description;
            IsRead = isRead;
        }

        public Book(string title, string author, string genre, string description)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Description = description;
            IsRead = false;
        }
    }
}
