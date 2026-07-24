using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileOrganizer.Backend_Services;

namespace FileOrganizer.ViewModels;
public partial class HomePageViewModel : ViewModelBase
{
    private FolderPickerDialouge _folderPickerDialouge;
    private FilePickerDialouge _filePickerDialouge; 
    [ObservableProperty] private string? _folderPath;
    [ObservableProperty] private string? _excludeFilesPath;
    private IOrganizer _organizer;

    private List<string> ListOfFolders;
    private List<string> ListOfExcludedFiles;

    [ObservableProperty] private int? _totalNumberOfFiles = 0;
    [ObservableProperty] private int? _totalNumberOfFolder = 0;
    [ObservableProperty] private double? _totalStorage = 0.0;
    [ObservableProperty] private int? _totalNumberOfFileOrg = 0;
    public HomePageViewModel(FolderPickerDialouge folderPickerDialouge, FilePickerDialouge filePickerDialouge ,IOrganizer organizer)
    {
        _folderPickerDialouge = folderPickerDialouge;
        _filePickerDialouge = filePickerDialouge;
        _organizer = organizer;
    }
    
    
    [RelayCommand]
    public void UndoButton()
    {
        FolderPath = string.Empty;
        ExcludeFilesPath = string.Empty;
        ListOfFolders = new List<string>();
        ListOfExcludedFiles = new List<string>();
        TotalNumberOfFileOrg = 0;
        TotalNumberOfFiles = 0;
        TotalNumberOfFolder = 0;
        TotalStorage = 0;
    }

    private async Task<List<string?>> OpenFolderPickerDialougeAsync()
    {
        List<string?> path = await _folderPickerDialouge.FolderSelector();
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
            string[] temp = new string[paths.Count];
            for(int i = 0; i < paths.Count; i++)
            {
                temp[i] = paths[i];
            }
            FolderPath = string.Join(" | ", temp);

            (TotalNumberOfFiles, TotalNumberOfFolder, TotalStorage) = _organizer.FolderInfos(paths);
        }
    }
    [RelayCommand]
    public async Task OpenExcludeFilePicker()
    {
        List<string?> paths = await _filePickerDialouge.FileSelector();
        if (paths.Count > 0)
        {
            ListOfExcludedFiles = paths;
            string[] temps =  new string[paths.Count]; 
            for (int i= 0; i < paths.Count; i++)
            {
                temps[i] = paths[i];
            }
            ExcludeFilesPath  = string.Join(" | ", temps);
        }
    }

    [RelayCommand]
    public void Organize()
    {
        bool isOrganized;
        (isOrganized, TotalNumberOfFileOrg) = _organizer.OrganizeFiles(ListOfFolders, ListOfExcludedFiles);
        if (isOrganized)
        {
            FolderPath = string.Empty;
            ExcludeFilesPath = string.Empty;
        }
    }
    
}