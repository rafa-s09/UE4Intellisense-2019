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

namespace UE4Intellisense.Actions
{
    internal class UE4SpecifierRemoveSuggestedAction : ISuggestedAction
    {
        private readonly ITrackingSpan _span;
        private readonly ITextSnapshot _snapshot;

        public UE4SpecifierRemoveSuggestedAction(ITrackingSpan span)
        {
            _span = span;
            _snapshot = span.TextBuffer.CurrentSnapshot;
            DisplayText = $"Remove '{span.GetText(_snapshot)}' specifier";
        }

        #region Disposable
        ~UE4SpecifierRemoveSuggestedAction() => Dispose();

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
            textBlock.Inlines.Add(new Run { Text = "" });
            return await Task.FromResult<object>(textBlock);
        }       

        public void Invoke(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested == false)
                _span.TextBuffer.Replace(_span.GetSpan(_snapshot), "");
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = Guid.Empty;
            return false;
        }
    }
}
