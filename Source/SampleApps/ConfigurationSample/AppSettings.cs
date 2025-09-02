using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using WelterKit.Std.Functional;
using WelterKit.Std.StaticUtilities;



namespace ConfigurationSample;


public enum SettingSource {
   // value indicates priority (lower number is 'higher' priority; e.g., 0 is higher than 1)
   EnvironmentVariable = 2,
   AppSettingsFile     = 3,
   None                = int.MaxValue
}


public static class AppSettings {

   private static readonly IEqualityComparer<string> KeyComparer = StringComparer.OrdinalIgnoreCase;


   public static IEnumerable<(string key, string? value, SettingSource source)> GetSettings(ILogger? logger, string envPrefix, Func<Maybe<string>> fileReader,
                                                                                            params HashSet<string> keys)
      => ingestSettings(logger, envPrefix, fileReader, keys)
           .coalesceSettings(logger);


   private static IEnumerable<(string key, string? value, SettingSource source)> ingestSettings(ILogger? logger, string envPrefix, Func<Maybe<string>> fileReader,
                                                                                                HashSet<string> keys) {
      IEnumerable<(string key, string? value)> fromEnv  = getValuesFromEnvironment(logger, envPrefix, keys);
      IEnumerable<(string key, string? value)> fromFile = readSettingsFromFile(logger, fileReader, keys);

      return         fromEnv .Select(tuple => (tuple.key, tuple.value, SettingSource.EnvironmentVariable))
             .Concat(fromFile.Select(tuple => (tuple.key, tuple.value, SettingSource.AppSettingsFile))
             );
   }


   private static IEnumerable<(string key, string? value, SettingSource source)> coalesceSettings(this IEnumerable<(string key, string? value, SettingSource source)> valueTuples,
                                                                                                  ILogger? logger)
      => valueTuples.GroupBy(t => t.key, KeyComparer)
                    .Select(grouping => grouping.OrderBy(g => (int)g.source) // 'highest' priority first
                                                .FirstOrNone(g => g.value is not null) // use the first non-null value, if any
                                                .Reduce((grouping.Key, null, SettingSource.None))); // yield null if no non-null value is found


   private static IEnumerable<(string, string?)> getValuesFromEnvironment(ILogger? logger, string envPrefix, IEnumerable<string> keys)
      => keys.Select(key => (key, getValueFromEnvironment(logger, envPrefix, key)));


   private static string? getValueFromEnvironment(ILogger? logger, string envPrefix, string key)
      => Environment.GetEnvironmentVariable(envPrefix + key);


   private static IEnumerable<(string, string?)> readSettingsFromFile(ILogger? logger, Func<Maybe<string>> fileReader, HashSet<string> keys) {
      Maybe<IReadOnlyCollection<(string, string?)>> settings_ = readSettingsFileJson(logger, fileReader);
      return settings_.Map(settings => settings.Where(((string key, string?) setting) => keys.Contains(setting.key))) // TODO: optimize with IntersectBy?
                      .Reduce([]);
   }


   private static Maybe<IReadOnlyCollection<(string, string?)>> readSettingsFileJson(ILogger? logger, Func<Maybe<string>> fileReader) {
      Maybe<string> json_ = fileReader();
      return json_.Map(IReadOnlyCollection<(string, string?)> (json)
                             => enumSettings(json).ToImmutableList());

      static IEnumerable<(string, string?)> enumSettings(string json) {
         JsonObject? jsonObject = JsonNode.Parse(json)
                                          .AsObject();
         return jsonObject.Select(x => (x.Key, x.Value.ToString()));
      }
   }
}
