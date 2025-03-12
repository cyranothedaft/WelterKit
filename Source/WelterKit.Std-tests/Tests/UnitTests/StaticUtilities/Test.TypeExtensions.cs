using System;
using System.Collections.Generic;
using System.Diagnostics;
using WelterKit.Std.StaticUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WelterKit_Tests.Tests.UnitTests.StaticUtilities {
   [TestClass]
   [TestCategory("Unit")]
   public class Test_TypeExtensions {
      [TestMethod]
      public void GetFriendlyType_String() {
         Type type = typeof( string );
         Debug.WriteLine(type.FullName);
         // FullName: System.String
         Assert.AreEqual(       "String", type.GetFriendlyName(0));
         Assert.AreEqual("System.String", type.GetFriendlyName(1));
         Assert.AreEqual("System.String", type.GetFriendlyName( ));
         Assert.AreEqual("System.String", type.GetFriendlyName(2));
         Assert.AreEqual("System.String", type.GetFriendlyName(3));
         Assert.AreEqual("System.String", type.GetFriendlyName(4));
         Assert.AreEqual("System.String", type.GetFriendlyName(5));
      }


      [TestMethod]
      public void GetFriendlyType_ASCIIEncoding() {
         Type type = System.Text.Encoding.ASCII.GetType();
         Debug.WriteLine(type.FullName);
         // FullName: System.Text.ASCIIEncoding+ASCIIEncodingSealed
         Assert.AreEqual(            "ASCIIEncodingSealed", type.GetFriendlyName(0));
         Assert.AreEqual(       "Text.ASCIIEncodingSealed", type.GetFriendlyName(1));
         Assert.AreEqual(       "Text.ASCIIEncodingSealed", type.GetFriendlyName( ));
         Assert.AreEqual("System.Text.ASCIIEncodingSealed", type.GetFriendlyName(2));
         Assert.AreEqual("System.Text.ASCIIEncodingSealed", type.GetFriendlyName(3));
         Assert.AreEqual("System.Text.ASCIIEncodingSealed", type.GetFriendlyName(4));
         Assert.AreEqual("System.Text.ASCIIEncodingSealed", type.GetFriendlyName(5));
      }


      [TestMethod]
      public void GetFriendlyType_CertificateRequest() {
         Type type = typeof( System.Security.Cryptography.X509Certificates.CertificateRequest );
         Debug.WriteLine(type.FullName);
         // FullName: System.Security.Cryptography.X509Certificates.CertificateRequest
         Assert.AreEqual(                                              "CertificateRequest", type.GetFriendlyName(0));
         Assert.AreEqual(                             "X509Certificates.CertificateRequest", type.GetFriendlyName(1));
         Assert.AreEqual(                             "X509Certificates.CertificateRequest", type.GetFriendlyName( ));
         Assert.AreEqual(                "Cryptography.X509Certificates.CertificateRequest", type.GetFriendlyName(2));
         Assert.AreEqual(       "Security.Cryptography.X509Certificates.CertificateRequest", type.GetFriendlyName(3));
         Assert.AreEqual("System.Security.Cryptography.X509Certificates.CertificateRequest", type.GetFriendlyName(4));
         Assert.AreEqual("System.Security.Cryptography.X509Certificates.CertificateRequest", type.GetFriendlyName(5));
      }


      [TestMethod]
      public void GetFriendlyType_Generic_List_String() {
         Type type = typeof( List<string> );
         Debug.WriteLine(type.FullName);
         // FullName: System.Collections.Generic.List`1[[System.String, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]
         Assert.AreEqual(                           "List<"+       "String>", type.GetFriendlyName(0));
         Assert.AreEqual(                   "Generic.List<"+"System.String>", type.GetFriendlyName(1));
         Assert.AreEqual(                   "Generic.List<"+"System.String>", type.GetFriendlyName( ));
         Assert.AreEqual(       "Collections.Generic.List<"+"System.String>", type.GetFriendlyName(2));
         Assert.AreEqual("System.Collections.Generic.List<"+"System.String>", type.GetFriendlyName(3));
         Assert.AreEqual("System.Collections.Generic.List<"+"System.String>", type.GetFriendlyName(4));
         Assert.AreEqual("System.Collections.Generic.List<"+"System.String>", type.GetFriendlyName(5));
      }
   }
}
