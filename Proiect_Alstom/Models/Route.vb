﻿Public Class Route
    Private ReadOnly TurnoutSegments As New List(Of Object)()
    Public RouteState As RouteStates
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
                SetRouteColor(Colors.Lime)
            Case RouteStates.Shunting
                SetRouteColor(Colors.Blue)
        End Select

        Me.RouteState = routeState
    End Sub

End Class
