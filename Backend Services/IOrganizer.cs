using System.Collections.Generic;

namespace FileOrganizer.Backend_Services;

public interface IOrganizer
{
    public void UndoMethod();
    public (bool, int) OrganizeFiles(List<string> folderPaths, List<string> excludedFiles, string orgMethod, string structure);
    public (int, int, int) FolderInfos(List<string?>? folderPaths);
}