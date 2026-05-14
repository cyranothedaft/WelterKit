using System;
using System.Collections.Generic;
using System.Linq;
using WelterKit.FunctionalContainers_tests.Theory;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;


namespace WelterKit.FunctionalContainers_tests.ContainerTests.Either_tests;

[TestClass]
public abstract class Laws_Functor<L> : Laws_Functor_Tests<Either<L>> {
   // internal override ILawsTestData<Either<L>> TestData { get; } = new LawsTestData<L>();
}



[TestClass]
public class Laws_Functor_string : Laws_Functor<string> {
   internal override ILawsTestData<Either<string>> TestData { get; } = new LawsTestData_LeftString();
}


[TestClass]
public class Laws_Functor_record : Laws_Functor<LawsTestData_LeftRecord.CustomRecord> {
   internal override ILawsTestData<Either<LawsTestData_LeftRecord.CustomRecord>> TestData { get; } = new LawsTestData_LeftRecord();
}
