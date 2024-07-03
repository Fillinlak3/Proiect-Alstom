Class MainWindow
    ' Variables
    Dim IXLState As Boolean = False ' False = Inactive
    Dim Route As Route
    Dim StaticPlusPosition As Object = M_10
    Dim TurnoutName As Object = M_5

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Initialize route with turnout's segments.
        Route = New Route(M_1, M_2, M_3, M_4, M_7, M_8, M_9)
    End Sub

    Private Sub ChangeIXLState(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            BTN_IXL_State.Background = New SolidColorBrush(Colors.Green)
            BTN_IXL_State.Content = "Active"
            Route.SetRoute(Route.RouteStates.DefaultRoute)
        Else
            BTN_IXL_State.Background = New SolidColorBrush(Colors.Red)
            BTN_IXL_State.Content = "Inactive"
            Route.SetRoute(Route.RouteStates.Inactive)
        End If

        IXLState = Not IXLState
    End Sub
End Class
