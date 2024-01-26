using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Presenters.Endgame;
using Assets.Codebase.Utils.Extensions;
using Assets.Codebase.Views.Base;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Codebase.Views.Endgame
{
    public class EndgameView : BaseView
    {
        [SerializeField] private TMP_Text _positionText;
        [SerializeField] private Button _continueButton;

        private IEndgamePresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as IEndgamePresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToPresenterEvents()
        {
            base.SubscribeToPresenterEvents();
            _presenter.PositionString.SubscribeToTMPText(_positionText);
        }

        protected override void SubscribeToUserInput()
        {
            _continueButton.OnClickAsObservable().Subscribe(_ => _presenter.ContinueButtonClicked());
        }
    }
}