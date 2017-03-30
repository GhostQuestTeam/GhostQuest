//Строит объект DialogGraph по описанию диалога в JSONе

using System;
using System.Collections.Generic;
using Extensions;
using SimpleJSON;
using Utils;

namespace DialogueSystem
{
    public class DialogueParseException : Exception
    {
        public DialogueParseException(string message) : base(message)
        {
        }
    }

    public class DialogueParser : Parser
    {

        public static readonly Dictionary<string, int> _FIELD_CONSTRAINTS = new Dictionary<string, int>()
        {
            {"id", _NUMBER},
            {"invitation", _STRING},
            {"answers", _ARRAY},
            {"answer", _OBJECT},
            {"message", _STRING},
            {"next", _OBJECT | _NUMBER | _NULL | _NOT_EXIST},
            {"condition", _OBJECT | _NOT_EXIST}
        };

        protected  static Dictionary<string, int> GetFieldConstraints()
        {
            return _FIELD_CONSTRAINTS;
        }

        protected new static JSONNode _getField(JSONNode node, string fieldName)
        {
            return _getFieldGeneric(node, fieldName, (str) => new DialogueParseException("Dialogue " + str));
        }
//
        private static int _getId(JSONNode node)
        {
            var idNode = _getField(node, "id");
            _validateType(idNode, "id");

            return idNode.AsInt;
        }

        protected new static  void  _validateType(JSONNode node, string fieldName)
        {
            _validateTypeGeneric(node, fieldName, (str) => new DialogueParseException(str), () => _FIELD_CONSTRAINTS);
        }

        public static DialogueGraph Parse(string json)
        {
            var dialogueJson = JSON.Parse(json);
            var dialogue = new DialogueGraph();
            Queue<JSONNode> nodesQueue = new Queue<JSONNode>();
            nodesQueue.Enqueue(dialogueJson);
            while (nodesQueue.Count != 0)
            {
                var currentNode = nodesQueue.Dequeue();

                int id = _getId(currentNode);

                var invitationNode = _getField(currentNode, "invitation");
                var answersNode = _getField(currentNode, "answers");

                _validateType(invitationNode, "invitation");
                _validateType(answersNode, "answers");

                string invitation = invitationNode.Value;
                JSONArray jsonAnswers = answersNode.AsArray;

                DialogueAnswer[] answers = new DialogueAnswer[jsonAnswers.Count];

                for (int i = 0; i < jsonAnswers.Count; i++)
                {
                    JSONNode answer = jsonAnswers[i];
                    _validateType(answer, "answer");

                    var messageNode = _getField(answer, "message");
                    int next = id; //By default next equals current node id

                    _validateType(messageNode, "message");

                    if (answer.HasKey("next"))
                    {
                        var nextNode = answer["next"];
                        if (nextNode.IsNumber)
                        {
                            next = nextNode.AsInt;
                        }
                        else if (nextNode.IsNull)
                        {
                            //-1 - выход из диалога
                            next = -1; //TODO Наверное лучше использовать тип (int?)
                        }
                        else if (nextNode.IsObject)
                        {
                            next = _getId(nextNode);
                            nodesQueue.Enqueue(nextNode);
                        }
                    }

                    answers[i] = new DialogueAnswer(messageNode.Value, next);

                    if (!answer.HasKey("condition")) continue;

                    var conditionNode = answer["condition"];
                    _validateType(conditionNode, "condition");

                    var condition = AnswerConditionParser.Parse(conditionNode);

                    answers[i].SetCondition(condition);
                }
                dialogue.addNode(id, new DialogueNode(invitation, answers));
            }
            return dialogue;
        }
    }
}