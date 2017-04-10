using System.Collections.Generic;
using NUnit.Framework;
using QuestSystem;

[TestFixture]
public class QuestManagerTest
{
    private const string _QUEST_TITLE_STUB = "title";
    private Quest _quest;

    private QuestTask[] _getTaskArrayStub()
    {
        return new[]
        {
            new QuestTask("0", false),
            new QuestTask("1", false),
            new QuestTask("2", true, true),
            new QuestTask("3", true, true)
        };
    }

    private Dictionary<string, Quest> _getQuestsStub()
    {
        var quest = new Quest(
            _QUEST_TITLE_STUB,
            _getTaskArrayStub(),
            new[] {"0", "1", "2", "3", "4"}
        );
        return new Dictionary<string, Quest>() {{_QUEST_TITLE_STUB, quest}};
    }

    [SetUp]
    public void Setuo()
    {
        var quests = _getQuestsStub();
        _quest = quests[_QUEST_TITLE_STUB];
        QuestManager.Init(quests);
    }

    [Test]
    public void GetTasksWorkCorrectly()
    {
        var expectedArray = _getTaskArrayStub();

        var realArray = QuestManager.GetTasks(_QUEST_TITLE_STUB);

        Assert.AreEqual(expectedArray, realArray);
    }

    [Test]
    public void GetTaskWorkCorrectly()
    {
        const int TEST_TASK_ID = 3;
        var expectedTask = _getTaskArrayStub()[TEST_TASK_ID];

        var realTask = QuestManager.GetTask(_QUEST_TITLE_STUB, TEST_TASK_ID);

        Assert.AreEqual(expectedTask, realTask);
    }

    [Test]
    public void ShowQuestNoteWorkCorrectly()
    {
        const int TEST_NOTE_ID = 1;

        QuestManager.ShowQuestNote(_QUEST_TITLE_STUB, TEST_NOTE_ID);

        Assert.IsTrue(_quest.isNoteVisible(TEST_NOTE_ID));
    }

    [Test]
    public void ShowTaskWorkCorrectly()
    {
        const int TEST_TASK_ID = 0;

        QuestManager.ShowTask(_QUEST_TITLE_STUB, TEST_TASK_ID);

        Assert.IsTrue(_quest.Tasks[TEST_TASK_ID].IsVisible);
    }

    [Test]
    public void DoTaskWorkCorrectly()
    {
        const int TEST_TASK_ID = 0;

        QuestManager.DoTask(_QUEST_TITLE_STUB, TEST_TASK_ID);

        Assert.IsTrue(_quest.Tasks[TEST_TASK_ID].IsDone);
    }

    [Test]
    public void GetVisibleTasksWorkCorrectly()
    {
        var expectedTasks = new[]
        {
            new QuestTask("2", true,true),
            new QuestTask("3", true,true)
        };

        var realTasks = QuestManager.GetVisibleTasks(_QUEST_TITLE_STUB);

        CollectionAssert.AreEquivalent(expectedTasks, realTasks);
    }

    [Test]
    public void GetVisibleNotesWorkCorrectly()
    {
        _quest.SetNoteVisible(0, true);
        _quest.SetNoteVisible(1, true);
        _quest.SetNoteVisible(2, true);
        _quest.SetNoteVisible(3, false);
        _quest.SetNoteVisible(4, false);

        string[] expectedNotes = {"0", "1", "2"};

        var realNotes = QuestManager.GetVisibleNotes(_QUEST_TITLE_STUB);

        CollectionAssert.AreEquivalent(expectedNotes, realNotes);
    }

    [Test]
    public void CheckTaskCompleteWithNotNullArgsWorks()
    {
        uint[] undone = {0, 1};
        uint[] done = {2, 3};

        Assert.True(QuestManager.CheckTaskComplete(_QUEST_TITLE_STUB, done, undone));
    }

    [Test]
    public void CheckTaskCompleteReturnsFalseIfUndoneTasksArrayContainDoneTask()
    {
        uint[] undone = {0, 1, 2, 3};
        uint[] done = {2, 3};

        Assert.False(QuestManager.CheckTaskComplete(_QUEST_TITLE_STUB, done, undone));
    }

    [Test]
    public void CheckTaskCompleteReturnsFalseIfDoneTasksArrayContainUndoneTask()
    {
        uint[] undone = {0, 1};
        uint[] done = {0,1, 2, 3};

        Assert.False(QuestManager.CheckTaskComplete(_QUEST_TITLE_STUB, done, undone));
    }

    [Test]
    public void CheckTaskCompleteWorksWithNullUndoneTasks()
    {
        uint[] undone = null;
        uint[] done = {2, 3};

        Assert.True(QuestManager.CheckTaskComplete(_QUEST_TITLE_STUB, done, undone));

    }


    [Test]
    public void CheckTaskCompleteWorksWithNullDoneTasks()
    {
        uint[] undone = {0, 1};
        uint[] done = null;

        Assert.True(QuestManager.CheckTaskComplete(_QUEST_TITLE_STUB, done, undone));
    }

    [Test]
    public void IsQuestStartedReturnsTrueForStartedQuest()
    {
        Assert.True(QuestManager.IsQuestStarted(_QUEST_TITLE_STUB));
    }

    [Test]
    public void IsQuestStartedReturnsFalseForNotStartedQuest()
    {
        const string NOT_STARTED_QUEST = "NOT_STARTED_QUEST";

        Assert.False(QuestManager.IsQuestStarted(NOT_STARTED_QUEST));
    }
}