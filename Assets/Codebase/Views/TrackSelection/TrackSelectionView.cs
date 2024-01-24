using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Views.Base;

namespace Assets.Codebase.Views.TrackSelection
{
    public class TrackSelectionView : BaseView
    {
        private ITrackSelectionPresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as ITrackSelectionPresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToUserInput()
        {
        }
    }
}