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
    public partial class AddBookForm: Form
    {
        public event Action OnBookAdded;
        public AddBookForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            string title = txtTitle.Text.Trim();
            string author = txtAuthor.Text.Trim();
            string genre = txtGenre.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(genre))
            {
                MessageBox.Show("PLease, enter the required fields: Title, Author и Genre.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Book newBook = new Book(title, author, genre, description);
            DatabaseHelper.AddBook(newBook);
            OnBookAdded?.Invoke();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
