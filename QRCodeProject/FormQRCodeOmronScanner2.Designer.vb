<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormQRCodeOmronScanner2
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormQRCodeOmronScanner2))
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.LblStatus = New System.Windows.Forms.Label()
        Me.LblStatusColor = New System.Windows.Forms.Label()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.BtnStart = New System.Windows.Forms.Button()
        Me.BtnStopManual = New System.Windows.Forms.Button()
        Me.LblJmlKemasDB = New System.Windows.Forms.Label()
        Me.BtnStop = New System.Windows.Forms.Button()
        Me.LabelDateTime = New System.Windows.Forms.Label()
        Me.LblJmlKartonDB = New System.Windows.Forms.Label()
        Me.TimerPlc = New System.Windows.Forms.Timer(Me.components)
        Me.TimerToScan = New System.Windows.Forms.Timer(Me.components)
        Me.LblBatasKemas = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.nomor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.LblJmlKemasUI = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.LblJmlKartonUI = New System.Windows.Forms.Label()
        Me.LblKarton = New System.Windows.Forms.Label()
        Me.LblStatusInsert = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.TimerDateTime = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'SerialPort1
        '
        Me.SerialPort1.DataBits = 7
        Me.SerialPort1.Parity = System.IO.Ports.Parity.Even
        Me.SerialPort1.PortName = "COM4"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.LblStatus)
        Me.GroupBox6.Controls.Add(Me.LblStatusColor)
        Me.GroupBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.Location = New System.Drawing.Point(492, 21)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(118, 59)
        Me.GroupBox6.TabIndex = 80
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Status"
        '
        'LblStatus
        '
        Me.LblStatus.AutoSize = True
        Me.LblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatus.ForeColor = System.Drawing.Color.Red
        Me.LblStatus.Location = New System.Drawing.Point(52, 23)
        Me.LblStatus.Name = "LblStatus"
        Me.LblStatus.Size = New System.Drawing.Size(52, 24)
        Me.LblStatus.TabIndex = 51
        Me.LblStatus.Text = "OFF"
        '
        'LblStatusColor
        '
        Me.LblStatusColor.BackColor = System.Drawing.Color.Red
        Me.LblStatusColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblStatusColor.Location = New System.Drawing.Point(22, 19)
        Me.LblStatusColor.Name = "LblStatusColor"
        Me.LblStatusColor.Size = New System.Drawing.Size(32, 32)
        Me.LblStatusColor.TabIndex = 52
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.ComboBox1)
        Me.GroupBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.Location = New System.Drawing.Point(23, 21)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(238, 53)
        Me.GroupBox5.TabIndex = 79
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Device Printer"
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(8, 19)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(220, 23)
        Me.ComboBox1.TabIndex = 64
        '
        'BtnStart
        '
        Me.BtnStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStart.Location = New System.Drawing.Point(149, 97)
        Me.BtnStart.Name = "BtnStart"
        Me.BtnStart.Size = New System.Drawing.Size(111, 42)
        Me.BtnStart.TabIndex = 67
        Me.BtnStart.Text = "Start"
        Me.BtnStart.UseVisualStyleBackColor = True
        '
        'BtnStopManual
        '
        Me.BtnStopManual.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStopManual.Location = New System.Drawing.Point(519, 458)
        Me.BtnStopManual.Name = "BtnStopManual"
        Me.BtnStopManual.Size = New System.Drawing.Size(112, 46)
        Me.BtnStopManual.TabIndex = 70
        Me.BtnStopManual.Text = "Stop"
        Me.BtnStopManual.UseVisualStyleBackColor = True
        '
        'LblJmlKemasDB
        '
        Me.LblJmlKemasDB.AutoSize = True
        Me.LblJmlKemasDB.BackColor = System.Drawing.Color.Transparent
        Me.LblJmlKemasDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblJmlKemasDB.Location = New System.Drawing.Point(340, 164)
        Me.LblJmlKemasDB.Name = "LblJmlKemasDB"
        Me.LblJmlKemasDB.Size = New System.Drawing.Size(17, 17)
        Me.LblJmlKemasDB.TabIndex = 69
        Me.LblJmlKemasDB.Text = "0"
        '
        'BtnStop
        '
        Me.BtnStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStop.Location = New System.Drawing.Point(137, 157)
        Me.BtnStop.Name = "BtnStop"
        Me.BtnStop.Size = New System.Drawing.Size(87, 29)
        Me.BtnStop.TabIndex = 68
        Me.BtnStop.Text = "Stop"
        Me.BtnStop.UseVisualStyleBackColor = True
        '
        'LabelDateTime
        '
        Me.LabelDateTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelDateTime.Location = New System.Drawing.Point(454, 109)
        Me.LabelDateTime.Name = "LabelDateTime"
        Me.LabelDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelDateTime.Size = New System.Drawing.Size(156, 23)
        Me.LabelDateTime.TabIndex = 78
        Me.LabelDateTime.Text = "Date Time"
        Me.LabelDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LblJmlKartonDB
        '
        Me.LblJmlKartonDB.AutoSize = True
        Me.LblJmlKartonDB.BackColor = System.Drawing.Color.Transparent
        Me.LblJmlKartonDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblJmlKartonDB.Location = New System.Drawing.Point(259, 164)
        Me.LblJmlKartonDB.Name = "LblJmlKartonDB"
        Me.LblJmlKartonDB.Size = New System.Drawing.Size(71, 17)
        Me.LblJmlKartonDB.TabIndex = 71
        Me.LblJmlKartonDB.Text = "0000000"
        '
        'TimerPlc
        '
        Me.TimerPlc.Enabled = True
        '
        'TimerToScan
        '
        '
        'LblBatasKemas
        '
        Me.LblBatasKemas.AutoSize = True
        Me.LblBatasKemas.BackColor = System.Drawing.Color.Transparent
        Me.LblBatasKemas.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblBatasKemas.Location = New System.Drawing.Point(42, 27)
        Me.LblBatasKemas.Name = "LblBatasKemas"
        Me.LblBatasKemas.Size = New System.Drawing.Size(25, 25)
        Me.LblBatasKemas.TabIndex = 30
        Me.LblBatasKemas.Text = "1"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.LblBatasKemas)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(23, 82)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(112, 67)
        Me.GroupBox1.TabIndex = 73
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Batas Kemas"
        '
        'nomor
        '
        Me.nomor.HeaderText = "No."
        Me.nomor.Name = "nomor"
        Me.nomor.ReadOnly = True
        Me.nomor.Width = 54
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.nomor})
        Me.DataGridView1.GridColor = System.Drawing.SystemColors.AppWorkspace
        Me.DataGridView1.Location = New System.Drawing.Point(11, 20)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(584, 234)
        Me.DataGridView1.TabIndex = 23
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(23, 185)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(608, 267)
        Me.GroupBox2.TabIndex = 75
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Hasil QR Code"
        '
        'LblJmlKemasUI
        '
        Me.LblJmlKemasUI.AutoSize = True
        Me.LblJmlKemasUI.BackColor = System.Drawing.Color.Transparent
        Me.LblJmlKemasUI.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblJmlKemasUI.Location = New System.Drawing.Point(54, 25)
        Me.LblJmlKemasUI.Name = "LblJmlKemasUI"
        Me.LblJmlKemasUI.Size = New System.Drawing.Size(21, 24)
        Me.LblJmlKemasUI.TabIndex = 34
        Me.LblJmlKemasUI.Text = "0"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.LblJmlKemasUI)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(317, 87)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(131, 62)
        Me.GroupBox3.TabIndex = 74
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Jumlah Kemas"
        '
        'LblJmlKartonUI
        '
        Me.LblJmlKartonUI.AutoSize = True
        Me.LblJmlKartonUI.BackColor = System.Drawing.Color.Transparent
        Me.LblJmlKartonUI.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblJmlKartonUI.Location = New System.Drawing.Point(43, 23)
        Me.LblJmlKartonUI.Name = "LblJmlKartonUI"
        Me.LblJmlKartonUI.Size = New System.Drawing.Size(87, 24)
        Me.LblJmlKartonUI.TabIndex = 28
        Me.LblJmlKartonUI.Text = "0000000"
        '
        'LblKarton
        '
        Me.LblKarton.AutoSize = True
        Me.LblKarton.BackColor = System.Drawing.Color.Transparent
        Me.LblKarton.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblKarton.Location = New System.Drawing.Point(8, 23)
        Me.LblKarton.Name = "LblKarton"
        Me.LblKarton.Size = New System.Drawing.Size(36, 24)
        Me.LblKarton.TabIndex = 29
        Me.LblKarton.Text = "KT"
        '
        'LblStatusInsert
        '
        Me.LblStatusInsert.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.LblStatusInsert.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatusInsert.Location = New System.Drawing.Point(300, 187)
        Me.LblStatusInsert.Name = "LblStatusInsert"
        Me.LblStatusInsert.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.LblStatusInsert.Size = New System.Drawing.Size(175, 25)
        Me.LblStatusInsert.TabIndex = 77
        Me.LblStatusInsert.Text = "         "
        Me.LblStatusInsert.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.LblJmlKartonUI)
        Me.GroupBox4.Controls.Add(Me.LblKarton)
        Me.GroupBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(315, 21)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(133, 59)
        Me.GroupBox4.TabIndex = 76
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Jumlah Karton"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(232, 164)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(28, 17)
        Me.Label12.TabIndex = 72
        Me.Label12.Text = "KT"
        '
        'TimerDateTime
        '
        '
        'FormQRCodeOmronScanner2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(649, 511)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.BtnStart)
        Me.Controls.Add(Me.BtnStopManual)
        Me.Controls.Add(Me.LblJmlKemasDB)
        Me.Controls.Add(Me.BtnStop)
        Me.Controls.Add(Me.LabelDateTime)
        Me.Controls.Add(Me.LblJmlKartonDB)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.LblStatusInsert)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.Label12)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormQRCodeOmronScanner2"
        Me.Text = "Agregasi (Tanpa Reject)"
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents LblStatus As Label
    Friend WithEvents LblStatusColor As Label
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents BtnStart As Button
    Friend WithEvents BtnStopManual As Button
    Friend WithEvents LblJmlKemasDB As Label
    Friend WithEvents BtnStop As Button
    Friend WithEvents LabelDateTime As Label
    Friend WithEvents LblJmlKartonDB As Label
    Friend WithEvents TimerPlc As Timer
    Private WithEvents TimerToScan As Timer
    Friend WithEvents LblBatasKemas As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents nomor As DataGridViewTextBoxColumn
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents LblJmlKemasUI As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents LblJmlKartonUI As Label
    Friend WithEvents LblKarton As Label
    Friend WithEvents LblStatusInsert As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Label12 As Label
    Friend WithEvents TimerDateTime As Timer
End Class
