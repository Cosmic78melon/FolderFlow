using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;
using Avalonia.Platform;

namespace FileOrganizer.Backend_Services;

public class Organizer: IOrganizer
{
    //* The Website URL: https://fileinfo.com/filetypes/audio
    HashSet<string> ignoreExt = new(StringComparer.OrdinalIgnoreCase)
        { "msi", "bat", "dll", "exe", "sys", "dat","log", "temp","sav","cache","tmp", "so","com","cfg","drv","cmd","ini","lib" };

    private Dictionary<string, HashSet<string>> category = new ();
    public void UndoMethod()
    {
        // TODO: Make a JSON to track where the and which folder are in which state
    }
    private string[] ext_creator(string textFileName)
    {
        var uri = new Uri($"avares://FileOrganizer/Assets/Data/{textFileName}.txt");
        using var stream = AssetLoader.Open(uri);
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd(); 
        string[] exts = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        return exts;
    }
    
    private void Load(string key, string fileName)
    {
        string[] exts = ext_creator(fileName);
        if (!category.ContainsKey(key))
        {
            category[key] = new HashSet<string>();
        }
        for (int i = 0; i < exts.Length; i++)
        {
            category[key].Add(exts[i]);
        }
    }
    private bool CategoryInit()
    {
        try
        {
            Load("3D images", "3D_Images_Extension");
            Load("Audio", "AudioExtension");
            Load("Cad Document", "CadExtension");
            Load("Codes", "CodesExtension");
            Load("Compressed Files", "CompressedExtension");
            Load("Ebooks", "EbooksExtension");
            Load("Fonts", "FontsExtension");
            Load("Images", "ImageExtensions");
            Load("Plugins", "PluginsFilesExtension");
            Load("Raster Files", "Raster_ImagesExtension");
            Load("Raw Images", "RawImagesExtension");
            Load("SpreadSheets", "SpreadsheetsExtension");
            Load("Vector Images", "Vector_Images_Extension");
            Load("Videos", "VideoExtension");
            Load("Web Files", "WebFilesExtension");

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    public (int, int, double) FolderInfos(List<string?>? folderPaths)
    {
        int totalNumberOfFiles = 0;        
        int totalNumberOfFolders = 0;
        double totalSize = 0.0;
        foreach(string folderPath in folderPaths)
        {
            if (!Directory.Exists(folderPath)) return (0,0,0.0);
            DirectoryInfo dirs = new DirectoryInfo(folderPath);
                
            var options = new EnumerationOptions
            {
                RecurseSubdirectories = true,
                IgnoreInaccessible = true
            };
            long totalBytes = dirs.EnumerateFiles("*", options)
                .Sum(file => file.Length);
                
            double totalMB = (double) totalBytes / (1024*1024);
            totalSize += totalMB;
            totalNumberOfFolders++;
            totalNumberOfFiles += Directory.GetFiles(folderPath).Length;
        }
        return (totalNumberOfFiles, totalNumberOfFolders, totalSize);
    }

    public (bool,int) OrganizeFiles(List<string?>? folderPaths, List<string?>? excludedFiles)
    {
        // TODO: There is no doucment section so Add that
        bool isAdded = CategoryInit();

        if (!isAdded) return (false,0);
        
        if (folderPaths == null || folderPaths.Count == 0) return (false,0);
        
        int totalFileOrg = 0;
        foreach(string folderPath in folderPaths)
        {
            if (!Directory.Exists(folderPath)) return (false,0);
            try
            {
                DirectoryInfo dirs = new DirectoryInfo(folderPath);
                DirectoryInfo[] subFolders = dirs.GetDirectories();
                
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    string raw_ext = Path.GetExtension(file).TrimStart('.');
                    string ext = char.ToUpper(raw_ext[0]) + raw_ext.Substring(1);
                    bool isFound = false;
                    int lengthOfSubFolders = subFolders.Length - 1;

                    HashSet<string> excludedFileNames = new();
                    if (excludedFiles != null)
                    {
                        foreach (string exFile in excludedFiles)
                        {
                            string exFileName = Path.GetFileName(exFile);
                            excludedFileNames.Add(exFileName);
                        }
                    }
                    string FileName = Path.GetFileName(file);
                    if (excludedFileNames.Contains(FileName))
                        continue;
                    
                    if (ignoreExt.Contains(raw_ext))
                        continue;

                    FileInfo fileinfo = new FileInfo(file);
                    long fileMB = fileinfo.Length / (1024 * 1024);
                    bool isMoved = false;
                    if (fileMB >= 1024)
                    {
                        isMoved = FolderSorter(file, "Large Files");
                        if (isMoved)
                            continue;
                    }
                    foreach (var format in category)
                    {
                        if (format.Value.Contains(raw_ext))
                        {
                            isMoved = FolderSorter(file, format.Key);
                            if (isMoved)
                                break;  
                        }
                    }
                    
                    if (isMoved)
                        continue;
                    
                    for (int i = 0; i < lengthOfSubFolders; i++)
                    {
                        foreach (DirectoryInfo subFolder in subFolders)
                        {
                            if (String.Equals(subFolder.Name, ext, StringComparison.OrdinalIgnoreCase))
                                isFound = true;
                        }
                    }

                    if (isFound)
                    {
                        string filename = Path.GetFileName(file);
                        
                        string dirPath = Path.Combine(folderPath, ext);
                        File.Move(file, Path.Combine(dirPath, filename));
                    }
                    else
                    {
                        string dirName = Path.Combine(folderPath, ext);
                        DirectoryInfo _ = Directory.CreateDirectory(dirName);
                        string fileName = Path.GetFileName(file);
                        File.Move(file, Path.Combine(dirName, fileName));
                    }
                }
            }
            catch (Exception ex) when(ex is PathTooLongException || ex is NotSupportedException || ex is ArgumentException || ex is DirectoryNotFoundException)
            {
                
                return (false,0);
            }
            totalFileOrg += Directory.GetFiles(folderPath).Length;
        }
        return (true, totalFileOrg);
    }

    private bool FolderSorter(string FilePath, string FolderName)
    {
        try
        {
            string fileName = Path.GetFileName(FilePath);
            string? folderName = Path.GetDirectoryName(FilePath);

            if (folderName == null)
            {
                Console.WriteLine("Folder is Null");
                return true;
            } 
                

            string extension = Path.GetExtension(FilePath)
                .TrimStart('.')
                .ToUpperInvariant();

            string destination = Path.Combine(folderName, FolderName, extension);

            Directory.CreateDirectory(destination);

            File.Move(FilePath, Path.Combine(destination, fileName));
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("The error in FolderSorter: ", e);
            return false;
        }
    }
}