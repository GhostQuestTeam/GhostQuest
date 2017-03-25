//Строит объект DialogGraph по описанию диалога в JSONе

using System;
using System.Collections.Generic;
using Extensions;
using SimpleJSON;

namespace DialogueSystem
{
    public class DialogueParseException : Exception
    {
        public DialogueParseException(string message) : base(message)
        {
        }
    }

    public class DialogueParser
    {
        private const int _NUMBER = 1;
        private const int _BOOL = 2;
        private const int _OBJECT = 4;
        private const int _ARRAY = 8;
        private const int _NULL = 16;
        private const int _STRING = 32;
        private const int _NOT_EXIST = 64;

        private const int _MAX_CONSTRAINT = 32;

        private delegate bool CheckTypeDelegate(JSONNode node);


        private static readonly Dictionary<string, int> _FIELD_CONSTARINTS = new Dictionary<string, int>()
        {
            {"id", _NUMBER},
            {"invitation", _STRING},
            {"answers", _ARRAY},
            {"answer", _OBJECT},
            {"message", _STRING},
            {"next", _OBJECT | _NUMBER | _NULL | _NOT_EXIST}
        };

        private static readonly Dictionary<int, CheckTypeDelegate> _CONSTRAINTS_CHECKS =
            new Dictionary<int, CheckTypeDelegate>()
            {
                {_OBJECT, (node) => node.IsObject},
                {_ARRAY, (node) => node.IsArray},
                {_BOOL, (node) => node.IsBoolean},
                {_STRING, (node) => node.IsString},
                {_NUMBER, (node) => node.IsNumber},
                {_NULL, (node) => node.IsNull},
            };

        private static string _getErrorMessage(string field)
        {
            if (!_FIELD_CONSTARINTS.ContainsKey(field))
            {
                return "field " + field + " not supported";
            }
            string messageTemplate = "field " + field + " must have type in list:";
            int fieldConstraints = _FIELD_CONSTARINTS[field];
            if ((fieldConstraints & _NUMBER) != 0)
            {
                messageTemplate += " number";
            }
            if ((fieldConstraints & _NULL) != 0)
            {
                messageTemplate += " null";
            }
            if ((fieldConstraints & _OBJECT) != 0)
            {
                messageTemplate += " object";
            }
            if ((fieldConstraints & _ARRAY) != 0)
            {
                messageTemplate += " array";
            }
            if ((fieldConstraints & _STRING) != 0)
            {
                messageTemplate += " string";
            }
            if ((fieldConstraints & _BOOL) != 0)
            {
                messageTemplate += " bool";
            }
            return messageTemplate;
        }

        private static JSONNode _getField(JSONNode node, string fieldName)
        {
            if (!node.HasKey(fieldName))
            {
                throw new DialogueParseException("Dialogue node must have field " + fieldName);
            }
            return node[fieldName];
        }

        private static int _getId(JSONNode node)
        {
            var idNode = _getField(node, "id");
            _validateType(idNode, "id");

            return idNode.AsInt;
        }

        private static void _validateType(JSONNode node, string fieldName)
        {
            if (_FIELD_CONSTARINTS.ContainsKey(fieldName))
            {
                int fieldContraints = _FIELD_CONSTARINTS[fieldName];
                for (int i = 1; i <= _MAX_CONSTRAINT; i *= 2)
                {
                    if ((fieldContraints & i) != 0)
                    {
                        if (_CONSTRAINTS_CHECKS[i](node))
                        {
                            return;
                        }
                    }
                }
            }
            throw new DialogueParseException(_getErrorMessage(fieldName));
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
                }
                dialogue.addNode(id, new DialogueNode(invitation, answers));
            }
            return dialogue;
        }
    }
}