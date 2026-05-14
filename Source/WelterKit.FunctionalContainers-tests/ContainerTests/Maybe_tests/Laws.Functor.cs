using System;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Maybe_tests;

[TestClass]
public class Laws_Functor : Laws_Functor_Tests<Maybe> {
   internal override ILawsTestData<Maybe> TestData { get; } = new LawsTestData();
}
