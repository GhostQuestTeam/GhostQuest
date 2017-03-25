using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DialogueSystem;
using Extensions;
using SimpleJSON;

namespace Utils
{
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
        }
    }

    //TODO Переделать парсеры в синглтоны с нормальным наследованием
    public class Parser
    {
        protected const int _NUMBER = 1;
        protected const int _BOOL = 2;
        protected const int _OBJECT = 4;
        protected const int _ARRAY = 8;
        protected const int _NULL = 16;
        protected const int _STRING = 32;
        protected const int _NOT_EXIST = 64;

        protected const int _MAX_CONSTRAINT = 32;

        protected delegate bool CheckTypeDelegate(JSONNode node);


        //private static readonly Dictionary<string, int> _FIELD_CONSTRAINTS = new Dictionary<string, int>();


        protected static readonly Dictionary<int, CheckTypeDelegate> _CONSTRAINTS_CHECKS =
            new Dictionary<int, CheckTypeDelegate>()
            {
                {_OBJECT, (node) => node.IsObject},
                {_ARRAY, (node) => node.IsArray},
                {_BOOL, (node) => node.IsBoolean},
                {_STRING, (node) => node.IsString},
                {_NUMBER, (node) => node.IsNumber},
                {_NULL, (node) => node.IsNull},
            };

        protected static string _getErrorMessage(string field, Dictionary<string, int> fieldsConstraints)
        {
            if (!fieldsConstraints.ContainsKey(field))
            {
                return "field " + field + " not supported";
            }
            string messageTemplate = "field " + field + " must have type in list:";
            int fieldConstraints = fieldsConstraints[field];
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

        protected static JSONNode _getField(JSONNode node, string fieldName)
        {
            return _getFieldGeneric(node, fieldName, (str) => new ParseException(str));
        }


        protected static JSONNode _getFieldGeneric<TException>(JSONNode node, string fieldName,
            Func<string, TException> creator)
            where TException : Exception
        {
            if (!node.HasKey(fieldName))
            {
                throw creator("node must have field " + fieldName);
            }
            return node[fieldName];
        }

        protected static void _validateType(JSONNode node, string fieldName)
        {
            _validateTypeGeneric(node, fieldName, (str) => new ParseException(str), () => new Dictionary<string, int>());
        }

        protected static void _validateTypeGeneric<TException>(JSONNode node, string fieldName,
            Func<string, TException> creator, Func<Dictionary<string, int>> getFieldConstraints) where TException : Exception
        {
            if (getFieldConstraints().ContainsKey(fieldName))
            {
                int fieldContraints = getFieldConstraints()[fieldName];
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
            throw creator(_getErrorMessage(fieldName, getFieldConstraints()));
        }
    }
}