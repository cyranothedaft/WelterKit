using System;


namespace WelterKit.Buses;

public interface IPublisher<TMsg> {
   void SubscribeTypedMessageHandler<TSubMsg>(HandleSpecificMessageDelegate<TSubMsg> handleMessage,
                                              string? callerMemberName = null) where TSubMsg : TMsg;

   ISubscriberListReader<TMsg> GetSubscriberListReader();
}
