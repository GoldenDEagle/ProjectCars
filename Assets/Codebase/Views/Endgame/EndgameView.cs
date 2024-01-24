using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Presenters.Endgame;
using Assets.Codebase.Views.Base;

namespace Assets.Codebase.Views.Endgame
{
    public class EndgameView : BaseView
    {
        private IEndgamePresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as IEndgamePresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToUserInput()
        {
        }
    }
}