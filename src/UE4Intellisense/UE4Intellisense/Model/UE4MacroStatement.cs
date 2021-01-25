using Microsoft.VisualStudio.Text;

namespace UE4Intellisense.Model
{
    internal class UE4MacroStatement
    {
        public UE4MacroStatement(SnapshotSpan specifiersSpan, UE4Macros macroConst)
        {
            SpecifiersSpan = specifiersSpan;
            MacroConst = macroConst;
        }

        public UE4Macros MacroConst { get; }
        public SnapshotSpan SpecifiersSpan { get; }

        public string[] Specifiers { get; set; }
        public string[] MetaSpecifiers { get; set; }
    }
}
