using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
namespace FileOrganizer.Backend_Services;

public class FilePicker(Func<TopLevel?> toplevel)
{
    public async Task<string?> FolderSelector()
    {
        var topLevel = toplevel();

        if (topLevel == null) return null;

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Select a Folder"
        });
        
        var path = folders.FirstOrDefault(); 
        if (path ==  null) return null;
        return path.TryGetLocalPath();
    }
}