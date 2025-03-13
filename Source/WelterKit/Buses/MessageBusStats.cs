using System;

namespace WelterKit.Buses;

public record MessageBusStats(
      int TotalMessageCount,
      int UnroutableMessageCount,
      int LeftoverMessageCount
);
