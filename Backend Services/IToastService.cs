using System.Threading.Tasks;
using FileOrganizer.ViewModels;

namespace FileOrganizer.Backend_Services;

public interface IToastService
{
    ToastNotificationViewModel Notification { get; }
    
    Task ShowMessageAsync(string title, string message, bool isVisible, string iconName, string hexCodeBG, string hexCodeSFG, int durationMilliseconds = 3000);
}