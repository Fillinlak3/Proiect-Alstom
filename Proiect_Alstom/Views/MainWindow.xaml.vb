Class MainWindow
    ' Variables
    Dim IXLState As Boolean = False ' False = Inactive
    Dim Turnout As Turnout

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Initialize route with turnout's segments.
        Dim Route = New Route(M_1, M_2, M_3, M_4, M_7, M_8, M_9)
        Turnout = New Turnout(Route, M_4, M_8, M_9, M_2, M_3, M_10, M_5)

    End Sub

    Private Sub ChangeIXLState(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            BTN_IXL_State.Background = New SolidColorBrush(Colors.Green)
            BTN_IXL_State.Content = "Active"
            Turnout.Activate(Route.RouteStates.DefaultRoute)
        Else
            BTN_IXL_State.Background = New SolidColorBrush(Colors.Red)
            BTN_IXL_State.Content = "Inactive"
            Turnout.Deactivate()
        End If

        IXLState = Not IXLState
    End Sub

    Private Sub BtnDirect(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        If Not TypeOf sender Is Button Then
            Return
        End If

        Dim btnName As String = DirectCast(sender, Button).Name
        Dim routeState As Route.RouteStates
        Select Case True
            Case btnName.Contains("Occupied")
                routeState = Route.RouteStates.Occupied
            Case btnName.Contains("Traffic")
                routeState = Route.RouteStates.Traffic
            Case btnName.Contains("Shunting")
                routeState = Route.RouteStates.Shunting
            Case Else
                routeState = Route.RouteStates.Inactive
        End Select


        Turnout.Activate(routeState)
        Turnout.SetDirect()
    End Sub

    Private Sub BtnDeviated(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        If Not TypeOf sender Is Button Then
            Return
        End If

        Dim btnName As String = DirectCast(sender, Button).Name
        Dim routeState As Route.RouteStates
        Select Case True
            Case btnName.Contains("Occupied")
                routeState = Route.RouteStates.Occupied
            Case btnName.Contains("Traffic")
                routeState = Route.RouteStates.Traffic
            Case btnName.Contains("Shunting")
                routeState = Route.RouteStates.Shunting
            Case Else
                routeState = Route.RouteStates.Inactive
        End Select


        Turnout.Activate(routeState)
        Turnout.SetDeviated()
    End Sub
End Class
