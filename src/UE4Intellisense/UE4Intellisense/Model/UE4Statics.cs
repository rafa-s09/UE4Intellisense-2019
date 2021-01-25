namespace UE4Intellisense.Model
{
    internal static class UE4Statics
    {
        public static string MacroNamesRegExPatern => string.Join("|", typeof(UE4Macros).GetEnumNames());
    }
}
