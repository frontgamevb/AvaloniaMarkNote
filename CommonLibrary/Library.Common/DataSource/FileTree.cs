using Log.Common;
using System.Collections.ObjectModel;

namespace Library.Common.DataSource;

public class FileNode
{
    public FileNode(string fullPath, string pattern, bool isFolder = true)
    {
        FullPath = fullPath;
        IsFolder = isFolder;
        Name = Path.GetFileName(fullPath);

        if(isFolder) SubNodes = GetSubNodes(fullPath, pattern);
    }

    public string FullPath { get; }
    public bool IsFolder { get; }
    public string Name { get; }
    public ObservableCollection<FileNode>? SubNodes { get; set; }

    public static ObservableCollection<FileNode> GetSubNodes(string fullPath, string pattern)
    {
        var subNodes = new ObservableCollection<FileNode>();
        try
        {
            foreach(var folder in Directory.GetDirectories(fullPath))
                subNodes.Add(new(folder, pattern));
        }
        catch { }
        try
        {
            foreach(var file in Directory.GetFiles(fullPath, pattern))
                subNodes.Add(new(file, pattern, false));
        }
        catch { }
        return subNodes;
    }
}

public class FileTree : ObservableCollection<FileNode>
{
    public FileTree(string fullPath, string pattern = "*.*")
    {
        if(Directory.Exists(fullPath))
        {
            Add(new(fullPath, pattern));
            return;
        }
        ExTrace.Warning(Texts.FolderNotExists, fullPath);
    }
}