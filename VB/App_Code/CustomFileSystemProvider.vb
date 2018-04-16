Option Infer On

Imports DevExpress.Web
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Security.Principal
Imports System.Web
Imports System.Xml.Linq

Public Class CustomFileSystemProvider
    Inherits PhysicalFileSystemProvider

    Public Sub New(ByVal rootFolder As String)
        MyBase.New(rootFolder)
    End Sub
    Protected Friend Overridable Function MapPath(ByVal virtualPath As String) As String
        Return System.Web.Hosting.HostingEnvironment.MapPath(virtualPath)
    End Function

    Private extensionsDisplayName_Renamed As Dictionary(Of String, String)
    Private ReadOnly Property ExtensionsDisplayName() As Dictionary(Of String, String)
        Get
            If extensionsDisplayName_Renamed Is Nothing Then
                extensionsDisplayName_Renamed = XDocument.Load(MapPath("~/Content/ExtensionsDisplayName.xml")).Descendants("Extension").ToDictionary(Function(n) n.Attribute("Extension").Value, Function(n) n.Attribute("DisplayName").Value)
            End If
            Return extensionsDisplayName_Renamed
        End Get
    End Property
    Public Overrides Iterator Function GetFiles(ByVal folder As FileManagerFolder) As IEnumerable(Of FileManagerFile)
        For Each file As FileManagerFile In MyBase.GetFiles(folder)
            Dim fileInfo = New FileInfo(MapPath(file.FullName))
            Dim fileProperties As New FileManagerFileProperties() With {.Metadata = New Dictionary(Of String, Object)()}
            Dim fileSecurity = System.IO.File.GetAccessControl(MapPath(file.FullName))
            Dim sid = fileSecurity.GetOwner(GetType(SecurityIdentifier))
            Dim myFileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(MapPath(file.FullName))
            Dim version = myFileVersionInfo.FileVersion
            Dim description As String = myFileVersionInfo.FileDescription
            fileProperties.Metadata.Add("Name", file.Name)
            fileProperties.Metadata.Add("Type", ExtensionsDisplayName(file.Extension))
            fileProperties.Metadata.Add("CreationDate", fileInfo.CreationTime.ToString("U"))
            fileProperties.Metadata.Add("AccessedDate", fileInfo.LastAccessTime.ToString("U"))
            fileProperties.Metadata.Add("Attributes", fileInfo.Attributes)
            fileProperties.Metadata.Add("LastWriteTime", fileInfo.LastWriteTime.ToString("U"))
            fileProperties.Metadata.Add("Owner", sid.Value)
            fileProperties.Metadata.Add("Description", description)
            fileProperties.Metadata.Add("Size", fileInfo.Length.ToString("#,#") & " bytes")
            Yield New FileManagerFile(Me, folder, file.Name, file.Id, fileProperties)
        Next file
    End Function
    Public Overrides Iterator Function GetFolders(ByVal parentFolder As FileManagerFolder) As IEnumerable(Of FileManagerFolder)
        For Each folder As FileManagerFolder In MyBase.GetFolders(parentFolder)
            Dim folderInfo = New FileInfo(MapPath(folder.FullName))
            Dim folderProperties As New FileManagerFolderProperties() With {.Metadata = New Dictionary(Of String, Object)()}
            Dim folderSecurity = File.GetAccessControl(MapPath(folder.FullName))
            Dim sid = folderSecurity.GetOwner(GetType(SecurityIdentifier))
            folderProperties.Metadata.Add("Name", folder.Name)
            folderProperties.Metadata.Add("Type", "Folder")
            folderProperties.Metadata.Add("CreationDate", folderInfo.CreationTime.ToString("U"))
            folderProperties.Metadata.Add("AccessedDate", folderInfo.LastAccessTime.ToString("U"))
            folderProperties.Metadata.Add("Attributes", folderInfo.Attributes)
            folderProperties.Metadata.Add("LastWriteTime", folderInfo.LastWriteTime.ToString("U"))
            folderProperties.Metadata.Add("Owner", sid.Value)
            Dim directoryInfo = New DirectoryInfo(MapPath(folder.FullName))
            Dim filesInfo = directoryInfo.GetFiles("*", SearchOption.AllDirectories)
            Dim folderSize As Long = 0
            For Each info In filesInfo
                folderSize += info.Length
            Next info
            Dim filesCount = filesInfo.Length
            Dim subDirectoriesCount = directoryInfo.GetDirectories("*", SearchOption.AllDirectories).Length
            folderProperties.Metadata.Add("Size", folderSize.ToString("#,#") & " bytes")
            folderProperties.Metadata.Add("Contains", String.Format("Files: {0}, Folders: {1}", filesCount, subDirectoriesCount))
            Yield New FileManagerFolder(Me, folder, folder.Name, folder.Id, folderProperties)
        Next folder
    End Function
End Class