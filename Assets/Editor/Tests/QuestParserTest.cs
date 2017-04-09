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
    public void QuestParseCorrectly()
    {
        string testJson =
            "{\"title\":\"test\", \"tasks\":[{\"description\":\"test\", \"visible\":false}], \"notes\":[\"test1\",\"test2\"] }";


        var expectedQuest = new Quest(
            "test",
            new[] {new QuestTask("test", false)},
            new[] {"test1", "test2"}
        );

        var realQuest = QuestParser.Parse(testJson);

        Assert.AreEqual(expectedQuest, realQuest);
    }
}