Imports Proiect_Alstom.Models
Imports Proiect_Alstom.Services

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
#End Region

        Public Sub New()
            _occupiedBttnCommand = New RelayCommand(AddressOf BttnOccupied)
            _trafficBttnCommand = New RelayCommand(AddressOf BttnTraffic)
            _shuntingBttnCommand = New RelayCommand(AddressOf BttnShunting)
            _trailedBttnCommand = New RelayCommand(AddressOf BttnTrailed)

            _MMZBttnCommand = New RelayCommand(AddressOf BttnMMZ)
            _MFMZBttnCommand = New RelayCommand(AddressOf BttnMFMZ)
            _MMZTBttnCommand = New RelayCommand(AddressOf BttnMMZT)
            _BMMZBttnCommand = New RelayCommand(AddressOf BttnBMMZ)
            _DMMZBttnCommand = New RelayCommand(AddressOf BttnDMMZ)
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
        End Sub

        Private Sub BttnTrailed(Optional parameter As Object = Nothing)
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

        Private Sub BttnMMZ(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            ' Bttn functionality enabled only for interlocking.
            If Not Turnout.IsInterlockingActive() Then
                Return
            End If

            If Turnout.IsTrailingActive() Then
                MessageBox.Show("Cannot operate while trailing.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            Turnout.ChangeDirection()
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
                Return
            End If

            Turnout.ChangeDirection()
        End Sub

        Dim _NeedTrailingStop As Boolean = False
        Private Sub BttnMMZT(Optional parameter As Object = Nothing)
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

        Private Sub BttnBMMZ(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Turnout.IsTrailingActive() Then
                MessageBox.Show("Cannot block while trailing.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Turnout.IsTurnoutDirectionBlocked() Then
                MessageBox.Show("Direction is already blocked.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            Turnout.BlockTurnoutDirection()
        End Sub

        Private Sub BttnDMMZ(Optional parameter As Object = Nothing)
            If IXLState = False Then
                MessageBox.Show("Interlocking is INACTIVE", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            If Not Turnout.IsTurnoutDirectionBlocked() Then
                MessageBox.Show("Direction is not blocked.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            Turnout.UnblockTurnoutDirection()
        End Sub
    End Class
End Namespace
