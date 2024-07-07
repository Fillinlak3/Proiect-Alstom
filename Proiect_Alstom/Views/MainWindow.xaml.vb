Imports Proiect_Alstom.Models

Class MainWindow
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Initialize route with turnout's segments.
        Dim Route = New Route(M_1, M_2, M_3, M_4, M_7, M_8, M_9)
        Program.Turnout = New Turnout(Route, M_4, M_8, M_9, M_2, M_3, M_10, M_5)
    End Sub

    Private Sub ChangeIXLState(sender As Object, e As RoutedEventArgs)
        If Program.IXLState = False Then
            BTN_IXL_State.Background = New SolidColorBrush(Colors.Green)
            BTN_IXL_State.Content = "Active"
            Program.Turnout.Activate(Route.RouteStates.DefaultRoute)
        Else
            BTN_IXL_State.Background = New SolidColorBrush(Colors.Red)
            BTN_IXL_State.Content = "Inactive"
            Program.Turnout.TrailingAnimation(Turnout.TrailingAnimationStates.Stopped)
            Program.Turnout.Deactivate()
        End If

        Program.IXLState = Not Program.IXLState
    End Sub
End Class
