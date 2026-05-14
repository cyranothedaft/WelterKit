using System;
using WelterKit.FunctionalContainers_tests.ContainerTests.Either_tests;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.KList_tests;

[TestClass]
public class Laws_Functor : Laws_Functor_Tests<KList> {
   internal override ILawsTestData<KList> TestData { get; } = new LawsTestData();
}
