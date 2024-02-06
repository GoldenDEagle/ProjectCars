using Assets.Codebase.Utils.UI;
using Assets.Codebase.Views.Base;
using DavidJalbert.TinyCarControllerAdvance;

namespace Assets.Codebase.Infrastructure.ServicesManagment.ViewCreation
{
    /// <summary>
    /// Allows to create views.
    /// </summary>
    public interface IViewProvider : IService
    {
        public Countdown Countdown { get; }
        public TCCAMobileInput MobileInput { get; }

        public BaseView CreateView(ViewId viewId);
        public AdPopupWindow CreateAdPopupWindow();
    }
}
