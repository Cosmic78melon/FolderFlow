using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Threading;
namespace FileOrganizer.ViewModels;

public partial class ToastNotificationViewModel: ViewModelBase
{
    [ObservableProperty] private string _colorBG;
    [ObservableProperty] private string _colorSFG;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private string? _iconName;
    [ObservableProperty] private string? _message;
    [ObservableProperty] private bool _isVisible = false;
}