namespace FileOrganizer.Backend_Services;

public interface IOrganizer
{
    public void UndoMethod();
    public bool OrganizeFiles(string folderPath, string exludedFolders);
    public void excludeFiles();
}