using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Utils.Extensions;
using Assets.Codebase.Views.Base;
using TMPro;
using UniRx;
using UnityEngine;

namespace Assets.Codebase.Views.Ingame
{
    public class IngameView : BaseView
    {
        [SerializeField] private TMP_Text _positionText;
        [SerializeField] private TMP_Text _lapText;

        private IIngamePresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as IIngamePresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToPresenterEvents()
        {
            base.SubscribeToPresenterEvents();
            _presenter.PositionString.SubscribeToTMPText(_positionText).AddTo(CompositeDisposable);
        }

        protected override void SubscribeToUserInput()
        {
        }
    }
}