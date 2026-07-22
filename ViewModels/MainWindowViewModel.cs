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
    private FolderPickerDialouge _folderPickerDialouge;
    private FilePickerDialouge _filePickerDialouge;
    private IOrganizer _organizer;
    public bool HomePageIsActive => SelectedViewModel == _homePageViewModel;
    public bool SettingsPageIsActive => SelectedViewModel == _settingsPageViewModel;
    
    public MainWindowViewModel(HomePageViewModel homePageViewModel, 
        SettingsPageViewModel settingsPageViewModel,
        FilePickerDialouge filePickerDialouge,
        FolderPickerDialouge folderPickerDialouge,
        IOrganizer organizer)
    {
        _homePageViewModel = homePageViewModel;
        _settingsPageViewModel = settingsPageViewModel;
        _folderPickerDialouge = folderPickerDialouge;
        _filePickerDialouge = filePickerDialouge;
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