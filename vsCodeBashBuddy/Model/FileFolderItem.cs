using System.Collections.Generic;

namespace vsCodeBashBuddy.Model {

  public enum FileFolderType {
    Directory,
    File,
  }

  public interface IFileFolderItem {
    FileFolderType Type { get; set; }
    string Path { get; set; }
    string Name { get; set; }
    int ChildCount { get; set; }
    int Size { get; set; }
    IEnumerable<IFileFolderItem> Descendents { get; set; }

    int TotalChildCount { get; set; }
    int TotalSize { get; set; }
    IEnumerable<IFileFolderItem> TotalDescendents { get; set; }
  }

  public class FileFolderItem : IFileFolderItem {
    public FileFolderType Type { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public int ChildCount { get; set; }
    public int Size { get; set; }
    public IEnumerable<IFileFolderItem> Descendents { get; set; }
    public int TotalChildCount { get; set; }
    public int TotalSize { get; set; }
    public IEnumerable<IFileFolderItem> TotalDescendents { get; set; }
  }

}
