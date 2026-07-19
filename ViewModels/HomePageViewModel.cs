using System;
using System.Collections.Generic;
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

    private List<string> ListOfFolders;
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
        ListOfFolders = new List<string>();
    }

    private async Task<List<string?>> OpenFolderPickerDialougeAsync()
    {
        List<string?> path = await _filePicker.FolderSelector();
        if (path.Count > 0)
        {
            return path;
        }
        return new List<string?>();
    }

    [RelayCommand]
    public async Task OpenFolderPicker()
    {
        List<string?> paths = await OpenFolderPickerDialougeAsync();
        if (paths.Count > 0)
        {
            ListOfFolders = paths;
            foreach (string path in paths)
            {
                FolderPath = string.Join(" | ", path);
            }
        }
    }
    
    [RelayCommand]
    public async Task OpenExcludeFolderPicker()
    {
        List<string?> paths = await OpenFolderPickerDialougeAsync();
        if (paths.Count > 0)
        {
            foreach (string path in paths)
            {
                ExcludeFolderPath = string.Join(" | ", path);
            }
        }
    }

    [RelayCommand]
    public void Organize()
    {
        bool isOrganized = _organizer.OrganizeFiles(ListOfFolders, ExcludeFolderPath);
        if (isOrganized)
        {
            FolderPath = string.Empty;
            ExcludeFolderPath = string.Empty;
        }
    }
    
}