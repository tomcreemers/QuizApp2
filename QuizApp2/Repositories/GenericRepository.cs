using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using QuizApp2.Data;       // for QuizSeedData
using QuizApp2.Models;    // for QuizQuestion

namespace QuizApp2.Repositories
{
    public class GenericRepository<T> where T : class, new()
    {
        private readonly SQLiteConnection _connection;
        public string StatusMessage { get; set; }

        public GenericRepository()
        {
            _connection = new SQLiteConnection(AppConstants.DatabasePath, AppConstants.Flags);
            _connection.CreateTable<T>();
        }

        public List<T> GetAll()
        {
            try
            {
                return _connection.Table<T>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                return null;
            }
        }

        public void Save(T item)
        {
            try
            {
                var idProp = typeof(T).GetProperty("Id");
                int currentId = (int)(idProp?.GetValue(item) ?? 0);

                if (currentId != 0)
                {
                    _connection.Update(item);
                }
                else
                {
                    _connection.Insert(item);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        public void SeedQuestions()
        {
            if (typeof(T) == typeof(QuizQuestion))
            {
                var existing = GetAll() as List<QuizQuestion>;
                if (existing == null || existing.Count == 0)
                {
                    // Call your QuizSeedData file
                    var seedData = QuizSeedData.GetSeedQuestions();
                    foreach (var q in seedData)
                    {
                        Save(q as T);
                    }
                }
            }
        }

    }
}
