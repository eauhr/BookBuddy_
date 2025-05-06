using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace BookBuddy_
{
    public static class DatabaseHelper
    {
        private const string DbFile = "books.db";

        public static void InitializeDatabase()
        {
            if (!File.Exists(DbFile))
            {
                SQLiteConnection.CreateFile(DbFile);
            }

            using (var connection = new SQLiteConnection($"Data Source={DbFile};Version=3;"))
            {
                connection.Open();
                string createTable = @"
                 CREATE TABLE IF NOT EXISTS Books (
                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                 Title TEXT NOT NULL,
                 Author TEXT NOT NULL,
                 Genre TEXT NOT NULL,
                 Description TEXT,
                 IsRead INTEGER NOT NULL DEFAULT 0
                 );";
                using (var command = new SQLiteCommand(createTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand cmdCount = new SQLiteCommand("SELECT COUNT(*) FROM Books;", connection)) 
                {
                    long count = (long)cmdCount.ExecuteScalar();
                    if (count == 0)
                    {
                        InsertInitialBooks(connection);
                    }
                }
            }
        }

        private static void InsertInitialBooks(SQLiteConnection connection)
        {
            List<Book> books = new List<Book>
            {
                new Book("To Kill a Mockingbird","Harper Lee","Classic","A novel about serious social issues."),
                new Book("1984","George Orwell","Dystopian","Government surveillance and control."),
                new Book("The Great Gatsby","F. Scott Fitzgerald","Classic","Critique of the American dream."),
                new Book("Harry Potter and the Sorcerer's Stone","J.K. Rowling","Fantasy","Start of Harry Potter series."),
                new Book("The Hobbit","J.R.R. Tolkien","Fantasy","Bilbo Baggins adventure."),
                new Book("Pride and Prejudice","Jane Austen","Romance","Classic love story."),
                new Book("The Catcher in the Rye","J.D. Salinger","Classic","Teenage angst story."),
                new Book("The Da Vinci Code","Dan Brown","Thriller","Mystery thriller."),
                new Book("The Alchemist","Paulo Coelho","Adventure","Philosophical story about dreams."),
                new Book("Moby Dick","Herman Melville","Classic","Quest for a white whale."),
                new Book("War and Peace","Leo Tolstoy","Classic","Russian society epic."),
                new Book("The Chronicles of Narnia","C.S. Lewis","Fantasy","Fantasy book series."),
                new Book("Brave New World","Aldous Huxley","Dystopian","Futuristic dystopia."),
                new Book("The Fault in Our Stars","John Green","Romance","Young love and illness."),
                new Book("Gone Girl","Gillian Flynn","Thriller","Psychological thriller."),
                new Book("The Hunger Games","Suzanne Collins","Dystopian","Survival competition."),
                new Book("The Book Thief","Markus Zusak","Historical","Set in Nazi Germany."),
                new Book("The Girl with the Dragon Tattoo","Stieg Larsson","Mystery","Thrilling mystery."),
                new Book("The Lord of the Rings","J.R.R. Tolkien","Fantasy","Epic fantasy trilogy."),
                new Book("Animal Farm","George Orwell","Satire","Political allegory.")
            };

            foreach (Book book in books)
            {
                using (SQLiteCommand insertCmd = new SQLiteCommand("INSERT INTO Books (Title, Author, Genre, Description, IsRead) VALUES (@title, @author, @genre, @desc, 0)", connection))
                {
                    insertCmd.Parameters.AddWithValue("@title", book.Title);
                    insertCmd.Parameters.AddWithValue("@author", book.Author);
                    insertCmd.Parameters.AddWithValue("@genre", book.Genre);
                    insertCmd.Parameters.AddWithValue("@desc", book.Description);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Book> GetBooks(string search = "")
        {
            List<Book> books = new List<Book>();

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DbFile};Version=3;"))
            {
                connection.Open();

                string sql = "SELECT Id, Title, Author, Genre, Description, IsRead FROM Books";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    sql += " WHERE Title LIKE @search OR Author LIKE @search OR Genre LIKE @search";
                }

                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                    }

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(new Book(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.IsDBNull(4) ? "" : reader.GetString(4),
                                reader.GetInt32(5) == 1
                            ));
                        }
                    }
                }
            }

            return books;
        }

        public static void AddBook(Book book)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DbFile};Version=3;"))
            {
                connection.Open();

                string sql = "INSERT INTO Books (Title, Author, Genre, Description, IsRead) VALUES (@title, @author, @genre, @desc, @isread)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@author", book.Author);
                    cmd.Parameters.AddWithValue("@genre", book.Genre);
                    cmd.Parameters.AddWithValue("@desc", book.Description);
                    cmd.Parameters.AddWithValue("@isread", book.IsRead ? 1 : 0);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateBook(Book book)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DbFile};Version=3;"))
            {
                connection.Open();

                string sql = "UPDATE Books SET Title=@title, Author=@author, Genre=@genre, Description=@desc, IsRead=@isread WHERE Id=@id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@author", book.Author);
                    cmd.Parameters.AddWithValue("@genre", book.Genre);
                    cmd.Parameters.AddWithValue("@desc", book.Description);
                    cmd.Parameters.AddWithValue("@isread", book.IsRead ? 1 : 0);
                    cmd.Parameters.AddWithValue("@id", book.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteBook(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DbFile};Version=3;"))
            {
                connection.Open();

                string sql = "DELETE FROM Books WHERE Id = @id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateBookReadStatus(int id, bool isRead)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DbFile};Version=3;"))
            {
                connection.Open();

                string sql = "UPDATE Books SET IsRead=@isread WHERE Id=@id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@isread", isRead ? 1 : 0);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static int GetReadCount()
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DbFile};Version=3;"))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Books WHERE IsRead=1", connection))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static int GetTotalCount()
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DbFile};Version=3;"))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Books", connection))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
