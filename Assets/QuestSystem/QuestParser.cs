using System.Collections.Generic;
using UnityEngine;
using Utils;
using SimpleJSON;

namespace QuestSystem{

    public class QuestParseException : ParseException
    {
        public QuestParseException(string message) : base(message)
        {
        }
    }

    public class QuestParser : Parser
    {

        public static readonly Dictionary<string, int> _FIELD_CONSTRAINTS = new Dictionary<string, int>()
        {
            {"title", _STRING},
            {"tasks", _ARRAY},
            {"task", _OBJECT},
            {"description", _STRING},
            {"notes", _ARRAY},
            {"note", _STRING},
            {"visible", _BOOL | _NOT_EXIST  },
        };

        protected new static  void  _validateType(JSONNode node, string fieldName)
        {
            _validateTypeGeneric(node, fieldName, (str) => new QuestParseException(str), () => _FIELD_CONSTRAINTS);
        }

        protected new static JSONNode _getField(JSONNode node, string fieldName)
        {
            return _getFieldGeneric(node, fieldName, (str) => new QuestParseException( str));
        }

        public static Quest Parse(string json)
        {
            var questJson = JSON.Parse(json);

            var titleNode = _getField(questJson, "title");
            var notesNode = _getField(questJson, "notes");
            var tasksNode = _getField(questJson, "tasks");

            _validateType(titleNode, "title");
            _validateType(notesNode, "notes");
            _validateType(tasksNode, "tasks");

            string title = titleNode.Value;
            var notesArray = notesNode.AsArray;
            var tasksArray = tasksNode.AsArray;

            string[] notes = new string[notesArray.Count];
            QuestTask[] tasks = new QuestTask[tasksArray.Count];

            for (int i = 0; i < notesArray.Count; i++)
            {
                _validateType(notesArray[i], "note");
                notes[i] = notesArray[i].Value;
            }

            for (int i = 0; i < tasksArray.Count; i++)
            {
                _validateType(tasksArray[i], "task");
                var taskNode = tasksArray[i];

                var descriptionNode =  _getField(taskNode, "description");
                var visibleNode = _getField(taskNode, "visible");

                _validateType(descriptionNode, "description");
                _validateType(visibleNode, "visible");

                tasks[i] = new QuestTask(descriptionNode.Value, visibleNode.AsBool);
            }

            return new Quest(title, tasks, notes);


        }
    }
}