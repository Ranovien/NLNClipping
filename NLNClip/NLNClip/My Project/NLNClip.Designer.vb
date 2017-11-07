<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NLNClip
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PictureMain = New System.Windows.Forms.PictureBox()
        Me.BtnDraw = New System.Windows.Forms.Button()
        Me.BtnClip = New System.Windows.Forms.Button()
        Me.BtnClear = New System.Windows.Forms.Button()
        Me.BtnLoad = New System.Windows.Forms.Button()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.ListofObject = New System.Windows.Forms.ListBox()
        Me.LblXPos = New System.Windows.Forms.Label()
        Me.LblYPos = New System.Windows.Forms.Label()
        Me.LblMode = New System.Windows.Forms.Label()
        Me.LblXVal = New System.Windows.Forms.Label()
        Me.LblYVal = New System.Windows.Forms.Label()
        Me.BtnDrawLine = New System.Windows.Forms.Button()
        Me.BtnRefresh = New System.Windows.Forms.Button()
        CType(Me.PictureMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureMain
        '
        Me.PictureMain.BackColor = System.Drawing.SystemColors.Window
        Me.PictureMain.Location = New System.Drawing.Point(13, 13)
        Me.PictureMain.Name = "PictureMain"
        Me.PictureMain.Size = New System.Drawing.Size(901, 571)
        Me.PictureMain.TabIndex = 0
        Me.PictureMain.TabStop = False
        '
        'BtnDraw
        '
        Me.BtnDraw.Location = New System.Drawing.Point(13, 600)
        Me.BtnDraw.Name = "BtnDraw"
        Me.BtnDraw.Size = New System.Drawing.Size(120, 52)
        Me.BtnDraw.TabIndex = 1
        Me.BtnDraw.Text = "Dot Mode"
        Me.BtnDraw.UseVisualStyleBackColor = True
        '
        'BtnClip
        '
        Me.BtnClip.Location = New System.Drawing.Point(265, 600)
        Me.BtnClip.Name = "BtnClip"
        Me.BtnClip.Size = New System.Drawing.Size(120, 52)
        Me.BtnClip.TabIndex = 2
        Me.BtnClip.Text = "Clip Mode"
        Me.BtnClip.UseVisualStyleBackColor = True
        '
        'BtnClear
        '
        Me.BtnClear.Location = New System.Drawing.Point(1040, 599)
        Me.BtnClear.Name = "BtnClear"
        Me.BtnClear.Size = New System.Drawing.Size(120, 52)
        Me.BtnClear.TabIndex = 3
        Me.BtnClear.Text = "Clear"
        Me.BtnClear.UseVisualStyleBackColor = True
        '
        'BtnLoad
        '
        Me.BtnLoad.Location = New System.Drawing.Point(794, 600)
        Me.BtnLoad.Name = "BtnLoad"
        Me.BtnLoad.Size = New System.Drawing.Size(120, 52)
        Me.BtnLoad.TabIndex = 4
        Me.BtnLoad.Text = "Load"
        Me.BtnLoad.UseVisualStyleBackColor = True
        '
        'BtnSave
        '
        Me.BtnSave.Location = New System.Drawing.Point(668, 600)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(120, 52)
        Me.BtnSave.TabIndex = 5
        Me.BtnSave.Text = "Save"
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        Me.BtnDelete.Location = New System.Drawing.Point(963, 345)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(195, 52)
        Me.BtnDelete.TabIndex = 6
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'ListofObject
        '
        Me.ListofObject.FormattingEnabled = True
        Me.ListofObject.ItemHeight = 20
        Me.ListofObject.Location = New System.Drawing.Point(963, 30)
        Me.ListofObject.Name = "ListofObject"
        Me.ListofObject.Size = New System.Drawing.Size(195, 284)
        Me.ListofObject.TabIndex = 7
        '
        'LblXPos
        '
        Me.LblXPos.AutoSize = True
        Me.LblXPos.Location = New System.Drawing.Point(975, 450)
        Me.LblXPos.Name = "LblXPos"
        Me.LblXPos.Size = New System.Drawing.Size(55, 20)
        Me.LblXPos.TabIndex = 8
        Me.LblXPos.Text = "X Pos:"
        '
        'LblYPos
        '
        Me.LblYPos.AutoSize = True
        Me.LblYPos.Location = New System.Drawing.Point(975, 491)
        Me.LblYPos.Name = "LblYPos"
        Me.LblYPos.Size = New System.Drawing.Size(55, 20)
        Me.LblYPos.TabIndex = 9
        Me.LblYPos.Text = "Y Pos:"
        '
        'LblMode
        '
        Me.LblMode.AutoSize = True
        Me.LblMode.Location = New System.Drawing.Point(975, 534)
        Me.LblMode.Name = "LblMode"
        Me.LblMode.Size = New System.Drawing.Size(66, 20)
        Me.LblMode.TabIndex = 10
        Me.LblMode.Text = "Mode : -"
        '
        'LblXVal
        '
        Me.LblXVal.AutoSize = True
        Me.LblXVal.Location = New System.Drawing.Point(1036, 450)
        Me.LblXVal.Name = "LblXVal"
        Me.LblXVal.Size = New System.Drawing.Size(14, 20)
        Me.LblXVal.TabIndex = 11
        Me.LblXVal.Text = "-"
        '
        'LblYVal
        '
        Me.LblYVal.AutoSize = True
        Me.LblYVal.Location = New System.Drawing.Point(1036, 491)
        Me.LblYVal.Name = "LblYVal"
        Me.LblYVal.Size = New System.Drawing.Size(14, 20)
        Me.LblYVal.TabIndex = 12
        Me.LblYVal.Text = "-"
        '
        'BtnDrawLine
        '
        Me.BtnDrawLine.Location = New System.Drawing.Point(139, 600)
        Me.BtnDrawLine.Name = "BtnDrawLine"
        Me.BtnDrawLine.Size = New System.Drawing.Size(120, 52)
        Me.BtnDrawLine.TabIndex = 13
        Me.BtnDrawLine.Text = "Line Mode"
        Me.BtnDrawLine.UseVisualStyleBackColor = True
        '
        'BtnRefresh
        '
        Me.BtnRefresh.Location = New System.Drawing.Point(542, 600)
        Me.BtnRefresh.Name = "BtnRefresh"
        Me.BtnRefresh.Size = New System.Drawing.Size(120, 52)
        Me.BtnRefresh.TabIndex = 14
        Me.BtnRefresh.Text = "Refresh"
        Me.BtnRefresh.UseVisualStyleBackColor = True
        '
        'NLNClip
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1206, 663)
        Me.Controls.Add(Me.BtnRefresh)
        Me.Controls.Add(Me.BtnDrawLine)
        Me.Controls.Add(Me.LblYVal)
        Me.Controls.Add(Me.LblXVal)
        Me.Controls.Add(Me.LblMode)
        Me.Controls.Add(Me.LblYPos)
        Me.Controls.Add(Me.LblXPos)
        Me.Controls.Add(Me.ListofObject)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.BtnLoad)
        Me.Controls.Add(Me.BtnClear)
        Me.Controls.Add(Me.BtnClip)
        Me.Controls.Add(Me.BtnDraw)
        Me.Controls.Add(Me.PictureMain)
        Me.Name = "NLNClip"
        Me.Text = "NLN Algorithm"
        CType(Me.PictureMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PictureMain As PictureBox
    Friend WithEvents BtnDraw As Button
    Friend WithEvents BtnClip As Button
    Friend WithEvents BtnClear As Button
    Friend WithEvents BtnLoad As Button
    Friend WithEvents BtnSave As Button
    Friend WithEvents BtnDelete As Button
    Friend WithEvents ListofObject As ListBox
    Friend WithEvents LblXPos As Label
    Friend WithEvents LblYPos As Label
    Friend WithEvents LblMode As Label
    Friend WithEvents LblXVal As Label
    Friend WithEvents LblYVal As Label
    Friend WithEvents BtnDrawLine As Button
    Friend WithEvents BtnRefresh As Button
End Class
