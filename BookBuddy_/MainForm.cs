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
    public partial class MainForm: Form
    {
        private List<Book> books;
        public event Action OnBookAdded;
        public event Action OnBookUpdated;
        public event Action OnBookDeleted;

            
        public MainForm()
        {
            InitializeComponent();
            LoadBooks();
        }
        private void LoadBooks(string filter = "")
        {
            books = DatabaseHelper.GetBooks(filter);
            dataGridViewBooks.Rows.Clear();

            foreach (Book book in books)
            {
                dataGridViewBooks.Rows.Add(book.Id, book.Title, book.Author, book.Genre, book.Description, book.IsRead);
            }

            UpdateReadCount();
        }
        private void UpdateReadCount()
        {
            int readCount = DatabaseHelper.GetReadCount();
            int totalCount = DatabaseHelper.GetTotalCount();
            lblReadCount.Text = $"Read: {readCount} / {totalCount}";
        }
        private void btnAddBook_Click(object sender, EventArgs e)
        {
            AddBookForm addForm = new AddBookForm();
            addForm.OnBookAdded += () => LoadBooks(txtSearch.Text.Trim());
            addForm.ShowDialog();
        }

        private void btnEditBook_Click(object sender, EventArgs e)
        {
            if (dataGridViewBooks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int selectedId = Convert.ToInt32(dataGridViewBooks.SelectedRows[0].Cells[0].Value);
            var bookToEdit = books.Find(b => b.Id == selectedId);
            if (bookToEdit == null)
            {
                MessageBox.Show("The selected book was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            EditBookForm editForm = new EditBookForm(bookToEdit);
            editForm.OnBookUpdated += () => LoadBooks(txtSearch.Text.Trim());
            editForm.OnBookDeleted += () => LoadBooks(txtSearch.Text.Trim());
            editForm.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadBooks(txtSearch.Text.Trim());
        }

        private void dataGridViewBooks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewBooks.Rows[e.RowIndex];
                int id = Convert.ToInt32(row.Cells[0].Value);
                bool isRead = Convert.ToBoolean(row.Cells[5].Value);
                DatabaseHelper.UpdateBookReadStatus(id, isRead);
                UpdateReadCount();
            }
        }

        private void dataGridViewBooks_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewBooks.IsCurrentCellDirty)
            {
                dataGridViewBooks.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}
