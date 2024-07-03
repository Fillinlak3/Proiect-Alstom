Public Class Route
    Dim TurnoverSegments As New List(Of Object)()
    Public Enum RouteStates
        Inactive = 0
        DefaultRoute
        Occupied
        Traffic
        Chanting
    End Enum

    Public Sub New(ParamArray turnoverSegments() As Object)
        For Each segment In turnoverSegments
            Me.TurnoverSegments.Add(segment)
        Next
    End Sub

    Private Sub SetRouteColor(color As Color)
        For Each segment In TurnoverSegments
            segment.Fill = New SolidColorBrush(color)
        Next
    End Sub

    Public Sub SetRoute(Optional ByVal routeState As RouteStates = Nothing)
        If routeState = Nothing Then
            routeState = RouteStates.Inactive
        End If

        Select Case routeState
            Case RouteStates.Inactive
                SetRouteColor(Colors.Gray)
            Case RouteStates.DefaultRoute
                SetRouteColor(Colors.Yellow)
            Case RouteStates.Occupied
                SetRouteColor(Colors.Red)
            Case RouteStates.Traffic
                SetRouteColor(Colors.Green)
            Case RouteStates.Chanting
                SetRouteColor(Colors.Blue)
        End Select
    End Sub
End Class
