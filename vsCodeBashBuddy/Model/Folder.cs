using System.Collections.Generic;
using System.IO;

namespace vsCodeBashBuddy.Model {
  public interface IFolder {
    bool IsHidden { get; set; }
    string Name { get; set; }
    string FullPath { get; set; }
    IList<IFolder> SubFolders { get; set; }
    IList<IFile> Files { get; set; }
  }

  public class Folder : IFolder {
    public bool IsHidden { get; set; } = false;
    public string Name { get; set; }
    public string FullPath { get; set; }
    public IList<IFolder> SubFolders { get; set; }
    public IList<IFile> Files { get; set; }

    public Folder(DirectoryInfo dir) {
      this.Name = dir.Name;
      this.FullPath = dir.FullName;
      this.Files = new List<IFile>();
      this.SubFolders = new List<IFolder>();
      foreach (var file in dir.GetFiles()) {
        Files.Add(new File(file));
      }

      foreach (var subdir in dir.GetDirectories()) {
        this.SubFolders.Add(new Folder(subdir));
      }
    }

  }
}
