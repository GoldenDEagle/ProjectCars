using Assets.Codebase.Models.Gameplay;
using Assets.Codebase.Models.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess
{
    public interface IModelAccessService : IService
    {
        /// <summary>
        /// Reference to progress model.
        /// </summary>
        public IProgressModel ProgressModel { get; }
        /// <summary>
        /// Reference to gameplay model.
        /// </summary>
        public IGameplayModel GameplayModel { get; }
    }
}
