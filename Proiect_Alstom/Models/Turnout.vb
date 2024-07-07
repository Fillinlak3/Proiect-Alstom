Namespace Models
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

        Enum TrailingAnimationStates
            Stopped
            Both
            Left
            Right
        End Enum
        Private TrailingAnimationState As TrailingAnimationStates

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

            TrailingAnimationTimer.Interval = 200
            TrailingAnimationTimer.Enabled = False
        End Sub

#Region "Directions"
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

        Public Sub ChangeDirection()
            If Me.IsTurnoutDirectionBlocked() Then
                Return
            End If

            If LeftPositionIndicator.Visibility = Visibility.Visible Then
                SetDirect()
            ElseIf RightPositionIndicator.Visibility = Visibility.Visible Then
                SetDeviated()
            End If
        End Sub

        Public Sub BlockTurnoutDirection()
            TurnoutName.Background = New SolidColorBrush(Colors.Red)
        End Sub

        Public Sub UnblockTurnoutDirection()
            TurnoutName.Background = New SolidColorBrush(Colors.Transparent)
        End Sub
#End Region

        Public Sub Activate(Optional ByVal routeState As Route.RouteStates = Nothing)
            If routeState = Nothing Then
                routeState = Route.RouteStates.DefaultRoute
            End If

            ' Uncomment this section to disable trailing animation when changing the route state.
            'If IsTrailingActive() Then
            '    Me.TrailingAnimation(TrailingAnimationStates.Stopped)
            'End If

            ' Activate turnout name.
            TurnoutName.Foreground = New SolidColorBrush(Colors.White)
            ' Set route and direction.
            Route.SetRoute(routeState)

            ' Checks for the BMMZ if its active and set to deviated to keep it's direction.
            If Me.IsTurnoutDirectionBlocked() And LeftPositionIndicator.Visibility = Visibility.Visible Then
                SetDeviated()
                Return
            End If

            ' Otherwise, it's set to direct so keep the direct direction.
            SetDirect()

            If IsTrailingActive() Then
                LeftPositionIndicator.Visibility = Visibility.Collapsed
                RightPositionIndicator.Visibility = Visibility.Collapsed
            End If
        End Sub

        Public Sub Deactivate()
            ' Activate turnout name.
            TurnoutName.Foreground = New SolidColorBrush(Colors.Gray)
            TurnoutName.Background = New SolidColorBrush(Colors.Transparent)
            ' Set route to inactive.
            Route.SetRoute(Route.RouteStates.Inactive)
            LeftPositionIndicator.Visibility = Visibility.Visible
            RightPositionIndicator.Visibility = Visibility.Visible
        End Sub

#Region "Checkings"
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

        Public Function IsTrailingActive() As Boolean
            Return Me.TrailingAnimationTimer.Enabled = True
        End Function

        Public Function IsTurnoutDirectionBlocked() As Boolean
            ' Check if the Background property is a SolidColorBrush
            Dim backgroundBrush As SolidColorBrush = TryCast(TurnoutName.Background, SolidColorBrush)

            ' If it is a SolidColorBrush and its color is Red, return True
            If backgroundBrush IsNot Nothing AndAlso backgroundBrush.Color = Colors.Red Then
                Return True
            Else
                Return False
            End If
        End Function
#End Region

        Private Sub ChangeVisibility(control As Object)
            If control Is Nothing Then
                Throw New Exception("Cannot change visibility to a null object.")
            End If

            control.Visibility = If(control.Visibility = Visibility.Visible, Visibility.Collapsed, Visibility.Visible)
        End Sub

        Private WithEvents TrailingAnimationTimer As New System.Windows.Forms.Timer
        Private Sub TrailAnimation_TimerElapsed(sender As Object, e As EventArgs) Handles TrailingAnimationTimer.Tick
            Select Case Me.TrailingAnimationState
                Case TrailingAnimationStates.Both
                    ChangeVisibility(LeftPositionIndicator)
                    ChangeVisibility(RightPositionIndicator)
                Case TrailingAnimationStates.Left
                    ChangeVisibility(LeftPositionIndicator)
            End Select
        End Sub

        Public Sub TrailingAnimation(trailingAnimationState As TrailingAnimationStates)
            Me.TrailingAnimationState = trailingAnimationState

            Select Case trailingAnimationState
                Case TrailingAnimationStates.Stopped
                    Me.TrailingAnimationTimer.Enabled = False
                    SetDirect()
                Case TrailingAnimationStates.Left
                    SetDeviated()
                Case Else
                    LeftPositionIndicator.Visibility = Visibility.Visible
                    RightPositionIndicator.Visibility = Visibility.Visible
                    Me.TrailingAnimationTimer.Enabled = True
            End Select
        End Sub


    End Class
End Namespace
