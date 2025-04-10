using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BirthdayApp.Models;

namespace BirthdayApp.Services
{
    public static class DataService
    {
        private static readonly string DataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");

        public static List<User> LoadUsers()
        {
            try
            {
                if (!File.Exists(DataFile)) return null;
                var json = File.ReadAllText(DataFile);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<User>>(json, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка завантаження даних: {ex.Message}");
                return null;
            }
        }

        public static void SaveUsers(IEnumerable<User> users)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(DataFile, json);
        }
    }
}