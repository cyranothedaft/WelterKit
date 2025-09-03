using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;


namespace WelterKit.Telemetry.Logging.MinimalFile;

public static class MinimalFileLoggerExtensions {
   public static ILoggingBuilder AddFile(this ILoggingBuilder builder) {
      builder.AddConfiguration();

      builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, MinimalFileLoggerProvider>());
      builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<MinimalFileLoggerOptions>, MinimalFileLoggerOptionsSetup>());
      builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<MinimalFileLoggerOptions>, LoggerProviderOptionsChangeTokenSource<MinimalFileLoggerOptions, MinimalFileLoggerProvider>>());
      return builder;
   }


   public static ILoggingBuilder AddFile(this ILoggingBuilder builder, Action<MinimalFileLoggerOptions> configure) {
      if (configure == null) {
         throw new ArgumentNullException(nameof( configure ));
      }

      builder.AddFile();
      builder.Services.Configure(configure);

      return builder;
   }
}
