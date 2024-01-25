using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Presenters.Endgame;
using Assets.Codebase.Views.Base;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Codebase.Views.Endgame
{
    public class EndgameView : BaseView
    {
        [SerializeField] private Button _continueButton;

        private IEndgamePresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as IEndgamePresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToUserInput()
        {
            _continueButton.OnClickAsObservable().Subscribe(_ => _presenter.ContinueButtonClicked());
        }
    }
}