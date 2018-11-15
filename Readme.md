<!-- default file list -->
*Files to look at*:

* [CustomFileSystemProvider.cs](./CS/App_Code/CustomFileSystemProvider.cs) (VB: [CustomFileSystemProvider.vb](./VB/App_Code/CustomFileSystemProvider.vb))
* [ExtensionsDisplayName.xml](./CS/Content/ExtensionsDisplayName.xml) (VB: [ExtensionsDisplayName.xml](./VB/Content/ExtensionsDisplayName.xml))
* **[Default.aspx](./CS/Default.aspx) (VB: [Default.aspx](./VB/Default.aspx))**
* [Default.aspx.cs](./CS/Default.aspx.cs) (VB: [Default.aspx](./VB/Default.aspx))
<!-- default file list end -->
# How to use the ASPxFileManager metadata property


<p>This new <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.FileManagerItemProperties.Metadata.property">feature</a> (FileManagerItemProperties.Metadata Property, starting from the release 17.2) provides access to a collection of an item's metadata. You could view metadata by clicked "Properties" in the corresponding item context menu. For this, create a custom file system <a href="https://documentation.devexpress.com/AspNet/9907/ASP-NET-WebForms-Controls/File-Management/File-Manager/Concepts/File-System-Providers/Custom-File-System-Provider">provider</a>, override base <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.FileSystemProviderBase.GetFiles.method">GetFiles</a> and <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.FileSystemProviderBase.GetFolders.method">GetFolders</a> methods, add key-value dictionary pairs to properties of a corresponding <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.FileManagerFile..ctor(gUlFIw)">FileManagerFile</a> or a <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.FileManagerFolder..ctor(gUlFIw)">FileManagerFolder</a> object. Handle the client-side event SelectionChanged to get the currently selected element. Use the client-side method <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.Scripts.ASPxClientFileManagerItem.GetMetadata.method">GetMetadata</a> to get the metadata dictionary of the element. Then, compose a text from keys and values and assign it as the <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.ASPxPopupControl.class">ASPxPopupControl</a> <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.Scripts.ASPxClientPopupControlBase.SetContentHtml.method">content</a>.</p>

<br/>


