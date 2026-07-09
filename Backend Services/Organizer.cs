using System;
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
        // TODO: Make new Folders and Move files in there
        if (string.IsNullOrWhiteSpace(folderPath)) return false;
        if (!Directory.Exists(folderPath)) return false;
        try
        {
            DirectoryInfo dirNames =  new DirectoryInfo(folderPath);
            DirectoryInfo[] subFolders =  dirNames.GetDirectories();
            
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                string ext = file.Split('.')[^1];
                bool isFound = false;
                int lengthOfSubFolders = subFolders.Length - 1;
                for (int i = 0; i < lengthOfSubFolders; i++)
                {
                    foreach (DirectoryInfo subFolder in subFolders)
                    {
                        if (String.Equals(subFolder.Name, ext, StringComparison.OrdinalIgnoreCase))
                        {
                            isFound = true;
                        }
                    }
                }

                if (isFound)
                {
                    string Filename = Path.GetFileName(file);
                    File.Move(file, Path.Combine(ext, Filename));
                }
                else
                {
                    string dirName = Path.Combine(folderPath, ext);
                    DirectoryInfo _ = Directory.CreateDirectory(dirName);
                    string FileName = Path.GetFileName(file);
                    File.Move(file, Path.Combine(dirName, FileName));
                }
            }
            return true;
        }
        catch (Exception ex) when(ex is PathTooLongException || ex is NotSupportedException || ex is ArgumentException || ex is DirectoryNotFoundException)
        {
            return false;
        }
    }

    public void excludeFiles()
    {
        // TODO: exclude the folders so the excluded folders are not organized
    }
}