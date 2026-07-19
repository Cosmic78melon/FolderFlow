using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
namespace FileOrganizer.Backend_Services;

public class FilePicker(Func<TopLevel?> toplevel)
{
    public async Task<List<string?>> FolderSelector()
    {
        var topLevel = toplevel();

        if (topLevel == null) return null;

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = "Select a Folder To Organize",
            AllowMultiple = true
        });

        if (folders != null && folders.Count > 0)
        {
            List<string?> selectedFolders = folders.Select( folder => folder.TryGetLocalPath()).Where(path => path != null).ToList();
            return selectedFolders;
        }

        return new List<string?>();
    }
}