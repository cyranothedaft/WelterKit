using System;


namespace WelterKit.FunctionalContainers_tests.Theory;


internal static class TestValues {
   internal static readonly int    [] _int     = [int.MinValue, -42, -1, 0, 1, 42, int.MaxValue];
   internal static readonly float  [] _float   = [float.NegativeInfinity, float.MinValue, -42.42e10f, -42.42f, -42.42e-10f, -float.Epsilon, float.NegativeZero, 0, 42.42e-10f, 42.42f, 42.42e10f, float.MaxValue, float.PositiveInfinity, float.NaN];
   internal static readonly string [] _string  = [string.Empty, "1", "abc XYZ", new('*', 420)];
   internal static readonly string?[] _stringn = [string.Empty, "1", "abc XYZ", new('*', 420), null];
   // TODO: more...?
}


internal static class TestFunctions {
   internal static readonly (Func<int, string>                      h, Func<string, string>               g) IntToStringToStringSet1           = (h: IntToString1             , g: StringToString1);
   internal static readonly (Func<float, int>                       h, Func<int, string>                  g) FloatToIntToStringSet1            = (h: FloatToInt1              , g: IntToString2);
   internal static readonly (Func<string, int>                      h, Func<int, TimeSpan>                g) StringToIntToTimeSpanSet1         = (h: StringToInt1             , g: IntToTimeSpan1);
   internal static readonly (Func<string?, (bool isNull, string s)> h, Func<(bool isNull, string s), int> g) StringNToBoolStringTupleToIntSet1 = (h: StringNToBoolStringTuple1, g: BoolStringTupleToInt1);

   internal static readonly (Func<int, string>                      h, Func<string, string>               g)[] IntToStringToStringFuncs           = [ IntToStringToStringSet1           ];
   internal static readonly (Func<float, int>                       h, Func<int, string>                  g)[] FloatToIntToStringFuncs            = [ FloatToIntToStringSet1            ];
   internal static readonly (Func<string, int>                      h, Func<int, TimeSpan>                g)[] StringToIntToTimeSpanFuncs         = [ StringToIntToTimeSpanSet1         ];
   internal static readonly (Func<string?, (bool isNull, string s)> h, Func<(bool isNull, string s), int> g)[] StringNToBoolStringTupleToIntFuncs = [ StringNToBoolStringTupleToIntSet1 ];
   // TODO: more...?

   internal static string IntToString1(int x) => x.ToString();
   internal static string StringToString1(string x) => x + "$";
   internal static int FloatToInt1(float x) => (int)x;
   internal static string IntToString2(int x) => x + "int";
   internal static int StringToInt1(string x) => x.Length;
   internal static TimeSpan IntToTimeSpan1(int x) => TimeSpan.FromMinutes(x);
   internal static (bool isNull, string s) StringNToBoolStringTuple1(string? x) => x is null ? (true, string.Empty) : (false, x!);
   internal static int BoolStringTupleToInt1((bool isNull, string s) x) => x.isNull ? -1 : x.s.Length;
}
