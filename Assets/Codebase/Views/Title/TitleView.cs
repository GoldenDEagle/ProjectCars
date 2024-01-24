using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Views.Base;

namespace Assets.Codebase.Views.Title
{
    public class TitleView : BaseView
    {
        private ITitlePresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as ITitlePresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToUserInput()
        {
        }
    }
}