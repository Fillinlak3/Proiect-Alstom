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

        Enum Directions
            Direct
            Deviated
        End Enum
        Private _direction As Directions
        Public Property Direction
            Get
                Return _direction
            End Get
            Private Set(value)
                _direction = value
            End Set
        End Property

        Enum TrailingAnimationStates
            Stopped
            Both
            Left
            Right
        End Enum
        Private TrailingAnimationState As TrailingAnimationStates

        Enum AnimationStates
            Stopped
            Running
        End Enum
        Private ForcedUnlockRouteAnimationState As AnimationStates
        Private GaugeViolationAnimationState As AnimationStates

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

            ForcedUnlockRouteAnimationTimer.Interval = 200
            ForcedUnlockRouteAnimationTimer.Enabled = False

            GaugeViolationAnimationTimer.Interval = 200
            GaugeViolationAnimationTimer.Enabled = False
        End Sub

#Region "Directions"
        Public Sub SetDirect()
            Direction = Directions.Direct

            ' SetDirect settings.
            If Not Me.IsBlockedAgainstRoutes() Then
                LeftBranch.Fill = New SolidColorBrush(Colors.White)
            Else
                LeftBranch.Stroke = New SolidColorBrush(Colors.White)
            End If
            LeftPositionIndicator.Visibility = Visibility.Collapsed
            If Not Me.IsRouteLocked() Then
                LockingElement.Fill = New SolidColorBrush(Colors.White)
            End If

            ' Return from SetDeviated.
            If Not Me.IsBlockedAgainstRoutes() Then
                RightBranch.Fill = LeftPositionIndicator.Fill
            Else
                RightBranch.Stroke = LeftPositionIndicator.Fill
            End If
            RightPositionIndicator.Visibility = Visibility.Visible
        End Sub

        Public Sub SetDeviated()
            Direction = Directions.Deviated

            ' SetDirect settings.
            If Not Me.IsBlockedAgainstRoutes() Then
                RightBranch.Fill = New SolidColorBrush(Colors.White)
            Else
                RightBranch.Stroke = New SolidColorBrush(Colors.White)
            End If
            RightPositionIndicator.Visibility = Visibility.Collapsed
            If Not Me.IsRouteLocked() Then
                LockingElement.Fill = New SolidColorBrush(Colors.White)
            End If

            ' Return from SetDirect.
            If Not Me.IsBlockedAgainstRoutes() Then
                LeftBranch.Fill = LeftPositionIndicator.Fill
            Else
                LeftBranch.Stroke = LeftPositionIndicator.Fill
            End If
            LeftPositionIndicator.Visibility = Visibility.Visible
        End Sub

        Public Sub ChangeDirection()
            If Me.IsTurnoutDirectionBlocked() Or Me.IsTrailingActive() Or IsAnyAnimationActive() Then
                Return
            End If

            If Direction = Directions.Deviated Then
                SetDirect()
            Else
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

            ' For route, get the route and display the corresponding color,
            ' but if BMZ active the M_1, M_2, M_3 segments will not be filled.
            If Me.IsBlockedAgainstRoutes() Then
                Route.SetRoute(routeState, True)
            Else
                Route.SetRoute(routeState)
            End If

            ' Checks for the BMMZ if its active and set to deviated to keep it's direction.
            If Me.IsTurnoutDirectionBlocked() And LeftPositionIndicator.Visibility = Visibility.Visible Then
                SetDeviated()
                Return
            End If

            ' Checks for the BMZ if its active and set to deviated to keep it's direction.
            If Me.IsBlockedAgainstRoutes() And LeftPositionIndicator.Visibility = Visibility.Visible Then
                SetDeviated()
                Return
            End If

            ' Checks for the MMZT if its active and set to deviated to keep it's direction.
            If Me.IsTrailingActive() And Me.TrailingAnimationState = TrailingAnimationStates.Left Then
                SetDeviated()
                Return
            End If

            ' Checks if interlocking already running and direction is to deviated
            If IsTurnoutFunctional() AndAlso Direction = Directions.Deviated Then
                SetDeviated()
            Else
                SetDirect()
            End If

            ' If trailing and route changed, keep 
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
            ' Also disable previous actions.
            _isRouteLocked = False
            _isBlockedAgainstRoutes = False
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
            Return Me.TrailingAnimationTimer.Enabled
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

        Public Function IsRouteLocked() As Boolean
            Return _isRouteLocked
        End Function

        Public Function IsBlockedAgainstRoutes() As Boolean
            Return _isBlockedAgainstRoutes
        End Function

        Public Function IsForcedUnlockRouteActive() As Boolean
            Return ForcedUnlockRouteAnimationTimer.Enabled
        End Function

        Public Function IsTurnoutFunctional() As Boolean
            Return Not Route.RouteState = Route.RouteStates.Inactive
        End Function

        Public Function IsGaugeViolationActive() As Boolean
            Return GaugeViolationAnimationTimer.Enabled
        End Function

        Public Function IsAnyAnimationActive() As Boolean
            Return IsTrailingActive() Or IsForcedUnlockRouteActive() Or IsGaugeViolationActive
        End Function
#End Region

        Private Sub ChangeVisibility(control As Object)
            If control Is Nothing Then
                Throw New Exception("Cannot change visibility to a null object.")
            End If

            control.Visibility = If(control.Visibility = Visibility.Visible, Visibility.Collapsed, Visibility.Visible)
        End Sub

        Private WithEvents TrailingAnimationTimer As New Forms.Timer
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
                    Me.Activate(Route.RouteState)
                    LeftPositionIndicator.Visibility = Visibility.Visible
                    RightPositionIndicator.Visibility = Visibility.Visible
                    Me.TrailingAnimationTimer.Enabled = True
            End Select
        End Sub

        Private WithEvents ForcedUnlockRouteAnimationTimer As New Forms.Timer
        Private Sub ForcedUnlockRouteAnimation_TimerElapsed(sender As Object, e As EventArgs) Handles ForcedUnlockRouteAnimationTimer.Tick
            If Me.ForcedUnlockRouteAnimationState = AnimationStates.Stopped Then
                Return
            End If

            For i As Integer = 0 To 2
                ChangeVisibility(Route.TurnoutSegments(i))
            Next

            If Route.TurnoutSegments(0).Visibility = Visibility.Visible Then
                TurnoutName.Background = New SolidColorBrush(Colors.Red)
            Else
                TurnoutName.Background = New SolidColorBrush(Colors.Transparent)
            End If
        End Sub

        Public Sub ForcedUnlockRouteAnimation(forcedUnlockRouteAnimationState As AnimationStates)
            Me.ForcedUnlockRouteAnimationState = forcedUnlockRouteAnimationState

            Select Case forcedUnlockRouteAnimationState
                Case AnimationStates.Stopped
                    ForcedUnlockRouteAnimationTimer.Enabled = False
                    For i As Integer = 0 To 2
                        Route.TurnoutSegments(i).Visibility = Visibility.Visible
                    Next
                    TurnoutName.Background = New SolidColorBrush(Colors.Transparent)
                Case AnimationStates.Running
                    ForcedUnlockRouteAnimationTimer.Enabled = True
            End Select
        End Sub

        Private WithEvents GaugeViolationAnimationTimer As New Forms.Timer
        Private Sub GaugeViolationAnimation_TimerElapsed(sender As Object, e As EventArgs) Handles GaugeViolationAnimationTimer.Tick
            If Me.GaugeViolationAnimationState = AnimationStates.Stopped Then
                Return
            End If

            Select Case Direction
                Case Directions.Direct
                    LeftBranch.Fill = New SolidColorBrush(Colors.Red)
                    ChangeVisibility(LeftBranch)
                Case Directions.Deviated
                    RightBranch.Fill = New SolidColorBrush(Colors.Red)
                    ChangeVisibility(RightBranch)
            End Select
        End Sub

        Public Sub GaugeViolationAnimation(gaugeViolationAnimationState As AnimationStates)
            Me.GaugeViolationAnimationState = gaugeViolationAnimationState

            Select Case gaugeViolationAnimationState
                Case AnimationStates.Stopped
                    GaugeViolationAnimationTimer.Enabled = False

                    If Direction = Directions.Direct Then
                        LeftBranch.Fill = New SolidColorBrush(Colors.White)
                        LeftBranch.Visibility = Visibility.Visible
                    Else
                        RightBranch.Fill = New SolidColorBrush(Colors.White)
                        RightBranch.Visibility = Visibility.Visible
                    End If
                Case AnimationStates.Running
                    GaugeViolationAnimationTimer.Enabled = True
            End Select
        End Sub

#Region "Routes"
        Private _isRouteLocked As Boolean = False
        Public Sub LockRoute()
            LockingElement.Fill = LeftPositionIndicator.Fill
            _isRouteLocked = True
        End Sub

        Public Sub UnlockRoute()
            LockingElement.Fill = New SolidColorBrush(Colors.White)
            _isRouteLocked = False
        End Sub

        Private _isBlockedAgainstRoutes As Boolean = False
        Public Sub BlockAgainstRoutes()
            _isBlockedAgainstRoutes = True
            Me.Activate(Route.RouteState)
        End Sub

        Public Sub UnblockAgainstRoutes()
            _isBlockedAgainstRoutes = False
            Me.ForcedUnlockRouteAnimation(AnimationStates.Stopped)
            Me.Activate(Route.RouteState)
            Debug.WriteLine(Direction = Directions.Deviated)
        End Sub
#End Region
    End Class
End Namespace
