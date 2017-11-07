Public Class TPoint
    Public X As Integer
    Public Y As Integer
    Public Index As Integer
    Public Color As Color

    Public Sub New()
        Index = -1
        Color = Color.Black
    End Sub

    Public Overloads Sub SetXY(xc As Integer, yc As Integer, dd As Integer)
        X = xc
        Y = yc
        Index = dd
    End Sub

    Public Overloads Sub SetXY(xc As Integer, yc As Integer, prev As TPoint)
        SetXY(xc, yc, prev.Index + 1)
    End Sub
End Class
