Class MainWindow

    Public Sub InactiveRoute() ' Galben
        For Each segment In TurnoverSegments
            segment.Fill = New SolidColorBrush(Colors.Gray)
        Next
    End Sub

    Public Sub DefaultRoute() ' Galben
        For Each segment In TurnoverSegments
            segment.Fill = New SolidColorBrush(Colors.Yellow)
        Next
    End Sub

    Public Sub OccupiedRoute() ' Rosu

    End Sub

    Public Sub TrafficRoute() ' Green

    End Sub

    Public Sub ChantingRoute() ' Albastru

    End Sub

    ' Variables
    Dim IXLState As Boolean = False ' False = Inactive
    Dim TurnoverSegments As New List(Of Object)()
    Dim StaticPlusPosition As Object = M_10
    Dim TurnoutName As Object = M_5

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        TurnoverSegments.Add(M_1)
        TurnoverSegments.Add(M_2)
        TurnoverSegments.Add(M_3)
        TurnoverSegments.Add(M_4)
        TurnoverSegments.Add(M_7)
        TurnoverSegments.Add(M_8)
        TurnoverSegments.Add(M_9)
    End Sub

    Private Sub ChangeIXLState(sender As Object, e As RoutedEventArgs)
        If IXLState = False Then
            BTN_IXL_State.Background = New SolidColorBrush(Colors.Green)
            BTN_IXL_State.Content = "Active"
            DefaultRoute()
        Else
            BTN_IXL_State.Background = New SolidColorBrush(Colors.Red)
            BTN_IXL_State.Content = "Inactive"
            InactiveRoute()
        End If

        IXLState = Not IXLState
    End Sub
End Class
