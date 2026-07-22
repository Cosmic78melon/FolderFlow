using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace FileOrganizer.Backend_Services;

public class FilePickerDialouge(Func<TopLevel?> toplevel)
{
    public async Task<List<string?>> FileSelector()
    {
        var topLevel = toplevel();
        if (toplevel == null) return new List<string?>();

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select a File",
            AllowMultiple = true
        });
        
        if (files != null && files.Count > 0)
        {
            List<string?> selectedFiles = files.Select(files => files.TryGetLocalPath()).Where(path => path != null).ToList();
            return selectedFiles;
        }

        return new List<string?>();
    }
}