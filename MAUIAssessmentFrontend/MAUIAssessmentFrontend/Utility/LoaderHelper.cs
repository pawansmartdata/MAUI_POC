using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAssessmentFrontend.Utility
{
    public static class LoaderHelper
    {
        /// <summary>
        /// Wraps a task with IsBusy state and optional snackbar feedback.
        /// </summary>
        public static async Task RunWithLoader(Func<Task> action, Action<bool> setIsBusy, string loadingMessage = null)
        {
            setIsBusy(true);
            Snackbar snackbar = null;

            if (!string.IsNullOrWhiteSpace(loadingMessage))
            {
                snackbar =(Snackbar?)Snackbar.Make(loadingMessage, duration: TimeSpan.FromSeconds(30));
                await snackbar.Show();
            }

            try
            {
                await action();
            }
            finally
            {
                setIsBusy(false);
                snackbar?.Dismiss();
            }
        }
    }
}
