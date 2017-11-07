Module NLN2
    Private Const TOP As Integer = 8        '1000
    Private Const BOTTOM As Integer = 4     '0100
    Private Const RIGHT As Integer = 2      '0010
    Private Const LEFT As Integer = 1       '0001
    Private Const INSIDE As Integer = 0     '0000
    Dim t As Double ' as a global variable slope


    Public Sub NLNClipping2(ClipArea As ClipArea, ByRef Listofline As TLine(), indexline As Integer, output As Graphics)

        Dim mypen As Pen = New Pen(Color.Red, 5)
        Dim Areacode1 As Integer ' areacode for startpoint
        Dim Areacode2 As Integer ' areacode for endpoint
        Dim Rx1, Ry1, Rx2, Ry2, xmin, ymin, xmax, ymax, Tx, Ty, Tarea, Tarea2 As Integer
        'These are temporary variables
        'Rx1,Rx2 : To store rotated X1,X2
        'Ry1,Ry2 : To store rotated Y1,Y2
        'Tx,Ty : Temporary X,Y for checking
        'Tarea : Temporary area for checking
        Dim slope, TR, BR, BL, TL As Double
        Dim recheck As Boolean
        'recheck : to check the intersection whether it's inside or outside
        'in case the endpoint is on corner area 

        For i As Integer = 0 To indexline
            'initialize
            Tx = 0
            Ty = 0
            'check the areacode for:
            CheckLocation(ClipArea, Listofline(i).Head, Areacode1) 'start point
            CheckLocation(ClipArea, Listofline(i).Tail, Areacode2) 'end point
            If (Areacode1 Or Areacode2) = False Then
                'Trvial Accept
                output.DrawLine(mypen, Listofline(i).Head.X, Listofline(i).Head.Y, Listofline(i).Tail.X, Listofline(i).Tail.Y)
            ElseIf Areacode1 And Areacode2 Then
                'Trivial Reject
            Else
                If Areacode1 = INSIDE Then
                    'Case 1: INSIDE
                    'use Rx1 and Ry1 as normal temporary variable
                    Rx1 = Listofline(i).Head.X
                    Ry1 = Listofline(i).Head.Y
                    Transformation(Areacode1, ClipArea, Listofline(i).Head, Listofline(i).Tail, Rx1, Ry1, Rx2, Ry2, xmin, ymin, xmax, ymax)
                    'Assign all value to temporary variable
                    FindAllSlopeAlt(slope, xmin, ymin, xmax, ymax, Rx1, Ry1, Rx2, Ry2, BR, TR, BL, TL)
                    'Calculate all slope

                    If Areacode2 = LEFT Then
                        'find intersection with left edge of clipping window
                        CalcIntercectionYc(ClipArea.X1, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                        output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                    ElseIf Areacode2 = RIGHT Then
                        'find intersection with right edge of clipping window
                        CalcIntercectionYc(ClipArea.X2, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                        output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                    ElseIf Areacode2 = BOTTOM Then
                        'find intersection with bottom edge of clipping window
                        CalcIntercectionXc(ClipArea.Y1, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                        output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                    ElseIf Areacode2 = TOP Then
                        'find intersection with top edge of clipping window
                        CalcIntercectionXc(ClipArea.Y2, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                        output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                    ElseIf Areacode2 = 9 Then 'the endpoint is on the Top Left area
                        'find intersection with left edge, store it in temporary variable Tx & Ty
                        CalcIntercectionYc(ClipArea.X1, Listofline(i), Tx, Ty, Rx1, Ry1)
                        CheckLocationInt(Tx, Ty, xmin, ymin, xmax, ymax, Tarea)
                        'check the temporary intersection
                        If Not (Tarea = INSIDE) Then
                            'if the intersection is outside, calculate again the intersection with top edge
                            CalcIntercectionXc(ClipArea.Y2, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                            output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                        Else
                            'if the intersection is inside, draw it
                            output.DrawLine(mypen, Rx1, Ry1, Tx, Ty)
                        End If

                    ElseIf Areacode2 = 10 Then ' the endpoint is on the Top Right
                        'find intersection with top edge, store it in temporary variable Tx & Ty
                        CalcIntercectionXc(ClipArea.Y2, Listofline(i), Tx, Ty, Rx1, Ry1)
                        CheckLocationInt(Tx, Ty, xmin, ymin, xmax, ymax, Tarea)
                        'check the temporary intersection
                        If Not (Tarea = INSIDE) Then
                            'if the intersection is outside, calculate again the intersection with right edge
                            CalcIntercectionYc(ClipArea.X2, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                            output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                        Else
                            'if the intersection is inside, draw it
                            output.DrawLine(mypen, Rx1, Ry1, Tx, Ty)
                        End If

                    ElseIf Areacode2 = 5 Then ' Bottom Left
                        'find intersection with bottom edge, store it in temporary variable Tx & Ty
                        CalcIntercectionXc(ClipArea.Y1, Listofline(i), Tx, Ty, Rx1, Ry1)
                        CheckLocationInt(Tx, Ty, xmin, ymin, xmax, ymax, Tarea)
                        'check the temporary intersection
                        If Not (Tarea = INSIDE) Then
                            'if the intersection is outside, calculate again the intersection with left edge
                            CalcIntercectionYc(ClipArea.X1, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                            output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                        Else
                            'if the intersection is inside, draw it
                            output.DrawLine(mypen, Rx1, Ry1, Tx, Ty)
                        End If

                    ElseIf Areacode2 = 6 Then ' Bottom Right
                        'find intersection with right edge, store it in temporary variable Tx & Ty
                        CalcIntercectionYc(ClipArea.X2, Listofline(i), Tx, Ty, Rx1, Ry1)
                        CheckLocationInt(Tx, Ty, xmin, ymin, xmax, ymax, Tarea)
                        'check the temporary intersection
                        If Not (Tarea = INSIDE) Then
                            'if the intersection is outside, calculate again the intersection with bottom edge
                            CalcIntercectionXc(ClipArea.Y1, Listofline(i), Rx2, Ry2, Rx1, Ry1)
                            output.DrawLine(mypen, Rx1, Ry1, Rx2, Ry2)
                        Else
                            'if the intersection is inside, draw it
                            output.DrawLine(mypen, Rx1, Ry1, Tx, Ty)
                        End If
                    End If

                Else 'The startPoint is not inside

                    Dim IX1, IY1, IX2, IY2, OrignAreacode As Integer
                    'OrignAreacode : to store the areacode of start point before transformed 
                    'IX1,IY1 : variable to store where the start point intersect 
                    'IX2,IX2 : variable to store where the end point intersect
                    OrignAreacode = Areacode1
                    'store the startpoint before it gets transformed

                    Transformation(Areacode1, ClipArea, Listofline(i).Head, Listofline(i).Tail, Rx1, Ry1, Rx2, Ry2, xmin, ymin, xmax, ymax)
                    'transform all

                    If Areacode1 = BOTTOM Then
                        'use the special case bottom to check the area code which use these logic:
                        'If y > bottom ,then Areacode + BOTTOM
                        'ElseIf y < top,then Areacode + TOP
                        'this is happened because bottom > top after transformation
                        CheckSpecialCaseBOTTOM(Rx1, Ry1, xmin, ymin, xmax, ymax, Areacode1)
                        CheckSpecialCaseBOTTOM(Rx2, Ry2, xmin, ymin, xmax, ymax, Areacode2)
                        'because bottom > top,switch the ymin with ymax
                        FindAllSlopeAlt(slope, xmin, ymax, xmax, ymin, Rx1, Ry1, Rx2, Ry2, TR, BR, TL, BL)
                        'but this maybe one of bug that i fix using quick bugfix

                    ElseIf Areacode1 = TOP Then
                        'use the special case top to check the area code which use these logic:
                        'if x > left , areacode + left
                        'else if x < right, areacode + right
                        'if  y > bottom , areacode + bottom
                        'else if y < top, areacode + top
                        ' this is happened because bottom > top and left
                        ' this may be the bug
                        CheckSpecialCaseTOP(Rx1, Ry1, xmin, ymin, xmax, ymax, Areacode1)
                        CheckSpecialCaseTOP(Rx2, Ry2, xmin, ymin, xmax, ymax, Areacode2)
                        FindAllSlopeAlt(slope, xmin, ymin, xmax, ymax, Rx1, Ry1, Rx2, Ry2, TR, BR, TL, BL)

                    Else
                        'normal without change
                        CheckLocationInt(Rx1, Ry1, xmin, ymin, xmax, ymax, Areacode1)
                        CheckLocationInt(Rx2, Ry2, xmin, ymin, xmax, ymax, Areacode2)
                        FindAllSlopeAlt(slope, xmin, ymin, xmax, ymax, Rx1, Ry1, Rx2, Ry2, TR, BR, TL, BL)

                    End If

                    If Areacode1 = LEFT Then
                        'Case 2: Left
                        recheck = False
                        If Areacode2 = INSIDE Then
                            'intersect with left only
                            CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                            ReTransform(OrignAreacode, IX1, IY1, Rx2, Ry2)
                            output.DrawLine(mypen, IX1, IY1, Rx2, Ry2)
                        ElseIf Areacode2 = RIGHT Then
                            'intersect with left and right
                            CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                            CalcIntercectionYcInt(xmax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                            ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                            output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                        Else ' other case
                            ' the condition BOTTOM (quick fixbug)=> because i switched something from the code xD
                            If TR < slope And slope <= TL Then
                                If Not (OrignAreacode = BOTTOM) Then
                                    'intersect with left and top
                                    CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    CalcIntercectionXcInt(ymax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                Else
                                    'intersect with  left and bottom
                                    CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    CalcIntercectionXcInt(ymin, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                End If
                                ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                                output.DrawLine(mypen, IX1, IY1, IX2, IY2)


                            ElseIf BL < slope And slope <= BR Then
                                If Not (OrignAreacode = BOTTOM) Then
                                    'intersect with left and bottom
                                    CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    CalcIntercectionXcInt(ymin, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                Else
                                    'intersect with left and top
                                    CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                    CalcIntercectionXcInt(ymax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                End If
                                ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                                output.DrawLine(mypen, IX1, IY1, IX2, IY2)

                            ElseIf BR < slope And slope <= TR Then
                                'intersect with left and right
                                'for the top right & bottom right case
                                CalcIntercectionYcInt(xmin, Rx2, Ry2, IX1, IY1, Rx1, Ry1)
                                CalcIntercectionYcInt(xmax, Rx2, Ry2, IX2, IY2, Rx1, Ry1)
                                ReTransform(OrignAreacode, IX1, IY1, IX2, IY2)
                                output.DrawLine(mypen, IX1, IY1, IX2, IY2)
                            End If

                        End If


                    ElseIf Areacode1 = 9 Then
                        'Case 3: TOP-LEFT
                        'MsgBox(slope)
                        'MsgBox(Areacode2)
                        If BR < TL Then 'Case A
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
                            ElseIf slope >= TL And slope <= TR Then 'not handled slope >=TL
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
                            ElseIf slope >= BR And slope <= TR Then 'not handled slope <= TR
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

End Module
