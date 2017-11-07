Public Class TLine
    Public Index As Integer
    Public Head As TPoint
    Public Tail As TPoint
    Public Color As Color

    Public Sub New()
        Index = -1
        Head = New TPoint
        Tail = New TPoint
        Color = Color.Black
    End Sub

    Public Overloads Sub SetFirstPointLine(n As TPoint, i As Integer)
        Head = n
        Index = i
    End Sub

    Public Overloads Sub SetFirstPointLine(xc As Integer, yc As Integer, i As Integer)
        Head.X = xc
        Head.Y = yc
        Index = i
    End Sub

    Public Overloads Sub SetFirstPointLine(xc As Integer, yc As Integer, prev As TLine)
        Head.X = xc
        Head.Y = yc
        Index = prev.Index + 1
    End Sub

    Public Overloads Sub SetLastPointLine(n As TPoint)
        Tail = n
    End Sub

    Public Overloads Sub SetLastPointLine(xc As Integer, yc As Integer)
        Tail.X = xc
        Tail.Y = yc
    End Sub

    Public Sub DisplayInfo()
        MsgBox("First Point : " + Head.X.ToString + "," + Head.Y.ToString)
        MsgBox("End Point : " + Tail.X.ToString + "," + Tail.Y.ToString)
    End Sub

End Class
