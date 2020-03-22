using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hunglish.Models;
using SQLite;

namespace Hunglish.Database
{
    public class WordDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;

        public WordDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Lesson).Name))
            {
                await Database.CreateTablesAsync(CreateFlags.None, typeof(Lesson)).ConfigureAwait(false);
            }

            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Word).Name))
            {
                await Database.CreateTablesAsync(CreateFlags.None, typeof(Word)).ConfigureAwait(false);
            }

            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(SystemSetting).Name))
            {
                await Database.CreateTablesAsync(CreateFlags.None, typeof(SystemSetting)).ConfigureAwait(false);

                var init = await Database.QueryAsync<SystemSetting>("SELECT * FROM [SystemSetting]");
                if (init?.Count == 0)
                {
                    await Database.InsertAsync(new SystemSetting() { Data = "initialized" });
                }
            }
        }

        internal async Task DeleteWordAsync(int id)
        {
            var word = await Database.Table<Word>().Where(x => x.ID == id).FirstOrDefaultAsync();
            await Database.DeleteAsync(word);
        }

        public async Task<List<Lesson>> GetLessonsAsync()
        {
            return await Database.Table<Lesson>().ToListAsync();
        }

        public async Task<Lesson> GetLessonAsync(long lessonId)
        {
            var lesson = await Database.Table<Lesson>().Where(x => x.ID == lessonId).ToListAsync();
                
            return lesson.FirstOrDefault();
        }

        public async Task<List<Word>> GetWordsAsync(long lessonId)
        {
            var words = await Database.Table<Word>().Where(x => x.LessonId == lessonId).ToListAsync();

            return words;
        }

        public async Task<int> GetWordsCountAsync(long lessonId)
        {
            var wordsCount = await Database.Table<Word>().Where(x => x.LessonId == lessonId).CountAsync();

            return wordsCount;
        }

        public async Task<Word> GetWordAsync(long lessonId, long wordId)
        {
            var word = await Database.Table<Word>()
                .Where(x => x.LessonId == lessonId && x.ID == wordId)
                .ToListAsync();

            return word.FirstOrDefault();
        }

        public async Task<int> InsertLessonAsync(Lesson lesson)
        {
            var id = await Database.InsertAsync(lesson);

            return id;
        }

        public async Task<int> InsertWordAsync(Word word)
        {
            return await Database.InsertAsync(word);
        }

        public async Task<int> UpdateLessonAsync(Lesson lesson)
        {
            return await Database.UpdateAsync(lesson);
        }

        public async Task<int> UpdateWordAsync(Word word)
        {
            var update = await GetWordAsync(word.LessonId, word.ID);

            update.English = word.English;
            update.Meaning = word.Meaning;
            update.ExampleSentence = word.ExampleSentence;
            update.LatestAnswer = word.LatestAnswer;
            update.IKnowCount = word.IKnowCount;
            update.IDontKnowCount = word.IDontKnowCount;

            return await Database.UpdateAsync(update);
        }
    }
}
