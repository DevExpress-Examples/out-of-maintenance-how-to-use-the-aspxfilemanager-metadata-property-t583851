using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Xml.Linq;

public class CustomFileSystemProvider : PhysicalFileSystemProvider {
    public CustomFileSystemProvider(string rootFolder) : base(rootFolder){
    }
    protected internal virtual string MapPath(string virtualPath) {
        return System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
    }
    Dictionary<string, string> extensionsDisplayName;
    Dictionary<string, string> ExtensionsDisplayName {
        get {
            if(extensionsDisplayName == null)
                extensionsDisplayName = XDocument.Load(MapPath("~/Content/ExtensionsDisplayName.xml")).Descendants("Extension").ToDictionary(n => n.Attribute("Extension").Value, n => n.Attribute("DisplayName").Value);
            return extensionsDisplayName;
        }
    }
    public override IEnumerable<FileManagerFile> GetFiles(FileManagerFolder folder) {
        foreach(FileManagerFile file in base.GetFiles(folder)) {
            var fileInfo = new FileInfo(MapPath(file.FullName));
            FileManagerFileProperties fileProperties = new FileManagerFileProperties() {
                Metadata = new Dictionary<string, object>()
            };
            var fileSecurity = File.GetAccessControl(MapPath(file.FullName));
            var sid = fileSecurity.GetOwner(typeof(SecurityIdentifier));
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(MapPath(file.FullName));
            var version = myFileVersionInfo.FileVersion;
            string description = myFileVersionInfo.FileDescription;
            fileProperties.Metadata.Add("Name", file.Name);
            fileProperties.Metadata.Add("Type", ExtensionsDisplayName[file.Extension]);
            fileProperties.Metadata.Add("CreationDate", fileInfo.CreationTime.ToString("U"));
            fileProperties.Metadata.Add("AccessedDate", fileInfo.LastAccessTime.ToString("U"));
            fileProperties.Metadata.Add("Attributes", fileInfo.Attributes);
            fileProperties.Metadata.Add("LastWriteTime", fileInfo.LastWriteTime.ToString("U"));
            fileProperties.Metadata.Add("Owner", sid.Value);
            fileProperties.Metadata.Add("Description", description);
            fileProperties.Metadata.Add("Size", fileInfo.Length.ToString("#,#") + " bytes");
            yield return new FileManagerFile(this, folder, file.Name, file.Id, fileProperties);
        }
    }
    public override IEnumerable<FileManagerFolder> GetFolders(FileManagerFolder parentFolder){
        foreach(FileManagerFolder folder in base.GetFolders(parentFolder)){
            var folderInfo = new FileInfo(MapPath(folder.FullName));
            FileManagerFolderProperties folderProperties = new FileManagerFolderProperties() {
                Metadata = new Dictionary<string, object>()
            };
            var folderSecurity = File.GetAccessControl(MapPath(folder.FullName));
            var sid = folderSecurity.GetOwner(typeof(SecurityIdentifier));
            folderProperties.Metadata.Add("Name", folder.Name);
            folderProperties.Metadata.Add("Type", "Folder");
            folderProperties.Metadata.Add("CreationDate", folderInfo.CreationTime.ToString("U"));
            folderProperties.Metadata.Add("AccessedDate", folderInfo.LastAccessTime.ToString("U"));
            folderProperties.Metadata.Add("Attributes", folderInfo.Attributes);
            folderProperties.Metadata.Add("LastWriteTime", folderInfo.LastWriteTime.ToString("U"));
            folderProperties.Metadata.Add("Owner", sid.Value);
            var directoryInfo = new DirectoryInfo(MapPath(folder.FullName));
            var filesInfo = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            long folderSize = 0;
            foreach(var info in filesInfo)
                folderSize += info.Length;
            var filesCount = filesInfo.Length;
            var subDirectoriesCount = directoryInfo.GetDirectories("*", SearchOption.AllDirectories).Length;
            folderProperties.Metadata.Add("Size", folderSize.ToString("#,#") + " bytes");
            folderProperties.Metadata.Add("Contains", string.Format("Files: {0}, Folders: {1}", filesCount, subDirectoriesCount));
            yield return new FileManagerFolder(this, folder, folder.Name, folder.Id, folderProperties);
        }
    }
}