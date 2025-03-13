using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using WelterKit.Std;
using WelterKit.Std.Diagnostics;
using WelterKit.Std.StaticUtilities;



namespace WelterKit.Testing {
   public static class TestUtil {
      public const string MetadataFileName = ".metadata.json";


      public static void RecoverTestState(string testName, string testArchiveDir, string targetDir, bool sourceIsZip) {
         testArchiveDir = FileSystemUtil.ResolveFullPath(testArchiveDir);
         targetDir = FileSystemUtil.ResolveFullPath(targetDir);

         Logger.L.Write(string.Format("Test state recovering from {0}:  \"{1}\" in \"{2}\" to \"{3}\"",
                                      sourceIsZip ? "zip" : "directory", testName, testArchiveDir, targetDir));

         string sourcePath = null;
         if ( sourceIsZip ) {
            sourcePath = Path.Combine(testArchiveDir, testName + ".zip");
            if ( !File.Exists(sourcePath) )
               throw new FileNotFoundException("Cannot recover from test archive file; it doesn't exist:  " + sourcePath, sourcePath);
         }
         else {
            sourcePath = Path.Combine(testArchiveDir, "repos", testName);
            if ( !Directory.Exists(sourcePath) )
               throw new DirectoryNotFoundException("Cannot recover from test directory; it doesn't exist:  " + sourcePath);
         }

         // purge contents of target directory
         string targetTestDir = Path.Combine(targetDir, testName);
         FileSystemUtil.PurgeDir(targetTestDir, Logger.L.Write);

         if ( sourceIsZip )
            ZipFile.ExtractToDirectory(sourcePath, targetDir);
         else
            FileSystemUtil.CopyDirContents(sourcePath, targetDir);

         updateFilesMetadata(targetDir);
         
         Logger.L.Write(string.Format("Test state recovered from {0}:  \"{1}\"  ->  {2}",
                                      sourceIsZip ? "zip" : "directory", sourcePath, targetDir));
         List<FileSysItem> contents = FileSystemUtil.EnumFileSysItemsRecursively(targetDir).ToList();
         Logger.L.Write($"  {contents.Count} items:\r\n" + string.Join("\r\n", contents.Select(f => $"  {f.GetPathRelativeTo(targetDir)}{( f.Type == FileSysItemType.Directory ? "\\" : "" )}")));
      }



      private static void updateFilesMetadata(string targetDir) {
         string metadataFilePath = Path.Combine(targetDir, MetadataFileName);
         if ( File.Exists(metadataFilePath) ) {
            Logger.L.Write($"{MetadataFileName} file detected - updating file/dir metadata...");
            var testFilesMetadata = TestFilesMetadata.ReadFromFile(metadataFilePath);
            foreach ( TestFileMetadata entry in testFilesMetadata.entries )
               updateFileMetadata(new BasedFilePath(targetDir, entry.filepath),
                                  entry.type == "d",
                                  entry.timestamp);
         }
         else
            Logger.L.Write($"{MetadataFileName} file not detected");
      }


      private static void updateFileMetadata(BasedFilePath filePath, bool isDir, string timeStampStr) {
         string fullFilePath = filePath.ToString();
         bool hasTimeStamp = !string.IsNullOrEmpty(timeStampStr);
         DateTime newTimeStamp = hasTimeStamp
                                    ? timeStampStr.ParseDateTimeForSync_utc()
                                    : DateTime.MinValue;
         if ( isDir ) {
            // ensure directory has been created
            if ( !Directory.Exists(fullFilePath) ) {
               Directory.CreateDirectory(fullFilePath);
               Logger.L.Write($@"  created dir                  :  {filePath}");
            }

            if ( hasTimeStamp ) {
               Directory.SetLastWriteTimeUtc(fullFilePath, newTimeStamp);
               Logger.L.Write($@"  {newTimeStamp.FormatForSync()} <- {filePath}");
            }
         }
         else {
            if ( hasTimeStamp ) {
               File.SetLastWriteTimeUtc(fullFilePath, newTimeStamp);
               Logger.L.Write($@"  {newTimeStamp.FormatForSync()} <- {filePath}");
            }
         }
      }


      public static string[][] ConstructFileListPairs(string parentDir1, string parentDir2, string[] relFilePaths) {
         return relFilePaths.Select(f => new string[]
                                               {
                                                     Path.Combine(parentDir1, f),
                                                     Path.Combine(parentDir2, f)
                                               })
                            .ToArray();
      }


//      /// <summary>
//      /// Compares the deserialized objects represented by the given JSON strings.
//      /// </summary>
//      public static void TestJsonStrings(string jsonStrExpected, string jsonStrActual) {
//         object jsonObjExpected = JsonConvert.DeserializeObject(jsonStrExpected),
//                jsonObjActual = JsonConvert.DeserializeObject(jsonStrActual);
//         Assert.AreEqual(jsonObjExpected, jsonObjActual);
//      }


      //
//      /// <summary>
//      /// Essentially produces a canonicalized version of the given JSON string
//      /// by removing whitespace and ordering elements.
//      /// </summary>
//      public static string CononicalizeJson(string jsonStr) {
//         var jsonObj = JsonConvert.DeserializeObject(jsonStr);
//         string newJsonStr = JsonConvert.SerializeObject(jsonObj, new JsonSerializerSettings()
//                                                                        {
//                                                                              Formatting = Formatting.None,
//                                                                              ContractResolver = new OrderedContractResolver(),
//                                                                        });
//         return newJsonStr;
//      }
//
//
////      private class OrderedContractResolver : DefaultContractResolver {
//         public override JsonContract ResolveContract(Type type) {
//            return base.ResolveContract(type);
//         }
//
//
//         protected override List<MemberInfo> GetSerializableMembers(Type objectType) {
//            return base.GetSerializableMembers(objectType);
//         }
//
//
//         protected override JsonObjectContract CreateObjectContract(Type objectType) {
//            return base.CreateObjectContract(objectType);
//         }
//
//
//         protected override IList<JsonProperty> CreateConstructorParameters(ConstructorInfo constructor, JsonPropertyCollection memberProperties) {
//            return base.CreateConstructorParameters(constructor, memberProperties);
//         }
//
//
//         protected override JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, ParameterInfo parameterInfo) {
//            return base.CreatePropertyFromConstructorParameter(matchingMemberProperty, parameterInfo);
//         }
//
//
//         protected override JsonConverter ResolveContractConverter(Type objectType) {
//            return base.ResolveContractConverter(objectType);
//         }
//
//
//         protected override JsonDictionaryContract CreateDictionaryContract(Type objectType) {
//            return base.CreateDictionaryContract(objectType);
//         }
//
//
//         protected override JsonArrayContract CreateArrayContract(Type objectType) {
//            return base.CreateArrayContract(objectType);
//         }
//
//
//         protected override JsonPrimitiveContract CreatePrimitiveContract(Type objectType) {
//            return base.CreatePrimitiveContract(objectType);
//         }
//
//
//         protected override JsonLinqContract CreateLinqContract(Type objectType) {
//            return base.CreateLinqContract(objectType);
//         }
//
//
//         protected override JsonISerializableContract CreateISerializableContract(Type objectType) {
//            return base.CreateISerializableContract(objectType);
//         }
//
//
//         protected override JsonDynamicContract CreateDynamicContract(Type objectType) {
//            return base.CreateDynamicContract(objectType);
//         }
//
//
//         protected override JsonStringContract CreateStringContract(Type objectType) {
//            return base.CreateStringContract(objectType);
//         }
//
//
//         protected override JsonContract CreateContract(Type objectType) {
//            return base.CreateContract(objectType);
//         }
//
//
//         protected override IValueProvider CreateMemberValueProvider(MemberInfo member) {
//            return base.CreateMemberValueProvider(member);
//         }
//
//
//         protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
//            return base.CreateProperty(member, memberSerialization);
//         }
//
//
//         protected override string ResolvePropertyName(string propertyName) {
//            return base.ResolvePropertyName(propertyName);
//         }
//
//
//         protected override string ResolveExtensionDataName(string extensionDataName) {
//            return base.ResolveExtensionDataName(extensionDataName);
//         }
//
//
//         protected override string ResolveDictionaryKey(string dictionaryKey) {
//            return base.ResolveDictionaryKey(dictionaryKey);
//         }
//
//
//         protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
//            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
//         }
//      }
      /// <summary>
      /// </summary>
      /// <param name="s"></param>
      /// <param name="encoding">Defaults to Encoding.UTF8</param>
      public static Stream CreateStreamForString(string s, Encoding encoding = null) {
         return new MemoryStream(( encoding ?? Encoding.UTF8 ).GetBytes(s));
      }
   }
}
