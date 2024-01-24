using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Views.Base;

namespace Assets.Codebase.Views.Ingame
{
    public class IngameView : BaseView
    {
        private IIngamePresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as IIngamePresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToUserInput()
        {
        }
    }
}