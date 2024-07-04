Imports Proiect_Alstom.Route

Public Class Turnout
    Dim Route As Route
    Dim StaticPlusPosition As Object
    Dim TurnoutName As Object

    ' Turnout changeable elements
    Dim LockingElement As Object
    Dim LeftPositionIndicator As Object
    Dim RightPositionIndicator As Object
    Dim LeftBranch As Object
    Dim RightBranch As Object

    Public Sub New(route As Route, lockingElement As Object, leftPositionIndicator As Object, rightPositionIndicator As Object,
                   leftBranch As Object, rightBranch As Object, staticPlusPosition As Object, turnoutName As Object)
        Me.Route = route
        Me.StaticPlusPosition = staticPlusPosition
        Me.TurnoutName = turnoutName

        Me.LockingElement = lockingElement
        Me.LeftPositionIndicator = leftPositionIndicator
        Me.RightPositionIndicator = rightPositionIndicator
        Me.LeftBranch = leftBranch
        Me.RightBranch = rightBranch
    End Sub

    Public Sub SetDirect()
        ' SetDirect settings.
        LeftBranch.Fill = New SolidColorBrush(Colors.White)
        LockingElement.Fill = New SolidColorBrush(Colors.White)
        LeftPositionIndicator.Visibility = Visibility.Collapsed

        ' Return from SetDeviated.
        RightBranch.Fill = LeftPositionIndicator.Fill
        RightPositionIndicator.Visibility = Visibility.Visible
    End Sub

    Public Sub SetDeviated()
        ' SetDirect settings.
        RightBranch.Fill = New SolidColorBrush(Colors.White)
        LockingElement.Fill = New SolidColorBrush(Colors.White)
        RightPositionIndicator.Visibility = Visibility.Collapsed

        ' Return from SetDirect.
        LeftBranch.Fill = LeftPositionIndicator.Fill
        LeftPositionIndicator.Visibility = Visibility.Visible
    End Sub

    Public Sub Activate(Optional ByVal routeState As RouteStates = Nothing)
        If routeState = Nothing Then
            routeState = RouteStates.DefaultRoute
        End If

        ' Activate turnout name.
        TurnoutName.Foreground = New SolidColorBrush(Colors.White)
        ' Set route and direction.
        Route.SetRoute(routeState)
        SetDirect()
    End Sub

    Public Sub Deactivate()
        ' Activate turnout name.
        TurnoutName.Foreground = New SolidColorBrush(Colors.Gray)
        ' Set route to inactive.
        Route.SetRoute(Route.RouteStates.Inactive)
        LeftPositionIndicator.Visibility = Visibility.Visible
        RightPositionIndicator.Visibility = Visibility.Visible
    End Sub

    Public Sub ChangeDirection()
        If LeftPositionIndicator.Visibility = Visibility.Visible Then
            SetDirect()
        ElseIf RightPositionIndicator.Visibility = Visibility.Visible Then
            SetDeviated()
        End If
    End Sub

    Public Function IsInterlockingActive() As Boolean
        Return Route.RouteState = Route.RouteStates.DefaultRoute
    End Function

    Public Function IsOccupiedActive() As Boolean
        Return Route.RouteState = Route.RouteStates.Occupied
    End Function

    Public Function IsTrafficActive() As Boolean
        Return Route.RouteState = Route.RouteStates.Traffic
    End Function

    Public Function IsShuntingActive() As Boolean
        Return Route.RouteState = Route.RouteStates.Shunting
    End Function

End Class
