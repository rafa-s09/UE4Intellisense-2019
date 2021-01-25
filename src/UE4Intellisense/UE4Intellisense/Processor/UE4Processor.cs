using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text.Operations;
using UE4Intellisense.Model;

namespace UE4Intellisense.Processor
{
    internal class UE4Processor
    {
        private readonly ITextStructureNavigator _navigator;

        public UE4Processor(ITextStructureNavigator navigator)
        {
            _navigator = navigator;
        }

        public bool TryGetUE4Macro(SnapshotPoint triggerPoint, out UE4MacroStatement ue4MacroStatement)
        {
            ue4MacroStatement = null;
            SnapshotPoint currentPoint = triggerPoint - 1;
            TextExtent extent = _navigator.GetExtentOfWord(currentPoint);
            SnapshotSpan statement = _navigator.GetSpanOfEnclosing(extent.Span);
            string statementText = statement.GetText();

            Match match = Regex.Match(statementText, $@"({UE4Statics.MacroNamesRegExPatern})\((.*)\)", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100));
            if (!match.Success)
                return false;
            if (!match.Groups[1].Success || !match.Groups[2].Success)
                return false;

            SnapshotPoint contentPosition = statement.Start + match.Groups[2].Index;
            SnapshotPoint contentEnd = contentPosition + match.Groups[2].Length;
            SnapshotSpan specifiersSpan = new SnapshotSpan(contentPosition, contentEnd);
            UE4Macros macro = (UE4Macros)Enum.Parse(typeof(UE4Macros), match.Groups[1].Value.ToUpper());
            ue4MacroStatement = new UE4MacroStatement(specifiersSpan, macro);

            return true;
        }

        public void ParseSpecifiers(SnapshotPoint triggerPoint, ref UE4MacroStatement macroStatement, out bool inMeta)
        {
            inMeta = false;
            if (string.IsNullOrWhiteSpace(macroStatement?.SpecifiersSpan.GetText())) 
                return;

            string inputstr = macroStatement.SpecifiersSpan.GetText();
            SnapshotPoint currentPoint = triggerPoint - 1;
            MatchCollection matchSpecs = Regex.Matches(inputstr, @"meta\s*=\s*\(([\w\s=""]+\,?)*\)|(\w+\s*=?\s*[\w""]*)\,?", RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(1000));
            List<string> specifiersList = new List<string>();
            List<string> metaList = new List<string>();

            foreach (var spec in matchSpecs)
            {
                Match mm = (Match)spec;

                if (mm.Groups[1].Success)
                {
                    SnapshotPoint metaPositionStart = macroStatement.SpecifiersSpan.Start + mm.Groups[1].Index;
                    SnapshotPoint metaPositionEnd = metaPositionStart + mm.Groups[1].Length;

                    if (!inMeta)
                        inMeta = currentPoint >= metaPositionStart && currentPoint <= metaPositionEnd;

                    foreach (var ms in mm.Groups[1].Captures)
                    {
                        string item = ((Capture)ms).Value.Trim(' ', ',');
                        metaList.Add(item);
                    }
                }

                if (mm.Groups[2].Success)
                {
                    string item = mm.Groups[2].Value.Trim(' ', ',');
                    specifiersList.Add(item);
                }
            }

            macroStatement.MetaSpecifiers = metaList.ToArray();
            macroStatement.Specifiers = specifiersList.ToArray();
        }
    }
}
