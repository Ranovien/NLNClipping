Public Class NLNClip
    'Draw/Clip status
    Dim DrawDotMode As Boolean
    Dim DrawLineMode As Boolean
    Dim ClipMode As Boolean
    Dim AfterClip As Boolean
    'Graphic Thingy
    Dim bit As Bitmap
    Dim g As Graphics
    Dim myPen As Pen
    'Index and Size of Array
    Dim LineIndex As Integer
    Dim DotIndex As Integer
    'The Array
    Dim ListofLine As TLine()
    Dim ListofPoint As TPoint()
    'Clip Area
    Dim ClipArea As ClipArea
    'Variable to store mouse location
    Dim currx As Integer
    Dim curry As Integer
    'Save location
    Dim FILE_PATH As String = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file")
    Dim FILE_NAME As String = IO.Path.Combine(FILE_PATH, "file1.txt")
    'Load location
    Dim fileReader As String



    Private Sub Init() Handles MyBase.Load
        bit = New Bitmap(PictureMain.Width, PictureMain.Height)
        g = Graphics.FromImage(bit)
        myPen = New Pen(Color.Black, 5)
        LineIndex = -1
        DotIndex = -1
        DrawDotMode = False
        DrawLineMode = False
        ClipMode = False
        AfterClip = False
        ClipArea = New ClipArea
        'MsgBox("Welcome to NLN Clipping Application")
    End Sub


    Private Sub DrawDotModeActivate(sender As Object, e As EventArgs) Handles BtnDraw.Click
        If AfterClip = False Then
            DrawDotMode = True
            DrawLineMode = False
            ClipMode = False
            LblMode.Text = "Mode : Draw Dot"
        End If
    End Sub

    Private Sub DrawLineModeActivate(sender As Object, e As EventArgs) Handles BtnDrawLine.Click
        If AfterClip = False Then
            DrawDotMode = False
            DrawLineMode = True
            ClipMode = False
            LblMode.Text = "Mode : Draw Line"
        End If
    End Sub

    Private Sub ClipModeActivate(sender As Object, e As EventArgs) Handles BtnClip.Click
        DrawDotMode = False
        DrawLineMode = False
        ClipMode = True
        LblMode.Text = "Mode : Clip"
    End Sub

    Private Sub MouseMovement(sender As Object, e As MouseEventArgs) Handles PictureMain.MouseMove
        LblXVal.Text = e.X
        LblYVal.Text = e.Y
    End Sub

    Private Sub GetMouseLocationFromLabel()
        currx = LblXVal.Text
        curry = LblYVal.Text
    End Sub

    Private Sub DrawDot(sender As Object, e As MouseEventArgs) Handles PictureMain.MouseClick

        If DrawDotMode Then
            DotIndex = DotIndex + 1
            'Get the dot data into array
            GetMouseLocationFromLabel()
            ResizePoint(ListofPoint, DotIndex)
            If (DotIndex = 0) Then
                ListofPoint(DotIndex).SetXY(currx, curry, DotIndex)
            Else
                ListofPoint(DotIndex).SetXY(currx, curry, ListofPoint(DotIndex - 1))
            End If
            ListofObject.Items.Add("Dot " + ListofPoint(DotIndex).Index.ToString)
            DisplayAll()
        End If

    End Sub

    Private Sub DrawLineStepOne(sender As Object, e As MouseEventArgs) Handles PictureMain.MouseDown

        If DrawLineMode Then
            LineIndex = LineIndex + 1
            GetMouseLocationFromLabel()
            ResizeLine(ListofLine, LineIndex)
            If LineIndex = 0 Then
                ListofLine(LineIndex).SetFirstPointLine(currx, curry, LineIndex)
            Else
                ListofLine(LineIndex).SetFirstPointLine(currx, curry, ListofLine(LineIndex - 1))
            End If
        End If

    End Sub

    Private Sub DrawLineStepTwo(sender As Object, e As MouseEventArgs) Handles PictureMain.MouseUp

        If DrawLineMode Then
            GetMouseLocationFromLabel()
            ListofLine(LineIndex).SetLastPointLine(currx, curry)
            ListofObject.Items.Add("Line " + ListofLine(LineIndex).Index.ToString)
            DisplayAll()
        End If

    End Sub

    Private Sub ClipWindowStart(sender As Object, e As MouseEventArgs) Handles PictureMain.MouseDown
        If AfterClip = False Then

            If ClipMode Then
                'start to make clipping window
                GetMouseLocationFromLabel()
                ClipArea.SetStartPoint(currx, curry)
            End If
        End If


    End Sub

    Private Sub ClipWindowEnd(sender As Object, e As MouseEventArgs) Handles PictureMain.MouseUp
        If AfterClip = False Then
            If ClipMode Then
                GetMouseLocationFromLabel()
                ClipArea.SetEndPoint(currx, curry)
                ClipArea.Fix()
                NLNClippingDot(ClipArea, ListofPoint, DotIndex)
                DisplayAll()
                NLNClipping2(ClipArea, ListofLine, LineIndex, g)
                DrawRectangle(ClipArea)
                AfterClip = True
            End If
        End If

    End Sub

    Public Sub DrawRectangle(area As ClipArea)
        myPen = New Pen(Color.Blue, 2)
        g.DrawLine(myPen, area.X1, area.Y1, area.X2, area.Y1)
        g.DrawLine(myPen, area.X1, area.Y1, area.X1, area.Y2)
        g.DrawLine(myPen, area.X1, area.Y2, area.X2, area.Y2)
        g.DrawLine(myPen, area.X2, area.Y1, area.X2, area.Y2)
    End Sub

    Private Sub DeleteSelected(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If AfterClip = False Then
            If Not (ListofObject.SelectedItem Is Nothing) Then
                Dim temparray(2) As String
                temparray = Split(ListofObject.SelectedItem.ToString, " ")

                If temparray(0).ToString = "Dot" Then
                    DeleteSelectedPoint(ListofPoint, temparray(1), DotIndex)
                    'if delete the index change, meaning cant delete from index 0 to up...
                    'MsgBox(temparray(0).ToString + " " + temparray(1).ToString)
                    ListofObject.Items.RemoveAt(ListofObject.SelectedIndex)
                ElseIf temparray(0).ToString = "Line" Then
                    DeleteSelectedLine(ListofLine, temparray(1), LineIndex)
                    'MsgBox(temparray(0).ToString + " " + temparray(1).ToString)
                    ListofObject.Items.RemoveAt(ListofObject.SelectedIndex)
                End If
                DisplayAll()
            End If
        End If
    End Sub
    Private Sub ClearScreen(sender As Object, e As EventArgs) Handles BtnClear.Click
        If AfterClip = False Then
            DeleteAll(ListofLine, LineIndex, ListofPoint, DotIndex)
            PictureMain.Image = Nothing
            ListofObject.Items.Clear()
            DisplayAll()
        End If
    End Sub

    Private Sub DisplayAll()
        'refresh
        ' PictureMain.Image = Nothing
        g.Clear(Color.White)
        'drawdot
        For i As Integer = 0 To DotIndex Step 1
            bit.SetPixel(ListofPoint(i).X, ListofPoint(i).Y, ListofPoint(i).Color)
        Next
        PictureMain.Image = bit
        For i As Integer = 0 To LineIndex Step 1
            myPen = New Pen(ListofLine(i).Color, 5)
            g.DrawLine(myPen, ListofLine(i).Head.X, ListofLine(i).Head.Y, ListofLine(i).Tail.X, ListofLine(i).Tail.Y)
        Next
        'MsgBox("tampil")
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        If AfterClip = True Then
            AfterClip = False
            resetdotcolor(ListofPoint, DotIndex)
            DisplayAll()
        End If
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        System.IO.File.WriteAllText(FILE_NAME, "")
        Dim objWriter As New System.IO.StreamWriter(FILE_NAME)
        If (DotIndex >= 0) Then
            objWriter.Write("dot ")
            objWriter.Write(Environment.NewLine)
            For i As Integer = 0 To DotIndex
                objWriter.Write(ListofPoint(i).X.ToString + " " + ListofPoint(i).Y.ToString)
                objWriter.Write(" ")
            Next
        End If
        If (LineIndex >= 0) Then
            objWriter.Write("line ")
            objWriter.Write(Environment.NewLine)
            For i As Integer = 0 To LineIndex
                objWriter.Write(ListofLine(i).Head.X.ToString + " " + ListofLine(i).Head.Y.ToString + " " + ListofLine(i).Tail.X.ToString + " " + ListofLine(i).Tail.Y.ToString)
                objWriter.Write(" ")
            Next
        End If
        objWriter.Close()
    End Sub

    Private Sub BtnLoad_Click(sender As Object, e As EventArgs) Handles BtnLoad.Click
        DeleteAll(ListofLine, LineIndex, ListofPoint, DotIndex)
        PictureMain.Image = Nothing
        ListofObject.Items.Clear()
        fileReader = My.Computer.FileSystem.ReadAllText(FILE_NAME)
        Dim file As String() = fileReader.Split(" ")
        Dim loadpoint As Boolean = False
        Dim loadline As Boolean = False
        Dim pointcount As Integer = 0
        Dim linecount As Integer = 0
        For i As Integer = 0 To file.Length - 1
            'MsgBox(file(i))
            If file(i) = "dot" Then
                loadpoint = True
                loadline = False
            ElseIf file(i) = "line" Then
                loadpoint = False
                loadline = True
            Else
                If (loadpoint = True) Then
                    pointcount = pointcount + 1
                    If pointcount = 2 Then
                        DotIndex = DotIndex + 1
                        ResizePoint(ListofPoint, DotIndex)
                        ListofPoint(DotIndex).SetXY(file(i - 1), file(i), DotIndex)
                        ListofObject.Items.Add("Dot " + ListofPoint(DotIndex).Index.ToString)
                        pointcount = 0
                    End If
                Else
                    linecount = linecount + 1
                    If linecount = 4 Then
                        LineIndex = LineIndex + 1
                        ResizeLine(ListofLine, LineIndex)
                        ListofLine(LineIndex).SetFirstPointLine(file(i - 3), file(i - 2), LineIndex)
                        ListofLine(LineIndex).SetLastPointLine(file(i - 1), file(i))
                        ListofObject.Items.Add("Line " + ListofLine(LineIndex).Index.ToString)
                        linecount = 0
                    End If
                End If
            End If
        Next
        DisplayAll()
    End Sub
End Class
