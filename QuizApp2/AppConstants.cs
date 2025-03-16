using SQLite;

namespace QuizApp2
{
    public static class AppConstants
    {
        public const string DBFilename = "QuizApp2.db3";

        public const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DBFilename);
    }
}
