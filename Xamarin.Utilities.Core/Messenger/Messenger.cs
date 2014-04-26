using System;
using TinyMessenger;

namespace Xamarin.Utilities.Core.Messenger
{
    public static class Messenger
    {
        private static readonly Lazy<TinyMessengerHub> TinyMessenger = new Lazy<TinyMessengerHub>();

        public static void Publish<TMessage>(TMessage message) where TMessage : MessageBase
        {
            TinyMessenger.Value.Publish<TMessage>(message);
        }

        public static object Subscribe<TMessage>(Action<TMessage> messageAction) where TMessage : MessageBase
        {
            return TinyMessenger.Value.Subscribe<TMessage>(messageAction, false);
        }
    }

    public abstract class MessageBase : TinyMessageBase
    {
        protected MessageBase(object sender) 
            : base(sender)
        {
        }
    }
}