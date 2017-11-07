Module Main
    Private Const TOP As Integer = 8        '1000
    Private Const BOTTOM As Integer = 4     '0100
    Private Const RIGHT As Integer = 2      '0010
    Private Const LEFT As Integer = 1       '0001
    Private Const INSIDE As Integer = 0     '0000
    Dim t As Double

    Public Sub CheckLocation(clip As ClipArea, point As TPoint, ByRef Areacode As Integer)
        'Normal Check Location
        Areacode = INSIDE
        If point.X < clip.X1 Then
            Areacode = Areacode + LEFT
        ElseIf point.X > clip.X2 Then
            Areacode = Areacode + RIGHT
        End If
        If point.Y < clip.Y1 Then
            Areacode = Areacode + BOTTOM
        ElseIf point.Y > clip.Y2 Then
            Areacode = Areacode + TOP
        End If
    End Sub

    Public Sub CheckLocationRecheck(clip As ClipArea, x As Integer, y As Integer, ByRef Areacode As Integer)
        'Check Location that used to recheck the result
        Areacode = INSIDE
        If x < clip.X1 Then
            Areacode = Areacode + LEFT
        ElseIf X > clip.X2 Then
            Areacode = Areacode + RIGHT
        End If
        If y < clip.Y1 Then
            Areacode = Areacode + BOTTOM
        ElseIf Y > clip.Y2 Then
            Areacode = Areacode + TOP
        End If
    End Sub

    Public Sub CheckLocationInt(x As Integer, y As Integer, x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, ByRef Areacode As Integer)
        'Check With Integer parameter
        Areacode = INSIDE
        If x < x1 Then
            Areacode = Areacode + LEFT
        ElseIf x > x2 Then
            Areacode = Areacode + RIGHT
        End If
        If y < y1 Then
            Areacode = Areacode + BOTTOM
        ElseIf y > y2 Then
            Areacode = Areacode + TOP
        End If
    End Sub

    Public Sub CheckSpecialCaseTOP(x As Integer, y As Integer, x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, ByRef Areacode As Integer)
        'Check only TOP case
        Areacode = INSIDE
        If x > x1 Then
            Areacode = Areacode + LEFT
        ElseIf x < x2 Then
            Areacode = Areacode + RIGHT
        End If
        If y > y1 Then
            Areacode = Areacode + BOTTOM
        ElseIf y < y2 Then
            Areacode = Areacode + TOP
        End If
    End Sub

    Public Sub CheckSpecialCaseBOTTOM(x As Integer, y As Integer, x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, ByRef Areacode As Integer)
        'Check only BOTTOM case
        Areacode = INSIDE
        If x < x1 Then
            Areacode = Areacode + LEFT
        ElseIf x > x2 Then
            Areacode = Areacode + RIGHT
        End If
        If y > y1 Then
            Areacode = Areacode + BOTTOM
        ElseIf y < y2 Then
            Areacode = Areacode + TOP
        End If
    End Sub

    Public Sub NLNClipping(ClipArea As ClipArea, ByRef Listofline As TLine(), indexline As Integer, output As Graphics)
        Dim mypen As Pen = New Pen(Color.Red, 5)
        Dim Areacode1 As Integer
        Dim Areacode2 As Integer
        Dim Rx1, Ry1, Rx2, Ry2, xmin, ymin, xmax, ymax, Tx, Ty, Tarea, Tarea2 As Integer
        Dim slope, TR, BR, BL, TL As Double
        Dim recheck As Boolean

        For i As Integer = 0 To indexline
            Tx = 0
            Ty = 0
            'check the normal location
            CheckLocation(ClipArea, Listofline(i).Head, Areacode1)
            CheckLocation(ClipArea, Listofline(i).Tail, Areacode2)
            If (Areacode1 Or Areacode2) = False Then
                'Trvial Accept
                output.DrawLine(mypen, Listofline(i).Head.X, Listofline(i).Head.Y, Listofline(i).Tail.X, Listofline(i).Tail.Y)
            ElseIf Areacode1 And Areacode2 Then
                'Trivial Reject
            Else
                If Areacode1 = INSIDE Then
                    'Case 1: INSIDE
                    Rx1 = Listofline(i).Head.X
                    Ry1 = Listofline(i).Head.Y
                    Transformation(Areacode1, ClipArea, Listofline(i).Head, Listofline(i).Tail, Rx1, Ry1, Rx2, Ry2, xmin, ymin, xmax, ymax)
                    FindAllSlopeAlt(slope, xmin, ymin, xmax, ymax, Rx1, Ry1, Rx2, Ry2, BR, TR, BL, TL)

                    If Areacode2 = LEFT Then
                        CalcIntercectionYc(ClipArea.X1, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                        output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                    ElseIf Areacode2 = RIGHT Then
                        CalcIntercectionYc(ClipArea.X2, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                        output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                    ElseIf Areacode2 = BOTTOM Then
                        CalcIntercectionXc(ClipArea.Y1, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                        output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                    ElseIf Areacode2 = TOP Then
                        CalcIntercectionXc(ClipArea.Y2, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                        output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                    ElseIf Areacode2 = 9 Then ' Top Left
                        CalcIntercectionYc(ClipArea.X1, Listofline(i), Tx, Ty, Rx1, Ry1)
                        CheckLocationInt(Tx, Ty, xmin, ymin, xmax, ymax, Tarea)
                        If Not (Tarea = INSIDE) Then
                            CalcIntercectionXc(ClipArea.Y2, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                            output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                        Else
                            output.DrawLine(mypen, Rx1, Ry1, Tx, Ty)
                        End If

                    ElseIf Areacode2 = 10 Then ' Top Right
                        CalcIntercectionXc(ClipArea.Y2, Listofline(i), Tx, Ty, Rx1, Ry1)
                        CheckLocationInt(Tx, Ty, xmin, ymin, xmax, ymax, Tarea)
                        If Not (Tarea = INSIDE) Then
                            CalcIntercectionYc(ClipArea.X2, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                            output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                        Else
                            output.DrawLine(mypen, Rx1, Ry1, Tx, Ty)
                        End If

                    ElseIf Areacode2 = 5 Then ' Bottom Left
                        CalcIntercectionXc(ClipArea.Y1, Listofline(i), Tx, Ty, Rx1, Ry1)
                        CheckLocationInt(Tx, Ty, xmin, ymin, xmax, ymax, Tarea)
                        If Not (Tarea = INSIDE) Then
                            CalcIntercectionYc(ClipArea.X1, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                            output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                        Else
                            output.DrawLine(mypen, Rx1, Ry1, Tx, Ty)
                        End If

                    ElseIf Areacode2 = 6 Then ' Bottom Right
                        CalcIntercectionYc(ClipArea.X2, Listofline(i), Tx, Ty, Rx1, Ry1)
                        CheckLocationInt(Tx, Ty, xmin, ymin, xmax, ymax, Tarea)
                        If Not (Tarea = INSIDE) Then
                            CalcIntercectionXc(ClipArea.Y1, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                            output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                        Else
                            output.DrawLine(mypen, Rx1, Ry1, Tx, Ty)
                        End If
                    End If

                Else 'The startPoint is not inside

                    Dim IX1, IY1, IX2, IY2, OrignAreacode As Integer
                    OrignAreacode = Areacode1

                    'CheckLocationInt(Rx2, Ry2, xmin, ymin, xmax, ymax, Areacode2)

                    Transformation(Areacode1, ClipArea, Listofline(i).Head, Listofline(i).Tail, Rx1, Ry1, Rx2, Ry2, xmin, ymin, xmax, ymax)

                    If Areacode1 = BOTTOM Then
                        CheckSpecialCaseBOTTOM(Rx1, Ry1, xmin, ymin, xmax, ymax, Areacode1)
                        CheckSpecialCaseBOTTOM(Rx2, Ry2, xmin, ymin, xmax, ymax, Areacode2)
                        FindAllSlopeAlt(slope, xmin, ymax, xmax, ymin, Rx1, Ry1, Rx2, Ry2, TR, BR, TL, BL)

                    ElseIf Areacode1 = TOP Then
                        CheckSpecialCaseTOP(Rx1, Ry1, xmin, ymin, xmax, ymax, Areacode1)
                        CheckSpecialCaseTOP(Rx2, Ry2, xmin, ymin, xmax, ymax, Areacode2)
                        FindAllSlopeAlt(slope, xmin, ymin, xmax, ymax, Rx1, Ry1, Rx2, Ry2, TR, BR, TL, BL)

                    Else
                        CheckLocationInt(Rx1, Ry1, xmin, ymin, xmax, ymax, Areacode1)
                        CheckLocationInt(Rx2, Ry2, xmin, ymin, xmax, ymax, Areacode2)
                        FindAllSlopeAlt(slope, xmin, ymin, xmax, ymax, Rx1, Ry1, Rx2, Ry2, TR, BR, TL, BL)

                    End If

                    If Areacode1 = LEFT Then
                        'Case 2: Left
                        recheck = False
                        If Areacode2 = INSIDE Then
                            CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                            ReTransform(OrignAreacode, IX1, IY1, Rx2, Ry2)
                            output.DrawLine(mypen, IX1, IY1, Rx2, Ry2)
                        ElseIf Areacode2 = RIGHT Then
                            CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                            CalcIntercectionYcInt(xmax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                            ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                            'CheckLocationRecheck(ClipArea, IX1, IY1, Tarea)
                            'MsgBox(Tarea)
                            output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                        ElseIf Areacode2 = TOP And slope < TL Then '
                            CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                            CalcIntercectionXcInt(ymax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                            ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                            CheckLocationRecheck(ClipArea, IX1, IY1, Tarea2)
                            CheckLocationRecheck(ClipArea, IX2, IY2, Tarea)
                            If Tarea = INSIDE And Tarea2 = INSIDE Then
                                output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                            End If
                        ElseIf Areacode2 = BOTTOM And slope > BL Then
                            CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                            CalcIntercectionXcInt(ymin, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                            ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                            CheckLocationRecheck(ClipArea, IX1, IY1, Tarea2)
                            CheckLocationRecheck(ClipArea, IX2, IY2, Tarea)
                            If Tarea = INSIDE And Tarea2 = INSIDE Then
                                output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                            End If
                        ElseIf (Areacode2 = 10) Then 'top right
                            'If slope >= TR Then
                            CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                            CalcIntercectionXcInt(ymax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                            ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                            CheckLocationRecheck(ClipArea, IX1, IY1, Tarea2)
                            CheckLocationRecheck(ClipArea, IX2, IY2, Tarea)
                            If Not (Tarea = INSIDE And Tarea2 = INSIDE) Then
                                CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                CalcIntercectionYcInt(xmax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                                CheckLocationRecheck(ClipArea, IX1, IY1, Tarea2)
                                CheckLocationRecheck(ClipArea, IX2, IY2, Tarea)
                            End If
                            If (Tarea = INSIDE And Tarea2 = INSIDE) Then
                                output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                            End If
                        End If
                        ElseIf Areacode2 = 6 Then 'bottom right
                        'If (slope >= BR) Then
                        CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                        CalcIntercectionYcInt(xmax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                        ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                        CheckLocationRecheck(ClipArea, IX1, IY1, Tarea2)
                        CheckLocationRecheck(ClipArea, IX2, IY2, Tarea)
                        If Not (Tarea = INSIDE And Tarea2 = INSIDE) Then
                            CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                            CalcIntercectionXcInt(ymin, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                            ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                            CheckLocationRecheck(ClipArea, IX1, IY1, Tarea2)
                            CheckLocationRecheck(ClipArea, IX2, IY2, Tarea)
                        End If
                        If (Tarea = INSIDE And Tarea2 = INSIDE) Then
                            output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                        End If

                    ElseIf Areacode1 = 9 Then
                        'Case 4: TOP-LEFT
                        'MsgBox(slope)
                        'MsgBox(Areacode2)
                        If TL > BR Then 'Case A
                            If slope >= BL And slope < BR Then
                                If Areacode2 = INSIDE Then
                                    'Intercept with Left
                                    CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, Rx2, Ry2)
                                    output.DrawLine(mypen, IX1, IY1, Rx2, Ry2)
                                ElseIf Areacode2 = BOTTOM Or Areacode2 = 6 Then
                                    'Intercept with Bottom and left
                                    CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    CalcIntercectionXcInt(ymin, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                                    output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                                End If
                            ElseIf slope >= BR And slope < TL Then
                                If Areacode2 = INSIDE Then
                                    'intercept with Left
                                    CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, Rx2, Ry2)
                                    output.DrawLine(mypen, IX1, IY1, Rx2, Ry2)
                                ElseIf Areacode2 = RIGHT Or Areacode2 = 6 Then 'bottom right
                                    'intercept with left and right
                                    CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    CalcIntercectionYcInt(xmax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                                    output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                                End If
                            ElseIf slope >= TL And slope <= TR Then
                                If Areacode2 = INSIDE Then
                                    'intercept with top
                                    CalcIntercectionXcInt(ymax, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, Rx2, Ry2)
                                    output.DrawLine(mypen, IX1, IY1, Rx2, Ry2)
                                ElseIf Areacode2 = RIGHT Then
                                    'intercept with top and right
                                    CalcIntercectionXcInt(ymax, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    CalcIntercectionYcInt(xmax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                                    output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                                End If
                            End If
                        ElseIf BR >= TL Then 'Case B
                            If slope >= BL And slope < TL Then
                                If Areacode2 = INSIDE Then
                                    'intercept with left
                                    CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, Rx2, Ry2)
                                    output.DrawLine(mypen, IX1, IY1, Rx2, Ry2)
                                ElseIf Areacode2 = BOTTOM Then
                                    'intercept with left and bottom
                                    CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    CalcIntercectionXcInt(ymin, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                                    output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                                End If
                            ElseIf slope >= TL And slope < BR Then
                                If Areacode2 = INSIDE Then
                                    'intercept with top
                                    CalcIntercectionXcInt(ymax, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, Rx2, Ry2)
                                    output.DrawLine(mypen, IX1, IY1, Rx2, Ry2)
                                ElseIf Areacode2 = BOTTOM Or Areacode2 = 6 Then
                                    'intercept with top and bottom
                                    CalcIntercectionXcInt(ymax, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    CalcIntercectionXcInt(ymin, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                                    output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                                End If
                            ElseIf slope >= BR And slope < TR Then
                                If Areacode2 = INSIDE Then
                                    'intercept with top
                                    CalcIntercectionXcInt(ymax, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, Rx2, Ry2)
                                    output.DrawLine(mypen, IX1, IY1, Rx2, Ry2)
                                ElseIf Areacode2 = RIGHT Or Areacode2 = 6 Then
                                    'intercept with top and right
                                    CalcIntercectionXcInt(ymax, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    CalcIntercectionYcInt(xmax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                    ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                                    output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                                End If
                            End If
                        Else
                            MsgBox("No Case => BUG")
                        End If
                    Else
                        MsgBox("No Case => BUG")
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub CalcIntercectionYc(cliparea As Integer, listofline As TLine, ByRef Xc As Integer, ByRef Yc As Integer, X1 As Integer, Y1 As Integer)
        'Intercect with X, Search the Yc
        Xc = cliparea
        t = (Xc - X1) / (listofline.Tail.X - X1)
        Yc = Y1 + t * (listofline.Tail.Y - Y1)
    End Sub

    Public Sub CalcIntercectionXc(cliparea As Integer, listofline As TLine, ByRef Xc As Integer, ByRef Yc As Integer, X1 As Integer, Y1 As Integer)
        'Intercect with Y, Search the Xc
        Yc = cliparea
        t = (Yc - Y1) / (listofline.Tail.Y - Y1)
        Xc = X1 + t * (listofline.Tail.X - X1)
    End Sub

    Public Sub CalcIntercectionYcInt(cliparea As Integer, X2 As Integer, Y2 As Integer, ByRef Xc As Integer, ByRef Yc As Integer, X1 As Integer, Y1 As Integer)
        'Other version
        Xc = cliparea
        t = (Xc - X1) / (X2 - X1)
        Yc = Y1 + t * (Y2 - Y1)
    End Sub

    Public Sub CalcIntercectionXcInt(cliparea As Integer, X2 As Integer, Y2 As Integer, ByRef Xc As Integer, ByRef Yc As Integer, X1 As Integer, Y1 As Integer)
        'Other Version
        Yc = cliparea
        t = (Yc - Y1) / (Y2 - Y1)
        Xc = X1 + t * (X2 - X1)
    End Sub

    Public Sub FindAllSlopeNormal(ByRef slope As Double, cliparea As ClipArea, Listofline As TLine, ByRef TR As Double, ByRef BR As Double, ByRef TL As Double, ByRef BL As Double)
        slope = (Listofline.Tail.Y - Listofline.Head.Y) / (Listofline.Tail.X - Listofline.Head.X)
        TR = (cliparea.Y2 - Listofline.Head.Y) / (cliparea.X2 - Listofline.Head.X)
        BR = (cliparea.Y1 - Listofline.Head.Y) / (cliparea.X2 - Listofline.Head.X)
        TL = (cliparea.Y2 - Listofline.Head.Y) / (cliparea.X1 - Listofline.Head.X)
        BL = (cliparea.Y1 - Listofline.Head.Y) / (cliparea.X1 - Listofline.Head.X)
    End Sub

    Public Sub FindAllSlopeAlt(ByRef slope As Double, clipx1 As Integer, clipy1 As Integer, clipx2 As Integer, clipy2 As Integer, x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, ByRef TR As Double, ByRef BR As Double, ByRef TL As Double, ByRef BL As Double)
        slope = (y2 - y1) / (x2 - x1)
        TR = (clipy2 - y1) / (clipx2 - x1)
        BR = (clipy1 - y1) / (clipx2 - x1)
        TL = (clipy2 - y1) / (clipx1 - x1)
        BL = (clipy1 - y1) / (clipx1 - x1)
    End Sub


    Public Sub Transformation(Areacode1 As Integer, Clip As ClipArea, Head As TPoint, Tail As TPoint, ByRef RX1 As Integer, ByRef RY1 As Integer, ByRef RX2 As Integer, ByRef RY2 As Integer, ByRef Xmin As Integer, ByRef Ymin As Integer, ByRef Xmax As Integer, ByRef Ymax As Integer)
        If Areacode1 = LEFT Then
            RX1 = Head.X
            RY1 = Head.Y
            RX2 = Tail.X
            RY2 = Tail.Y
            Xmin = Clip.X1
            Ymin = Clip.Y1
            Xmax = Clip.X2
            Ymax = Clip.Y2
        ElseIf Areacode1 = 9 Then 'TOP LEFT
            RX1 = Head.X
            RY1 = Head.Y
            RX2 = Tail.X
            RY2 = Tail.Y
            Xmin = Clip.X1
            Ymin = Clip.Y1
            Xmax = Clip.X2
            Ymax = Clip.Y2
        ElseIf Areacode1 = INSIDE Then
            RX1 = Head.X
            RY1 = Head.Y
            RX2 = Tail.X
            RY2 = Tail.Y
            Xmin = Clip.X1
            Ymin = Clip.Y1
            Xmax = Clip.X2
            Ymax = Clip.Y2
        ElseIf Areacode1 = 2 Then 'Right Case
            RX1 = -(Head.X)
            RY1 = Head.Y
            RX2 = -(Tail.X)
            RY2 = Tail.Y
            Xmin = -(Clip.X2)
            Ymin = Clip.Y1
            Xmax = -(Clip.X1)
            Ymax = Clip.Y2

        ElseIf Areacode1 = 4 Then 'Bottom Case
            'RX1 = -(Head.Y)
            'RY1 = Head.X
            'RX2 = -(Tail.Y)
            'RY2 = Tail.X
            'Xmin = -(Clip.Y2)
            'Ymin = Clip.X1
            'Xmax = -(Clip.Y1)
            'Ymax = Clip.X2
            RX1 = Head.Y
            RY1 = Head.X
            RX2 = Tail.Y
            RY2 = Tail.X
            Xmin = Clip.Y1
            Ymin = Clip.X2
            Xmax = Clip.Y2
            Ymax = Clip.X1
        ElseIf Areacode1 = 8 Then ' Top Case
            RX1 = Head.Y
            RY1 = Head.X
            RX2 = Tail.Y
            RY2 = Tail.X
            Xmin = Clip.Y2
            Ymin = Clip.X2
            Xmax = Clip.Y1
            Ymax = Clip.X1

        ElseIf Areacode1 = 10 Then ' Top Right Case
            RX1 = -(Head.X)
            RY1 = Head.Y
            RX2 = -(Tail.X)
            RY2 = Tail.Y
            Xmin = -(Clip.X2)
            Ymin = Clip.Y1
            Xmax = -(Clip.X1)
            Ymax = Clip.Y2

        ElseIf Areacode1 = 6 Then 'Bottom Right Case
            RX1 = -(Head.X)
            RY1 = -(Head.Y)
            RX2 = -(Tail.X)
            RY2 = -(Tail.Y)
            Xmin = -(Clip.X2)
            Ymin = -(Clip.Y2)
            Xmax = -(Clip.X1)
            Ymax = -(Clip.Y1)

        ElseIf Areacode1 = 5 Then ' Bottom Left Case
            RX1 = Head.X
            RY1 = -(Head.Y)
            RX2 = Tail.X
            RY2 = -(Tail.Y)
            Xmin = Clip.X1
            Ymin = -(Clip.Y2)
            Xmax = Clip.X2
            Ymax = -(Clip.Y1)

        End If
    End Sub

    Public Sub ReTransform(Areacode1 As Integer, ByRef RX1 As Integer, ByRef RY1 As Integer, ByRef RX2 As Integer, ByRef RY2 As Integer)
        Dim Temp As Integer
        If Areacode1 = LEFT Then
            RX1 = RX1
            RY1 = RY1
            RX2 = RX2
            RY2 = RY2
        ElseIf Areacode1 = 9 Then 'TopLeft
            RX1 = RX1
            RY1 = RY1
            RX2 = RX2
            RY2 = RY2

        ElseIf Areacode1 = 2 Then 'Right Case
            RX1 = -(RX1)
            RY1 = RY1
            RX2 = -(RX2)
            RY2 = RY2

        ElseIf Areacode1 = 4 Then 'Bottom Case
            Temp = RX1
            'RX1 = -(RY1)
            'RY1 = Temp
            'Temp = RX2
            'RX2 = -(RY2)
            'RY2 = Temp

            RX1 = RY1
            RY1 = Temp
            Temp = RX2
            RX2 = RY2
            RY2 = Temp

        ElseIf Areacode1 = 8 Then ' Top Case
            Temp = RX1
            RX1 = RY1
            RY1 = Temp
            Temp = RX2
            RX2 = RY2
            RY2 = Temp

        ElseIf Areacode1 = 10 Then ' Top Right Case
            RX1 = -(RX1)
            RY1 = RY1
            RX2 = -(RX2)
            RY2 = RY2

        ElseIf Areacode1 = 6 Then 'Bottom Right Case
            RX1 = -(RX1)
            RY1 = -(RY1)
            RX2 = -(RX2)
            RY2 = -(RY2)


        ElseIf Areacode1 = 5 Then ' Bottom Left Case
            RX1 = RX1
            RY1 = -(RY1)
            RX2 = RX2
            RY2 = -(RY2)

        End If

    End Sub

    Public Sub NLNClippingDot(ClipArea As ClipArea, ByRef Listofpoint As TPoint(), indexpoint As Integer)
        Dim AreaCode As Integer
        For i As Integer = 0 To indexpoint
            AreaCode = 0
            CheckLocation(ClipArea, Listofpoint(i), AreaCode)
            If AreaCode = INSIDE Then
                Listofpoint(i).Color = Color.Red
            End If
        Next
    End Sub

    Public Sub ResizePoint(ByRef point As TPoint(), size As Integer)
        ReDim Preserve point(size + 1)
        For i As Integer = 0 To size Step 1
            If point(i) Is Nothing Then
                point(i) = New TPoint
                'MsgBox("create")
            End If
        Next
    End Sub

    Public Sub ResizeLine(ByRef line As TLine(), size As Integer)
        ReDim Preserve line(size + 1)
        For i As Integer = 0 To size Step 1
            If line(i) Is Nothing Then
                line(i) = New TLine
            End If
        Next
    End Sub

    Public Sub DeleteSelectedPoint(ByRef point As TPoint(), choosenindex As Integer, ByRef IndexofArray As Integer)
        If choosenindex = IndexofArray Then
            'point(choosenindex).X = 0
            'point(choosenindex).Y = 0
            'point(choosenindex).Index = 0
            point(choosenindex) = Nothing
        Else
            For i As Integer = choosenindex To IndexofArray - 1 Step 1
                point(i).X = point(i + 1).X
                point(i).Y = point(i + 1).Y
                'point(i).Index = point(i).Index - 1
            Next
        End If
        ResizePoint(point, IndexofArray)
        IndexofArray = IndexofArray - 1

    End Sub

    Public Sub DeleteSelectedLine(ByRef line As TLine(), choosenindex As Integer, ByRef IndexofArray As Integer)
        If choosenindex = IndexofArray Then
            'line(choosenindex).Head = Nothing
            'line(choosenindex).Tail = Nothing
            'line(choosenindex).Index = 0
            line(choosenindex) = Nothing
        Else
            For i As Integer = choosenindex To IndexofArray - 1 Step 1
                line(i).Head.X = line(i + 1).Head.X
                line(i).Head.Y = line(i + 1).Head.Y
                line(i).Tail.X = line(i + 1).Tail.X
                line(i).Tail.Y = line(i + 1).Tail.Y
                'line(i).Index = line(i + 1).Index - 1
                'If i = IndexofArray - 1 Then
                'line(i) = Nothing
                'End If
            Next
        End If
        ResizeLine(line, IndexofArray)
        IndexofArray = IndexofArray - 1

    End Sub

    Public Sub DeleteAll(ByRef line As TLine(), ByRef lineindex As Integer, ByRef point As TPoint(), ByRef dotindex As Integer)
        For i As Integer = dotindex To 0 Step -1
            If i = 0 Then
                point(i) = New TPoint
            Else
                point(i) = Nothing
            End If
            'point(i).Index = point(i).Index - 1
        Next
        dotindex = 0
        ResizePoint(point, dotindex)


        For i As Integer = lineindex To 0 Step -1
            If i = 0 Then
                line(i) = New TLine
            Else
                line(i) = Nothing
            End If
        Next
        lineindex = 0
        ResizeLine(line, lineindex)

    End Sub

    Public Sub resetdotcolor(ByRef listofdot As TPoint(), dotindex As Integer)
        For i As Integer = 0 To dotindex Step 1
            listofdot(i).Color = Color.Black
        Next

    End Sub
End Module
