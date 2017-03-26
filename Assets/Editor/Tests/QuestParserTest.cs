using NUnit.Framework;
using QuestSystem;

//TODO улучишить тесты

[TestFixture]
public class QuestParserTest
{
    [Test]
    public void QuestWithoutTitleThrowsException()
    {
        string testJson = "{}";

        Assert.Throws<QuestParseException>(
            () => QuestParser.Parse(testJson)
        );
    }

    [Test]
    public void QuestWithoutTasksThrowsException()
    {
        string testJson = "{\"title\":\"test\"}";

        Assert.Throws<QuestParseException>(
            () => QuestParser.Parse(testJson)
        );
    }

    [Test]
    public void QuestWithoutNotesThrowsException()
    {
        string testJson = "{\"title\":\"test\", \"tasks\":[]}";

        Assert.Throws<QuestParseException>(
            () => QuestParser.Parse(testJson)
        );
    }


    [Test]
    public void QuestWithoutTaskDescriptionThrowsException()
    {
        string testJson = "{\"title\":\"test\", \"tasks\":[{}], \"notes\":[]}";

        Assert.Throws<QuestParseException>(
            () => QuestParser.Parse(testJson)
        );
    }

    [Test]
    public void TitleWithInvalidTypeThrowsException()
    {
        string testJson = "{\"title\":null, \"tasks\":[{}], \"notes\":[]}";

        Assert.Throws<QuestParseException>(
            () => QuestParser.Parse(testJson)
        );
    }

    [Test]
    public void TasksWithInvalidTypeThrowsException()
    {
        string testJson = "{\"title\":\"null\", \"tasks\":null, \"notes\":[] }";

        Assert.Throws<QuestParseException>(
            () => QuestParser.Parse(testJson)
        );
    }

    [Test]
    public void NotesWithInvalidTypeThrowsException()
    {
        string testJson = "{\"title\":\"null\", \"tasks\":[{}], \"notes\":null }";

        Assert.Throws<QuestParseException>(
            () => QuestParser.Parse(testJson)
        );
    }

    [Test]
    public void TaskWithInvalidTypeThrowsException()
    {
        string testJson = "{\"title\":\"null\", \"tasks\":[null], \"notes\":[] }";

        Assert.Throws<QuestParseException>(
            () => QuestParser.Parse(testJson)
        );
    }

    [Test]
    public void NoteWithInvalidTypeThrowsException()
    {
        string testJson = "{\"title\":\"null\", \"tasks\":[{}], \"notes\":[null] }";

        Assert.Throws<QuestParseException>(
            () => QuestParser.Parse(testJson)
        );
    }

    [Test]
    public void TaskWithInvalidDescriptionTypeThrowsException()
    {
        string testJson = "{\"title\":\"null\", \"tasks\":[{\"description\":null}], \"notes\":[] }";

        Assert.Throws<QuestParseException>(
            () => QuestParser.Parse(testJson)
        );
    }

    [Test]
    public void QuestTitleParseCorrectly()
    {
        string testJson = "{\"title\":\"test\", \"tasks\":[{\"description\":\"test\"}], \"notes\":[] }";

        var quest = QuestParser.Parse(testJson);

        string expectedTitle = "test";
        string realTitle = quest.Title;

        Assert.AreEqual(realTitle, expectedTitle);
    }

    [Test]
    public void QuestNotesParseCorrectly()
    {
        string testJson =
            "{\"title\":\"test\", \"tasks\":[{\"description\":\"test\"}], \"notes\":[\"test1\",\"test2\"] }";

        var quest = QuestParser.Parse(testJson);

        string[] expectedNotes = new string[2]{ "test1", "test2" };
        string[] realNotes = quest.QuestNotes;

        Assert.AreEqual(realNotes, expectedNotes);
    }

    [Test]
    public void QuestTasksParseCorrectly()
    {
        string testJson =
            "{\"title\":\"test\", \"tasks\":[{\"description\":\"test\", \"visible\":false}], \"notes\":[] }";

        var quest = QuestParser.Parse(testJson);

        QuestTask[] expectedTasks = new QuestTask[1]{ new QuestTask("test", false) };
        QuestTask[] realTasks = quest.Tasks;

        Assert.AreEqual(realTasks, expectedTasks);
    }
}