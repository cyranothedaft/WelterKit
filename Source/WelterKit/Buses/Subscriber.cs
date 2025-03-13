using System;


namespace WelterKit.Buses;

public interface ISubscriber<in TMsg> {
   bool IsFor<TSubMsg>(TMsg msg) => msg is TSubMsg;
   bool Handles(TMsg msg);
   bool HandlesType<TSubMsg>();
   /// <summary>
   /// Returns true if message is handled because it's the right type; false otherwise.
   /// </summary>
   bool HandleMessage(TMsg msg);
}


internal record Subscriber<TMsg, TSubMsg>(HandleSpecificMessageDelegate<TSubMsg> HandleSpecificMessage) : ISubscriber<TMsg> {
   public bool Handles(TMsg msg) => ( ( ISubscriber<TMsg> )this ).IsFor<TSubMsg>(msg);
   public bool HandlesType<TSubMsgAsk>() => typeof( TSubMsgAsk ).IsAssignableTo(typeof( TSubMsg ));

   public bool HandleMessage(TMsg msg) {
      if ( msg is TSubMsg subMsg ) {
         HandleSpecificMessage(subMsg);
         return true;
      }
      return false;
   }
}


// internal abstract class SubscriberBase<TMsg> {
//    public bool IsFor<TSubMsg>(TMsg msg) => msg is TSubMsg;
//    public abstract bool Handles(TMsg msg);
//    public abstract bool HandlesType<TSubMsg>();
//    /// <summary>
//    /// Returns true if message is handled because it's the right type; false otherwise.
//    /// </summary>
//    public abstract bool HandleMessage(TMsg msg);
// }
//
//
// internal sealed class Subscriber<TMsg, TSubMsg> : SubscriberBase<TMsg> {
//    private readonly Action<TSubMsg> _handleSpecificMessage;
//
//    public Subscriber(Action<TSubMsg> handleSpecificMessage) {
//       _handleSpecificMessage = handleSpecificMessage;
//    }
//
//    public override bool Handles(TMsg msg) => ( ( SubscriberBase<TMsg> )this ).IsFor<TSubMsg>(msg);
//    public override bool HandlesType<TSubMsgAsk>() => typeof( TSubMsgAsk ).IsAssignableTo(typeof( TSubMsg ));
//
//
//    public override bool HandleMessage(TMsg msg) {
//       if ( msg is TSubMsg subMsg ) {
//          _handleSpecificMessage(subMsg);
//          return true;
//       }
//       return false;
//    }
// }
