using System;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;


namespace WelterKit.Telemetry.Logging.MinimalFile;

internal class MinimalFileLoggerOptionsSetup : ConfigureFromConfigurationOptions<MinimalFileLoggerOptions> {
   public MinimalFileLoggerOptionsSetup(ILoggerProviderConfiguration<MinimalFileLoggerProvider>
                                       providerConfiguration)
         : base(providerConfiguration.Configuration) { }
}
