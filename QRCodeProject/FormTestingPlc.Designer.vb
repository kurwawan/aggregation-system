<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTestingPlc
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
        Me.BtnMonitor = New System.Windows.Forms.Button()
        Me.BtnProgram = New System.Windows.Forms.Button()
        Me.BtnRun = New System.Windows.Forms.Button()
        Me.BtnStart = New System.Windows.Forms.Button()
        Me.BtnStop = New System.Windows.Forms.Button()
        Me.LblStatus = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LblReadDm = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.BtnStop2 = New System.Windows.Forms.Button()
        Me.LblStatus2 = New System.Windows.Forms.Label()
        Me.BtnStart2 = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnMonitor
        '
        Me.BtnMonitor.Location = New System.Drawing.Point(15, 21)
        Me.BtnMonitor.Name = "BtnMonitor"
        Me.BtnMonitor.Size = New System.Drawing.Size(99, 40)
        Me.BtnMonitor.TabIndex = 0
        Me.BtnMonitor.Text = "Monitor"
        Me.BtnMonitor.UseVisualStyleBackColor = True
        '
        'BtnProgram
        '
        Me.BtnProgram.Location = New System.Drawing.Point(15, 76)
        Me.BtnProgram.Name = "BtnProgram"
        Me.BtnProgram.Size = New System.Drawing.Size(99, 40)
        Me.BtnProgram.TabIndex = 1
        Me.BtnProgram.Text = "Program"
        Me.BtnProgram.UseVisualStyleBackColor = True
        '
        'BtnRun
        '
        Me.BtnRun.Location = New System.Drawing.Point(15, 130)
        Me.BtnRun.Name = "BtnRun"
        Me.BtnRun.Size = New System.Drawing.Size(99, 40)
        Me.BtnRun.TabIndex = 2
        Me.BtnRun.Text = "Run"
        Me.BtnRun.UseVisualStyleBackColor = True
        '
        'BtnStart
        '
        Me.BtnStart.Location = New System.Drawing.Point(14, 54)
        Me.BtnStart.Name = "BtnStart"
        Me.BtnStart.Size = New System.Drawing.Size(77, 41)
        Me.BtnStart.TabIndex = 3
        Me.BtnStart.Text = "Start"
        Me.BtnStart.UseVisualStyleBackColor = True
        '
        'BtnStop
        '
        Me.BtnStop.Location = New System.Drawing.Point(107, 54)
        Me.BtnStop.Name = "BtnStop"
        Me.BtnStop.Size = New System.Drawing.Size(77, 41)
        Me.BtnStop.TabIndex = 4
        Me.BtnStop.Text = "Stop"
        Me.BtnStop.UseVisualStyleBackColor = True
        '
        'LblStatus
        '
        Me.LblStatus.AutoSize = True
        Me.LblStatus.Location = New System.Drawing.Point(75, 27)
        Me.LblStatus.Name = "LblStatus"
        Me.LblStatus.Size = New System.Drawing.Size(45, 16)
        Me.LblStatus.TabIndex = 5
        Me.LblStatus.Text = "STOP"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BtnStop)
        Me.GroupBox1.Controls.Add(Me.LblStatus)
        Me.GroupBox1.Controls.Add(Me.BtnStart)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(169, 27)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 114)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Konveyor"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Red Value"
        '
        'LblReadDm
        '
        Me.LblReadDm.AutoSize = True
        Me.LblReadDm.Location = New System.Drawing.Point(140, 26)
        Me.LblReadDm.Name = "LblReadDm"
        Me.LblReadDm.Size = New System.Drawing.Size(36, 16)
        Me.LblReadDm.TabIndex = 7
        Me.LblReadDm.Text = "hasil"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.BtnMonitor)
        Me.GroupBox2.Controls.Add(Me.BtnProgram)
        Me.GroupBox2.Controls.Add(Me.BtnRun)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(19, 27)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(127, 180)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Status PLC"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.LblReadDm)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(169, 147)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(200, 60)
        Me.GroupBox3.TabIndex = 9
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Counter Encoder"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        '
        'SerialPort1
        '
        Me.SerialPort1.DataBits = 7
        Me.SerialPort1.Parity = System.IO.Ports.Parity.Even
        Me.SerialPort1.PortName = "COM4"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.BtnStop2)
        Me.GroupBox4.Controls.Add(Me.LblStatus2)
        Me.GroupBox4.Controls.Add(Me.BtnStart2)
        Me.GroupBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(169, 217)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(200, 114)
        Me.GroupBox4.TabIndex = 7
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Konveyor"
        '
        'BtnStop2
        '
        Me.BtnStop2.Location = New System.Drawing.Point(107, 54)
        Me.BtnStop2.Name = "BtnStop2"
        Me.BtnStop2.Size = New System.Drawing.Size(77, 41)
        Me.BtnStop2.TabIndex = 4
        Me.BtnStop2.Text = "Stop"
        Me.BtnStop2.UseVisualStyleBackColor = True
        '
        'LblStatus2
        '
        Me.LblStatus2.AutoSize = True
        Me.LblStatus2.Location = New System.Drawing.Point(75, 27)
        Me.LblStatus2.Name = "LblStatus2"
        Me.LblStatus2.Size = New System.Drawing.Size(45, 16)
        Me.LblStatus2.TabIndex = 5
        Me.LblStatus2.Text = "STOP"
        '
        'BtnStart2
        '
        Me.BtnStart2.Location = New System.Drawing.Point(14, 54)
        Me.BtnStart2.Name = "BtnStart2"
        Me.BtnStart2.Size = New System.Drawing.Size(77, 41)
        Me.BtnStart2.TabIndex = 3
        Me.BtnStart2.Text = "Start"
        Me.BtnStart2.UseVisualStyleBackColor = True
        '
        'FormTestingPlc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(387, 357)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "FormTestingPlc"
        Me.Text = "FormTestingPlc"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BtnMonitor As Button
    Friend WithEvents BtnProgram As Button
    Friend WithEvents BtnRun As Button
    Friend WithEvents BtnStart As Button
    Friend WithEvents BtnStop As Button
    Friend WithEvents LblStatus As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents LblReadDm As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Timer1 As Timer
    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents BtnStop2 As Button
    Friend WithEvents LblStatus2 As Label
    Friend WithEvents BtnStart2 As Button
End Class
