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

        // We'll do a simple seed if T is QuizQuestion
        public void SeedQuestions()
        {
            if (typeof(T) == typeof(QuizApp2.Models.QuizQuestion))
            {
                var existing = GetAll() as List<QuizApp2.Models.QuizQuestion>;
                if (existing == null || existing.Count == 0)
                {
                    var seedData = new List<QuizApp2.Models.QuizQuestion>
                    {
                        // Economy
                        new QuizApp2.Models.QuizQuestion
                        {
                            Prompt = "What is inflation?",
                            Category = "Economy",
                            Difficulty = "Easy"
                        },
                        new QuizApp2.Models.QuizQuestion
                        {
                            Prompt = "Name one cause of a recession.",
                            Category = "Economy",
                            Difficulty = "Medium"
                        },

                        // Sports
                        new QuizApp2.Models.QuizQuestion
                        {
                            Prompt = "Which country won the 2018 FIFA World Cup?",
                            Category = "Sports",
                            Difficulty = "Easy"
                        },
                        new QuizApp2.Models.QuizQuestion
                        {
                            Prompt = "Who is known as the GOAT in basketball?",
                            Category = "Sports",
                            Difficulty = "Medium"
                        },

                        // Tech
                        new QuizApp2.Models.QuizQuestion
                        {
                            Prompt = "What does 'CPU' stand for?",
                            Category = "Tech",
                            Difficulty = "Easy"
                        },
                        new QuizApp2.Models.QuizQuestion
                        {
                            Prompt = "Which company created the Linux kernel?",
                            Category = "Tech",
                            Difficulty = "Medium"
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
