using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Language.Intellisense;

namespace UE4Intellisense.Providers
{
    [Export(typeof(ICompletionSourceProvider))]
    [Name("UE4 Completion")]
    [ContentType("C/C++")]
    internal class UE4SpecifiersCompletionSourceProvider : ICompletionSourceProvider
    {
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new UE4SpecifiersCompletionSource(this, textBuffer);
        }
    }
}
