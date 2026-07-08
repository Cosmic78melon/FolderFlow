using System.Text.Json;
using System.IO;
namespace FileOrganizer.Backend_Services;

public class Organizer: IOrganizer
{
    // TODO: No git add git
    public void UndoMethod()
    {
        // TODO: Make a json to track where the and which folder are in which state
    }

    public bool OrganizeFiles(string folderPath, string exludedFolders)
    {
        // TODO: Organize folder only using extension at first and than add year and than year -> month and then all features
        string FinalPath = Path.Combine(folderPath, "Test");
        DirectoryInfo directoryInfo = Directory.CreateDirectory(FinalPath);
        return true;
    }

    public void excludeFiles()
    {
        // TODO: exclude the folders so the excluded folders are not organized
    }
}