using System;
using System.Text;
using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class UserManager
{
    private readonly SQLiteConnection _db;

    public UserManager(string dbPath)
    {
        _db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        _db.CreateTable<User>();
    }

    public bool Register(string username, string password)
    {
        try
        {
            // Проверяем, есть ли пользователь с таким именем
            var existingUser = _db.Table<User>().FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                Debug.Log("Username already exists.");
                return false; // Возвращаем false, если пользователь уже существует
            }

            // Хэшируем пароль и создаем запись
            string passwordHash = HashPassword(password);
            var user = new User { Username = username, PasswordHash = passwordHash };
            _db.Insert(user);

            Debug.Log("Registration successful.");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Registration failed: {e.Message}");
            return false;
        }
    }

    public int Login(string username, string password)
    {
        try
        {
            string passwordHash = HashPassword(password);
            var user = _db.Table<User>().FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);
            return user?.UserID ?? -1; // Возвращает UserID или -1, если пользователь не найден
        }
        catch (Exception e)
        {
            Debug.Log($"Login failed: {e.Message}");
            return -1;
        }
    }

    private string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}

[Table("Users")]
public class User
{
    [PrimaryKey, AutoIncrement]
    public int UserID { get; set; }

    [Unique, NotNull]
    public string Username { get; set; }

    [NotNull]
    public string PasswordHash { get; set; }
}
