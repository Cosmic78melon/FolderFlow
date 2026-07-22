using System.Collections.Generic;

namespace FileOrganizer.Backend_Services;

public interface IOrganizer
{
    public void UndoMethod();
    public bool OrganizeFiles(List<string?>? folderPaths, List<string?>? excludedFiles);
    public void excludeFiles();
}