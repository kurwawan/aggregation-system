<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormQRCodeOmronScanner
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormQRCodeOmronScanner))
        Me.TimerDateTime = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBoxJmlKarton = New System.Windows.Forms.GroupBox()
        Me.LblJmlKartonUI = New System.Windows.Forms.Label()
        Me.LblKarton = New System.Windows.Forms.Label()
        Me.GroupBoxJmlKemas = New System.Windows.Forms.GroupBox()
        Me.LblJmlKemasUI = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.LblPbJmlKemas = New System.Windows.Forms.Label()
        Me.LblPbBatasKemas = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.nomor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBoxBatasKemas = New System.Windows.Forms.GroupBox()
        Me.LblBatasKemas = New System.Windows.Forms.Label()
        Me.TimerToScan = New System.Windows.Forms.Timer(Me.components)
        Me.LabelDateTime = New System.Windows.Forms.Label()
        Me.LblJmlKartonDB = New System.Windows.Forms.Label()
        Me.LblStatusColor = New System.Windows.Forms.Label()
        Me.LblStatus = New System.Windows.Forms.Label()
        Me.BtnStart = New System.Windows.Forms.Button()
        Me.BtnStopManual = New System.Windows.Forms.Button()
        Me.BtnStop = New System.Windows.Forms.Button()
        Me.GroupBoxStatus = New System.Windows.Forms.GroupBox()
        Me.TimerPlc = New System.Windows.Forms.Timer(Me.components)
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.LblStatusInsert = New System.Windows.Forms.Label()
        Me.LblCopyRight = New System.Windows.Forms.Label()
        Me.TimerTextOnOff = New System.Windows.Forms.Timer(Me.components)
        Me.BtnCekKoneksi = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BtnCekPrinter = New System.Windows.Forms.Button()
        Me.PrintPreviewDialog1 = New System.Windows.Forms.PrintPreviewDialog()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LblNamaProduk = New System.Windows.Forms.Label()
        Me.LblKodeProduk = New System.Windows.Forms.Label()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.LblNoBatch = New System.Windows.Forms.Label()
        Me.GroupBoxJmlKarton.SuspendLayout()
        Me.GroupBoxJmlKemas.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxBatasKemas.SuspendLayout()
        Me.GroupBoxStatus.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'TimerDateTime
        '
        '
        'GroupBoxJmlKarton
        '
        Me.GroupBoxJmlKarton.Controls.Add(Me.LblJmlKartonUI)
        Me.GroupBoxJmlKarton.Controls.Add(Me.LblKarton)
        Me.GroupBoxJmlKarton.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxJmlKarton.Location = New System.Drawing.Point(331, 10)
        Me.GroupBoxJmlKarton.Name = "GroupBoxJmlKarton"
        Me.GroupBoxJmlKarton.Size = New System.Drawing.Size(133, 59)
        Me.GroupBoxJmlKarton.TabIndex = 60
        Me.GroupBoxJmlKarton.TabStop = False
        Me.GroupBoxJmlKarton.Text = "Jumlah Karton"
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
        'GroupBoxJmlKemas
        '
        Me.GroupBoxJmlKemas.Controls.Add(Me.LblJmlKemasUI)
        Me.GroupBoxJmlKemas.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxJmlKemas.Location = New System.Drawing.Point(331, 76)
        Me.GroupBoxJmlKemas.Name = "GroupBoxJmlKemas"
        Me.GroupBoxJmlKemas.Size = New System.Drawing.Size(133, 62)
        Me.GroupBoxJmlKemas.TabIndex = 58
        Me.GroupBoxJmlKemas.TabStop = False
        Me.GroupBoxJmlKemas.Text = "Jumlah Kemas"
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
        Me.LblJmlKemasUI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.PictureBox2)
        Me.GroupBox2.Controls.Add(Me.PictureBox1)
        Me.GroupBox2.Controls.Add(Me.LblPbJmlKemas)
        Me.GroupBox2.Controls.Add(Me.LblPbBatasKemas)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.ProgressBar1)
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(16, 212)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(608, 293)
        Me.GroupBox2.TabIndex = 59
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Hasil QR Code"
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.QRCodeProject.My.Resources.Resources.rsz_imgonline_com_ua_replacecolor_1hnjdhf4echm
        Me.PictureBox2.Location = New System.Drawing.Point(265, 76)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(125, 127)
        Me.PictureBox2.TabIndex = 72
        Me.PictureBox2.TabStop = False
        Me.PictureBox2.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(197, 2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(251, 251)
        Me.PictureBox1.TabIndex = 72
        Me.PictureBox1.TabStop = False
        Me.PictureBox1.Visible = False
        '
        'LblPbJmlKemas
        '
        Me.LblPbJmlKemas.AutoSize = True
        Me.LblPbJmlKemas.BackColor = System.Drawing.Color.Transparent
        Me.LblPbJmlKemas.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPbJmlKemas.Location = New System.Drawing.Point(284, 22)
        Me.LblPbJmlKemas.Name = "LblPbJmlKemas"
        Me.LblPbJmlKemas.Size = New System.Drawing.Size(17, 18)
        Me.LblPbJmlKemas.TabIndex = 33
        Me.LblPbJmlKemas.Text = "0"
        Me.LblPbJmlKemas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblPbBatasKemas
        '
        Me.LblPbBatasKemas.AutoSize = True
        Me.LblPbBatasKemas.BackColor = System.Drawing.Color.Transparent
        Me.LblPbBatasKemas.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPbBatasKemas.Location = New System.Drawing.Point(325, 22)
        Me.LblPbBatasKemas.Name = "LblPbBatasKemas"
        Me.LblPbBatasKemas.Size = New System.Drawing.Size(17, 18)
        Me.LblPbBatasKemas.TabIndex = 31
        Me.LblPbBatasKemas.Text = "0"
        Me.LblPbBatasKemas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(306, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(12, 18)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "/"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(11, 20)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(584, 23)
        Me.ProgressBar1.TabIndex = 24
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.nomor})
        Me.DataGridView1.GridColor = System.Drawing.SystemColors.AppWorkspace
        Me.DataGridView1.Location = New System.Drawing.Point(11, 55)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(584, 234)
        Me.DataGridView1.TabIndex = 23
        '
        'nomor
        '
        Me.nomor.HeaderText = "No."
        Me.nomor.Name = "nomor"
        Me.nomor.ReadOnly = True
        Me.nomor.Width = 54
        '
        'GroupBoxBatasKemas
        '
        Me.GroupBoxBatasKemas.Controls.Add(Me.LblBatasKemas)
        Me.GroupBoxBatasKemas.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxBatasKemas.Location = New System.Drawing.Point(16, 76)
        Me.GroupBoxBatasKemas.Name = "GroupBoxBatasKemas"
        Me.GroupBoxBatasKemas.Size = New System.Drawing.Size(112, 62)
        Me.GroupBoxBatasKemas.TabIndex = 57
        Me.GroupBoxBatasKemas.TabStop = False
        Me.GroupBoxBatasKemas.Text = "Batas Kemas"
        '
        'LblBatasKemas
        '
        Me.LblBatasKemas.AutoSize = True
        Me.LblBatasKemas.BackColor = System.Drawing.Color.Transparent
        Me.LblBatasKemas.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblBatasKemas.Location = New System.Drawing.Point(42, 26)
        Me.LblBatasKemas.Name = "LblBatasKemas"
        Me.LblBatasKemas.Size = New System.Drawing.Size(25, 25)
        Me.LblBatasKemas.TabIndex = 30
        Me.LblBatasKemas.Text = "1"
        Me.LblBatasKemas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TimerToScan
        '
        '
        'LabelDateTime
        '
        Me.LabelDateTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelDateTime.Location = New System.Drawing.Point(13, 25)
        Me.LabelDateTime.Name = "LabelDateTime"
        Me.LabelDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelDateTime.Size = New System.Drawing.Size(126, 23)
        Me.LabelDateTime.TabIndex = 62
        Me.LabelDateTime.Text = "Date Time"
        Me.LabelDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblJmlKartonDB
        '
        Me.LblJmlKartonDB.AutoSize = True
        Me.LblJmlKartonDB.BackColor = System.Drawing.Color.Transparent
        Me.LblJmlKartonDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblJmlKartonDB.Location = New System.Drawing.Point(383, 522)
        Me.LblJmlKartonDB.Name = "LblJmlKartonDB"
        Me.LblJmlKartonDB.Size = New System.Drawing.Size(71, 17)
        Me.LblJmlKartonDB.TabIndex = 55
        Me.LblJmlKartonDB.Text = "0000000"
        '
        'LblStatusColor
        '
        Me.LblStatusColor.BackColor = System.Drawing.Color.Red
        Me.LblStatusColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblStatusColor.Location = New System.Drawing.Point(14, 19)
        Me.LblStatusColor.Name = "LblStatusColor"
        Me.LblStatusColor.Size = New System.Drawing.Size(32, 32)
        Me.LblStatusColor.TabIndex = 52
        '
        'LblStatus
        '
        Me.LblStatus.AutoSize = True
        Me.LblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatus.ForeColor = System.Drawing.Color.Red
        Me.LblStatus.Location = New System.Drawing.Point(49, 20)
        Me.LblStatus.Name = "LblStatus"
        Me.LblStatus.Size = New System.Drawing.Size(65, 29)
        Me.LblStatus.TabIndex = 51
        Me.LblStatus.Text = "OFF"
        '
        'BtnStart
        '
        Me.BtnStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStart.Location = New System.Drawing.Point(506, 84)
        Me.BtnStart.Name = "BtnStart"
        Me.BtnStart.Size = New System.Drawing.Size(118, 53)
        Me.BtnStart.TabIndex = 48
        Me.BtnStart.Text = "Start"
        Me.BtnStart.UseVisualStyleBackColor = True
        '
        'BtnStopManual
        '
        Me.BtnStopManual.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStopManual.Location = New System.Drawing.Point(506, 518)
        Me.BtnStopManual.Name = "BtnStopManual"
        Me.BtnStopManual.Size = New System.Drawing.Size(118, 28)
        Me.BtnStopManual.TabIndex = 54
        Me.BtnStopManual.Text = "Stop"
        Me.BtnStopManual.UseVisualStyleBackColor = True
        '
        'BtnStop
        '
        Me.BtnStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStop.Location = New System.Drawing.Point(290, 515)
        Me.BtnStop.Name = "BtnStop"
        Me.BtnStop.Size = New System.Drawing.Size(87, 21)
        Me.BtnStop.TabIndex = 49
        Me.BtnStop.Text = "Stop"
        Me.BtnStop.UseVisualStyleBackColor = True
        '
        'GroupBoxStatus
        '
        Me.GroupBoxStatus.Controls.Add(Me.LblStatus)
        Me.GroupBoxStatus.Controls.Add(Me.LblStatusColor)
        Me.GroupBoxStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxStatus.Location = New System.Drawing.Point(506, 10)
        Me.GroupBoxStatus.Name = "GroupBoxStatus"
        Me.GroupBoxStatus.Size = New System.Drawing.Size(118, 59)
        Me.GroupBoxStatus.TabIndex = 66
        Me.GroupBoxStatus.TabStop = False
        Me.GroupBoxStatus.Text = "Status"
        '
        'TimerPlc
        '
        Me.TimerPlc.Enabled = True
        '
        'SerialPort1
        '
        Me.SerialPort1.DataBits = 7
        Me.SerialPort1.Parity = System.IO.Ports.Parity.Even
        Me.SerialPort1.PortName = "COM4"
        '
        'LblStatusInsert
        '
        Me.LblStatusInsert.AutoSize = True
        Me.LblStatusInsert.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblStatusInsert.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatusInsert.Location = New System.Drawing.Point(512, 165)
        Me.LblStatusInsert.Name = "LblStatusInsert"
        Me.LblStatusInsert.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.LblStatusInsert.Size = New System.Drawing.Size(2, 27)
        Me.LblStatusInsert.TabIndex = 53
        '
        'LblCopyRight
        '
        Me.LblCopyRight.AutoSize = True
        Me.LblCopyRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCopyRight.Location = New System.Drawing.Point(3, 544)
        Me.LblCopyRight.Name = "LblCopyRight"
        Me.LblCopyRight.Size = New System.Drawing.Size(420, 12)
        Me.LblCopyRight.TabIndex = 67
        Me.LblCopyRight.Text = "Copyright © 2021 Kurniawan Ridwan .S,  PT IFARS Pharmaceutical Laboratories. All " &
    "rights reserved. "
        '
        'TimerTextOnOff
        '
        Me.TimerTextOnOff.Enabled = True
        Me.TimerTextOnOff.Interval = 50
        '
        'BtnCekKoneksi
        '
        Me.BtnCekKoneksi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCekKoneksi.Image = CType(resources.GetObject("BtnCekKoneksi.Image"), System.Drawing.Image)
        Me.BtnCekKoneksi.Location = New System.Drawing.Point(27, 14)
        Me.BtnCekKoneksi.Name = "BtnCekKoneksi"
        Me.BtnCekKoneksi.Size = New System.Drawing.Size(92, 49)
        Me.BtnCekKoneksi.TabIndex = 68
        Me.BtnCekKoneksi.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(420, 544)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(24, 12)
        Me.Label1.TabIndex = 69
        Me.Label1.Text = "v1.0"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.LabelDateTime)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(134, 76)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(150, 62)
        Me.GroupBox1.TabIndex = 70
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Tanggal /Waktu"
        '
        'BtnCekPrinter
        '
        Me.BtnCekPrinter.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCekPrinter.Image = CType(resources.GetObject("BtnCekPrinter.Image"), System.Drawing.Image)
        Me.BtnCekPrinter.Location = New System.Drawing.Point(164, 14)
        Me.BtnCekPrinter.Name = "BtnCekPrinter"
        Me.BtnCekPrinter.Size = New System.Drawing.Size(92, 49)
        Me.BtnCekPrinter.TabIndex = 71
        Me.BtnCekPrinter.UseVisualStyleBackColor = True
        '
        'PrintPreviewDialog1
        '
        Me.PrintPreviewDialog1.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.ClientSize = New System.Drawing.Size(400, 300)
        Me.PrintPreviewDialog1.Document = Me.PrintDocument1
        Me.PrintPreviewDialog1.Enabled = True
        Me.PrintPreviewDialog1.Icon = CType(resources.GetObject("PrintPreviewDialog1.Icon"), System.Drawing.Icon)
        Me.PrintPreviewDialog1.Name = "PrintPreviewDialog1"
        Me.PrintPreviewDialog1.Visible = False
        '
        'PrintDocument1
        '
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.LblNamaProduk)
        Me.GroupBox3.Controls.Add(Me.LblKodeProduk)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(16, 148)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(331, 52)
        Me.GroupBox3.TabIndex = 58
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Produk"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(82, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(12, 15)
        Me.Label4.TabIndex = 32
        Me.Label4.Text = "-"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblNamaProduk
        '
        Me.LblNamaProduk.AutoSize = True
        Me.LblNamaProduk.BackColor = System.Drawing.Color.Transparent
        Me.LblNamaProduk.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNamaProduk.Location = New System.Drawing.Point(98, 24)
        Me.LblNamaProduk.Name = "LblNamaProduk"
        Me.LblNamaProduk.Size = New System.Drawing.Size(140, 15)
        Me.LblNamaProduk.TabIndex = 31
        Me.LblNamaProduk.Text = "FARSIRETIC TABLET"
        Me.LblNamaProduk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblKodeProduk
        '
        Me.LblKodeProduk.AutoSize = True
        Me.LblKodeProduk.BackColor = System.Drawing.Color.Transparent
        Me.LblKodeProduk.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblKodeProduk.Location = New System.Drawing.Point(8, 24)
        Me.LblKodeProduk.Name = "LblKodeProduk"
        Me.LblKodeProduk.Size = New System.Drawing.Size(72, 15)
        Me.LblKodeProduk.TabIndex = 30
        Me.LblKodeProduk.Text = "TAFST225"
        Me.LblKodeProduk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.LblNoBatch)
        Me.GroupBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.Location = New System.Drawing.Point(353, 148)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(111, 52)
        Me.GroupBox5.TabIndex = 60
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Batch"
        '
        'LblNoBatch
        '
        Me.LblNoBatch.AutoSize = True
        Me.LblNoBatch.BackColor = System.Drawing.Color.Transparent
        Me.LblNoBatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNoBatch.Location = New System.Drawing.Point(33, 24)
        Me.LblNoBatch.Name = "LblNoBatch"
        Me.LblNoBatch.Size = New System.Drawing.Size(47, 15)
        Me.LblNoBatch.TabIndex = 30
        Me.LblNoBatch.Text = "99999"
        Me.LblNoBatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FormQRCodeOmronScanner
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(640, 562)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.BtnCekKoneksi)
        Me.Controls.Add(Me.BtnCekPrinter)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblCopyRight)
        Me.Controls.Add(Me.LblStatusInsert)
        Me.Controls.Add(Me.GroupBoxStatus)
        Me.Controls.Add(Me.GroupBoxJmlKarton)
        Me.Controls.Add(Me.GroupBoxJmlKemas)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBoxBatasKemas)
        Me.Controls.Add(Me.LblJmlKartonDB)
        Me.Controls.Add(Me.BtnStart)
        Me.Controls.Add(Me.BtnStopManual)
        Me.Controls.Add(Me.BtnStop)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormQRCodeOmronScanner"
        Me.Text = "Agregasi"
        Me.GroupBoxJmlKarton.ResumeLayout(False)
        Me.GroupBoxJmlKarton.PerformLayout()
        Me.GroupBoxJmlKemas.ResumeLayout(False)
        Me.GroupBoxJmlKemas.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxBatasKemas.ResumeLayout(False)
        Me.GroupBoxBatasKemas.PerformLayout()
        Me.GroupBoxStatus.ResumeLayout(False)
        Me.GroupBoxStatus.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TimerDateTime As Timer
    Friend WithEvents GroupBoxJmlKarton As GroupBox
    Friend WithEvents LblJmlKartonUI As Label
    Friend WithEvents LblKarton As Label
    Friend WithEvents GroupBoxJmlKemas As GroupBox
    Friend WithEvents LblJmlKemasUI As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents nomor As DataGridViewTextBoxColumn
    Friend WithEvents GroupBoxBatasKemas As GroupBox
    Private WithEvents TimerToScan As Timer
    Friend WithEvents LabelDateTime As Label
    Friend WithEvents LblJmlKartonDB As Label
    Friend WithEvents LblStatusColor As Label
    Friend WithEvents LblStatus As Label
    Friend WithEvents BtnStart As Button
    Friend WithEvents BtnStopManual As Button
    Friend WithEvents BtnStop As Button
    Friend WithEvents LblBatasKemas As Label
    Friend WithEvents GroupBoxStatus As GroupBox
    Friend WithEvents TimerPlc As Timer
    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
    Friend WithEvents LblStatusInsert As Label
    Friend WithEvents LblCopyRight As Label
    Friend WithEvents TimerTextOnOff As Timer
    Friend WithEvents BtnCekKoneksi As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents LblPbBatasKemas As Label
    Friend WithEvents LblPbJmlKemas As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents BtnCekPrinter As Button
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents LblKodeProduk As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents LblNamaProduk As Label
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents LblNoBatch As Label
End Class
