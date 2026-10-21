# Student Management System

Aplikasi desktop berbasis **WPF (.NET 8)** untuk mengelola data mahasiswa, dibangun dengan pola **MVVM**, **Data Binding**, dan **SQL Server LocalDB** melalui **ADO.NET**.

> Studi kasus: *Rich UI + MVVM + Data Binding + SQL Server*

---

## 1. Deskripsi

Student Management System adalah aplikasi untuk mencatat dan mengelola data mahasiswa. Pengguna dapat menambah, mengubah, menghapus, dan mencari data mahasiswa, serta melihat ringkasan statistik secara langsung. Seluruh data disimpan di database SQL Server LocalDB (`StudentDB`).

### Tampilan Utama

<img width="1917" height="1020" alt="image" src="https://github.com/user-attachments/assets/492491f1-7185-4c88-acc0-232128684c65" />

---

## 2. Fitur

- **Input mahasiswa**: NIM, Nama, Jurusan, Gender, Email.
- **Statistik otomatis**: Total Mahasiswa, Informatika, Sistem Informasi, Laki-laki, Perempuan.
- **Pencarian** berdasarkan NIM, Nama, atau Jurusan.
- **DataGrid** untuk menampilkan seluruh data.
- **Edit / Update** dengan klik baris pada tabel.
- **Hapus** dengan dialog konfirmasi.
- **Reset** form dan tabel.
- **Validasi** field wajib (NIM, Nama, Jurusan, Gender).
- **Database otomatis**: database, tabel, dan data contoh dibuat saat aplikasi pertama kali dijalankan.

---

## 3. Teknologi

| Layer | Teknologi |
|---|---|
| Language | C# |
| Framework | .NET 8 WPF |
| UI | XAML |
| Architecture | MVVM |
| Data Binding | WPF Binding |
| Database | SQL Server LocalDB |
| Data Access | ADO.NET |
| SQL Provider | Microsoft.Data.SqlClient |
| IDE | Visual Studio 2022 |

---

## 4. Arsitektur MVVM

```
USER
  |
WPF / XAML (View)  <-- Data Binding / Command -->  ViewModel
                                                      |
                                                    Model (Student)
                                                      |
                                                Repository (ADO.NET)
                                                      |
                                              SQL Server (StudentDB)
```

| Komponen | Tanggung jawab |
|---|---|
| **View** (`MainWindow.xaml`) | Tampilan, tanpa logika aplikasi |
| **ViewModel** (`StudentViewModel`) | State, command, pencarian, statistik, logika UI |
| **Model** (`Student`) | Representasi data mahasiswa |
| **Repository** (`StudentRepository`) | Akses database (CRUD dan Search) |

### Pemetaan Data Binding

| Arah | Binding |
|---|---|
| Input | `TextBox` -> Binding -> `FormStudent.Nama` |
| Output | ViewModel -> Binding -> `TextBlock` (statistik) |
| Tabel | `ObservableCollection<Student>` -> `ItemsSource` -> `DataGrid` |
| Seleksi | `DataGrid.SelectedItem` -> `SelectedStudent` -> Form |
| Aksi | `Button` -> `Command` -> ViewModel |

---

## 5. Struktur Project

```
StudentManager
|-- StudentManager.csproj
|-- App.xaml / App.xaml.cs
|-- MainWindow.xaml / MainWindow.xaml.cs
|-- database.sql
|-- Models
|   `-- Student.cs
|-- ViewModels
|   |-- StudentViewModel.cs
|   `-- RelayCommand.cs
`-- Data
    `-- StudentRepository.cs
```

---

## 6. Desain Database

**Database:** `StudentDB`  **Tabel:** `Students`

| Kolom | Tipe | Keterangan |
|---|---|---|
| `Id` | INT IDENTITY(1,1) | Primary key |
| `NIM` | VARCHAR(20) | Wajib |
| `Nama` | VARCHAR(100) | Wajib |
| `Jurusan` | VARCHAR(100) | Wajib |
| `Gender` | VARCHAR(20) | Wajib |
| `Email` | VARCHAR(100) | Opsional |

Script lengkap ada di [`database.sql`](database.sql).

---

## 7. Penjelasan Kode

### Model: `Student.cs`
Berisi properti `Id`, `NIM`, `Nama`, `Jurusan`, `Gender`, `Email` dan method `Clone()` agar form bekerja pada salinan data.

### Repository: `StudentRepository.cs`
Menangani seluruh akses database dengan query berparameter (`@NIM`, `@Nama`, dst.) untuk mencegah SQL injection.

| Method | Fungsi |
|---|---|
| `GetAll()` | Mengambil semua data (SELECT) |
| `Insert(student)` | Menambah data (INSERT) |
| `Update(student)` | Mengubah data (UPDATE) |
| `Delete(id)` | Menghapus data (DELETE) |
| `Search(keyword)` | Mencari berdasarkan NIM / Nama / Jurusan (LIKE) |

### ViewModel: `StudentViewModel.cs`
Mengimplementasikan `INotifyPropertyChanged`, menyimpan `ObservableCollection<Student>`, properti statistik, dan empat command: `SaveCommand`, `DeleteCommand`, `ResetCommand`, `SearchCommand`.

### Command: `RelayCommand.cs`
Implementasi `ICommand` sederhana agar tombol di XAML dapat terhubung ke method di ViewModel tanpa code-behind.

### View: `MainWindow.xaml`
Terdiri dari header, form input, panel informasi (statistik), kolom pencarian, dan DataGrid. `DataContext` diatur ke `StudentViewModel` di `MainWindow.xaml.cs`.

### Alur CRUD

```
CREATE : Form -> Binding -> SaveCommand -> ViewModel -> Repository.Insert -> SQL Server -> LoadData -> DataGrid
READ   : SQL Server -> SELECT -> Repository -> ObservableCollection -> ItemsSource -> DataGrid
UPDATE : DataGrid -> SelectedStudent -> Form -> SaveCommand -> Repository.Update -> SQL Server
DELETE : DataGrid -> SelectedStudent -> DeleteCommand -> Repository.Delete -> SQL Server -> LoadData
SEARCH : SearchText -> SearchCommand -> Repository.Search -> Students -> DataGrid
```

---

## 8. Pengujian dan Dokumentasi

Isi kolom **Hasil** setelah menjalankan pengujian (Berhasil / Gagal) dan tempel screenshot masing-masing.

| No | Test | Langkah | Hasil yang Diharapkan | Hasil |
|---|---|---|---|---|
| 1 | READ | Jalankan aplikasi | Data sample muncul di DataGrid | |
| 2 | CREATE | Isi form, klik Simpan | Data baru tersimpan dan muncul di tabel | |
| 3 | UPDATE | Klik baris, ubah data, klik Simpan | Perubahan tersimpan | |
| 4 | DELETE | Klik baris, klik Hapus, konfirmasi | Data terhapus, tabel dan statistik diperbarui | |
| 5 | SEARCH | Ketik kata kunci, klik Cari | Data terfilter berdasarkan NIM/Nama/Jurusan | |
| 6 | STATISTICS | Lakukan CRUD | Angka statistik berubah | |
| 7 | RESET | Klik Reset | Form kembali ke kondisi awal | |

### 8.1 READ - Data awal tampil

<img width="1917" height="1020" alt="image" src="https://github.com/user-attachments/assets/68fcd533-02cf-4a6d-bc6e-80411c850a78" />

### 8.2 CREATE - Tambah data

<img width="1917" height="1016" alt="image" src="https://github.com/user-attachments/assets/0a3677ab-97d0-4bb7-92c1-9492a2e3857b" />

<img width="1917" height="1020" alt="image" src="https://github.com/user-attachments/assets/09e6e081-4432-4990-b532-bb89b88f81fc" />

### 8.3 UPDATE - Ubah data

<img width="1917" height="1016" alt="image" src="https://github.com/user-attachments/assets/676217d4-2656-4e4d-9b5a-f1115e764e9f" />

<img width="1917" height="1020" alt="image" src="https://github.com/user-attachments/assets/445808c6-82da-43fb-9d34-9836a781605b" />

### 8.4 DELETE - Hapus data

<img width="1917" height="1016" alt="image" src="https://github.com/user-attachments/assets/ee4d3af5-c33f-4d91-b69d-883d42bb3cb9" />

### 8.5 SEARCH - Pencarian

<img width="1917" height="1017" alt="image" src="https://github.com/user-attachments/assets/ad66e790-fa25-49f4-b798-7feb2b99d874" />

### 8.8 Validasi input

<img width="1917" height="1015" alt="image" src="https://github.com/user-attachments/assets/96fb75f1-d6ca-4035-bcb3-42b780c703aa" />

---

## 9. Kendala dan Solusi

| Kendala | Solusi |
|---|---|
| Form tidak bisa diketik karena `SelectedStudent` bernilai `null` saat aplikasi dimulai | Menambah properti `FormStudent` yang selalu berisi objek `Student` |
| Data di tabel ikut berubah sebelum tombol Simpan ditekan | Form memakai salinan data (`Clone()`) |
| Nilai ComboBox tidak terbaca saat memakai `ComboBoxItem` | Memakai `ItemsSource` berisi `List<string>` |
| Database harus dibuat manual | `EnsureDatabase()` membuat database, tabel, dan data contoh otomatis |

---

## 10. Kesimpulan

Aplikasi ini menunjukkan bahwa kekuatan WPF terletak pada kombinasi XAML, Data Binding, Command, MVVM, dan pemisahan akses data. Dengan pola ini, kode tampilan, logika aplikasi, dan akses database terpisah sehingga lebih terstruktur, mudah diuji, dan mudah dikembangkan.
