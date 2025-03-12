using System;
using System.IO;
using Newtonsoft.Json;



namespace WelterKit.Testing {
   public class TestFilesMetadata {
      public TestFileMetadata[] entries;

      public static TestFilesMetadata ReadFromFile(string metadataFilePath)
         => JsonConvert.DeserializeObject<TestFilesMetadata>(readText(metadataFilePath));

      private static string readText(string metadataFilePath)
         => File.ReadAllText(metadataFilePath);
   }


   public class TestFileMetadata {
      public string type;
      public string timestamp;
      public string filepath;
   }
}
