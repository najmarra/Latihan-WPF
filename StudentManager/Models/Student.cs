namespace StudentManager.Models;

public class Student
{
    public int Id { get; set; }
    public string NIM { get; set; } = "";
    public string Nama { get; set; } = "";
    public string Jurusan { get; set; } = "";
    public string Gender { get; set; } = "";
    public string Email { get; set; } = "";

    // Dipakai agar form bekerja pada salinan data,
    // sehingga baris di DataGrid tidak berubah sebelum tombol Simpan ditekan.
    public Student Clone() => new Student
    {
        Id = Id,
        NIM = NIM,
        Nama = Nama,
        Jurusan = Jurusan,
        Gender = Gender,
        Email = Email
    };
}
