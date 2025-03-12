using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WelterKit.Std.Functional;
using WelterKit.Std.StaticUtilities;
using WelterKit.StaticUtilities;
using Microsoft.Extensions.Logging;



namespace WelterKit.Buses;


public interface ISubscriberListReader<TMsg> {
   int Count { get; }
   Maybe<ISubscriber<TMsg>> TryGet(TMsg msg);
}


internal class SubscriberList<TMsg> : ISubscriberListReader<TMsg> {

   private readonly ILogger? _logger = null;

   private readonly ImmutableList<ISubscriber<TMsg>> _list;


   public SubscriberList(ILogger? logger = null)
         : this(ImmutableList<ISubscriber<TMsg>>.Empty, logger) {
      _logger?.LogTrace("constructed (default)");
   }


   private SubscriberList(ImmutableList<ISubscriber<TMsg>> list, ILogger? logger = null) {
      _list   = list;
      _logger = logger;
      _logger?.LogTrace("constructed - list: {list}", list);
   }


   public int Count => _list.Count;


   public bool Contains<TSubMsg>() {
      bool result = containsActual<TSubMsg>();
      _logger?.LogTrace("Contains submsg type <{type}> - returning {result} (_list: {list})", typeof( TSubMsg ), result, _list);
      return result;
   }


   private bool containsActual<TSubMsg>()
      => _list.Any(entry => entry.HandlesType<TSubMsg>());


   public Maybe<ISubscriber<TMsg>> TryGet(TMsg msg) {
      Maybe<ISubscriber<TMsg>> result = tryGetActual(msg);
      _logger?.LogTrace("TryGet handler for msg {msg} - returning {result} (_list: {list})", msg, result, _list);
      return result;
   }


   private Maybe<ISubscriber<TMsg>> tryGetActual(TMsg msg)
      => _list.FirstOrNone(sub => sub.Handles(msg));


   public SubscriberList<TMsg> TryAdd<TSubMsg>(HandleSpecificMessageDelegate<TSubMsg> handleSpecificMessage, out bool success) {
      var result = tryAddActual(handleSpecificMessage, out success);
      _logger?.LogTrace("TryAdd handler for submsg type <{type}> - returning {result}, success: {success}", typeof( TSubMsg ), result, success);
      return result;
   }


   private SubscriberList<TMsg> tryAddActual<TSubMsg>(HandleSpecificMessageDelegate<TSubMsg> handleSpecificMessage, out bool success) {
      if ( Contains<TSubMsg>() ) {
         success = false;
         return this;
      }
      else {
         success = true;
         return new SubscriberList<TMsg>(addSubscriberToList(_list, new Subscriber<TMsg, TSubMsg>(handleSpecificMessage), _logger), _logger);
      }
   }


   private static ImmutableList<ISubscriber<TMsg>> addSubscriberToList(ImmutableList<ISubscriber<TMsg>> list, ISubscriber<TMsg> subscriber, ILogger? logger) {
      var result = addSubscriberToList(list, subscriber);
      logger?.LogTrace("addSubscriberToListActual(list: {list}, subscriber: {subscriber}) - returning: {result}", list, subscriber, result);
      return result;
   }

   private static ImmutableList<ISubscriber<TMsg>> addSubscriberToList(ImmutableList<ISubscriber<TMsg>> list, ISubscriber<TMsg> subscriber)
      => list.Add(subscriber);
}
