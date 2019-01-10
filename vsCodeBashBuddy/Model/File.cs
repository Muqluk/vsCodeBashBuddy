using System;
using System.IO;

namespace vsCodeBashBuddy.Model {
  public interface IFile {
    string Name { get; set; }
    string FullPath { get; set; }
    long Size { get; set; }
    DateTime LastModified { get; set; }
  }
  public class File : IFile {
    public string Name { get; set; }
    public string FullPath { get; set; }
    public long Size { get; set; }
    public DateTime LastModified { get; set; }

    public File(FileInfo file) {
      this.Name = file.Name;
      this.FullPath = file.FullName;
      this.Size = file.Length;
      this.LastModified = file.LastAccessTime;
    }
  }
}
