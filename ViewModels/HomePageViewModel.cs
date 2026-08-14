using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileOrganizer.Backend_Services;

namespace FileOrganizer.ViewModels;

public class FolderStruct
{
    public required string? type {get; set;} 
    public required string? DisplayedName {get; set;} 
}

public class OrganizatoinTypes
{
    public required string? types {get; set;} 
    public required string? DisplayedNames {get; set;}
}
public partial class HomePageViewModel : ViewModelBase
{
    private FolderPickerDialouge _folderPickerDialouge;
    private FilePickerDialouge _filePickerDialouge;
    private IToastService _toastService;
    [ObservableProperty] private string? _folderPath;
    [ObservableProperty] private string? _excludeFilesPath;
    [ObservableProperty] private FolderStruct _selectedattrs;
    [ObservableProperty] private OrganizatoinTypes _seletectedMethod;
    private IOrganizer _organizer;
    private List<string> ListOfFolders;
    private List<string> ListOfExcludedFiles;
    
    public ToastNotificationViewModel Toast =>
        _toastService.Notification;

    [ObservableProperty] private int? _totalNumberOfFiles = 0;
    [ObservableProperty] private int? _totalNumberOfFolder = 0;
    [ObservableProperty] private double? _totalStorage = 0.0;
    [ObservableProperty] private int? _totalNumberOfFileOrg = 0;

    public ObservableCollection<OrganizatoinTypes> OrganizationMethods { get; } =
    [
        new OrganizatoinTypes
        {
            DisplayedNames = "File Type",
            types = "typeWise"
        },
        new OrganizatoinTypes
        {
            DisplayedNames = "Date Created",
            types = "dateWise"
        }
    ];
    
    public ObservableCollection<FolderStruct> AttributesOfFs { get; } =
    [
        new FolderStruct
        {
            DisplayedName = "Single Folder",
            type = "singleFold"
        },
        new FolderStruct
        {
            DisplayedName = "Subfolders → Year",
            type = "yearBasedFold"
        },
        new FolderStruct
        {
            DisplayedName = "Subfolders → Year → Month",
            type = "yearToMonBasedFold"
        },
        new FolderStruct
        {
            DisplayedName = "Subfolders → Extension",
            type = "extensionFold"
        }
    ];
    
    public HomePageViewModel(FolderPickerDialouge folderPickerDialouge, FilePickerDialouge filePickerDialouge ,IOrganizer organizer, IToastService toastService)
    {
        _folderPickerDialouge = folderPickerDialouge;
        _filePickerDialouge = filePickerDialouge;
        _organizer = organizer;
        _toastService = toastService;
        SeletectedMethod = OrganizationMethods.FirstOrDefault();
        Selectedattrs = AttributesOfFs.FirstOrDefault();
    }
    
    enum SymbolTypes
    {
        CheckmarkCircle = 100,
        Info = 111,
        DismissCircle = 404
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
    public async Task Organize()
    {
        bool isOrganized;

        if (ListOfFolders.Count == 0)
        {
            SymbolTypes failed = (SymbolTypes)404;
            await _toastService.ShowMessageAsync("Folder is Not Organized", "Select a Folder", true, failed.ToString(), "Red", "#D10000", 2500);
            return;
        }
        if (SeletectedMethod.types == null) return;
        if (Selectedattrs.type == null) return;
        
        (isOrganized, TotalNumberOfFileOrg) = _organizer.OrganizeFiles(ListOfFolders, ListOfExcludedFiles, SeletectedMethod.types, Selectedattrs.type);
        if (isOrganized)
        {
            FolderPath = string.Empty;
            ExcludeFilesPath = string.Empty;
            SymbolTypes success = (SymbolTypes)100;
            await _toastService.ShowMessageAsync("Organized", "Folder is Successfully Organized", true, success.ToString(), "#008000", "#50C878", 1700);
        }
        else
        {
            SymbolTypes failed = (SymbolTypes)404;
            await _toastService.ShowMessageAsync("Folder is Not Organized", "Something Went Wrong", true, failed.ToString(), "Red", "#D10000", 2500);
        }
    }
    
}