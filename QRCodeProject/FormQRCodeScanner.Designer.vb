<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormQRCodeScanner
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormQRCodeScanner))
        Me.LblJmlKartonDB = New System.Windows.Forms.Label()
        Me.LblJmlKemasUI = New System.Windows.Forms.Label()
        Me.BtnStopManual = New System.Windows.Forms.Button()
        Me.LblJmlKemasDB = New System.Windows.Forms.Label()
        Me.LblKarton = New System.Windows.Forms.Label()
        Me.LblJmlKartonUI = New System.Windows.Forms.Label()
        Me.LblStatusColor = New System.Windows.Forms.Label()
        Me.LblStatus = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.nomor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnStop = New System.Windows.Forms.Button()
        Me.TimerToScan = New System.Windows.Forms.Timer(Me.components)
        Me.BtnStart = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TxtBoxBatasKemas = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.LblStatusInsert = New System.Windows.Forms.Label()
        Me.LabelDateTime = New System.Windows.Forms.Label()
        Me.TimerDateTime = New System.Windows.Forms.Timer(Me.components)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblJmlKartonDB
        '
        Me.LblJmlKartonDB.AutoSize = True
        Me.LblJmlKartonDB.BackColor = System.Drawing.Color.Transparent
        Me.LblJmlKartonDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblJmlKartonDB.Location = New System.Drawing.Point(244, 101)
        Me.LblJmlKartonDB.Name = "LblJmlKartonDB"
        Me.LblJmlKartonDB.Size = New System.Drawing.Size(71, 17)
        Me.LblJmlKartonDB.TabIndex = 35
        Me.LblJmlKartonDB.Text = "0000000"
        '
        'LblJmlKemasUI
        '
        Me.LblJmlKemasUI.AutoSize = True
        Me.LblJmlKemasUI.BackColor = System.Drawing.Color.Transparent
        Me.LblJmlKemasUI.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblJmlKemasUI.Location = New System.Drawing.Point(51, 18)
        Me.LblJmlKemasUI.Name = "LblJmlKemasUI"
        Me.LblJmlKemasUI.Size = New System.Drawing.Size(21, 24)
        Me.LblJmlKemasUI.TabIndex = 34
        Me.LblJmlKemasUI.Text = "0"
        '
        'BtnStopManual
        '
        Me.BtnStopManual.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStopManual.Location = New System.Drawing.Point(508, 439)
        Me.BtnStopManual.Name = "BtnStopManual"
        Me.BtnStopManual.Size = New System.Drawing.Size(112, 46)
        Me.BtnStopManual.TabIndex = 33
        Me.BtnStopManual.Text = "Stop"
        Me.BtnStopManual.UseVisualStyleBackColor = True
        '
        'LblJmlKemasDB
        '
        Me.LblJmlKemasDB.AutoSize = True
        Me.LblJmlKemasDB.BackColor = System.Drawing.Color.Transparent
        Me.LblJmlKemasDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblJmlKemasDB.Location = New System.Drawing.Point(325, 101)
        Me.LblJmlKemasDB.Name = "LblJmlKemasDB"
        Me.LblJmlKemasDB.Size = New System.Drawing.Size(17, 17)
        Me.LblJmlKemasDB.TabIndex = 30
        Me.LblJmlKemasDB.Text = "0"
        '
        'LblKarton
        '
        Me.LblKarton.AutoSize = True
        Me.LblKarton.BackColor = System.Drawing.Color.Transparent
        Me.LblKarton.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblKarton.Location = New System.Drawing.Point(5, 18)
        Me.LblKarton.Name = "LblKarton"
        Me.LblKarton.Size = New System.Drawing.Size(36, 24)
        Me.LblKarton.TabIndex = 29
        Me.LblKarton.Text = "KT"
        '
        'LblJmlKartonUI
        '
        Me.LblJmlKartonUI.AutoSize = True
        Me.LblJmlKartonUI.BackColor = System.Drawing.Color.Transparent
        Me.LblJmlKartonUI.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblJmlKartonUI.Location = New System.Drawing.Point(40, 18)
        Me.LblJmlKartonUI.Name = "LblJmlKartonUI"
        Me.LblJmlKartonUI.Size = New System.Drawing.Size(87, 24)
        Me.LblJmlKartonUI.TabIndex = 28
        Me.LblJmlKartonUI.Text = "0000000"
        '
        'LblStatusColor
        '
        Me.LblStatusColor.BackColor = System.Drawing.Color.Red
        Me.LblStatusColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblStatusColor.Location = New System.Drawing.Point(130, 42)
        Me.LblStatusColor.Name = "LblStatusColor"
        Me.LblStatusColor.Size = New System.Drawing.Size(20, 20)
        Me.LblStatusColor.TabIndex = 27
        '
        'LblStatus
        '
        Me.LblStatus.AutoSize = True
        Me.LblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatus.ForeColor = System.Drawing.Color.Red
        Me.LblStatus.Location = New System.Drawing.Point(149, 40)
        Me.LblStatus.Name = "LblStatus"
        Me.LblStatus.Size = New System.Drawing.Size(52, 24)
        Me.LblStatus.TabIndex = 25
        Me.LblStatus.Text = "OFF"
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.nomor})
        Me.DataGridView1.GridColor = System.Drawing.SystemColors.AppWorkspace
        Me.DataGridView1.Location = New System.Drawing.Point(11, 20)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(584, 234)
        Me.DataGridView1.TabIndex = 23
        '
        'nomor
        '
        Me.nomor.HeaderText = "No."
        Me.nomor.Name = "nomor"
        Me.nomor.Width = 54
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(214, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(101, 17)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "Hasil QR Code"
        '
        'BtnStop
        '
        Me.BtnStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStop.Location = New System.Drawing.Point(114, 88)
        Me.BtnStop.Name = "BtnStop"
        Me.BtnStop.Size = New System.Drawing.Size(87, 40)
        Me.BtnStop.TabIndex = 21
        Me.BtnStop.Text = "Stop"
        Me.BtnStop.UseVisualStyleBackColor = True
        '
        'TimerToScan
        '
        Me.TimerToScan.Interval = 3000
        '
        'BtnStart
        '
        Me.BtnStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStart.Location = New System.Drawing.Point(12, 88)
        Me.BtnStart.Name = "BtnStart"
        Me.BtnStart.Size = New System.Drawing.Size(87, 40)
        Me.BtnStart.TabIndex = 20
        Me.BtnStart.Text = "Start"
        Me.BtnStart.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(217, 48)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(403, 21)
        Me.TextBox1.TabIndex = 19
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxtBoxBatasKemas
        '
        Me.TxtBoxBatasKemas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtBoxBatasKemas.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtBoxBatasKemas.Location = New System.Drawing.Point(9, 20)
        Me.TxtBoxBatasKemas.Multiline = True
        Me.TxtBoxBatasKemas.Name = "TxtBoxBatasKemas"
        Me.TxtBoxBatasKemas.Size = New System.Drawing.Size(93, 38)
        Me.TxtBoxBatasKemas.TabIndex = 41
        Me.TxtBoxBatasKemas.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TxtBoxBatasKemas)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(112, 67)
        Me.GroupBox1.TabIndex = 42
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Batas Kemas"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(12, 165)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(608, 267)
        Me.GroupBox2.TabIndex = 43
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Hasil QR Code"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(217, 101)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(28, 17)
        Me.Label12.TabIndex = 36
        Me.Label12.Text = "KT"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.LblJmlKemasUI)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(495, 82)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(125, 48)
        Me.GroupBox3.TabIndex = 43
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Jumlah Kemas"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.LblJmlKartonUI)
        Me.GroupBox4.Controls.Add(Me.LblKarton)
        Me.GroupBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(350, 82)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(133, 48)
        Me.GroupBox4.TabIndex = 44
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Jumlah Karton"
        '
        'LblStatusInsert
        '
        Me.LblStatusInsert.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LblStatusInsert.AutoSize = True
        Me.LblStatusInsert.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatusInsert.Location = New System.Drawing.Point(525, 145)
        Me.LblStatusInsert.Name = "LblStatusInsert"
        Me.LblStatusInsert.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.LblStatusInsert.Size = New System.Drawing.Size(75, 25)
        Me.LblStatusInsert.TabIndex = 45
        Me.LblStatusInsert.Text = "         "
        Me.LblStatusInsert.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelDateTime
        '
        Me.LabelDateTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelDateTime.Location = New System.Drawing.Point(394, 12)
        Me.LabelDateTime.Name = "LabelDateTime"
        Me.LabelDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelDateTime.Size = New System.Drawing.Size(226, 23)
        Me.LabelDateTime.TabIndex = 46
        Me.LabelDateTime.Text = "Date Time"
        Me.LabelDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TimerDateTime
        '
        '
        'FormQRCodeScanner
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(633, 497)
        Me.Controls.Add(Me.LabelDateTime)
        Me.Controls.Add(Me.LblStatusInsert)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.LblJmlKartonDB)
        Me.Controls.Add(Me.BtnStopManual)
        Me.Controls.Add(Me.LblJmlKemasDB)
        Me.Controls.Add(Me.LblStatusColor)
        Me.Controls.Add(Me.LblStatus)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnStop)
        Me.Controls.Add(Me.BtnStart)
        Me.Controls.Add(Me.TextBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormQRCodeScanner"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Agregasi - Cetak Karton"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblJmlKartonDB As Label
    Friend WithEvents LblJmlKemasUI As Label
    Friend WithEvents BtnStopManual As Button
    Friend WithEvents LblJmlKemasDB As Label
    Friend WithEvents LblKarton As Label
    Friend WithEvents LblJmlKartonUI As Label
    Friend WithEvents LblStatusColor As Label
    Friend WithEvents LblStatus As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Label1 As Label
    Friend WithEvents BtnStop As Button
    Private WithEvents TimerToScan As Timer
    Friend WithEvents BtnStart As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents nomor As DataGridViewTextBoxColumn
    Friend WithEvents TxtBoxBatasKemas As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label12 As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents LblStatusInsert As Label
    Friend WithEvents LabelDateTime As Label
    Friend WithEvents TimerDateTime As Timer
End Class
