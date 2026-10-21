using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StudentManager.Data;
using StudentManager.Models;

namespace StudentManager.ViewModels;

public class StudentViewModel : INotifyPropertyChanged
{
    private readonly StudentRepository _repository;

    public ObservableCollection<Student> Students { get; } = new();

    // Pilihan untuk ComboBox
    public List<string> JurusanList { get; } = new() { "Informatika", "Sistem Informasi" };
    public List<string> GenderList { get; } = new() { "Laki-laki", "Perempuan" };

    // Baris yang dipilih di DataGrid
    private Student? _selectedStudent;
    public Student? SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            _selectedStudent = value;
            OnPropertyChanged();

            // Klik baris di DataGrid -> data masuk ke form (mode Edit)
            if (value != null)
            {
                FormStudent = value.Clone();
                StatusMessage = "Mode edit: ubah data lalu klik Simpan.";
            }
        }
    }

    // Data yang sedang diisi/diedit di form
    private Student _formStudent = new();
    public Student FormStudent
    {
        get => _formStudent;
        set
        {
            _formStudent = value;
            OnPropertyChanged();
        }
    }

    private string _searchText = "";
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
        }
    }

    private string _statusMessage = "";
    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    // Statistik
    public int TotalStudents => Students.Count;
    public int TotalInformatika => Students.Count(x => x.Jurusan == "Informatika");
    public int TotalSistemInformasi => Students.Count(x => x.Jurusan == "Sistem Informasi");
    public int TotalLakiLaki => Students.Count(x => x.Gender == "Laki-laki");
    public int TotalPerempuan => Students.Count(x => x.Gender == "Perempuan");

    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand SearchCommand { get; }

    public StudentViewModel()
    {
        _repository = new StudentRepository();

        SaveCommand = new RelayCommand(Save);
        DeleteCommand = new RelayCommand(Delete);
        ResetCommand = new RelayCommand(Reset);
        SearchCommand = new RelayCommand(Search);

        LoadData();
    }

    private void LoadData()
    {
        ShowStudents(_repository.GetAll());
    }

    private void ShowStudents(IEnumerable<Student> data)
    {
        Students.Clear();
        foreach (var student in data)
            Students.Add(student);

        RefreshStatistics();
    }

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(FormStudent.NIM) ||
            string.IsNullOrWhiteSpace(FormStudent.Nama) ||
            string.IsNullOrWhiteSpace(FormStudent.Jurusan) ||
            string.IsNullOrWhiteSpace(FormStudent.Gender))
        {
            StatusMessage = "NIM, Nama, Jurusan, dan Gender wajib diisi.";
            return;
        }

        if (FormStudent.Id == 0)
        {
            _repository.Insert(FormStudent);
            StatusMessage = "Data mahasiswa berhasil ditambahkan.";
        }
        else
        {
            _repository.Update(FormStudent);
            StatusMessage = "Data mahasiswa berhasil diperbarui.";
        }

        SearchText = "";
        LoadData();
        ClearForm();
    }

    private void Delete()
    {
        if (FormStudent.Id == 0)
        {
            StatusMessage = "Pilih data di tabel terlebih dahulu.";
            return;
        }

        var confirm = MessageBox.Show(
            $"Hapus data {FormStudent.Nama}?",
            "Konfirmasi Hapus",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirm != MessageBoxResult.Yes) return;

        _repository.Delete(FormStudent.Id);
        StatusMessage = "Data mahasiswa berhasil dihapus.";

        LoadData();
        ClearForm();
    }

    private void Search()
    {
        var result = string.IsNullOrWhiteSpace(SearchText)
            ? _repository.GetAll()
            : _repository.Search(SearchText);

        ShowStudents(result);
        StatusMessage = $"Ditemukan {Students.Count} data.";
    }

    private void Reset()
    {
        ClearForm();
        SearchText = "";
        LoadData();
        StatusMessage = "Form direset.";
    }

    private void ClearForm()
    {
        SelectedStudent = null;
        FormStudent = new Student();
    }

    private void RefreshStatistics()
    {
        OnPropertyChanged(nameof(TotalStudents));
        OnPropertyChanged(nameof(TotalInformatika));
        OnPropertyChanged(nameof(TotalSistemInformasi));
        OnPropertyChanged(nameof(TotalLakiLaki));
        OnPropertyChanged(nameof(TotalPerempuan));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
