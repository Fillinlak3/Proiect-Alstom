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
            Turnout.TrailingAnimation(Turnout.TrailingAnimationStates.Stopped)
            Turnout.Deactivate()
        End If

        IXLState = Not IXLState
    End Sub

    Private Sub BttnOccupied(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        If Not Turnout.IsOccupiedActive() Then
            Turnout.Activate(Route.RouteStates.Occupied)
            Turnout.SetDirect()
            Return
        End If

        Turnout.ChangeDirection()
    End Sub

    Private Sub BttnTraffic(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        If Not Turnout.IsTrafficActive() Then
            Turnout.Activate(Route.RouteStates.Traffic)
            Turnout.SetDirect()
            Return
        End If

        Turnout.ChangeDirection()
    End Sub

    Private Sub BttnShunting(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        If Not Turnout.IsShuntingActive() Then
            Turnout.Activate(Route.RouteStates.Shunting)
            Turnout.SetDirect()
            Return
        End If

        Turnout.ChangeDirection()
    End Sub

    Private Sub BttnMMZ(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        If Turnout.IsInterlockingActive() Then
            Turnout.ChangeDirection()
        End If
    End Sub

    Private Sub BttnMFMZ(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        If Turnout.IsOccupiedActive() Then
            Turnout.ChangeDirection()
        End If
    End Sub

    Dim _NeedTrailingStop As Boolean = False
    Private Sub BttnMMZT(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        If Not Turnout.IsTrailingActive() Then
            MessageBox.Show("The turnout is not trailed.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        If _NeedTrailingStop = True Then
            Turnout.TrailingAnimation(Turnout.TrailingAnimationStates.Stopped)
            _NeedTrailingStop = False
            Return
        End If

        Turnout.TrailingAnimation(Turnout.TrailingAnimationStates.Left)
        _NeedTrailingStop = True
    End Sub

    Private Sub BttnTrailed(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        If Turnout.IsTrailingActive() Then
            MessageBox.Show("Trailing already active", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        Turnout.Activate(Route.RouteStates.DefaultRoute)
        Turnout.TrailingAnimation(Turnout.TrailingAnimationStates.Both)
    End Sub
End Class
