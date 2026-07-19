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
    private HashSet<string> normImg_ext = new();
    private HashSet<string> video_ext = new();
    private HashSet<string> threeD_image = new();
    private HashSet<string> audio_ext = new();
    private HashSet<string> cad_ext = new();
    private HashSet<string> codes_ext = new();
    private HashSet<string> compressedFile_ext = new();
    private HashSet<string> Ebooks_ext = new();
    private HashSet<string> Fonts_ext = new();
    private HashSet<string> Plugins_ext = new();
    private HashSet<string> Ransters_ext = new();
    private HashSet<string> RawImage_ext = new();
    private HashSet<string> SpreadSheet_ext = new();
    private HashSet<string> VectorImg_ext = new();
    private HashSet<string> webFile_ext = new();
    HashSet<string> ignoreExt = new(StringComparer.OrdinalIgnoreCase)
        { "msi", "bat", "dll", "exe", "sys", "dat","log", "temp","sav","cache","tmp", "so","com","cfg","drv","cmd","ini","lib" }; 
    // TODO: No git add git
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

    private bool extAdderHastsets()
    {
        try
        {
            string[] threeDExt = ext_creator("3D_Images_Extension");
            for (int i = 0; i < threeDExt.Length; i++)
            {
                threeD_image.Add(threeDExt[i]);
            }
            string[] audio_exts = ext_creator("AudioExtension");
            for (int i = 0; i < audio_exts.Length; i++)
            {
                audio_ext.Add(audio_exts[i]);
            }
            string[] cadExt = ext_creator("CadExtension");
            for (int i = 0; i < cadExt.Length; i++)
            {
                cad_ext.Add(cadExt[i]);
            }
            string[] codesExt = ext_creator("CodesExtension");
            for (int i = 0; i < codesExt.Length; i++)
            {
                codes_ext.Add(codesExt[i]);
            }
            string[] compExt = ext_creator("CompressedExtension");
            for (int i = 0; i < compExt.Length; i++)
            {
                compressedFile_ext.Add(compExt[i]);
            }
            string[] ebookExt = ext_creator("EbooksExtension");
            for (int i = 0; i < ebookExt.Length; i++)
            {
                Ebooks_ext.Add(ebookExt[i]);
            }
            string[] fontExt = ext_creator("FontsExtension");
            for (int i = 0; i < fontExt.Length; i++)
            {
                Fonts_ext.Add(fontExt[i]);
            }
            string[] normExt = ext_creator("ImageExtensions");
            for (int i = 0; i < normExt.Length; i++)
            {
                normImg_ext.Add(normExt[i]);
            }
            string[] pluginExt = ext_creator("PluginsFilesExtension");
            for (int i = 0; i < pluginExt.Length; i++)
            {
                Plugins_ext.Add(pluginExt[i]);
            }
            string[] rasterExt = ext_creator("Raster_ImagesExtension");
            for (int i = 0; i < rasterExt.Length; i++)
            {
                Ransters_ext.Add(rasterExt[i]);
            }
            string[] rawExt = ext_creator("RawImagesExtension");
            for (int i = 0; i < rawExt.Length; i++)
            {
                RawImage_ext.Add(rawExt[i]);
            }
            string[] spreadExt = ext_creator("SpreadsheetsExtension");
            for (int i = 0; i < spreadExt.Length; i++)
            {
                SpreadSheet_ext.Add(spreadExt[i]);
            }
            string[] vectorExt = ext_creator("Vector_Images_Extension");
            for (int i = 0; i < vectorExt.Length; i++)
            {
                VectorImg_ext.Add(vectorExt[i]);
            }
            string[] videoExt = ext_creator("VideoExtension");
            for (int i = 0; i < videoExt.Length; i++)
            {
                video_ext.Add(videoExt[i]);
            }
            string[] webExt = ext_creator("WebFilesExtension");
            for (int i = 0; i < webExt.Length; i++)
            {
                webFile_ext.Add(webExt[i]);
            }

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public bool OrganizeFiles(List<string?>? folderPaths, string includedFiles)
    {
        bool isAdded = extAdderHastsets();

        if (!isAdded) return false;
        
        if (folderPaths == null || folderPaths.Count == 0) return false;
        
        foreach(string folderPath in folderPaths)
        {
            if (!Directory.Exists(folderPath)) return false;
            try
            {
                DirectoryInfo dirNames = new DirectoryInfo(folderPath);
                DirectoryInfo[] subFolders = dirNames.GetDirectories();

                string[] files = Directory.GetFiles(folderPath);
                
                foreach (string file in files)
                {
                    string raw_ext = Path.GetExtension(file).TrimStart('.');
                    string ext = char.ToUpper(raw_ext[0]) + raw_ext.Substring(1);
                    bool isFound = false;
                    int lengthOfSubFolders = subFolders.Length - 1;
                    
                    if (ignoreExt.Contains(raw_ext))
                        continue;
                    
                    if (threeD_image.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "3D Images");
                        if (isSucc) 
                            continue;
                    }
                    
                    if (audio_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Audio Files");
                        if (isSucc) 
                            continue;
                    }
                    
                    if (cad_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Cad Files");
                        if (isSucc) 
                            continue;
                    }
                    
                    if (codes_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Codes");
                        if (isSucc) 
                            continue;
                    }
                    
                    if (compressedFile_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Compressed Files");
                        if (isSucc) 
                            continue;
                    }
                    
                    if (Ebooks_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Ebooks");
                        if (isSucc) 
                            continue;
                    }
                    
                    if (Fonts_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Font Files");
                        if (isSucc) 
                            continue;
                    }
                    
                    if (normImg_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Images");
                        if (isSucc) 
                            continue;
                    }
                    
                    if (Plugins_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Plugins");
                        if (isSucc) 
                            continue;
                    }

                    if (Ransters_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Raster Files");
                        if (isSucc) 
                            continue;
                    }

                    if (RawImage_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Raw Images");
                        if (isSucc) 
                            continue;
                    }

                    if (SpreadSheet_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Spreadsheets");
                        if (isSucc) 
                            continue;
                    }
                    
                    if (VectorImg_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Vector Images");
                        if (isSucc) 
                            continue;
                    }
                    
                    if (video_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Videos");
                        if (isSucc) 
                            continue;
                    }

                    if (webFile_ext.Contains(raw_ext))
                    {
                        bool isSucc = ImageSorter(file, "Web Files");
                        if (isSucc) 
                            continue;
                    }
                    
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
                
                return false;
            }
        }

        return true;
    }

    private bool ImageSorter(string imagePath, string FolderName)
    {
        try
        {
            string fileName = Path.GetFileName(imagePath);
            string? folderPath = Path.GetDirectoryName(imagePath);

            if (folderPath == null)
                return false;

            string extension = Path.GetExtension(imagePath)
                .TrimStart('.')
                .ToUpperInvariant();

            string destination = Path.Combine(folderPath, FolderName, extension);

            Directory.CreateDirectory(destination);

            File.Move(imagePath, Path.Combine(destination, fileName));

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void excludeFiles()
    {
        // TODO: exclude the folders so the excluded folders are not organized
    }
}