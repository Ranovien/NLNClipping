Public Class ClipArea
    Public X1 As Integer
    Public Y1 As Integer
    Public X2 As Integer
    Public Y2 As Integer
    'Dim ClippedDot As TPoint()
    'Dim ClippedLine As TLine()
    'those are variables to save the clipped line, but actually we don't need that
    Dim Index As Integer

    Sub New()
        'ClippedDot = Nothing
        'ClippedLine = Nothing
        Index = -1
    End Sub

    Sub SetStartPoint(xc As Integer, yc As Integer)
        X1 = xc
        Y1 = yc
    End Sub

    Sub SetEndPoint(xc As Integer, yc As Integer)
        X2 = xc
        Y2 = yc
    End Sub

    Sub Fix()
        Dim temp As Integer
        If (X1 > X2) Then
            temp = X1
            X1 = X2
            X2 = temp
        End If
        If (Y1 > Y2) Then
            temp = Y1
            Y1 = Y2
            Y2 = temp
        End If
    End Sub
End Class
