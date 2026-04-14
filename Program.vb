Option Strict On
Option Infer Off
Imports VbPixelGameEngine

Public NotInheritable Class Program
    Inherits PixelGameEngine

    Private ReadOnly m_digitMatrix(79, 59) As Integer
    Private Const CELL_SIZE As Integer = 10
    Private Const UPDATE_INTERVAL As Single = 0.5F

    Public Sub New()
        AppName = "VBPGE Cinquain Wave"
    End Sub

    Protected Overrides Function OnUserCreate() As Boolean
        For j As Integer = 0 To 59
            For i As Integer = 0 To 79
                Dim waveValue As Double = CalculateWaveValue(i, j)
                m_digitMatrix(i, j) = CInt(Math.Max(0, Math.Min(CELL_SIZE, waveValue)))
            Next i
        Next j
        Return True
    End Function

    Private Shared Function CalculateWaveValue(i As Integer, j As Integer,
                                               Optional phase As Double = 0) As Double
        Return Math.Sin(i * 0.1 + j * 0.05 + phase) * (CELL_SIZE / 2) + (CELL_SIZE / 2)
    End Function

    Private Sub UpdateEachCell()
        Static phase As Double = 0
        phase += 0.1
        For j As Integer = 0 To 59
            For i As Integer = 0 To 79
                Dim waveValue As Double = CalculateWaveValue(i, j, phase)
                m_digitMatrix(i, j) = CInt(Math.Max(0, Math.Min(CELL_SIZE, waveValue)))
            Next i
        Next j
    End Sub

    Private Sub DrawEachCell()
        For i As Integer = 0 To 79
            For j As Integer = 0 To 59
                Dim x As Integer = i * CELL_SIZE
                Dim y As Integer = j * CELL_SIZE
                Dim val As Integer = m_digitMatrix(i, j)
                DrawLine(x, y - val \ 2, x, y + val \ 2, Presets.Mint)
            Next j
        Next i
    End Sub

    Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
        Clear(Presets.Black)
        Static timer As Single = 0
        timer += elapsedTime
        If timer >= UPDATE_INTERVAL Then
            UpdateEachCell()
            timer = 0
        End If
        DrawEachCell()
        Return Not GetKey(Key.ESCAPE).Pressed
    End Function

    Friend Shared Sub Main()
        With New Program
            If .Construct(800, 600, fullScreen:=True) Then .Start()
        End With
    End Sub
End Class