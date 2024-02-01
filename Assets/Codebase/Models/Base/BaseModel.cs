using UniRx;

namespace Assets.Codebase.Models.Base
{
    public abstract class BaseModel
    {
        protected CompositeDisposable CompositeDisposable = new CompositeDisposable();
    }
}
