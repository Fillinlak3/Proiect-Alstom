Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Namespace ViewModels
    Public MustInherit Class BaseViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Overridable Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Protected Overridable Function SetProperty(Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing) As Boolean
            If Equals(storage, value) Then
                Return False
            End If

            storage = value
            OnPropertyChanged(propertyName)
            Return True
        End Function
    End Class
End Namespace
