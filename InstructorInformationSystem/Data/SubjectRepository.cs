using InstructorInformationSystem.Models;

namespace InstructorInformationSystem.Data;

// In-memory store for Subject records (demo data, resets when the app restarts).
public static class SubjectRepository
{
    private static readonly List<Subject> subjects = new()
    {
        new Subject { SubjectId = 1, SubjectCode = "IT312", SubjectName = "Systems Integration and Architecture", Units = 3, InstructorName = "Prof. Dela Cruz" },
        new Subject { SubjectId = 2, SubjectCode = "IT313", SubjectName = "Capstone Project 1", Units = 3, InstructorName = "Prof. Santos" }
    };

    private static int nextId = 3;

    public static List<Subject> GetAll() => subjects;

    public static Subject? GetById(int id) => subjects.FirstOrDefault(s => s.SubjectId == id);

    public static void Add(Subject subject)
    {
        subject.SubjectId = nextId++;
        subjects.Add(subject);
    }

    public static void Update(Subject subject)
    {
        var existing = GetById(subject.SubjectId);
        if (existing is null) return;

        existing.SubjectCode = subject.SubjectCode;
        existing.SubjectName = subject.SubjectName;
        existing.Units = subject.Units;
        existing.InstructorName = subject.InstructorName;
    }

    public static void Delete(int id)
    {
        var existing = GetById(id);
        if (existing is not null) subjects.Remove(existing);
    }
}