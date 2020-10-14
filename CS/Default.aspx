<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.16.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ASPxFileManager metadata property</title>
    <script>
        var text, metadata;
        function OnSelectionChanged(s, e) {
            text = "";
            var selectedFiles = s.GetSelectedItems();
            if (selectedFiles[0] != null) {
                metadata = selectedFiles[0].GetMetadata();
                for (var key in metadata) {
                    if (!metadata.hasOwnProperty) continue;
                    var value = metadata[key];
                    text += "<p><b>" + key + ":</b> " + value + "</p>" + "\r\n";
                }
            }
            if (selectedFiles[0] != null && selectedFiles[0].name == "..")
                text += "Parent folder";
        }
        function OnCustomCommand(s, e) {
            switch (e.commandName) {
                case "Properties":
                    PopupControl.SetContentHtml(text);
                    PopupControl.ShowAtElement(FileManager.GetMainElement());
                    break;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxFileManager ID="ASPxFileManager1" ClientInstanceName="FileManager" runat="server" 
                CustomFileSystemProviderTypeName="CustomFileSystemProvider">
                <ClientSideEvents CustomCommand="OnCustomCommand" SelectionChanged="OnSelectionChanged" />
                <Settings RootFolder="~/Content/logs/" ThumbnailFolder="~/Thumb/" EnableClientSideItemMetadata="true" 
                    AllowedFileExtensions=".log, .txt" />
                <SettingsFileList ShowFolders="true" ShowParentFolder="true"></SettingsFileList>
                <SettingsFolders Visible="false" />
                <SettingsUpload Enabled="false"></SettingsUpload>
                <SettingsContextMenu Enabled="true">
                    <Items>
                        <dx:FileManagerToolbarCustomButton Text="Properties" CommandName="Properties" BeginGroup="true">
                            <Image IconID="setup_properties_16x16" />
                        </dx:FileManagerToolbarCustomButton>
                    </Items>
                </SettingsContextMenu>
            </dx:ASPxFileManager>

            <dx:ASPxPopupControl ID="PopupControl" runat="server" ClientInstanceName="PopupControl"
                Width="430" HeaderText="Properties"
                ShowHeader="true" ShowFooter="false" PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"
                AllowDragging="true" DragElement="Header" CssClass="DetailsPopup" CloseOnEscape="true">
            </dx:ASPxPopupControl>
        </div>
    </form>
</body>
</html>
