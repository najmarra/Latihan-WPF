using Microsoft.Data.SqlClient;
using StudentManager.Models;

namespace StudentManager.Data;

public class StudentRepository
{
    private const string DatabaseName = "StudentDB";

    private readonly string connectionString =
        @"Server=(localdb)\MSSQLLocalDB;
          Database=StudentDB;
          Trusted_Connection=True;
          TrustServerCertificate=True;";

    public StudentRepository()
    {
        EnsureDatabase();
    }

    // Membuat database + tabel + data contoh secara otomatis bila belum ada,
    // jadi aplikasi bisa langsung jalan tanpa menjalankan script SQL manual.
    private void EnsureDatabase()
    {
        var builder = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = "master"
        };

        using (var master = new SqlConnection(builder.ConnectionString))
        {
            master.Open();
            string createDb =
                $"IF DB_ID('{DatabaseName}') IS NULL CREATE DATABASE {DatabaseName};";
            using var cmd = new SqlCommand(createDb, master);
            cmd.ExecuteNonQuery();
        }

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        string createTable = @"
IF OBJECT_ID('dbo.Students', 'U') IS NULL
BEGIN
    CREATE TABLE Students
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        NIM VARCHAR(20) NOT NULL,
        Nama VARCHAR(100) NOT NULL,
        Jurusan VARCHAR(100) NOT NULL,
        Gender VARCHAR(20) NOT NULL,
        Email VARCHAR(100)
    );

    INSERT INTO Students (NIM, Nama, Jurusan, Gender, Email) VALUES
    ('23001','Budi Santoso','Informatika','Laki-laki','budi@gmail.com'),
    ('23002','Siti Aminah','Sistem Informasi','Perempuan','siti@gmail.com'),
    ('23003','Andi Wijaya','Informatika','Laki-laki','andi@gmail.com'),
    ('23004','Rina Sari','Sistem Informasi','Perempuan','rina@gmail.com');
END";
        using var tableCmd = new SqlCommand(createTable, connection);
        tableCmd.ExecuteNonQuery();
    }

    private static Student MapStudent(SqlDataReader reader) => new Student
    {
        Id = Convert.ToInt32(reader["Id"]),
        NIM = reader["NIM"].ToString()!,
        Nama = reader["Nama"].ToString()!,
        Jurusan = reader["Jurusan"].ToString()!,
        Gender = reader["Gender"].ToString()!,
        Email = reader["Email"] == DBNull.Value ? "" : reader["Email"].ToString()!
    };

    // READ
    public List<Student> GetAll()
    {
        var students = new List<Student>();

        using SqlConnection connection = new SqlConnection(connectionString);
        string sql =
            "SELECT Id, NIM, Nama, Jurusan, Gender, Email " +
            "FROM Students ORDER BY Id DESC";

        using SqlCommand command = new SqlCommand(sql, connection);
        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
            students.Add(MapStudent(reader));

        return students;
    }

    // INSERT
    public void Insert(Student student)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        string sql =
            "INSERT INTO Students " +
            "(NIM, Nama, Jurusan, Gender, Email) " +
            "VALUES (@NIM,@Nama,@Jurusan,@Gender,@Email)";

        using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@NIM", student.NIM);
        command.Parameters.AddWithValue("@Nama", student.Nama);
        command.Parameters.AddWithValue("@Jurusan", student.Jurusan);
        command.Parameters.AddWithValue("@Gender", student.Gender);
        command.Parameters.AddWithValue("@Email", student.Email);

        connection.Open();
        command.ExecuteNonQuery();
    }

    // UPDATE
    public void Update(Student student)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        string sql =
            "UPDATE Students SET NIM=@NIM, Nama=@Nama, " +
            "Jurusan=@Jurusan, Gender=@Gender, Email=@Email " +
            "WHERE Id=@Id";

        using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", student.Id);
        command.Parameters.AddWithValue("@NIM", student.NIM);
        command.Parameters.AddWithValue("@Nama", student.Nama);
        command.Parameters.AddWithValue("@Jurusan", student.Jurusan);
        command.Parameters.AddWithValue("@Gender", student.Gender);
        command.Parameters.AddWithValue("@Email", student.Email);

        connection.Open();
        command.ExecuteNonQuery();
    }

    // DELETE
    public void Delete(int id)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        using SqlCommand command =
            new SqlCommand("DELETE FROM Students WHERE Id=@Id", connection);

        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        command.ExecuteNonQuery();
    }

    // SEARCH
    public List<Student> Search(string keyword)
    {
        var students = new List<Student>();

        using SqlConnection connection = new SqlConnection(connectionString);
        string sql =
            "SELECT Id, NIM, Nama, Jurusan, Gender, Email " +
            "FROM Students WHERE NIM LIKE @Keyword " +
            "OR Nama LIKE @Keyword OR Jurusan LIKE @Keyword " +
            "ORDER BY Id DESC";

        using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

        connection.Open();
        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
            students.Add(MapStudent(reader));

        return students;
    }
}
