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
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _doubleRewardButton;
        [SerializeField] private Button _leaderboardButton;

        private IEndgamePresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as IEndgamePresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToPresenterEvents()
        {
            base.SubscribeToPresenterEvents();
            _presenter.PositionString.SubscribeToTMPText(_positionText).AddTo(CompositeDisposable);
            _presenter.CoinRewardString.SubscribeToTMPText(_rewardText).AddTo(CompositeDisposable);
            _presenter.TimeString.SubscribeToTMPText(_timeText).AddTo(CompositeDisposable);
            _presenter.DoubleRewardButtonActiveState.Subscribe(value => _doubleRewardButton.gameObject.SetActive(value)).AddTo(CompositeDisposable);
        }

        protected override void SubscribeToUserInput()
        {
            _continueButton.OnClickAsObservable().Subscribe(_ => _presenter.ContinueButtonClicked()).AddTo(CompositeDisposable);
            _doubleRewardButton.OnClickAsObservable().Subscribe(_ => _presenter.DoubleRewardButtonClicked()).AddTo(CompositeDisposable);
            _leaderboardButton.OnClickAsObservable().Subscribe(_ => _presenter.LeaderboardClicked()).AddTo(CompositeDisposable);
        }
    }
}