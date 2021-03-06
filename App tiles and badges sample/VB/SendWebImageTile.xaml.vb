'*********************************************************
'
' Copyright (c) Microsoft. All rights reserved.
' THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
' ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
' IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
' PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
'
'*********************************************************

Imports System
Imports NotificationsExtensions.TileContent
Imports Windows.UI.Notifications

Partial Public NotInheritable Class SendWebImageTile
    Inherits SDKTemplate.Common.LayoutAwarePage

#Region "TemplateCode"
    Private rootPage As MainPage = MainPage.Current

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Protected Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
#End Region ' TemplateCode

    Private Sub UpdateTileWithWebImage_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        ' Note: This sample contains an additional project, NotificationsExtensions.
        ' NotificationsExtensions exposes an object model for creating notifications, but you can also
        ' modify the strings directly. See UpdateTileWithWebImageWithStringManipulation_Click for an example.

        ' !Important!
        ' The Internet (Client) capability must be checked in the manifest in the Capabilities tab
        ' to display web images in tiles (either the http:// or https:// protocols)

        ' Users can resize any app tile to the small (Square70x70) and medium (Square150x150) tile sizes.
        ' These are both tile sizes an app must minimally support.
        ' An app can additionally support the wide (Wide310x150) tile size as well as the large (Square310x310) tile size.
        ' Note that in order to support a large (Square310x310) tile size, an app must also support the wide (Wide310x150) tile size (but not vice versa).

        ' This sample application supports all four tile sizes �" small (Square70x70), medium (Square150x150), wide (Wide310x150) and large (Square310x310).
        ' This means that the user may have resized their tile to any of these four sizes for their custom Start screen layout.
        ' Because an app has no way of knowing what size the user resized their app tile to, an app should include template bindings for each supported tile sizes in their notifications.
        ' The only exception is the small (Square70x70) tile size because this size does not support live tile notifications, which is why there are no Square70x70 tile templates.             // We assemble one notification with three template bindings by including the content for each smaller
        ' tile in the next size up. Square310x310 includes Wide310x150, which includes Square150x150.
        ' If we leave off the content for a tile size which the application supports, the user will not see the
        ' notification if the tile is set to that size.

        ' Create a notification for the Square310x310 tile using one of the available templates for the size.
        Dim tileContent As ITileSquare310x310Image = TileContentFactory.CreateTileSquare310x310Image()
        tileContent.AddImageQuery = True
        tileContent.Image.Src = ImageUrl.Text
        tileContent.Image.Alt = "Web Image"

        ' Create a notification for the Wide310x150 tile using one of the available templates for the size.
        Dim wide310x150Content As ITileWide310x150ImageAndText01 = TileContentFactory.CreateTileWide310x150ImageAndText01()
        wide310x150Content.TextCaptionWrap.Text = "This tile notification uses web images."
        wide310x150Content.Image.Src = ImageUrl.Text
        wide310x150Content.Image.Alt = "Web image"

        ' Create a notification for the Square150x150 tile using one of the available templates for the size.
        Dim square150x150Content As ITileSquare150x150Image = TileContentFactory.CreateTileSquare150x150Image()
        square150x150Content.Image.Src = ImageUrl.Text
        square150x150Content.Image.Alt = "Web image"

        ' Attach the Square150x150 template to the Wide310x150 template.
        wide310x150Content.Square150x150Content = square150x150Content

        ' Attach the Wide310x150 template to the Square310x310 template.
        tileContent.Wide310x150Content = wide310x150Content

        ' Send the notification to the application’s tile.
        TileUpdateManager.CreateTileUpdaterForApplication().Update(tileContent.CreateNotification())

        OutputTextBlock.Text = MainPage.PrettyPrint(tileContent.GetContent())
    End Sub

    Private Sub UpdateTileWithWebImageWithStringManipulation_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        ' Create a string with the tile template xml.
        ' Note that the version is set to "2" and that fallbacks are provided for the Square150x150 and Wide310x150 tile sizes.
        ' This is so that the notification can be understood by a Windows 8 machine as well.
        Dim tileXmlString As String = "<tile>" & "<visual version='2' addImageQuery='true'>" & "<binding template='TileSquare150x150Image' fallback='TileSquareImage'>" & "<image id='1' src='" & ImageUrl.Text & "' alt='Web image'/>" & "</binding>" & "<binding template='TileWide310x150ImageAndText01' fallback='TileWideImageAndText01'>" & "<image id='1' src='" & ImageUrl.Text & "' alt='Web image'/>" & "<text id='1'>This tile notification uses web images.</text>" & "</binding>" & "<binding template='TileSquare310x310Image'>" & "<image id='1' src='" & ImageUrl.Text & "' alt='Web image'/>" & "</binding>" & "</visual>" & "</tile>"

        ' Create a DOM.
        Dim tileDOM As New Windows.Data.Xml.Dom.XmlDocument()
        Try
            ' Load the xml string into the DOM, catching any invalid xml characters.
            tileDOM.LoadXml(tileXmlString)

            ' Create a tile notification.
            Dim tile As New TileNotification(tileDOM)

            ' Send the notification to the application’s tile.
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tile)

            OutputTextBlock.Text = MainPage.PrettyPrint(tileDOM.GetXml())
        Catch e1 As Exception
            OutputTextBlock.Text = "Error loading the xml, check for invalid characters in the input"
        End Try
    End Sub

    Private Sub ClearTile_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        TileUpdateManager.CreateTileUpdaterForApplication().Clear()
        OutputTextBlock.Text = "Tile cleared"
    End Sub
End Class
