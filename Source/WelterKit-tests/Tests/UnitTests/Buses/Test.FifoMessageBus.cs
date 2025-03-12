using System;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.Buses;
using WelterKit.StaticUtilities;


namespace WelterKit_Tests.Tests_Unit.Buses;

[TestClass()]
public class Test_FifoMessageBus : Test_MessageBusBase {

   private readonly ILogger _logger = new MyDebugLogger();

   protected override IMessageBus<object> CreateTestBus()
      => new FifoMessageBus<object>(_logger?.WithPrefix("[bus] "));
}
