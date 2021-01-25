using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;
using UE4Intellisense.Model;

namespace UE4Intellisense.Actions
{
    internal class UE4SpecifierNotValidSuggestedAction : ISuggestedAction
    {
        private readonly ITrackingSpan _span;
        private readonly UE4MacroStatement _ue4Statement;
        private readonly ITextSnapshot _snapshot;

        public UE4SpecifierNotValidSuggestedAction(ITrackingSpan span, UE4MacroStatement ue4Statement)
        {
            _span = span;
            _ue4Statement = ue4Statement;
            _snapshot = span.TextBuffer.CurrentSnapshot;
            DisplayText = $"'{span.GetText(_snapshot)}' is not valid {ue4Statement.MacroConst} specifier";
        }

        #region Disposable
        ~UE4SpecifierNotValidSuggestedAction() => Dispose();

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
        public bool HasActionSets => true;
        public bool HasPreview => false;
        public Task<object> GetPreviewAsync(CancellationToken cancellationToken) => null;
        ImageMoniker ISuggestedAction.IconMoniker => default;

        [Obsolete]
        public async Task<IEnumerable<SuggestedActionSet>> GetActionSetsAsync(CancellationToken cancellationToken)
        {
            UE4SpecifierRemoveSuggestedAction removeSuggestedAction = new UE4SpecifierRemoveSuggestedAction(_span);
            return await Task.FromResult(new[] { new SuggestedActionSet(new ISuggestedAction[] { removeSuggestedAction }) }.AsEnumerable()); //Obsolete
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
