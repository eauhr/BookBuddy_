using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookBuddy_
{
    public partial class EditBookForm: Form
    {
        private Book book;
        public event Action OnBookUpdated;
        public event Action OnBookDeleted;

        public EditBookForm(Book book)
        {
            InitializeComponent();
            this.book = book;
            LoadBook();
        }
        private void LoadBook()
        {
            txtTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            txtGenre.Text = book.Genre;
            txtDescription.Text = book.Description;
            chkIsRead.Checked = book.IsRead;
        }

        public EditBookForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string author = txtAuthor.Text.Trim();
            string genre = txtGenre.Text.Trim();
            string description = txtDescription.Text.Trim();
            bool isRead = chkIsRead.Checked;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(genre))
            {
                MessageBox.Show("PLease, enter the required fields: Title, Author и Genre.", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            book.Title = title;
            book.Author = author;
            book.Genre = genre;
            book.Description = description;
            book.IsRead = isRead;

            DatabaseHelper.UpdateBook(book);
            OnBookUpdated?.Invoke();
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this book?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                DatabaseHelper.DeleteBook(book.Id);
                OnBookDeleted?.Invoke();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
