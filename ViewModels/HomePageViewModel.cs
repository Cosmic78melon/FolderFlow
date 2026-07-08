using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileOrganizer.Backend_Services;

namespace FileOrganizer.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    private FilePicker _filePicker;
    [ObservableProperty] private string? _folderPath;
    [ObservableProperty] private string? _excludeFolderPath;
    private IOrganizer _organizer;
    public HomePageViewModel(FilePicker filePicker, IOrganizer organizer)
    {
        _filePicker = filePicker;
        _organizer = organizer;
    }
    
    
    [RelayCommand]
    public void UndoButton()
    {
        FolderPath = string.Empty;
        ExcludeFolderPath = string.Empty;
    }

    private async Task<string?> OpenFolderPickerDialougeAsync()
    {
        var path = await _filePicker.FolderSelector();
        if (path != null)
        {
            return Convert.ToString(path);
        }
        return null;
    }

    [RelayCommand]
    public async Task OpenFolderPicker()
    {
        string? path = await OpenFolderPickerDialougeAsync();
        FolderPath = Convert.ToString(path);
    }
    
    [RelayCommand]
    public async Task OpenExcludeFolderPicker()
    {
        string? path = await OpenFolderPickerDialougeAsync();
        ExcludeFolderPath = Convert.ToString(path);
    }

    [RelayCommand]
    public void Organize()
    {
        bool isOrganized = _organizer.OrganizeFiles(FolderPath, ExcludeFolderPath);
        if (isOrganized)
        {
            FolderPath = string.Empty;
            ExcludeFolderPath = string.Empty;
        }
    }
    
}