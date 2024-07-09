Imports Proiect_Alstom.Models
Imports Proiect_Alstom.Services
Imports Services.Logging

Namespace ViewModels
    Public Class ProgramViewModel
        Inherits BaseViewModel

        Public IXLState As Boolean = False ' False = Inactive
        Public Turnout As Turnout

#Region "RelayCommands"
        Private _occupiedBttnCommand As RelayCommand
        Public Property OccupiedBttnCommand As RelayCommand
            Get
                Return _occupiedBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _occupiedBttnCommand = value
            End Set
        End Property

        Private _trafficBttnCommand As RelayCommand
        Public Property TrafficBttnCommand As RelayCommand
            Get
                Return _trafficBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _trafficBttnCommand = value
            End Set
        End Property

        Private _shuntingBttnCommand As RelayCommand
        Public Property ShuntingBttnCommand As RelayCommand
            Get
                Return _shuntingBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _shuntingBttnCommand = value
            End Set
        End Property

        Private _trailedBttnCommand As RelayCommand
        Public Property TrailedBttnCommand As RelayCommand
            Get
                Return _trailedBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _trailedBttnCommand = value
            End Set
        End Property

        Private _lockinRouteBttnCommand As RelayCommand
        Public Property LockinRouteBttnCommand As RelayCommand
            Get
                Return _lockinRouteBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _lockinRouteBttnCommand = value
            End Set
        End Property

        Private _MMZBttnCommand As RelayCommand
        Public Property MMZBttnCommand As RelayCommand
            Get
                Return _MMZBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _MMZBttnCommand = value
            End Set
        End Property

        Private _MFMZBttnCommand As RelayCommand
        Public Property MFMZBttnCommand As RelayCommand
            Get
                Return _MFMZBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _MFMZBttnCommand = value
            End Set
        End Property

        Private _MMZTBttnCommand As RelayCommand
        Public Property MMZTBttnCommand As RelayCommand
            Get
                Return _MMZTBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _MMZTBttnCommand = value
            End Set
        End Property

        Private _BMMZBttnCommand As RelayCommand
        Public Property BMMZBttnCommand As RelayCommand
            Get
                Return _BMMZBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _BMMZBttnCommand = value
            End Set
        End Property

        Private _DMMZBttnCommand As RelayCommand
        Public Property DMMZBttnCommand As RelayCommand
            Get
                Return _DMMZBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _DMMZBttnCommand = value
            End Set
        End Property

        Private _BMZBttnCommand As RelayCommand
        Public Property BMZBttnCommand As RelayCommand
            Get
                Return _BMZBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _BMZBttnCommand = value
            End Set
        End Property

        Private _DMZBttnCommand As RelayCommand
        Public Property DMZBttnCommand As RelayCommand
            Get
                Return _DMZBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _DMZBttnCommand = value
            End Set
        End Property

        Private _FDMZBttnCommand As RelayCommand
        Public Property FDMZBttnCommand As RelayCommand
            Get
                Return _FDMZBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _FDMZBttnCommand = value
            End Set
        End Property

        Private _AVGBttnCommand As RelayCommand
        Public Property AVGBttnCommand As RelayCommand
            Get
                Return _AVGBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _AVGBttnCommand = value
            End Set
        End Property

        Private _gaugeViolationBttnCommand As RelayCommand
        Public Property GaugeViolationBttnCommand As RelayCommand
            Get
                Return _gaugeViolationBttnCommand
            End Get
            Private Set(value As RelayCommand)
                _gaugeViolationBttnCommand = value
            End Set
        End Property
#End Region

        Public Sub New()
            _occupiedBttnCommand = New RelayCommand(AddressOf BttnOccupied)
            _trafficBttnCommand = New RelayCommand(AddressOf BttnTraffic)
            _shuntingBttnCommand = New RelayCommand(AddressOf BttnShunting)
            _trailedBttnCommand = New RelayCommand(AddressOf BttnTrailed)
            _lockinRouteBttnCommand = New RelayCommand(AddressOf BttnLockinRoute)
            _gaugeViolationBttnCommand = New RelayCommand(AddressOf BttnGaugeViolation)

            _MMZBttnCommand = New RelayCommand(AddressOf BttnMMZ)
            _MFMZBttnCommand = New RelayCommand(AddressOf BttnMFMZ)
            _MMZTBttnCommand = New RelayCommand(AddressOf BttnMMZT)
            _BMMZBttnCommand = New RelayCommand(AddressOf BttnBMMZ)
            _DMMZBttnCommand = New RelayCommand(AddressOf BttnDMMZ)
            _BMZBttnCommand = New RelayCommand(AddressOf BttnBMZ)
            _DMZBttnCommand = New RelayCommand(AddressOf BttnDMZ)
            _FDMZBttnCommand = New RelayCommand(AddressOf BttnFDMZ)
            _AVGBttnCommand = New RelayCommand(AddressOf BttnAVG)

            Logger.LogInfo("ProgramViewModel/ctor", "Object created.")
        End Sub

        Public Sub Reset()
            _needToBeConfirmed = False
            _NeedTrailingStop = False
            _lockedInRoute = False

            Turnout.TrailingAnimation(Turnout.TrailingAnimationStates.Stopped)
            Turnout.ForcedUnlockRouteAnimation(Turnout.AnimationStates.Stopped)
            Turnout.GaugeViolationAnimation(Turnout.AnimationStates.Stopped)
            Turnout.Deactivate()
            Logger.LogInfo("ProgramViewModel/Reset", "Completed.")
        End Sub

        Private Sub BttnOccupied(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Not Turnout.IsOccupiedActive() Then
                Turnout.Activate(Route.RouteStates.Occupied)
                Return
            End If

            Turnout.ChangeDirection()
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button Occupied", "Route set to occupied.")
        End Sub

        Private Sub BttnTraffic(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Not Turnout.IsTrafficActive() Then
                Turnout.Activate(Route.RouteStates.Traffic)
                Return
            End If

            Turnout.ChangeDirection()
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button Traffic", "Route set to traffic.")
        End Sub

        Private Sub BttnShunting(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Not Turnout.IsShuntingActive() Then
                Turnout.Activate(Route.RouteStates.Shunting)
                Return
            End If

            Turnout.ChangeDirection()
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button Shunting", "Route set to shunting.")
        End Sub

        Private Sub BttnTrailed(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Turnout.IsTrailingActive() Then
                MessageBox.Show("Trailing already active", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button Trailed", "Action denied.")
                Return
            End If

            If Turnout.IsAnyAnimationActive() Or Turnout.IsBlockedAgainstRoutes() Or Turnout.IsTurnoutDirectionBlocked() Then
                MessageBox.Show("Another action is in progress.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button Trailed", "Another action in progress.")
                Return
            End If

            Turnout.TrailingAnimation(Turnout.TrailingAnimationStates.Both)
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button Trailed", "Trailing active.")
        End Sub

        Private Sub BttnLockinRoute(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Turnout.IsInterlockingActive() Then
                MessageBox.Show("Cannot lock a default route.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button Lock-in Route", "Action denied for the default route.")
                Return
            End If

            ' If trailing active, cannot lock route
            If Turnout.IsTrailingActive() Then
                MessageBox.Show("Cannot lock route while trailing.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button Lock-in Route", "Action denied for a trailed turnout.")
                Return
            End If

            ' If route locked by FDMZ, then needs to be confirmed by
            ' pressing Lock-in again to stop FDMZ animation
            If _lockedInRoute = True Then
                _lockedInRoute = False
                _needToBeConfirmed = True
                Turnout.LockRoute()
                FileLogger.Write(Logger.LogMessageTypes.Warning, "Button Lock-in Route", "Route locked-in for FDMZ. Awaiting route to be freed.")

                Return
            ElseIf _needToBeConfirmed = True Then
                _needToBeConfirmed = False
                Turnout.UnlockRoute()
                Turnout.ForcedUnlockRouteAnimation(Turnout.AnimationStates.Stopped)
                FileLogger.Write(Logger.LogMessageTypes.Info, "Button Lock-in Route", "Route freed.")
                Return
            End If

            ' If BMZ active, cannot Lock-in. Only possible when FDMZ active.
            If Turnout.IsBlockedAgainstRoutes() Then
                If Turnout.IsRouteLocked() Then
                    MessageBox.Show("Cannot unlock route while it's blocked against routes.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                    FileLogger.Write(Logger.LogMessageTypes.Error, "Button Lock-in Route", "Cannot unlock route while BMZ active.")
                Else
                    MessageBox.Show("Cannot lock route while it's blocked against routes.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                    FileLogger.Write(Logger.LogMessageTypes.Error, "Button Lock-in Route", "Cannot lock route while BMZ active.")
                End If
                Return
            End If

            ' Change lock state.
            If Not Turnout.IsRouteLocked() Then
                Turnout.LockRoute()
                FileLogger.Write(Logger.LogMessageTypes.Classic, "Button Lock-in Route", "Locked.")
            Else
                Turnout.UnlockRoute()
                FileLogger.Write(Logger.LogMessageTypes.Classic, "Button Lock-in Route", "Unlocked.")
            End If
        End Sub

        Private Sub BttnMMZ(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            ' Bttn functionality enabled only for default route.
            If Not Turnout.IsInterlockingActive() Then
                Return
            End If

            If Turnout.IsTrailingActive() Then
                MessageBox.Show("Cannot operate while trailing.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button MMZ", "Action denied while trailing is active.")
                Return
            End If

            Turnout.ChangeDirection()
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button MMZ", $"Direction changed to: {If(Turnout.Direction = Turnout.Directions.Direct, "Direct", "Deviated")}.")

        End Sub

        Private Sub BttnMFMZ(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            ' Bttn functionality enabled only for occupied.
            If Not Turnout.IsOccupiedActive() Then
                Return
            End If

            If Turnout.IsTrailingActive() Then
                MessageBox.Show("Cannot operate while trailing.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button MFMZ", "Action denied while trailing active.")
                Return
            End If

            Turnout.ChangeDirection()
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button MFMZ", $"Direction changed to: {If(Turnout.Direction = Turnout.Directions.Direct, "Direct", "Deviated")}.")
        End Sub

        Dim _NeedTrailingStop As Boolean = False
        Private Sub BttnMMZT(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Not Turnout.IsTrailingActive() Then
                MessageBox.Show("The turnout is not trailed.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button MMZT", "Action denied trailing must be active.")
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

        Private Sub BttnBMMZ(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Turnout.IsTrailingActive() Then
                MessageBox.Show("Cannot block while trailing.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button BMMZ", "Action denied while trailing active.")
                Return
            End If

            If Turnout.IsBlockedAgainstRoutes() Then
                MessageBox.Show("Cannot block while locked against routes.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button BMMZ", "Action denied while BMZ active.")
                Return
            End If

            If Turnout.IsTurnoutDirectionBlocked() Then
                MessageBox.Show("Direction is already blocked.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button BMMZ", "Action denied. Direction already blocked.")
                Return
            End If

            Turnout.BlockTurnoutDirection()
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button BMMZ", "Turnout direction blocked.")
        End Sub

        Private Sub BttnDMMZ(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Not Turnout.IsTurnoutDirectionBlocked() Then
                MessageBox.Show("Direction is not blocked.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button DMMZ", "Action denied. Direction is not blocked.")
                Return
            End If

            Turnout.UnblockTurnoutDirection()
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button DMMZ", "Turnout direction unblocked.")
        End Sub

        Private Sub BttnBMZ(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Turnout.IsRouteLocked() Then
                MessageBox.Show("Cannot block. The route is locked.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button BMZ", "Action denied. The route is locked-in.")
                Return
            End If

            If Turnout.IsBlockedAgainstRoutes() Then
                MessageBox.Show("Already blocked against routes.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button BMZ", "Action denied. Route already blocked.")
                Return
            End If

            If Turnout.IsAnyAnimationActive() Or Turnout.IsBlockedAgainstRoutes() Or Turnout.IsTurnoutDirectionBlocked() Then
                MessageBox.Show("Another action is in progress.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button BMZ", "Action denied. Another action is in progress.")
                Return
            End If

            Turnout.BlockAgainstRoutes()
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button BMZ", "Blocked against routes.")
        End Sub

        Private Sub BttnDMZ(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Not Turnout.IsBlockedAgainstRoutes() Then
                MessageBox.Show("Direction is not blocked against routes.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button DMZ", "Action denied. The route is not blocked against routes.")
                Return
            End If

            _needToBeConfirmed = False
            _lockedInRoute = False
            Turnout.UnblockAgainstRoutes()
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button DMZ", "Unblocked against routes.")
        End Sub

        Private _needToBeConfirmed = False
        Private _lockedInRoute = False
        Private Sub BttnFDMZ(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Not Turnout.IsBlockedAgainstRoutes() Then
                MessageBox.Show("The route is not blocked.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button FDMZ", "Action denied. Route is not blocked.")

                Return
            End If

            If _lockedInRoute = True OrElse _needToBeConfirmed = True Then
                MessageBox.Show("The route was already forcefully unlocked. Awaiting lock-in for the route.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            Turnout.ForcedUnlockRouteAnimation(Turnout.AnimationStates.Running)
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button FDMZ", "Forced unlocked route is active. Awaiting for lock-in.")

            _lockedInRoute = True
        End Sub

        Private Sub BttnAVG(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Not Turnout.IsGaugeViolationActive() Then
                MessageBox.Show("Gauge violation not active.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button AVG", "Action denied. Gauge violation not active.")
                Return
            End If

            Turnout.GaugeViolationAnimation(Turnout.AnimationStates.Stopped)
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button AVG", "Gauge violation canceled.")
        End Sub

        Private Sub BttnGaugeViolation(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Turnout.IsGaugeViolationActive() Then
                MessageBox.Show("Gauge violation already set.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button Gauge Violation", "Action denied. Gauge violation already active.")
                Return
            End If

            If Turnout.IsAnyAnimationActive() Or Turnout.IsBlockedAgainstRoutes() Or Turnout.IsTurnoutDirectionBlocked() Then
                MessageBox.Show("Another action is in progress.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                FileLogger.Write(Logger.LogMessageTypes.Error, "Button Gauge Violation", "Action denied. Another action in progress.")
                Return
            End If

            Turnout.GaugeViolationAnimation(Turnout.AnimationStates.Running)
            FileLogger.Write(Logger.LogMessageTypes.Classic, "Button Gauge Violation", "Gauge violation active.")
        End Sub
    End Class
End Namespace