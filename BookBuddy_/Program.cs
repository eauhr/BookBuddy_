using BookBuddy_;
using System;
using System.Windows.Forms;

namespace BookBuddy_
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DatabaseHelper.InitializeDatabase();
            Application.Run(new MainForm());
        }
    }
}

