'*********************************************************
'
' Copyright (c) Microsoft. All rights reserved.
'
'*********************************************************

Imports System

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Partial Public NotInheritable Class Scenario1
    Inherits SDKTemplate.Common.LayoutAwarePage

    ' A pointer back to the main page.  This is needed if you want to call methods in MainPage such
    ' as NotifyUser()
    Private rootPage As MainPage = MainPage.Current

    ' holds the sample data - See SampleData folder
    Private storeData As Expression.Blend.SampleData.SampleDataSource.StoreData = Nothing

    Public Sub New()
        Me.InitializeComponent()

        ' create a new instance of store data
        storeData = New Expression.Blend.SampleData.SampleDataSource.StoreData()
        ' set the source of the GridView to be the sample data
        ItemGridView.ItemsSource = storeData.Collection
    End Sub

    ''' <summary>
    ''' Invoked when this page is about to be displayed in a Frame.
    ''' </summary>
    ''' <param name="e">Event data that describes how this page was reached.  The Parameter
    ''' property is typically used to configure the page.</param>
    Protected Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub


    ''' <summary>
    ''' This is the click handler for the 'Default' button.  You would replace this with your own handler
    ''' if you have a button or buttons on this page.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Default_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim b As Button = TryCast(sender, Button)
        If b IsNot Nothing Then
            rootPage.NotifyUser("You clicked the " & b.Name & " button", NotifyType.StatusMessage)
        End If
    End Sub

    ''' <summary>
    ''' This is the click handler for the 'Other' button.  You would replace this with your own handler
    ''' if you have a button or buttons on this page.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Other_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim b As Button = TryCast(sender, Button)
        If b IsNot Nothing Then
            rootPage.NotifyUser("You clicked the " & b.Name & " button", NotifyType.StatusMessage)
        End If
    End Sub

    ''' <summary>
    ''' We will visualize the data item in asynchronously in multiple phases for improved panning user experience 
    ''' of large lists.  In this sample scneario, we will visualize different parts of the data item
    ''' in the following order:
    ''' 
    '''     1) Placeholders (visualized synchronously - Phase 0)
    '''     2) Tilte (visualized asynchronously - Phase 1)
    '''     3) Category and Image (visualized asynchronously - Phase 2)
    '''
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    Private Sub ItemGridView_ContainerContentChanging(ByVal sender As ListViewBase, ByVal args As ContainerContentChangingEventArgs)
        Dim iv As ItemViewer = TryCast(args.ItemContainer.ContentTemplateRoot, ItemViewer)

        If args.InRecycleQueue = True Then
            iv.ClearData()
        ElseIf args.Phase = 0 Then
            iv.ShowPlaceholder(TryCast(args.Item, Expression.Blend.SampleData.SampleDataSource.Item))

            ' Register for async callback to visualize Title asynchronously
            args.RegisterUpdateCallback(ContainerContentChangingDelegate)
        ElseIf args.Phase = 1 Then
            iv.ShowTitle()
            args.RegisterUpdateCallback(ContainerContentChangingDelegate)
        ElseIf args.Phase = 2 Then
            iv.ShowCategory()
            iv.ShowImage()
        End If

        ' For imporved performance, set Handled to true since app is visualizing the data item
        args.Handled = True
    End Sub

    ''' <summary>
    ''' Managing delegate creation to ensure we instantiate a single instance for 
    ''' optimal performance. 
    ''' </summary>
    Private ReadOnly Property ContainerContentChangingDelegate() As TypedEventHandler(Of ListViewBase, ContainerContentChangingEventArgs)
        Get
            If _delegate Is Nothing Then
                _delegate = New TypedEventHandler(Of ListViewBase, ContainerContentChangingEventArgs)(AddressOf ItemGridView_ContainerContentChanging)
            End If
            Return _delegate
        End Get
    End Property
    Private _delegate As TypedEventHandler(Of ListViewBase, ContainerContentChangingEventArgs)
End Class
