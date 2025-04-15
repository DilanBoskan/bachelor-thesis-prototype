using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

namespace Presentation;

public static class Program {
    static void Main(string[] _) {
        IActivatedEventArgs activatedArgs = AppInstance.GetActivatedEventArgs();

        // If the Windows shell indicates a recommended instance, then
        // the app can choose to redirect this activation to that instance instead.
        if (AppInstance.RecommendedInstance is not null) {
            AppInstance.RecommendedInstance.RedirectActivationTo();
        } else {
            // Define a key for this instance, based on some app-specific logic.
            // If the key is always unique, then the app will never redirect.
            // If the key is always non-unique, then the app will always redirect
            // to the first instance. In practice, the app should produce a key
            // that is sometimes unique and sometimes not, depending on its own needs.

            var key = Guid.NewGuid().ToString();

            var instance = AppInstance.FindOrRegisterInstanceForKey(key);

            if (instance.IsCurrentInstance) {
                // If we successfully registered this instance, we can now just
                // go ahead and do normal XAML initialization.
                Windows.UI.Xaml.Application.Start((p) => {
                    var context = new global::Windows.System.DispatcherQueueSynchronizationContext(global::Windows.System.DispatcherQueue.GetForCurrentThread());
                    global::System.Threading.SynchronizationContext.SetSynchronizationContext(context);
                    var _ = new App();
                });
            } else {
                // Some other instance has registered for this key, so we'll 
                // redirect this activation to that instance instead.
                instance.RedirectActivationTo();
            }
        }
    }
}
