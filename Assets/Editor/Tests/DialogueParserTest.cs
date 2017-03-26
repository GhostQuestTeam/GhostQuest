using System; 
using NUnit.Framework;
using DialogueSystem;

//TODO улучишить тесты

[TestFixture]
public class DialogueParserTest {

	[Test]
	public void NodeWithoutIdThrowsException(){
		string testJson = "{}";

		var exc = Assert.Throws<DialogueParseException> (
			() => DialogueParser.Parse (testJson)
		);

	}
		
	[Test]
	public void NodeWithoutInvitationThrowsException(){
		string testJson = "{\"id\" : 0}";

		var exc = Assert.Throws<DialogueParseException> (
			() => DialogueParser.Parse (testJson)
		);

	}

	[Test]
	public void NodeWithoutAnswersThrowsException(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\"}";

		var exc = Assert.Throws<DialogueParseException> (
			() => DialogueParser.Parse (testJson)
		);

	}

	[Test]
	public void NodeWithInvalidIdTypeThrowsException(){
		string testJson = "{\"id\" : \"text\", \"invitation\" : \"test\" , \"answers\" : []}";

		var exc = Assert.Throws<DialogueParseException> (
			() => DialogueParser.Parse (testJson)
		);

	}

	[Test]
	public void NodeWithInvalidInvitationTypeThrowsException(){
		string testJson = "{\"id\" : 0, \"invitation\" : null , \"answers\" : []}";

		var exc = Assert.Throws<DialogueParseException> (
			() => DialogueParser.Parse (testJson)
		);
	}

	[Test]
	public void NodeWithInvalidAnswersTypeThrowsException(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\" , \"answers\" : null}";

		var exc = Assert.Throws<DialogueParseException> (
			() => DialogueParser.Parse (testJson)
		);
	}

	[Test]
	public void NodeWithoutAnswerMessageThrowsException(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\" , \"answers\" : [{}]}";

		var exc = Assert.Throws<DialogueParseException> (
			() => DialogueParser.Parse (testJson)
		);
	}

	[Test]
	public void NodeWithInvalidAnswerMessageTypeThrowsException(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\" , \"answers\" : [{\"message\":null}]}";

		var exc = Assert.Throws<DialogueParseException> (
			() => DialogueParser.Parse (testJson)
		);
	}

	[Test]
	public void NodeIdParseCorrectly(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\" , \"answers\" : []}";
		int expectedId = 0;

		var dialogue = DialogueParser.Parse (testJson);
		int realId = dialogue.CurrentNodeId;

		Assert.AreEqual(expectedId, realId);
	}

	[Test]
	public void InvitationParseCorrectly(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\" , \"answers\" : []}";
		string expectedInvitation = "test";

		var dialogue = DialogueParser.Parse (testJson);
		string realInvitation = dialogue.CurrentNode.Invitation;

		Assert.AreEqual(expectedInvitation, realInvitation);
	}

	[Test]
	public void AnswerMessageParseCorrectly(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\" , \"answers\" : [{\"message\":\"test message\"}]}";
		string expectedMessage = "test message";

		var dialogue = DialogueParser.Parse (testJson);
		string realMessage = dialogue.CurrentNode.Answers[0].Message;

		Assert.AreEqual(expectedMessage, realMessage);
	}

	[Test]
	public void DefaultAnswerNextFieldEqualsCurrentNodeId(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\" , \"answers\" : [{\"message\":\"test message\"}]}";

		var dialogue = DialogueParser.Parse (testJson);

		int expectedNext = dialogue.CurrentNodeId;
		int realNext = dialogue.CurrentNode.Answers[0].Next;

		Assert.AreEqual(expectedNext, realNext);
	}

	[Test]
	public void NullNextFieldParseToMinusOne(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\" , \"answers\" : [{\"message\":\"test message\", \"next\" : null}]}";

		var dialogue = DialogueParser.Parse (testJson);

		int expectedNext = -1;
		int realNext = dialogue.CurrentNode.Answers[0].Next;

		Assert.AreEqual(expectedNext, realNext);

	}

	[Test]
	public void IntNextFieldParseCorrectly(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\" , \"answers\" : [{\"message\":\"test message\", \"next\" : 3}]}";

		var dialogue = DialogueParser.Parse (testJson);

		int expectedNext = 3;
		int realNext = dialogue.CurrentNode.Answers[0].Next;

		Assert.AreEqual(expectedNext, realNext);

	}

	[Test]
	public void ObjectNextFieldParseCorrectly(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\" , \"answers\" : [{\"message\":\"test message\", \"next\" : {\"id\":2, \"invitation\" : \"test\", \"answers\":[] }}]}";

		var dialogue = DialogueParser.Parse (testJson);

		int expectedNext = 2;
		int realNext = dialogue.CurrentNode.Answers[0].Next;

		Assert.AreEqual(expectedNext, realNext);

	}

	[Test]
	public void NestedNodeParseCorrectly(){
		string testJson = "{\"id\" : 0, \"invitation\" : \"test\" , \"answers\" : [{\"message\":\"test message\", \"next\" : {\"id\":2, \"invitation\" : \"test\", \"answers\":" +
			"[{\"message\":\"test message\", \"next\" : {\"id\":5, \"invitation\" : \"id5_ivitation\", \"answers\":[] }}] }}]}";

		var dialogue = DialogueParser.Parse (testJson);

		string expectedInvitation ="id5_ivitation" ;
	
		dialogue.ChooseAnswer (0);
		dialogue.ChooseAnswer (0);

		string realInvitation = dialogue.CurrentNode.Invitation;

		Assert.AreEqual(expectedInvitation, realInvitation);

	}
}