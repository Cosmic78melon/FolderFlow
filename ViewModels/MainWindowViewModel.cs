using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileOrganizer.Backend_Services;

namespace FileOrganizer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HomePageIsActive))]
    [NotifyPropertyChangedFor(nameof(SettingsPageIsActive))]
    private ViewModelBase _selectedViewModel;
    
    [ObservableProperty] private HomePageViewModel _homePageViewModel;
    [ObservableProperty] private SettingsPageViewModel _settingsPageViewModel;
    private FilePicker _filePicker;
    private IOrganizer _organizer;
    public bool HomePageIsActive => SelectedViewModel == _homePageViewModel;
    public bool SettingsPageIsActive => SelectedViewModel == _settingsPageViewModel;
    
    public MainWindowViewModel(HomePageViewModel homePageViewModel, 
        SettingsPageViewModel settingsPageViewModel,
        FilePicker filePicker,
        IOrganizer organizer)
    {
        _homePageViewModel = homePageViewModel;
        _settingsPageViewModel = settingsPageViewModel;
        _filePicker = filePicker;
        _organizer = organizer;
        SelectedViewModel = _homePageViewModel;
    }
    

    [RelayCommand]
    public void GoToHome()
    {
        SelectedViewModel = _homePageViewModel;
    }
    
    [RelayCommand]
    public void GoToSettings()
    {
        SelectedViewModel = _settingsPageViewModel;
    }
    
}