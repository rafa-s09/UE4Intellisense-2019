using System;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;
using UE4Intellisense.Model;

namespace UE4Intellisense.Actions
{
    internal class UE4SpecifierCollisitionSuggestedAction : ISuggestedAction
    {
        private readonly ITrackingSpan _span;
        private readonly UE4MacroStatement _ue4Statement;
        private readonly ITextSnapshot _snapshot;
        private readonly string _lower;

        public UE4SpecifierCollisitionSuggestedAction(ITrackingSpan span, UE4MacroStatement ue4Statement)
        {
            _span = span;
            _ue4Statement = ue4Statement;
            _snapshot = span.TextBuffer.CurrentSnapshot;
            _lower = span.GetText(_snapshot).ToLower();
            DisplayText = $"Convert '{span.GetText(_snapshot)}' to lower case";
        }

        #region Disposable
        ~UE4SpecifierCollisitionSuggestedAction() => Dispose();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing = false)
        {
            if (disposing) { }
        }
        #endregion

        public string DisplayText { get; }
        public string IconAutomationText => null;        
        public string InputGestureText => null;
        public bool HasActionSets => false;
        public bool HasPreview => true;        
        public Task<IEnumerable<SuggestedActionSet>> GetActionSetsAsync(CancellationToken cancellationToken) => null;
        ImageMoniker ISuggestedAction.IconMoniker => default;

        public async Task<object> GetPreviewAsync(CancellationToken cancellationToken)
        {
            TextBlock textBlock = new TextBlock { Padding = new Thickness(5) };
            textBlock.Inlines.Add(new Run { Text = _lower });
            return await Task.FromResult<object>(textBlock);
        }

        public void Invoke(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested == false)
                _span.TextBuffer.Replace(_span.GetSpan(_snapshot), _lower);
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = Guid.Empty;
            return false;
        }
    }
}
