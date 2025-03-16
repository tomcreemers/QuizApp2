using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

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

        public T Get(int id)
        {
            try
            {
                return _connection.Find<T>(id);
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
                // Attempt to read 'Id' property
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
            // Minimal seeding example
            if (typeof(T) == typeof(QuizApp2.Models.QuizQuestion))
            {
                var existing = GetAll() as List<QuizApp2.Models.QuizQuestion>;
                if (existing.Count == 0)
                {
                    var seedData = new List<QuizApp2.Models.QuizQuestion>
                    {
                        new QuizApp2.Models.QuizQuestion
                        {
                            Prompt = "What is 2+2?",
                            Category = "Math",
                            Difficulty = "Easy"
                        },
                        new QuizApp2.Models.QuizQuestion
                        {
                            Prompt = "What is the capital of France?",
                            Category = "Geography",
                            Difficulty = "Easy"
                        }
                    };
                    foreach (var q in seedData)
                    {
                        Save(q as T);
                    }
                }
            }
        }
    }
}
