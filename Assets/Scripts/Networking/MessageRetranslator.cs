using System;
using System.Collections.Generic;
using GameSparks.Api.Messages;
using GameSparks.Core;

namespace HauntedCity.Networking
{
    public delegate void MessageHandler(GSData data);

    public enum MessageType
    {
        PLAYER_STATS_UPDATE        
    }

    public class MessageRetranslator
    {
        private readonly Dictionary<MessageType, HashSet<MessageHandler>> _subscriptions = new Dictionary<MessageType, HashSet<MessageHandler>>();

        public MessageRetranslator()
        {
            ScriptMessage.Listener += ScriptMessageListener;
            foreach (MessageType type in Enum.GetValues(typeof(MessageType)))
            {
                _subscriptions.Add(type, new HashSet<MessageHandler>());
            }
        }

        void ScriptMessageListener(ScriptMessage message)
        {
            MessageType type = (MessageType) Enum.Parse(typeof(MessageType), message.Data.GetString("type"));
            var data = message.Data.GetGSData("data");

            foreach (MessageHandler handler in _subscriptions[type])
            {
                handler(data);
            }
        }

        public void Subscribe(MessageType type, MessageHandler handler)
        {
            _subscriptions[type].Add(handler);
        }

        public void Unsubscribe(MessageType type, MessageHandler handler)
        {
            _subscriptions[type].Remove(handler);
        }
    }
}