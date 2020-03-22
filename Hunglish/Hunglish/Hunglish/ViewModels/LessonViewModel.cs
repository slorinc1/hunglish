using Hunglish.Models;

namespace Hunglish.ViewModels
{
    public class LessonViewModel : BaseViewModel
    {
        public Lesson ToLesson()
        {
            return new Lesson()
            {
                ID = Id,
                Name = Name,
                Score = Score,
                IsCompleted = IsCompleted
            };
        }

        public LessonViewModel()
        {
        }

        public LessonViewModel(Lesson lesson)
        {
            Id = lesson.ID;
            Name = lesson.Name;
            IsCompleted = lesson.IsCompleted;
            Score = lesson.Score;
        }

        int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        bool _isCompleted;
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                SetProperty(ref _isCompleted, value);
            }
        }

        double _score;
        public double Score
        {
            get { return _score; }
            set
            {
                SetProperty(ref _score, value);
            }
        }

        string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }
    }
}