using System.Windows;
using StudentManager.ViewModels;

namespace StudentManager;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        try
        {
            DataContext = new StudentViewModel();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Gagal terhubung ke SQL Server LocalDB.\n\n" + ex.Message,
                "Database Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
