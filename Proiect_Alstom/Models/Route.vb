Namespace Models
    Public Class Route
        Public ReadOnly TurnoutSegments As New List(Of Object)
        Private _routeState As RouteStates
        Public Property RouteState As RouteStates
            Get
                Return _routeState
            End Get
            Private Set(value As RouteStates)
                _routeState = value
            End Set
        End Property
        Public Enum RouteStates
            Inactive = 0
            DefaultRoute
            Occupied
            Traffic
            Shunting
        End Enum

        Public Sub New(ParamArray turnoverSegments() As Object)
            For Each segment In turnoverSegments
                TurnoutSegments.Add(segment)
            Next
        End Sub

        Private Sub SetRouteColor(color As Color)
            For Each segment In TurnoutSegments
                segment.Fill = New SolidColorBrush(color)
                segment.Stroke = New SolidColorBrush(Colors.Black)
            Next
        End Sub

        Public Sub SetRoute(Optional routeState As RouteStates = Nothing, Optional blockedAgainstRoutes As Boolean = False)
            If routeState = Nothing Then
                routeState = RouteStates.Inactive
            End If

            Dim strokeColor As Color = Nothing
            Select Case routeState
                Case RouteStates.Inactive
                    SetRouteColor(Colors.Gray)
                Case RouteStates.DefaultRoute
                    SetRouteColor(Colors.Yellow)
                    strokeColor = Colors.Yellow
                Case RouteStates.Occupied
                    SetRouteColor(Colors.Red)
                    strokeColor = Colors.Red
                Case RouteStates.Traffic
                    SetRouteColor(Colors.Lime)
                    strokeColor = Colors.Lime
                Case RouteStates.Shunting
                    SetRouteColor(Colors.Blue)
                    strokeColor = Colors.Blue
            End Select

            ' Save routstate
            Me.RouteState = routeState

            ' If blocked against routes request, M_1, M_2, M_3 will not be entirely filled.
            If blockedAgainstRoutes = True Then
                For i As Integer = 0 To 2
                    TurnoutSegments(i).Fill = New SolidColorBrush(Colors.Transparent)
                    TurnoutSegments(i).Stroke = New SolidColorBrush(strokeColor)
                Next
            End If
        End Sub
    End Class
End Namespace
