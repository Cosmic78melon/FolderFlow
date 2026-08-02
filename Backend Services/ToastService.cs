using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using FileOrganizer.ViewModels;

namespace FileOrganizer.Backend_Services;

    public class ToastService: IToastService
    {
        private CancellationTokenSource? _cts;

        public ToastNotificationViewModel Notification { get; } = new();

        public async Task ShowMessageAsync(string title, string message, bool isVisible, string iconName, string hexCodeBG, string hexCodeSFG, int durationMilliseconds = 3000)
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _cts = new CancellationTokenSource();
            
            CancellationToken cancellationToken = _cts.Token;
            
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Notification.Title = title;
                Notification.Message = message;
                Notification.IconName = iconName;
                Notification.ColorBG = hexCodeBG;
                Notification.ColorSFG = hexCodeSFG;
                Notification.IsVisible = isVisible;
            });

            try
            {
                await Task.Delay(durationMilliseconds, cancellationToken);

                await Dispatcher.UIThread.InvokeAsync(() => { Notification.IsVisible = false; });
            }
            catch (OperationCanceledException)
            {
                // Nothing add here it is intentianal
            }
        }
    }