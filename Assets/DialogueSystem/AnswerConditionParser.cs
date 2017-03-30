using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using QuestSystem;
using SimpleJSON;
using Utils;

namespace DialogueSystem
{
    public class AnswerConditionParser : Parser
    {

        public static readonly Dictionary<string, int> _FIELD_CONSTRAINTS = new Dictionary<string, int>()
        {
            {"quests", _OBJECT},
            {"title",_STRING},
            {"done_tasks", _ARRAY | _NOT_EXIST},
            {"undone_tasks", _ARRAY | _NOT_EXIST},
            {"started", _BOOL | _NOT_EXIST},
        };


        protected new static JSONNode _getField(JSONNode node, string fieldName)
        {
            return _getFieldGeneric(node, fieldName, (str) => new DialogueParseException("Dialogue " + str));
        }

        protected new static  void  _validateType(JSONNode node, string fieldName)
        {
            _validateTypeGeneric(node, fieldName, (str) => new DialogueParseException(str), () => _FIELD_CONSTRAINTS);
        }

        private static IEnumerable<uint> _parseTasksCondition(JSONNode questsNode, string key )
        {
            if (!questsNode.HasKey(key)) return new uint[0];

            var doneTasksNode = questsNode[key];
            _validateType(doneTasksNode,key);

            var tasksJsonArray = doneTasksNode.AsArray;

            uint[] tasksArray = new uint[tasksJsonArray.Count];

            for (int i = 0; i < tasksJsonArray.Count; i++)
            {
                tasksArray[i] = (uint) tasksJsonArray.AsInt;
            }

            return tasksArray;
        }

        public static Func<bool> Parse(JSONNode json)
        {
            Func<bool, bool> template = (prev) => true;

            var questsNode = _getField(json, "quests");
            _validateType(questsNode, "quests");

            var titleNode = _getField(questsNode, "title");
            _validateType(titleNode, "title");
            string title = titleNode.Value;


            IEnumerable<uint> doneTasks = _parseTasksCondition(questsNode, "done_tasks");
            IEnumerable<uint> undoneTaks = _parseTasksCondition(questsNode, "undone_tasks");

            template += (prev) => prev && QuestManager.CheckTaskComplete(title, doneTasks, undoneTaks);

            if (questsNode.HasKey("started"))
            {
                var startedNode = questsNode["started"];
                _validateType(startedNode, "started");
                bool started = startedNode.AsBool;
                template += (prev) => (!started ^ QuestManager.IsQuestStarted(title));
            }

            return () => template(true);
        }
    }
}