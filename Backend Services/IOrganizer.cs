using System.Collections.Generic;

namespace FileOrganizer.Backend_Services;

public interface IOrganizer
{
    public void UndoMethod();
    public (bool,int) OrganizeFiles(List<string?>? folderPaths, List<string?>? excludedFiles);
    public (int, int, double) FolderInfos(List<string?>? folderPaths);
    public void excludeFiles();
}