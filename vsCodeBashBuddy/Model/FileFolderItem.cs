using System.Collections.Generic;

namespace vsCodeBashBuddy.Model {

  public enum FileFolderType {
    Directory,
    File,
    Root,
  }

  public interface IFileFolderItem {
    string Name { get; set; }
    string Path { get; set; }
    bool isLoaded { get; set; }
    FileFolderType FileFolderType { get; set; }
    IList<IFileFolderItem> Descendents { get; set; }
    int ChildCount { get; set; }
    long Size { get; set; }
    string Extension { get; set; }
    int TotalChildCount { get; set; }
    int TotalSize { get; set; }
    IList<IFileFolderItem> TotalDescendents { get; set; }
  }

  public class FileFolderItem : IFileFolderItem {
    public string Name { get; set; }
    public string Path { get; set; }
    public bool isLoaded { get; set; }
    public FileFolderType FileFolderType { get; set; }
    public IList<IFileFolderItem> Descendents { get; set; }
    public long Size { get; set; }
    public string Extension { get; set; }
    public int ChildCount { get; set; }
    public int TotalChildCount { get; set; }
    public int TotalSize { get; set; }
    public IList<IFileFolderItem> TotalDescendents { get; set; }
  }

}
