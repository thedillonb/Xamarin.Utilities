using ReactiveUI;

namespace Xamarin.Utilities.Core.Services
{
    public interface ITransitionOrchestrationService
    {
        void Transition(IViewFor fromView, IViewFor toView);
    }
}

