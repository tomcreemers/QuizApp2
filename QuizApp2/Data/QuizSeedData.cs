using System.Collections.Generic;
using QuizApp2.Models;

namespace QuizApp2.Data
{
    public static class QuizSeedData
    {
        public static List<QuizQuestion> GetSeedQuestions()
        {
            var questions = new List<QuizQuestion>();

            // Economy: 2 Q per difficulty 1..5
            // Difficulty 1
            questions.Add(new QuizQuestion
            {
                Prompt = "What is a budget?",
                Category = "Economy",
                Difficulty = 1
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "What does GDP stand for?",
                Category = "Economy",
                Difficulty = 1
            });

            // Difficulty 2
            questions.Add(new QuizQuestion
            {
                Prompt = "Name one factor that influences inflation.",
                Category = "Economy",
                Difficulty = 2
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "Which policy controls the money supply?",
                Category = "Economy",
                Difficulty = 2
            });

            // Difficulty 3
            questions.Add(new QuizQuestion
            {
                Prompt = "Define the term 'recession'.",
                Category = "Economy",
                Difficulty = 3
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "What is the difference between fiscal and monetary policy?",
                Category = "Economy",
                Difficulty = 3
            });

            // Difficulty 4
            questions.Add(new QuizQuestion
            {
                Prompt = "Explain how interest rates can affect consumer spending.",
                Category = "Economy",
                Difficulty = 4
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "What role do central banks play in stabilizing the economy?",
                Category = "Economy",
                Difficulty = 4
            });

            // Difficulty 5
            questions.Add(new QuizQuestion
            {
                Prompt = "Discuss the long-term impact of quantitative easing.",
                Category = "Economy",
                Difficulty = 5
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "How does international trade influence a country's currency value?",
                Category = "Economy",
                Difficulty = 5
            });

            // Sports: 2 Q per difficulty 1..5
            // Difficulty 1
            questions.Add(new QuizQuestion
            {
                Prompt = "Which sport uses a bat and ball and is played by two teams of nine?",
                Category = "Sports",
                Difficulty = 1
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "Name the sport where players typically wear helmets and skates on ice.",
                Category = "Sports",
                Difficulty = 1
            });

            // Difficulty 2
            questions.Add(new QuizQuestion
            {
                Prompt = "Which country won the 2018 FIFA World Cup?",
                Category = "Sports",
                Difficulty = 2
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "What is the main objective in basketball?",
                Category = "Sports",
                Difficulty = 2
            });

            // Difficulty 3
            questions.Add(new QuizQuestion
            {
                Prompt = "Name two tennis Grand Slam tournaments.",
                Category = "Sports",
                Difficulty = 3
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "In which sport is the term 'birdie' used?",
                Category = "Sports",
                Difficulty = 3
            });

            // Difficulty 4
            questions.Add(new QuizQuestion
            {
                Prompt = "Explain the offside rule in soccer.",
                Category = "Sports",
                Difficulty = 4
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "What is the difference between a marathon and an ultramarathon?",
                Category = "Sports",
                Difficulty = 4
            });

            // Difficulty 5
            questions.Add(new QuizQuestion
            {
                Prompt = "Describe the scoring system in Olympic boxing.",
                Category = "Sports",
                Difficulty = 5
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "Analyze how analytics have changed professional baseball strategies.",
                Category = "Sports",
                Difficulty = 5
            });

            // Tech: 2 Q per difficulty 1..5
            // Difficulty 1
            questions.Add(new QuizQuestion
            {
                Prompt = "What does 'CPU' stand for?",
                Category = "Tech",
                Difficulty = 1
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "Which device is primarily used to store data (HDD or SSD)?",
                Category = "Tech",
                Difficulty = 1
            });

            // Difficulty 2
            questions.Add(new QuizQuestion
            {
                Prompt = "Name the operating system developed by Microsoft.",
                Category = "Tech",
                Difficulty = 2
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "What is the main function of a router?",
                Category = "Tech",
                Difficulty = 2
            });

            // Difficulty 3
            questions.Add(new QuizQuestion
            {
                Prompt = "Define 'open-source software'.",
                Category = "Tech",
                Difficulty = 3
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "What is the difference between RAM and ROM?",
                Category = "Tech",
                Difficulty = 3
            });

            // Difficulty 4
            questions.Add(new QuizQuestion
            {
                Prompt = "Explain the concept of cloud computing.",
                Category = "Tech",
                Difficulty = 4
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "What is an API, and why is it important in modern software?",
                Category = "Tech",
                Difficulty = 4
            });

            // Difficulty 5
            questions.Add(new QuizQuestion
            {
                Prompt = "Discuss the role of encryption in data security.",
                Category = "Tech",
                Difficulty = 5
            });
            questions.Add(new QuizQuestion
            {
                Prompt = "Analyze how AI might impact the future job market.",
                Category = "Tech",
                Difficulty = 5
            });

            return questions;
        }
    }
}
