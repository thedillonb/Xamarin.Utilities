using ReactiveUI;

namespace Xamarin.Utilities.Services
{
    public interface ITransitionOrchestrationService
    {
        void Transition(IViewFor fromView, IViewFor toView);
    }
}

