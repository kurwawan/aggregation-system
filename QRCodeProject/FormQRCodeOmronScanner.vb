Imports Oracle.ManagedDataAccess.Client
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.CrystalReports
Imports System.Net.Sockets
Imports System.Drawing.Printing
Imports QRCoder

Public Class FormQRCodeOmronScanner
    Dim print As String
    Dim agregasiReport As New ReportAgregasi
    Dim countKarton As Integer = 0 ' (9999999 => Limit)
    Dim countKartonSecond As Integer = 0 'server
    Dim countKarton2 As Integer
    Dim countKemasUI As Integer
    Dim kodeProduk, kodeProduk2, jenisKemasan, isiTiapKemasan, nieProduk, noBatch, batchProdukParam, kodeProdukParam As String
    Dim countCode As Integer

    Dim txt As CrystalDecisions.CrystalReports.Engine.TextObject
    Dim a1, a2 As String

    Dim kemasan, puom, suhu, rilis, productCode, namaProduk, subIsi, isi, ed, batch, desc, kodeLabel, ketItem As String

    Dim TopLeft As StringFormat = New StringFormat()
    Dim TopCenter As StringFormat = New StringFormat()
    Dim TopRight As StringFormat = New StringFormat()
    Dim MidLeft As StringFormat = New StringFormat()
    Dim MidCenter As StringFormat = New StringFormat()
    Dim MidRight As StringFormat = New StringFormat()
    Dim BottomLeft As StringFormat = New StringFormat()
    Dim BottomCenter As StringFormat = New StringFormat()
    Dim BottomRight As StringFormat = New StringFormat()

    Dim vIDs() As String, iv As Int32 = 0
    Dim VID_PID_device '= "VID_03F0&PID_3E17" '"VID_0781&PID_5581" ' Change to your's 
    Dim device_name As String
    Dim bDsp As Boolean = True
    Dim bAttached As Boolean = False

    'omron
    Dim clientIp As TcpClient
    Dim stream As NetworkStream
    Dim ip As String
    Dim port As String
    Dim responseData As [String] = String.Empty
    'plc
    Public TX As String
    Public FCS As String
    Public RXD As String

    Private Sub FormQRCodeOmronScanner_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        MaximizeBox = False
        MinimizeBox = False
        Me.FormBorderStyle = FormBorderStyle.FixedDialog

        LblPbJmlKemas.BackColor = Color.Transparent
        LblPbJmlKemas.BringToFront()
        LblPbBatasKemas.BackColor = Color.Transparent
        LblPbBatasKemas.BringToFront()

        LblNoBatch.Text = ""
        LblKodeProduk.Text = ""
        LblNamaProduk.Text = ""
        Label4.Visible = False

        TimerDateTime.Start()
        LblStatusInsert.Visible = False
        BtnStop.Visible = False

        'GroupBoxDevicePrinter.Enabled = False
        BtnStart.Enabled = False
        BtnCekPrinter.Enabled = False

        'helper
        LblJmlKartonDB.Visible = False

        BtnStop.Enabled = False
        BtnStopManual.Enabled = False

        'Dim devicePrinters As String
        'For Each devicePrinters In PrinterSettings.InstalledPrinters
        '    ComboBox1.Items.Add(devicePrinters)
        'Next
        'ComboBox1.SelectedIndex = 0

        'Call AgregasiConnection()
        'cmdCount5 = New MySqlCommand(
        '    "select cast(substring(kode_karton,45,7) as unsigned integer) as new 
        '    from agregasi_line order by id desc limit 1", 'agregasi_header
        '    conn)
        'Dim getCountKodeKarton = cmdCount5.ExecuteScalar()
        'countKarton = CInt(getCountKodeKarton)
        'Call CheckLimitKarton()

        'cmdCount4 = New MySqlCommand(
        '    "select count(kode_kemas) 
        '    from agregasi_line where status_print = 'FALSE'",
        '    conn)
        'Dim getCountKodeKemas = cmdCount4.ExecuteScalar
        'countKemasUI = CInt(getCountKodeKemas)
        'LblJmlKemasUI.Text = countKemasUI

        'LblPbJmlKemas.Text = countKemasUI

        Call isiGrid()
        'MsgBox(Login.UserID)

        PrintDocument1.DefaultPageSettings.PaperSize = (From s As PaperSize In PrintDocument1.PrinterSettings.PaperSizes.Cast(Of PaperSize) Where s.RawKind = PaperKind.A4).FirstOrDefault
        TopLeft.LineAlignment = StringAlignment.Near
        TopLeft.Alignment = StringAlignment.Near

        TopCenter.LineAlignment = StringAlignment.Near
        TopCenter.Alignment = StringAlignment.Center

        TopRight.LineAlignment = StringAlignment.Near
        TopRight.Alignment = StringAlignment.Far

        MidLeft.LineAlignment = StringAlignment.Center
        MidLeft.Alignment = StringAlignment.Near

        MidCenter.LineAlignment = StringAlignment.Center
        MidCenter.Alignment = StringAlignment.Center

        MidRight.LineAlignment = StringAlignment.Center
        MidRight.Alignment = StringAlignment.Far

        BottomLeft.LineAlignment = StringAlignment.Far
        BottomLeft.Alignment = StringAlignment.Near

        BottomCenter.LineAlignment = StringAlignment.Far
        BottomCenter.Alignment = StringAlignment.Center

        BottomRight.LineAlignment = StringAlignment.Far
        BottomRight.Alignment = StringAlignment.Far
    End Sub

    Private Sub BtnCekKoneksi_Click(sender As Object, e As EventArgs) Handles BtnCekKoneksi.Click
        Dim success As String
        Call AgregasiServerConnection()
        conn.Close()
        Call OracleConnection()
        conn.Close()
        success = MsgBox("Koneksi server terhubung", MsgBoxStyle.Information, "Sukses")
        If success = vbOK Then
            'GroupBoxDevicePrinter.Enabled = True
            BtnStart.Enabled = False
            BtnCekKoneksi.Enabled = False
            BtnCekPrinter.Enabled = True
        End If
    End Sub

    Private Sub VidPidDevice()
        Call AgregasiConnection()

        cmd = New MySqlCommand("SELECT vid_pid_device FROM nomor_vid_pid ORDER BY id LIMIT 1", conn)
        VID_PID_device = cmd.ExecuteScalar

        cmd2 = New MySqlCommand("SELECT device_name FROM nomor_vid_pid ORDER BY id LIMIT 1", conn)
        device_name = cmd2.ExecuteScalar

        cmd.Dispose()
        conn.Close()
    End Sub

    Private Sub BtnCekPrinter_Click(sender As Object, e As EventArgs) Handles BtnCekPrinter.Click
        Dim success As String
        Dim settings As PrinterSettings = New PrinterSettings()
        settings.PrinterName = "HP LaserJet P1006"

        'If ComboBox1.Text = settings.PrinterName Then
        '    success = MsgBox("Koneksi printer '" & ComboBox1.Text & "' terhubung", MsgBoxStyle.Information, "Sukses")
        '    If success = vbOK Then
        '        BtnStart.Enabled = True
        '        BtnCekKoneksi.Enabled = False
        '        BtnCekPrinter.Enabled = False
        '        GroupBoxDevicePrinter.Enabled = False
        '    End If
        'Else
        '    MsgBox("Koneksi printer salah", MsgBoxStyle.Exclamation, "Gagal")
        'End If
        Call VidPidDevice()

        If Find_Device_By_VID_PID(VID_PID_device) Then
            success = MsgBox("Koneksi printer '" & VID_PID_device & "' terhubung", MsgBoxStyle.Information, "Sukses")
            If success = vbOK Then
                BtnStart.Enabled = True
                BtnCekKoneksi.Enabled = False
                BtnCekPrinter.Enabled = False
                'GroupBoxDevicePrinter.Enabled = False
            End If
        Else
            MsgBox("Koneksi printer salah", MsgBoxStyle.Exclamation, "Gagal")
        End If

    End Sub

    Private Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnStart.Click
        Try
            LblStatusColor.BackColor = Color.Green
            'Call Monitor()
            'ComboBox1.Enabled = False
            LblStatus.Text = ""

            ProgressBar1.Value = 0

            ip = "192.168.188.2"
            port = "2001"
            clientIp = New TcpClient
            clientIp.Connect(ip, port)

            If clientIp.Connected = True Then
                stream = clientIp.GetStream
                Console.WriteLine("CONNECT")
                TimerToScan.Start()
                Call status()
            Else
                Console.WriteLine("FAILED")
            End If
        Catch ex As Exception
            LblStatusColor.BackColor = Color.Red
            MsgBox("Gagal koneksi Omron Scanner", MsgBoxStyle.Exclamation, "Error")
            Console.WriteLine("Connection Error: ", ex.Message)
        End Try
    End Sub

    Private Sub BtnStop_Click(sender As Object, e As EventArgs) Handles BtnStop.Click
        ip = "192.168.188.2"
        port = "2001"
        clientIp = New TcpClient
        clientIp.Connect(ip, port)

        If clientIp.Connected = True Then
            stream = clientIp.GetStream
            clientIp.GetStream.Close()
            MsgBox("Disconnected")
            LblStatusColor.BackColor = Color.Red
            Console.WriteLine("DiSCONNECTED")
            TimerToScan.Stop()
            Call status()
        Else
            MsgBox("Omron Scanner can't start.")
            Console.WriteLine("Waiting to connect")
        End If
    End Sub

    'TODO: function showing db to datagridview
    Sub isiGrid()
        Dim i As Integer
        Dim fs As New Font(DataGridView1.ColumnHeadersDefaultCellStyle.Font.Size, 14)

        Call AgregasiConnection()
        da = New MySqlDataAdapter("select kode_kemas from agregasi_line     
                                   where status_print='FALSE' order by id DESC",
                                  conn)
        ds = New DataSet
        da.Fill(ds, "agregasi_line")
        DataGridView1.DataSource = ds.Tables("agregasi_line")
        DataGridView1.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.DefaultCellStyle.Font = fs

        'kolom nomor
        For i = 0 To DataGridView1.Rows.Count - 2
            DataGridView1.Rows(i).Cells(0).Value &= (i + 1)
        Next

        Dim row As DataGridViewRow
        For Each row In DataGridView1.Rows
            With DataGridView1
                .RowHeadersVisible = False
                .Columns(1).HeaderCell.Value = "Kode QR Code"
            End With

            If DataGridView1.Rows.Count = 0 Then
                DataGridView1.Rows(0).Selected = True
            ElseIf DataGridView1.Rows.Count = 1 Then
                DataGridView1.Rows(DataGridView1.RowCount - 1).Selected = True
            Else
                DataGridView1.Rows(DataGridView1.RowCount - 2).Selected = True
            End If
        Next
    End Sub

    'TODO: function for label (ON/OFF)
    Sub status()
        If TimerToScan.Enabled = True Then
            LblStatus.Text = "ON"
            LblStatus.ForeColor = Color.Green

            BtnStart.Enabled = False
            BtnStop.Enabled = True
            BtnStopManual.Enabled = True
            Me.ControlBox = False
            LblStatusInsert.Visible = True
        ElseIf TimerToScan.Enabled = False Then
            LblStatus.Text = "OFF"
            LblStatus.ForeColor = Color.Red

            BtnStart.Enabled = True
            BtnStop.Enabled = False
            BtnStopManual.Enabled = False
            Me.ControlBox = True
            LblStatusInsert.Visible = False
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles TimerToScan.Tick
        If clientIp.Available > 0 Then
            Dim data(clientIp.Available - 1) As Byte
            Dim bytes As Int32 = stream.Read(data, 0, data.Length)
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes)

            Try
                Call AgregasiConnection()
                cmdCount = New MySqlCommand(
                    "select cast(count(kode_kemas) as unsigned integer) as new from agregasi_line 
                    where status_print = 'FALSE'",
                    conn)
                countCode = CInt(cmdCount.ExecuteScalar)

                cmdCount2 = New MySqlCommand(
                    "select cast(substring(kode_karton,45,7) as unsigned integer) as karton 
                    from agregasi_header 
                    where substring(kode_karton,1,42) = substring('" & responseData & "',1,42) 
                    order by karton desc limit 1",
                    conn)
                Dim countKodeKarton = cmdCount2.ExecuteScalar()

                cmdCount5 = New MySqlCommand(
                    "select cast(substring(kode_karton,45,7) as unsigned integer) as karton 
                    from agregasi_header 
                    where substring(kode_karton,1,42) = substring('" & responseData & "',1,42) 
                    order by karton desc limit 1",
                    conn)
                Dim countKodeKartonSecond = cmdCount5.ExecuteScalar()

                'select utk cek kode karton dari kode kemas berbeda atau tidak
                cmdCheck = New MySqlCommand(
                    "select cast(substring(kode_karton,45,7) as unsigned integer) as karton 
                    from agregasi_header 
                    where substring(kode_karton,1,42) = substring('" & responseData & "',1,42) 
                    order by karton desc limit 1",
                    conn)
                rd = cmdCheck.ExecuteReader
                rd.Read()

                ProgressBar1.Minimum = 0
                ProgressBar1.Maximum = Convert.ToInt32(LblBatasKemas.Text)

                'update karton ke ui
                If countCode = 0 And responseData.Length = 51 Then
                    countKarton = CInt(countKodeKartonSecond) + 1
                    Call CheckLimitKarton()
                ElseIf responseData.Length = 51 And countCode Mod Convert.ToInt32(LblBatasKemas.Text) = 4 Then
                    countKarton = CInt(countKodeKartonSecond) + 1
                    Call CheckLimitKarton()
                End If
                'update karton ke db
                If (countCode Mod Convert.ToInt32(LblBatasKemas.Text) = 4) And responseData.Length = 51 Then
                    If IsDBNull(rd) Then
                        countKarton2 = countKarton2 + 1
                    ElseIf (Not IsDBNull(rd) And rd.HasRows = True) Then
                        Dim countKodeKartonTemp2 As Integer = CInt(countKodeKarton)
                        countKodeKartonTemp2 = countKodeKartonTemp2 + 1
                        countKarton2 = countKodeKartonTemp2
                    ElseIf Not IsDBNull(rd) Then
                        countKarton2 = countKarton2 + 1
                    End If
                    Call CheckLimitKarton()
                ElseIf countCode Mod Convert.ToInt32(LblBatasKemas.Text) = 0 And responseData.Length = 51 Then
                    If (rd.HasRows = False) Then 'jika beda balik ke 0
                        countKarton2 = 0
                    End If
                End If

                'plc
                If countKodeKarton Mod 2 = 0 Then
                    Call StartArahKarton()
                ElseIf countKodeKarton Mod 2 <> 0 Then
                    Call StopArahKarton()
                End If
                rd.Close()
                cmdCheck.Dispose()
                conn.Close()

                Call AgregasiConnection()
                cmdCheck2 = New MySqlCommand("SELECT kode_kemas FROM agregasi_line WHERE kode_kemas Like '" & responseData & "'", conn)
                rd2 = cmdCheck2.ExecuteReader
                rd2.Read()
                'conn.Close()

                'Call AgregasiConnection()
                'cmd3 = New MySqlCommand("select count(kode_kemas) from agregasi_line",
                '                        conn)
                'Dim countTemp As Integer = CInt(cmd3.ExecuteScalar)
                'cmd3.Dispose()

                'If countTemp = 1 Then
                '    ProgressBar1.Value = 1 / Convert.ToInt32(LblBatasKemas.Text)
                'End If

                'compare kode kemas => insert data
                If responseData.Length = 51 And rd2.HasRows = False Then
                    Call InsertKodeKemas() 'sudah dispose dan close

                    Console.Write("berhasil")
                    LblStatusInsert.Text = "Berhasil"
                    LblStatusInsert.ForeColor = Color.Green

                    countKemasUI += 1
                    LblJmlKemasUI.Text = countKemasUI

                    If LblJmlKemasUI.Text = "1" Then
                        ProgressBar1.Value = 0
                        Call GetIsiKartonOracle()
                    End If
                    LblPbJmlKemas.Text = countKemasUI
                    ProgressBar1.Value += 1

                    Call AgregasiConnection()
                    If countCode Mod Convert.ToInt32(LblBatasKemas.Text) = (Convert.ToInt32(LblBatasKemas.Text) - 1) And countCode <> 0 Then
                        TimerToScan.Dispose()
                        TimerToScan.Stop()

                        Call UpdateKodeKarton()

                        'PrintDocument1.PrinterSettings.PrinterName = ComboBox1.Text
                        'Call GetProdukOracle()
                        'print
                        'Call SettingPrint()

                        ProgressBar1.Value = Convert.ToInt32(LblBatasKemas.Text)
                        countKemasUI = 0
                        'TimerToScan.Start()
                    End If

                    'Call GetIsiKartonOracle()

                    rd2.Close()
                    cmdCheck2.Dispose()
                    conn.Close()

                    TimerToScan.Start()
                ElseIf responseData.Length <> 51 Or rd2.HasRows = True Then
                    Console.Write("gagal")
                    LblStatusInsert.Text = "Gagal"
                    LblStatusInsert.ForeColor = Color.Red

                    'Call StartReject()

                    rd2.Close()
                    cmdCheck2.Dispose()
                    conn.Close()
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End If
    End Sub

    Sub CheckLimitKarton()
        Dim alertLimit As String
        If countKarton < 10 Then
            LblJmlKartonUI.Text = countKarton.ToString("0000000")
            LblJmlKartonDB.Text = countKarton2.ToString("0000000")
        ElseIf countKarton >= 10 And countKarton < 100 Then
            LblJmlKartonUI.Text = countKarton.ToString("0000000")
            LblJmlKartonDB.Text = countKarton2.ToString("0000000")
        ElseIf countKarton >= 100 And countKarton < 1000 Then
            LblJmlKartonUI.Text = countKarton.ToString("0000000")
            LblJmlKartonDB.Text = countKarton2.ToString("0000000")
        ElseIf countKarton >= 1000 And countKarton < 10000 Then
            LblJmlKartonUI.Text = countKarton.ToString("0000000")
            LblJmlKartonDB.Text = countKarton2.ToString("0000000")
        ElseIf countKarton >= 10000 And countKarton < 100000 Then
            LblJmlKartonUI.Text = countKarton.ToString("0000000")
            LblJmlKartonDB.Text = countKarton2.ToString("0000000")
        ElseIf countKarton >= 100000 And countKarton < 1000000 Then
            LblJmlKartonUI.Text = countKarton.ToString("0000000")
            LblJmlKartonDB.Text = countKarton2.ToString("0000000")
        ElseIf countKarton >= 1000000 And countKarton < 10000000 Then
            LblJmlKartonUI.Text = countKarton.ToString("0000000")
            LblJmlKartonDB.Text = countKarton2.ToString("0000000")
        Else
            TimerToScan.Stop()
            alertLimit = MsgBox("Kode Karton Sudah Mencapai Batas", MsgBoxStyle.Exclamation, "Peringatan")
            If alertLimit = vbOK And TimerToScan.Enabled = False Then
                BtnStop.PerformClick()
                Exit Sub
            End If
        End If
    End Sub

    Sub SettingPrint()
        Call VidPidDevice()
        agregasiReport.Refresh()
        agregasiReport.PrintOptions.PrinterName = device_name
        agregasiReport.PrintToPrinter(0, False, 0, 0)
    End Sub

    Sub InsertKodeKemas()
        If responseData.Length = 51 Then
            'local
            Call AgregasiConnection()
            simpan = "INSERT INTO agregasi_line (id_agregasi, kode_kemas) 
                      VALUES (0, '" & responseData & "')"
            cmd = New MySqlCommand(simpan)
            cmd.Connection = conn
            cmd.ExecuteNonQuery()
            Call isiGrid()

            cmd.Dispose()
            conn.Close()

            'server 192.168.2.194
            Call AgregasiServerConnection()
            simpanServer = "insert into agregasi_line(id_agregasi, kode_kemas, is_active, is_sample, is_sold) 
                            values(0, '" & responseData & "', 'TRUE', 'FALSE', 'FALSE')"
            cmd2 = New MySqlCommand(simpanServer)
            cmd2.Connection = conn
            cmd2.ExecuteNonQuery()
            conn.Close()
        End If
    End Sub

    Sub UpdateKodeKarton()
        'server get username
        Call AgregasiServerConnection()
        cmd = New MySqlCommand("select id from user 
                                where id='" & Login.UserID & "'",
                               conn)
        Dim username As String = cmd.ExecuteScalar

        Call AgregasiServerConnection()
        cmd4 = New MySqlCommand("SELECT kode_produk FROM generate_kode
                                WHERE kode_generate = substring('" & responseData & "',1,48)
                                and rand_char = substring('" & responseData & "',49,3)",
                               conn)
        kodeProduk = cmd4.ExecuteScalar
        conn.Close()

        'local
        Call AgregasiConnection()
        simpan = "insert into agregasi_header(
                    kode_karton, rand_char, kode_produk,
                    no_nie, no_batch, expired_date, 
                    isi, status_print, created_by)
                  values(
                         concat(left('" & responseData & "', 42),'KT',@karton), 
                         CONCAT( 
                                CHAR( FLOOR(65 + (RAND() * 25))), 
                                CHAR( FLOOR(65 + (RAND() * 25))), 
                                CHAR( FLOOR(65 + (RAND() * 25))) 
                                ),
                         '" & kodeProduk & "',
                         substring('" & responseData & "',5,15),
                         substring('" & responseData & "',24,5), 
                         substring('" & responseData & "',33,6),
                         @isi, 
                         @status_print,
                         " & username & "
                        )"
        ubah = "update agregasi_line 
                set id_agregasi = (select id from agregasi_header order by id desc limit 1), status_print = 'TRUE'
                where id_agregasi = 0 and status_print = 'FALSE'"
        cmd = conn.CreateCommand

        With cmd
            .CommandText = simpan
            .Connection = conn
            .Parameters.Add("karton", MySqlDbType.String).Value = LblJmlKartonDB.Text
            .Parameters.Add("isi", MySqlDbType.String).Value = LblJmlKemasUI.Text
            .Parameters.Add("status_print", MySqlDbType.String).Value = "TRUE"
            .ExecuteNonQuery()
        End With

        With cmd
            .CommandText = ubah
            .Connection = conn
            .ExecuteNonQuery()
        End With
        cmd.Dispose()
        'conn.Close()

        'Call AgregasiConnection()
        'Dim queryProduk As String = "select substring(kode_kemas,5,15) from agregasi_line 
        '                            where substring(kode_kemas,5,15) = substring('" & responseData & "',5,15)
        '                            order by id desc limit 1"
        'cmd3 = New MySqlCommand(queryProduk, conn)
        'nieProduk = cmd3.ExecuteScalar
        cmd.Dispose()
        'cmd3.Dispose()
        'conn.Close()

        Call OracleConnection()
        cmdOracle = connOracle.CreateCommand
        cmdOracle.CommandText = "SELECT MSIB.PRIMARY_UNIT_OF_MEASURE
                                 ,CASE WHEN UPPER(MSIB.DESCRIPTION) LIKE '%AMPLOP%'
                                      THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'AMPLOP')-3,9))
                                      WHEN UPPER(MSIB.DESCRIPTION) LIKE '%BLISTER%' AND MSIB.SEGMENT1 != 'FGTAPRE233'
                                      THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'BLISTER')-3,10))
                                      WHEN UPPER(MSIB.DESCRIPTION) LIKE '%STRIP%' AND MSIB.SEGMENT1 != 'FGTFFLO325'
                                      THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'STRIP')-3,8))
                                      WHEN UPPER(MSIB.DESCRIPTION) LIKE '%TUBE%' OR UPPER(MSIB.DESCRIPTION) LIKE '%BOTOL%'
                                      THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(MSIB.DESCRIPTION,'@')+1))
                                      WHEN MSIB.SEGMENT1 = 'FGTFFLO325' THEN '10 strip'
                                      WHEN MSIB.SEGMENT1 = 'FGTAPRE233' THEN '10 blister'
                                      ELSE NULL
                                END KEMASAN
                                 FROM MTL_SYSTEM_ITEMS_B MSIB, MTL_UOM_CONVERSIONS MUC
                                WHERE 1=1
                                AND MUC.INVENTORY_ITEM_ID = MSIB.INVENTORY_ITEM_ID
                                AND MSIB.ORGANIZATION_ID = 83
                                AND SUBSTR(MSIB.SEGMENT1,1,2) in ('FG','BP')
                                AND MUC.UOM_CODE = 'CRT'
                                AND SUBSTR(MSIB.SEGMENT1,3) = :SEGMENT1"
        cmdOracle.CommandType = CommandType.Text
        Dim produkOracle As OracleParameter
        produkOracle = New OracleParameter("SEGMENT1", kodeProduk)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(produkOracle)
        rdOracle = cmdOracle.ExecuteReader
        If rdOracle.Read() Then
            jenisKemasan = rdOracle.Item("PRIMARY_UNIT_OF_MEASURE").ToString
            isiTiapKemasan = rdOracle.Item("KEMASAN").ToString
        End If
        rdOracle.Close()
        rdOracle.Dispose()
        connOracle.Close()

        'Call AgregasiConnection()
        Dim queryGetRandChar As String = "select rand_char from agregasi_header 
                                              where substring(kode_karton,1,42) = (select substring(kode_kemas,1,42) from agregasi_line order by id DESC limit 1)  
                                              order by id desc limit 1"
        cmd2 = New MySqlCommand(queryGetRandChar, conn)
        Dim randChar As String = cmd2.ExecuteScalar
        cmd2.Dispose()
        conn.Close()

        Call AgregasiServerConnection()
        simpanServer = "insert into agregasi_header(kode_karton, rand_char, kode_produk, no_nie, no_batch, expired_date, isi, created_at, created_by, updated_at, updated_by) 
                        values(
                                concat(left('" & responseData & "', 42),'KT',@p4), 
                                '" & randChar & "', 
                                '" & kodeProduk & "', 
                                substring('" & responseData & "',5,15), 
                                substring('" & responseData & "',24,5), 
                                substring('" & responseData & "',33,6),
                                CONCAT(@p5, CONCAT(' ', CONCAT('" & jenisKemasan & "', CONCAT(' @ ', '" & isiTiapKemasan & "')))), 
                                now(),
                                '" & Login.UserID & "',
                                now(),
                                '" & Login.UserID & "'
                              )"
        ubahServer = "update agregasi_line 
                      set id_agregasi =  (select id from agregasi_header order by id desc limit 1)
                      where id_agregasi = 0"
        cmd2 = conn.CreateCommand
        With cmd2
            .CommandText = simpanServer
            .Connection = conn
            .Parameters.Add("p4", MySqlDbType.String).Value = LblJmlKartonDB.Text
            .Parameters.Add("p5", MySqlDbType.Int32).Value = LblJmlKemasUI.Text
            .ExecuteNonQuery()
        End With

        With cmd2
            .CommandText = ubahServer
            .Connection = conn
            .ExecuteNonQuery()
        End With
        conn.Close()

        Call VidPidDevice()
        PrintDocument1.PrinterSettings.PrinterName = device_name
        PrintDocument1.Print()
    End Sub

    Sub UpdateKodeKartonManual()
        'server get username
        Call AgregasiServerConnection()
        cmd = New MySqlCommand("select id from user 
                                where id='" & Login.UserID & "'",
                               conn)
        Dim username As String = cmd.ExecuteScalar

        Call AgregasiServerConnection()
        cmd4 = New MySqlCommand("SELECT kode_produk FROM generate_kode
                                WHERE kode_generate = substring('" & responseData & "',1,48)
                                and rand_char = substring('" & responseData & "',49,3)",
                               conn)
        kodeProduk = cmd4.ExecuteScalar
        cmd4.Dispose()
        cmd.Dispose()
        conn.Close()

        ''get PRIMARY_UNIT_OF_MEASURE oracle
        'Call ProductManagementServerConnection()
        'Dim queryKodeProduk As String = "select B.kd_produk from 
        '                                qr_code.agregasi_line A inner join ap_product_management_2.tbl_nie B 
        '                                on substring(A.kode_kemas,5,15) = B.nomor_nie inner join ap_product_management_2.tbl_produk C 
        '                                on B.kd_produk = C.kd_produk order by A.id 
        '                                desc limit 1"
        'cmd3 = New MySqlCommand(queryKodeProduk, conn)
        'kodeProduk = cmd3.ExecuteScalar
        'cmd3.Dispose()
        'conn.Close()

        'Call AgregasiConnection()
        'Dim queryProduk As String = "select substring(kode_kemas,5,15) from agregasi_line 
        '                            where substring(kode_kemas,5,15) = substring('" & responseData & "',5,15)
        '                            order by id desc limit 1"
        'cmd3 = New MySqlCommand(queryProduk, conn)
        'nieProduk = cmd3.ExecuteScalar
        'cmd3.Dispose()
        'conn.Close()

        Call OracleConnection()
        cmdOracle = connOracle.CreateCommand
        cmdOracle.CommandText = "SELECT MSIB.PRIMARY_UNIT_OF_MEASURE
                                ,CASE WHEN UPPER(MSIB.DESCRIPTION) LIKE '%AMPLOP%'
                                      THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'AMPLOP')-3,9))
                                      WHEN UPPER(MSIB.DESCRIPTION) LIKE '%BLISTER%' AND MSIB.SEGMENT1 != 'FGTAPRE233'
                                      THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'BLISTER')-3,10))
                                      WHEN UPPER(MSIB.DESCRIPTION) LIKE '%STRIP%' AND MSIB.SEGMENT1 != 'FGTFFLO325'
                                      THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'STRIP')-3,8))
                                      WHEN UPPER(MSIB.DESCRIPTION) LIKE '%TUBE%' OR UPPER(MSIB.DESCRIPTION) LIKE '%BOTOL%'
                                      THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(MSIB.DESCRIPTION,'@')+1))
                                      WHEN MSIB.SEGMENT1 = 'FGTFFLO325' THEN '10 strip'
                                      WHEN MSIB.SEGMENT1 = 'FGTAPRE233' THEN '10 blister'
                                      ELSE NULL
                                  END KEMASAN
                                FROM MTL_SYSTEM_ITEMS_B MSIB, MTL_UOM_CONVERSIONS MUC
                                WHERE 1=1
                                AND MUC.INVENTORY_ITEM_ID = MSIB.INVENTORY_ITEM_ID
                                AND MSIB.ORGANIZATION_ID = 83
                                AND SUBSTR(MSIB.SEGMENT1,1,2) in ('FG','BP')
                                AND MUC.UOM_CODE = 'CRT'
                                AND SUBSTR(MSIB.SEGMENT1,3) = :SEGMENT1"
        cmdOracle.CommandType = CommandType.Text
        Dim produkOracle As OracleParameter
        produkOracle = New OracleParameter("SEGMENT1", kodeProduk)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(produkOracle)
        rdOracle = cmdOracle.ExecuteReader

        If rdOracle.Read() Then
            jenisKemasan = rdOracle.Item("PRIMARY_UNIT_OF_MEASURE").ToString
            isiTiapKemasan = rdOracle.Item("KEMASAN").ToString
        End If
        rdOracle.Close()
        rdOracle.Dispose()
        connOracle.Close()

        'local
        Call AgregasiConnection()
        cmdCount5 = New MySqlCommand(
                    "select cast(substring(kode_karton,45,7) as unsigned integer) as new from agregasi_header 
                     where substring(kode_karton,1,42) = (select substring(kode_kemas,1,42) 
                     from agregasi_line order by id DESC limit 1) 
                     order by cast(substring(kode_karton,45,7) as unsigned integer) desc limit 1",
                conn)
        Dim countKodeKartonSecond = cmdCount5.ExecuteScalar()
        countKarton = CInt(countKodeKartonSecond) + 1
        Call CheckLimitKarton()

        simpan = "insert into agregasi_header(
                    kode_karton, rand_char, kode_produk, 
                    no_nie, no_batch, expired_date, 
                    isi, status_print, created_by)
                  values(
                         concat(left('" & responseData & "', 42),'KT',@karton), 
                         CONCAT( 
                                CHAR( FLOOR(65 + (RAND() * 25))), 
                                CHAR( FLOOR(65 + (RAND() * 25))), 
                                CHAR( FLOOR(65 + (RAND() * 25))) 
                                ),
                         '" & kodeProduk & "', 
                         substring('" & responseData & "',5,15),
                         substring('" & responseData & "',24,5), 
                         substring('" & responseData & "',33,6),
                         @isi, 
                         @status_print,
                         " & username & "
                        )"
        'ubah = "update agregasi_line 
        '        set id_agregasi = (select id from agregasi_header order by id desc limit 1), status_print = 'TRUE'
        '        where id_agregasi = 0 and status_print = 'FALSE'"
        ubah = "update agregasi_line 
                set id_agregasi = (select id from agregasi_header order by id desc limit 1), status_print = 'TRUE'
                where id_agregasi = 0"
        cmd2 = conn.CreateCommand
        With cmd2
            .CommandText = simpan
            .Connection = conn
            .Parameters.Add("karton", MySqlDbType.String).Value = LblJmlKartonDB.Text
            .Parameters.Add("isi", MySqlDbType.String).Value = LblJmlKemasUI.Text
            .Parameters.Add("status_print", MySqlDbType.String).Value = "TRUE"
            .ExecuteNonQuery()
        End With

        With cmd2
            .CommandText = ubah
            .Connection = conn
            .ExecuteNonQuery()
        End With
        cmd2.Dispose()
        'conn.Close()

        ''server agregasi
        'Call AgregasiServerConnection()
        'Dim queryGetKodeKemasServer As String = "select substring(kode_kemas,1,42) as new from agregasi_line 
        '                                         order by id desc limit 1"
        'cmd4 = New MySqlCommand(queryGetKodeKemasServer, conn)
        'Dim kodeKemasServer As String = cmd4.ExecuteScalar
        'cmd4.Dispose()
        'conn.Close()

        'Call AgregasiConnection()
        'Dim queryGetRandChar As String = "select rand_char from agregasi_header 
        '                                  where substring(kode_karton,1,42) = '" & kodeKemasServer & "' 
        '                                  order by id desc limit 1"
        Dim queryGetRandChar As String = "select rand_char from agregasi_header 
                                          where substring(kode_karton,1,42) = (select substring(kode_kemas,1,42) from agregasi_line order by id DESC limit 1) 
                                          order by id desc limit 1"
        cmd2 = New MySqlCommand(queryGetRandChar, conn)
        Dim randChar As String = cmd2.ExecuteScalar
        cmd2.Dispose()
        conn.Close()

        Call AgregasiServerConnection()
        cmdCount4 = New MySqlCommand(
            "select cast(substring(kode_karton,45,7) as unsigned integer) as new from agregasi_header 
             where substring(kode_karton,1,42) = (select substring(kode_kemas,1,42) 
             from agregasi_line order by id DESC limit 1) 
             order by cast(substring(kode_karton,45,7) as unsigned integer) desc limit 1",
            conn)
        Dim countKodeKartonSecond2 = cmdCount4.ExecuteScalar()
        If countCode <> Convert.ToInt32(LblBatasKemas.Text) Then
            countKartonSecond = CInt(countKodeKartonSecond2) + 1
            Call CheckLimitKarton()
        ElseIf countCode = Convert.ToInt32(LblBatasKemas.Text) Then
            countKartonSecond = CInt(countKodeKartonSecond2)
            Call CheckLimitKarton()
        End If

        simpanServer = "insert into agregasi_header(kode_karton, rand_char, kode_produk, no_nie, no_batch, expired_date, isi, created_at, created_by, updated_at, updated_by) 
                        values(
                                concat(left('" & responseData & "', 42),'KT',@p4), 
                                '" & randChar & "', 
                                '" & kodeProduk & "', 
                                substring('" & responseData & "',5,15), 
                                substring('" & responseData & "',24,5), 
                                substring('" & responseData & "',33,6),
                                CONCAT(@p5, CONCAT(' ', CONCAT('" & jenisKemasan & "', CONCAT(' @ ', '" & isiTiapKemasan & "')))), 
                                now(),
                                '" & Login.UserID & "',
                                now(), 
                                '" & Login.UserID & "'
                              )"
        ubahServer = "update agregasi_line 
                      set id_agregasi =  (select id from agregasi_header order by id desc limit 1)
                      where id_agregasi = 0"
        cmd2 = conn.CreateCommand
        With cmd2
            .CommandText = simpanServer
            .Connection = conn
            .Parameters.Add("p4", MySqlDbType.String).Value = LblJmlKartonUI.Text
            .Parameters.Add("p5", MySqlDbType.Int32).Value = LblJmlKemasUI.Text
            .ExecuteNonQuery()
        End With

        With cmd2
            .CommandText = ubahServer
            .Connection = conn
            .ExecuteNonQuery()
        End With
        conn.Close()
    End Sub

    Private Sub BtnStopManual_Click(sender As Object, e As EventArgs) Handles BtnStopManual.Click
        LblStatusColor.BackColor = Color.Red
        countKemasUI = 0

        Call AgregasiConnection()
        Dim queryCekKosong As String = "select kode_kemas from agregasi_line where status_print = 'FALSE' order by id limit 1"
        cmdCount2 = New MySqlCommand(queryCekKosong, conn)
        Dim cekKosong As String = cmdCount2.ExecuteScalar
        cmdCount2.Dispose()
        conn.Close()

        'Call ProductManagementServerConnection()
        'Dim queryKodeProduk As String = "select B.kd_produk from 
        '                                qr_code.agregasi_line A inner join ap_product_management_2.tbl_nie B 
        '                                on substring(A.kode_kemas,5,15) = B.nomor_nie inner join ap_product_management_2.tbl_produk C 
        '                                on B.kd_produk = C.kd_produk order by A.id 
        '                                desc limit 1"
        'cmd = New MySqlCommand(queryKodeProduk, conn)
        'kodeProduk = cmd.ExecuteScalar

        'Call OracleConnection()
        'cmdOracle = connOracle.CreateCommand
        'cmdOracle.CommandText = "SELECT SUBSTR(MSIB.SEGMENT1,3) KODE, MUC.CONVERSION_RATE
        '        ,MSIB.PRIMARY_UNIT_OF_MEASURE
        '        ,CASE WHEN UPPER(MSIB.DESCRIPTION) LIKE '%AMPLOP%'
        '              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'AMPLOP')-3,9))
        '              WHEN UPPER(MSIB.DESCRIPTION) LIKE '%BLISTER%' AND MSIB.SEGMENT1 != 'FGTAPRE233'
        '              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'BLISTER')-3,10))
        '              WHEN UPPER(MSIB.DESCRIPTION) LIKE '%STRIP%' AND MSIB.SEGMENT1 != 'FGTFFLO325'
        '              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'STRIP')-3,8))
        '              WHEN UPPER(MSIB.DESCRIPTION) LIKE '%TUBE%' OR UPPER(MSIB.DESCRIPTION) LIKE '%BOTOL%'
        '              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(MSIB.DESCRIPTION,'@')+1))
        '              WHEN MSIB.SEGMENT1 = 'FGTFFLO325' THEN '10 strip'
        '              WHEN MSIB.SEGMENT1 = 'FGTAPRE233' THEN '10 blister'
        '              ELSE NULL
        '          END KEMASAN
        '         FROM MTL_SYSTEM_ITEMS_B MSIB, MTL_UOM_CONVERSIONS MUC
        '        WHERE 1=1
        '        AND MUC.INVENTORY_ITEM_ID = MSIB.INVENTORY_ITEM_ID
        '        AND MSIB.ORGANIZATION_ID = 83
        '        AND SUBSTR(MSIB.SEGMENT1,1,2) in ('FG','BP')
        '        AND MUC.UOM_CODE = 'CRT'
        '        AND SUBSTR(MSIB.SEGMENT1,3) = :SEGMENT1"
        'cmdOracle.CommandType = CommandType.Text
        'Dim segment1 As OracleParameter
        'segment1 = New OracleParameter("SEGMENT1", kodeProduk)
        'cmdOracle.Parameters.Clear()
        'cmdOracle.Parameters.Add(segment1)
        'rdOracle = cmdOracle.ExecuteReader

        'Call AgregasiConnection()
        'Dim queryProduk As String = "select substring(kode_kemas,5,15) from agregasi_line 
        '                            where substring(kode_kemas,5,15) = substring('" & responseData & "',5,15)
        '                            order by id desc limit 1"
        'cmd3 = New MySqlCommand(queryProduk, conn)
        'nieProduk = cmd.ExecuteScalar
        'cmd3.Dispose()
        'conn.Close()

        'Call OracleConnection()
        'cmdOracle = connOracle.CreateCommand
        'cmdOracle.CommandText = "SELECT MSIB.PRIMARY_UNIT_OF_MEASURE
        '                        ,CASE WHEN UPPER(MSIB.DESCRIPTION) LIKE '%AMPLOP%'
        '                              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'AMPLOP')-3,9))
        '                              WHEN UPPER(MSIB.DESCRIPTION) LIKE '%BLISTER%' AND MSIB.SEGMENT1 != 'FGTAPRE233'
        '                              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'BLISTER')-3,10))
        '                              WHEN UPPER(MSIB.DESCRIPTION) LIKE '%STRIP%' AND MSIB.SEGMENT1 != 'FGTFFLO325'
        '                              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'STRIP')-3,8))
        '                              WHEN UPPER(MSIB.DESCRIPTION) LIKE '%TUBE%' OR UPPER(MSIB.DESCRIPTION) LIKE '%BOTOL%'
        '                              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(MSIB.DESCRIPTION,'@')+1))
        '                              WHEN MSIB.SEGMENT1 = 'FGTFFLO325' THEN '10 strip'
        '                              WHEN MSIB.SEGMENT1 = 'FGTAPRE233' THEN '10 blister'
        '                              ELSE NULL
        '                          END KEMASAN
        '                        FROM MTL_SYSTEM_ITEMS_B MSIB, MTL_UOM_CONVERSIONS MUC
        '                        WHERE 1=1
        '                        AND MUC.INVENTORY_ITEM_ID = MSIB.INVENTORY_ITEM_ID
        '                        AND MSIB.ORGANIZATION_ID = 83
        '                        AND SUBSTR(MSIB.SEGMENT1,1,2) in ('FG','BP')
        '                        AND MUC.UOM_CODE = 'CRT'
        '                        AND MSIB.ATTRIBUTE2 = :NO_NIE"
        'cmdOracle.CommandType = CommandType.Text
        'Dim produkOracle As OracleParameter
        'produkOracle = New OracleParameter("NO_NIE", nieProduk)
        'cmdOracle.Parameters.Clear()
        'cmdOracle.Parameters.Add(produkOracle)
        'rdOracle = cmdOracle.ExecuteReader

        'While (rdOracle.Read())
        '    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text1"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("PRIMARY_UNIT_OF_MEASURE")
        '    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text2"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("KEMASAN")
        'End While

        'cmdOracle = connOracle.CreateCommand
        'cmdOracle.CommandText = "select MSIB.ATTRIBUTE7
        '                         ,MSIB.ATTRIBUTE8
        '                         ,SUBSTR(MSIB.SEGMENT1,3) as KODE_PRODUK
        '                         ,NVL(SUBSTR(MSIB.DESCRIPTION,1,INSTR(MSIB.DESCRIPTION,'~')-1),MSIB.DESCRIPTION) as NAMA_PRODUK
        '                         ,SUBSTR(MSIB.DESCRIPTION,INSTR(MSIB.DESCRIPTION,'@', 1,LENGTH(MSIB.DESCRIPTION)- LENGTH(REPLACE(MSIB.DESCRIPTION, '@', '')) )) as SUB_ISI
        '                         from mtl_system_items_b msib
        '                         where 1=1
        '                         and msib.ORGANIZATION_ID = 83
        '                         and substr(msib.SEGMENT1,1,2) in ('FG','BP')
        '                         AND MSIB.INVENTORY_ITEM_STATUS_CODE = 'Active'
        '                         and upper(msib.DESCRIPTION) not like '%SALAH%'
        '                         AND MSIB.ATTRIBUTE2 = :NO_NIE"
        'cmdOracle.CommandType = CommandType.Text
        'Dim produkOracle2 As OracleParameter
        'produkOracle2 = New OracleParameter("NO_NIE", nieProduk)
        'cmdOracle.Parameters.Clear()
        'cmdOracle.Parameters.Add(produkOracle2)
        'rdOracle = cmdOracle.ExecuteReader

        'If rdOracle.Read Then
        'Dim temp As String = rdOracle.Item("ATTRIBUTE7").ToString
        'If Not IsDBNull(rdOracle) Then
        '    If temp = "" Then
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text22"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("NAMA_PRODUK")
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text24"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("NAMA_PRODUK")
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text16"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("KODE_PRODUK")
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text27"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("SUB_ISI")
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text7"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text10"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text4"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text21"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text14"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text23"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
        '    Else
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text22"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("NAMA_PRODUK")
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text24"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("NAMA_PRODUK")
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text16"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("KODE_PRODUK")
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text27"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("SUB_ISI")
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text7"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE7").ToString
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text14"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE8").ToString
        '        DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text15"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
        '    End If
        'End If
        'End If

        print = MsgBox("Berhenti scan kemas?", MsgBoxStyle.YesNo, "Stop")
        If print = vbYes Then
            If cekKosong = "" Then
                DataGridView1.DataSource.Rows.Clear()
            ElseIf Convert.ToInt32(LblJmlKemasUI.Text) <> Convert.ToInt32(LblBatasKemas.Text) Then
                Call UpdateKodeKartonManual()
                'Call SettingPrint()
                Call VidPidDevice()
                PrintDocument1.PrinterSettings.PrinterName = device_name
                PrintDocument1.Print()

                DataGridView1.DataSource.Rows.Clear()
            ElseIf Convert.ToInt32(LblJmlKemasUI.Text) = Convert.ToInt32(LblBatasKemas.Text) Then
                DataGridView1.DataSource.Rows.Clear()
            End If
            'rdOracle.Close()
            'rdOracle.Dispose()
            'connOracle.Close()
        Else

        End If
        TimerToScan.Stop()
        Call status()

        ProgressBar1.Value = 0
        'ComboBox1.Enabled = False
        LblPbJmlKemas.Text = "0"
        LblPbBatasKemas.Text = "0"
        LblJmlKemasUI.Text = "0"

        BtnCekKoneksi.Enabled = False
        BtnCekPrinter.Enabled = False
        LblStatusInsert.Visible = False

        LblNoBatch.Text = ""
        LblKodeProduk.Text = ""
        LblNamaProduk.Text = ""
        Label4.Visible = False
    End Sub

    'Private Sub GetIsiOracle()
    '    Call AgregasiConnection()
    '    Dim queryProduk As String = "select substring(kode_kemas,5,15) from agregasi_line 
    '                                where substring(kode_kemas,5,15) = substring('" & responseData & "',5,15)
    '                                order by id desc limit 1"
    '    cmd = New MySqlCommand(queryProduk, conn)
    '    nieProduk = cmd.ExecuteScalar

    '    Call OracleConnection()
    '    cmdOracle = connOracle.CreateCommand
    '    cmdOracle.CommandText = "SELECT SUBSTR(MSIB.SEGMENT1,3) KODE, MUC.CONVERSION_RATE
    '            ,MSIB.PRIMARY_UNIT_OF_MEASURE
    '            ,CASE WHEN UPPER(MSIB.DESCRIPTION) LIKE '%AMPLOP%'
    '                  THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'AMPLOP')-3,9))
    '                  WHEN UPPER(MSIB.DESCRIPTION) LIKE '%BLISTER%' AND MSIB.SEGMENT1 != 'FGTAPRE233'
    '                  THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'BLISTER')-3,10))
    '                  WHEN UPPER(MSIB.DESCRIPTION) LIKE '%STRIP%' AND MSIB.SEGMENT1 != 'FGTFFLO325'
    '                  THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'STRIP')-3,8))
    '                  WHEN UPPER(MSIB.DESCRIPTION) LIKE '%TUBE%' OR UPPER(MSIB.DESCRIPTION) LIKE '%BOTOL%'
    '                  THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(MSIB.DESCRIPTION,'@')+1))
    '                  WHEN MSIB.SEGMENT1 = 'FGTFFLO325' THEN '10 strip'
    '                  WHEN MSIB.SEGMENT1 = 'FGTAPRE233' THEN '10 blister'
    '                  ELSE NULL
    '              END KEMASAN
    '             FROM MTL_SYSTEM_ITEMS_B MSIB, MTL_UOM_CONVERSIONS MUC
    '            WHERE 1=1
    '            AND MUC.INVENTORY_ITEM_ID = MSIB.INVENTORY_ITEM_ID
    '            AND MSIB.ORGANIZATION_ID = 83
    '            AND SUBSTR(MSIB.SEGMENT1,1,2) in ('FG','BP')
    '            AND MUC.UOM_CODE = 'CRT'
    '            AND MSIB.ATTRIBUTE2 = :NO_NIE"
    '    cmdOracle.CommandType = CommandType.Text
    '    Dim produkOracle As OracleParameter
    '    produkOracle = New OracleParameter("NO_NIE", nieProduk)
    '    cmdOracle.Parameters.Clear()
    '    cmdOracle.Parameters.Add(produkOracle)
    '    rdOracle = cmdOracle.ExecuteReader

    '    If rdOracle.Read Then
    '        'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text1"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("PRIMARY_UNIT_OF_MEASURE")
    '        'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text2"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("KEMASAN")

    '        'agregasiReport.SetParameterValue("Text1", rdOracle.Item("PRIMARY_UNIT_OF_MEASURE"))
    '        'agregasiReport.SetParameterValue("Text2", rdOracle.Item("KEMASAN"))

    '        'txt = agregasiReport.ReportDefinition.ReportObjects("Text1")
    '        'txt.Text = rdOracle.Item("PRIMARY_UNIT_OF_MEASURE")
    '        'txt = agregasiReport.ReportDefinition.ReportObjects("Text2")
    '        'txt.Text = rdOracle.Item("KEMASAN")

    '        a1 = rdOracle.Item("KEMASAN")
    '        a2 = rdOracle.Item("PRIMARY_UNIT_OF_MEASURE")
    '    End If
    '    rdOracle.Close()
    '    rdOracle.Dispose()
    '    connOracle.Close()
    'End Sub

    Private Sub GetIsiKartonOracle()
        Call AgregasiServerConnection()
        cmd4 = New MySqlCommand("SELECT kode_produk FROM generate_kode
                                WHERE kode_generate = substring('" & responseData & "',1,48)
                                and rand_char = substring('" & responseData & "',49,3)",
                               conn)
        kodeProduk = cmd4.ExecuteScalar
        conn.Close()

        Call AgregasiConnection()
        'Dim queryProduk As String = "select substring(kode_kemas,5,15) from agregasi_line 
        '                            where substring(kode_kemas,5,15) = substring('" & responseData & "',5,15)
        '                            order by id desc limit 1"
        'cmd = New MySqlCommand(queryProduk, conn)
        'nieProduk = cmd.ExecuteScalar

        Dim queryProdukSecond As String = "select substring(kode_kemas,24,5) from agregasi_line 
                                    where substring(kode_kemas,5,15) = substring('" & responseData & "',5,15)
                                    order by id desc limit 1"
        cmd2 = New MySqlCommand(queryProdukSecond, conn)
        noBatch = cmd2.ExecuteScalar
        LblNoBatch.Text = noBatch
        Label4.Visible = True

        Call OracleConnection()
        cmdOracle = connOracle.CreateCommand
        cmdOracle.CommandText = "SELECT SUBSTR(MSIB.SEGMENT1,3) KODE, MUC.CONVERSION_RATE
                 FROM MTL_SYSTEM_ITEMS_B MSIB, MTL_UOM_CONVERSIONS MUC
                WHERE 1=1
                AND MUC.INVENTORY_ITEM_ID = MSIB.INVENTORY_ITEM_ID
                AND MSIB.ORGANIZATION_ID = 83
                AND SUBSTR(MSIB.SEGMENT1,1,2) in ('FG','BP')
                AND MUC.UOM_CODE = 'CRT'
                AND SUBSTR(MSIB.SEGMENT1,3) = :SEGMENT1"
        cmdOracle.CommandType = CommandType.Text
        Dim produkOracle As OracleParameter
        produkOracle = New OracleParameter("SEGMENT1", kodeProduk)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(produkOracle)
        rdOracle = cmdOracle.ExecuteReader

        If rdOracle.Read Then
            LblBatasKemas.Text = rdOracle.Item("CONVERSION_RATE").ToString
            LblPbBatasKemas.Text = rdOracle.Item("CONVERSION_RATE").ToString
        End If

        cmdOracle = connOracle.CreateCommand
        cmdOracle.CommandText = "select SUBSTR(MSIB.SEGMENT1,3) as KODE_PRODUK
                                 ,NVL(SUBSTR(MSIB.DESCRIPTION,1,INSTR(MSIB.DESCRIPTION,'~')-1),MSIB.DESCRIPTION) as NAMA_PRODUK
                                 from mtl_system_items_b msib
                                 where 1=1
                                 and msib.ORGANIZATION_ID = 83
                                 and substr(msib.SEGMENT1,1,2) in ('FG','BP')
                                 AND MSIB.INVENTORY_ITEM_STATUS_CODE = 'Active'
                                 and upper(msib.DESCRIPTION) not like '%SALAH%'
                                 AND SUBSTR(MSIB.SEGMENT1,3) = :SEGMENT1"
        cmdOracle.CommandType = CommandType.Text
        Dim produkOracleSecond As OracleParameter
        produkOracleSecond = New OracleParameter("SEGMENT1", kodeProduk)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(produkOracleSecond)
        rdOracle = cmdOracle.ExecuteReader

        If rdOracle.Read Then
            LblKodeProduk.Text = rdOracle.Item("KODE_PRODUK")
            LblNamaProduk.Text = rdOracle.Item("NAMA_PRODUK")
        End If

        rdOracle.Close()
        rdOracle.Dispose()
        connOracle.Close()
    End Sub

    'Private Sub GetProdukOracle()
    '    Call AgregasiConnection()
    '    Dim queryProduk As String = "select substring(kode_kemas,5,15) from agregasi_line 
    '                                where substring(kode_kemas,5,15) = substring('" & responseData & "',5,15)
    '                                order by id desc limit 1"
    '    cmd = New MySqlCommand(queryProduk, conn)
    '    nieProduk = cmd.ExecuteScalar

    '    Call OracleConnection()
    '    cmdOracle = connOracle.CreateCommand
    '    cmdOracle.CommandText = "select MSIB.ATTRIBUTE7
    '                             ,MSIB.ATTRIBUTE8
    '                             ,SUBSTR(MSIB.SEGMENT1,3) as KODE_PRODUK
    '                             ,NVL(SUBSTR(MSIB.DESCRIPTION,1,INSTR(MSIB.DESCRIPTION,'~')-1),MSIB.DESCRIPTION) as NAMA_PRODUK
    '                             ,SUBSTR(MSIB.DESCRIPTION,INSTR(MSIB.DESCRIPTION,'@', 1,LENGTH(MSIB.DESCRIPTION)- LENGTH(REPLACE(MSIB.DESCRIPTION, '@', '')) )) as SUB_ISI
    '                             from mtl_system_items_b msib
    '                             where 1=1
    '                             and msib.ORGANIZATION_ID = 83
    '                             and substr(msib.SEGMENT1,1,2) in ('FG','BP')
    '                             AND MSIB.INVENTORY_ITEM_STATUS_CODE = 'Active'
    '                             and upper(msib.DESCRIPTION) not like '%SALAH%'
    '                             AND MSIB.ATTRIBUTE2 = :NO_NIE"
    '    cmdOracle.CommandType = CommandType.Text
    '    Dim produkOracle As OracleParameter
    '    produkOracle = New OracleParameter("NO_NIE", nieProduk)
    '    cmdOracle.Parameters.Clear()
    '    cmdOracle.Parameters.Add(produkOracle)
    '    rdOracle = cmdOracle.ExecuteReader

    '    If rdOracle.Read Then
    '        Dim temp As String = rdOracle.Item("ATTRIBUTE7").ToString
    '        If Not IsDBNull(rdOracle) Then
    '            If temp = "" Then
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text22"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("NAMA_PRODUK")
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text24"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("NAMA_PRODUK")
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text16"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("KODE_PRODUK")
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text27"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("SUB_ISI")
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text7"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text10"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text4"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text21"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text14"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text23"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""

    '                'agregasiReport.SetParameterValue("Text22", rdOracle.Item("NAMA_PRODUK"))
    '                'agregasiReport.SetParameterValue("Text24", rdOracle.Item("NAMA_PRODUK"))
    '                'agregasiReport.SetParameterValue("Text16", rdOracle.Item("KODE_PRODUK"))
    '                'agregasiReport.SetParameterValue("Text27", rdOracle.Item("SUB_ISI"))
    '                'agregasiReport.SetParameterValue("Text7", rdOracle.Item(""))
    '                'agregasiReport.SetParameterValue("Text10", rdOracle.Item(""))
    '                'agregasiReport.SetParameterValue("Text4", rdOracle.Item(""))
    '                'agregasiReport.SetParameterValue("Text21", rdOracle.Item(""))
    '                'agregasiReport.SetParameterValue("Text14", rdOracle.Item(""))
    '                'agregasiReport.SetParameterValue("Text23", rdOracle.Item(""))

    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text22")
    '                'txt.Text = rdOracle.Item("NAMA_PRODUK")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text24")
    '                'txt.Text = rdOracle.Item("NAMA_PRODUK")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text16")
    '                'txt.Text = rdOracle.Item("KODE_PRODUK")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text27")
    '                'txt.Text = rdOracle.Item("SUB_ISI")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text7")
    '                'txt.Text = rdOracle.Item("")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text10")
    '                'txt.Text = rdOracle.Item("")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text4")
    '                'txt.Text = rdOracle.Item("")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text21")
    '                'txt.Text = rdOracle.Item("")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text14")
    '                'txt.Text = rdOracle.Item("")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text23")
    '                'txt.Text = rdOracle.Item("")
    '            Else
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text22"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("NAMA_PRODUK")
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text24"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("NAMA_PRODUK")
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text16"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("KODE_PRODUK")
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text27"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("SUB_ISI")
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text7"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE7").ToString
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text14"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE8").ToString
    '                'DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text15"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""

    '                'agregasiReport.SetParameterValue("Text22", rdOracle.Item("NAMA_PRODUK"))
    '                'agregasiReport.SetParameterValue("Text24", rdOracle.Item("NAMA_PRODUK"))
    '                'agregasiReport.SetParameterValue("Text16", rdOracle.Item("KODE_PRODUK"))
    '                'agregasiReport.SetParameterValue("Text27", rdOracle.Item("SUB_ISI"))
    '                'agregasiReport.SetParameterValue("Text7", rdOracle.Item("ATTRIBUTE7").ToString)
    '                'agregasiReport.SetParameterValue("Text14", rdOracle.Item("ATTRIBUTE8").ToString)
    '                'agregasiReport.SetParameterValue("Text15", rdOracle.Item(""))

    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text22")
    '                'txt.Text = rdOracle.Item("NAMA_PRODUK")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text24")
    '                'txt.Text = rdOracle.Item("NAMA_PRODUK")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text16")
    '                'txt.Text = rdOracle.Item("KODE_PRODUK")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text27")
    '                'txt.Text = rdOracle.Item("SUB_ISI")
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text7")
    '                'txt.Text = rdOracle.Item("ATTRIBUTE7").ToString
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text14")
    '                'txt.Text = rdOracle.Item("ATTRIBUTE8").ToString
    '                'txt = agregasiReport.ReportDefinition.ReportObjects("Text15")
    '                'txt.Text = rdOracle.Item("")
    '            End If
    '        End If
    '    End If
    '    rdOracle.Close()
    '    rdOracle.Dispose()
    '    connOracle.Close()
    'End Sub
    Private Sub GetProdukOracle()
        'Call AgregasiConnection()
        'Dim queryProduk As String = "select substring(kode_kemas,5,15) from agregasi_line 
        '                            where substring(kode_kemas,5,15) = substring('" & responseData & "',5,15)
        '                            order by id desc limit 1"
        'cmd = New MySqlCommand(queryProduk, conn)
        'nieProduk = cmd.ExecuteScalar
        Call AgregasiServerConnection()
        cmd = New MySqlCommand("SELECT kode_produk FROM generate_kode
                                WHERE kode_generate = substring('" & responseData & "',1,48)
                                and rand_char = substring('" & responseData & "',49,3)",
                               conn)
        kodeProdukParam = cmd.ExecuteScalar
        conn.Close()

        Call AgregasiServerConnection()
        cmd2 = New MySqlCommand("SELECT substring(kode_generate,24,5) as batch FROM generate_kode
                                    WHERE kode_generate = substring('" & responseData & "',1,48)
                                    and rand_char = substring('" & responseData & "',49,3)", conn)
        batchProdukParam = cmd2.ExecuteScalar
        conn.Close()

        Call OracleConnection()
        cmdOracle = connOracle.CreateCommand
        cmdOracle.CommandText = "SELECT 
                                    HSL.SEGMENT1
                                    ,HSL.DESCRIPTION
                                    ,HSL.KODE
                                    ,HSL.DESCITEM
                                    ,HSL.KETITEM
                                    ,HSL.LOT_NUMBER
                                    ,HSL.INVENTORY_ITEM_ID
                                    ,HSL.ISI
                                    ,HSL.DTL_UM
                                    ,HSL.EXPIRATION_DATE
                                    ,HSL.ED
                                    ,HSL.SUHU
                                    ,HSL.JMLHARI
                                    ,HSL.PIC
                                    ,HSL.TYPECETAK
                                    ,CASE WHEN HSL.PANJANG - HSL.HURUFI + HSL.HURUFM < 14 THEN 1
                                        WHEN HSL.PANJANG - HSL.HURUFI + HSL.HURUFM = 14  THEN 2
                                        WHEN HSL.PANJANG - HSL.HURUFI + HSL.HURUFM = 15 THEN 3
                                        WHEN HSL.PANJANG - HSL.HURUFI + HSL.HURUFM = 16 THEN 4
                                        WHEN HSL.PANJANG - HSL.HURUFI + HSL.HURUFM = 17 THEN 5
                                        WHEN HSL.PANJANG - HSL.HURUFI + HSL.HURUFM = 18 THEN 5
                                        WHEN HSL.PANJANG - HSL.HURUFI + HSL.HURUFM = 19 THEN 6
                                        WHEN HSL.PANJANG - HSL.HURUFI + HSL.HURUFM = 20 THEN 6
                                        WHEN HSL.PANJANG - HSL.HURUFI + HSL.HURUFM = 21 THEN 8
                                        WHEN HSL.PANJANG - HSL.HURUFI + HSL.HURUFM = 22 THEN 8
                                        ELSE 9
                                    END AS TYPEFONT
                                    ,NVL(HSL.KODELABEL, HSL.KODE || ' ' || HSL.DESCITEM) KODELABEL
                                    FROM
                                    (
                                    SELECT 
                                    DISTINCT
                                    MSIB.SEGMENT1
                                    ,MSIB.DESCRIPTION 
                                    ,MSIB.ATTRIBUTE1 KODE
                                    ,NVL(SUBSTR(MSIB.DESCRIPTION,1,INSTR(MSIB.DESCRIPTION,'~')-1),MSIB.DESCRIPTION) DESCITEM
                                    ,NVL(SUBSTR(MSIB.DESCRIPTION,INSTR(MSIB.DESCRIPTION,'~')+1),MSIB.DESCRIPTION) KETITEM
                                    ,SUBSTR(MLN.LOT_NUMBER,1,5) LOT_NUMBER
                                    ,MSIB.INVENTORY_ITEM_ID
                                    ,(
                                        SELECT NVL(MUC.CONVERSION_RATE,0) 
                                        FROM MTL_UOM_CONVERSIONS MUC
                                        WHERE MUC.INVENTORY_ITEM_ID=MSIB.INVENTORY_ITEM_ID
                                        AND MUC.UOM_CODE='CRT'
                                    )ISI
                                    --,GMD.DTL_UM
                                    ,'' DTL_UM
                                    ,MLN.EXPIRATION_DATE
                                    ,to_char(MLN.EXPIRATION_DATE, 'MM YYYY') ED
                                    ,MSIB.ATTRIBUTE7 SUHU
                                    ,MSIB.ATTRIBUTE8 JMLHARI
                                    ,IFS_FND_GENERAL_PKG.GET_ITEM_URL(MSIB.SEGMENT1, 'png') PIC
                                    ,CASE WHEN SUBSTR(MSIB.SEGMENT1,1,4) IN ('FGSY', 'BPSY', 'FGDS', 'BPDS', 'FGDR', 'BPDR')
                                    THEN '1'
                                    ELSE '2'
                                    END TYPECETAK
                                    ,LENGTH(NVL(SUBSTR(MSIB.DESCRIPTION,1,INSTR(MSIB.DESCRIPTION,'~')-1),MSIB.DESCRIPTION)) PANJANG
                                    --,CASE WHEN UPPER(NVL(SUBSTR(MSIB.DESCRIPTION,1,INSTR(MSIB.DESCRIPTION,'~')-1),MSIB.DESCRIPTION)) LIKE '%I%' THEN 1 ELSE 0 END HURUFI
                                    ,0 HURUFI
                                    ,CASE WHEN UPPER(NVL(SUBSTR(MSIB.DESCRIPTION,1,INSTR(MSIB.DESCRIPTION,'~')-1),MSIB.DESCRIPTION)) LIKE '%M%' THEN 1 
                                    WHEN UPPER(NVL(SUBSTR(MSIB.DESCRIPTION,1,INSTR(MSIB.DESCRIPTION,'~')-1),MSIB.DESCRIPTION)) LIKE '%LANSOPRAZOLE%' THEN 2
                                    ELSE 0 END HURUFM
                                    ,MSIB.ATTRIBUTE16 KODELABEL
                                    FROM GME_MATERIAL_DETAILS GMD
                                    JOIN MTL_SYSTEM_ITEMS_B MSIB2 ON MSIB2.INVENTORY_ITEM_ID = GMD.INVENTORY_ITEM_ID
                                        AND MSIB2.ORGANIZATION_ID=82
                                        AND SUBSTR(MSIB2.SEGMENT1,1,2) IN ('PP', 'MX', 'GR', 'FL')
                                    JOIN MTL_SYSTEM_ITEMS_B MSIB ON SUBSTR(MSIB.SEGMENT1,3) = SUBSTR(MSIB2.SEGMENT1,3)
                                        AND MSIB.ORGANIZATION_ID=83
                                    JOIN GME_BATCH_HEADER GBH ON GMD.BATCH_ID = GBH.BATCH_ID
                                    JOIN MTL_LOT_NUMBERS MLN ON GMD.ORGANIZATION_ID = MLN.ORGANIZATION_ID
                                        AND MLN.INVENTORY_ITEM_ID = GMD.INVENTORY_ITEM_ID
                                        AND GBH.ATTRIBUTE1 = MLN.LOT_NUMBER
                                    WHERE MSIB.ATTRIBUTE1=:P_PRODUK
                                    AND GMD.ORGANIZATION_ID=82
                                    AND 
                                    (
                                    ((:P_BATCH1)=(:P_BATCH2) AND MLN.LOT_NUMBER=(:P_BATCH1))
                                    OR 
                                    ((:P_BATCH1)!=(:P_BATCH2) AND
                                    TRIM(SUBSTR(MLN.LOT_NUMBER,1,1)||SUBSTR(MLN.LOT_NUMBER,4,2)||SUBSTR(MLN.LOT_NUMBER,2,2))
                                        BETWEEN TRIM(SUBSTR(:P_BATCH1,1,1)||SUBSTR(:P_BATCH1,4,2)||SUBSTR(:P_BATCH1,2,2))
                                        AND TRIM(SUBSTR(:P_BATCH2,1,1)||SUBSTR(:P_BATCH2,4,2)||SUBSTR(:P_BATCH2,2,2))
                                    )
                                    )
                                    )HSL
                                    ORDER BY 
                                    LOT_NUMBER"
        cmdOracle.CommandType = CommandType.Text
        Dim kodeProdukOracle, batchAwalProdukOralce, batchAkhirProdukOralce As OracleParameter
        kodeProdukOracle = New OracleParameter("P_PRODUK", kodeProdukParam)
        batchAwalProdukOralce = New OracleParameter("P_BATCH1", batchProdukParam)
        batchAkhirProdukOralce = New OracleParameter("P_BATCH2", batchProdukParam)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(kodeProdukOracle)
        cmdOracle.Parameters.Add(batchAwalProdukOralce)
        cmdOracle.Parameters.Add(batchAkhirProdukOralce)
        rdOracle = cmdOracle.ExecuteReader

        If rdOracle.Read Then
            'kemasan = rdOracle.Item("KEMASAN")
            'puom = rdOracle.Item("PRIMARY_UNIT_OF_MEASURE")
            'suhu = rdOracle.Item("ATTRIBUTE7")
            'rilis = rdOracle.Item("ATTRIBUTE8")
            'productCode = rdOracle.Item("KODE_PRODUK")
            'namaProduk = rdOracle.Item("NAMA_PRODUK")
            'subIsi = rdOracle.Item("SUB_ISI")

            desc = rdOracle.Item("DESCITEM")
            kodeLabel = rdOracle.Item("KODELABEL")
            ketItem = rdOracle.Item("KETITEM")
            suhu = rdOracle.Item("SUHU")
            rilis = rdOracle.Item("JMLHARI")
        End If

        rdOracle.Close()
        rdOracle.Dispose()
        connOracle.Close()
        'Call AgregasiServerConnection()
        'cmd = New MySqlCommand("SELECT kode_produk FROM generate_kode
        '                        WHERE kode_generate = substring('" & responseData & "',1,48)
        '                        and rand_char = substring('" & responseData & "',49,3)",
        '                       conn)
        'kodeProduk = cmd.ExecuteScalar
        'conn.Close()

        'Call OracleConnection()
        'cmdOracle = connOracle.CreateCommand
        'cmdOracle.CommandText = "SELECT SUBSTR(MSIB.SEGMENT1,3) KODE, MUC.CONVERSION_RATE
        '        ,MSIB.PRIMARY_UNIT_OF_MEASURE
        '        ,CASE WHEN UPPER(MSIB.DESCRIPTION) LIKE '%AMPLOP%'
        '              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'AMPLOP')-3,9))
        '              WHEN UPPER(MSIB.DESCRIPTION) LIKE '%BLISTER%' AND MSIB.SEGMENT1 != 'FGTAPRE233'
        '              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'BLISTER')-3,10))
        '              WHEN UPPER(MSIB.DESCRIPTION) LIKE '%STRIP%' AND MSIB.SEGMENT1 != 'FGTFFLO325'
        '              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(UPPER(MSIB.DESCRIPTION),'STRIP')-3,8))
        '              WHEN UPPER(MSIB.DESCRIPTION) LIKE '%TUBE%' OR UPPER(MSIB.DESCRIPTION) LIKE '%BOTOL%'
        '              THEN TRIM(SUBSTR(MSIB.DESCRIPTION,INSTR(MSIB.DESCRIPTION,'@')+1))
        '              WHEN MSIB.SEGMENT1 = 'FGTFFLO325' THEN '10 strip'
        '              WHEN MSIB.SEGMENT1 = 'FGTAPRE233' THEN '10 blister'
        '              ELSE NULL
        '          END KEMASAN
        '         FROM MTL_SYSTEM_ITEMS_B MSIB, MTL_UOM_CONVERSIONS MUC
        '        WHERE 1=1
        '        AND MUC.INVENTORY_ITEM_ID = MSIB.INVENTORY_ITEM_ID
        '        AND MSIB.ORGANIZATION_ID = 83
        '        AND SUBSTR(MSIB.SEGMENT1,1,2) in ('FG','BP')
        '        AND MUC.UOM_CODE = 'CRT'
        '        AND SUBSTR(MSIB.SEGMENT1,3) = :SEGMENT1"
        'cmdOracle.CommandType = CommandType.Text
        'Dim produkOracle As OracleParameter
        'produkOracle = New OracleParameter("SEGMENT1", kodeProduk)
        'cmdOracle.Parameters.Clear()
        'cmdOracle.Parameters.Add(produkOracle)
        'rdOracle = cmdOracle.ExecuteReader

        'If rdOracle.Read Then
        '    kemasan = rdOracle.Item("KEMASAN")
        '    puom = rdOracle.Item("PRIMARY_UNIT_OF_MEASURE")
        'End If

        'cmdOracle = connOracle.CreateCommand
        'cmdOracle.CommandText = "select MSIB.ATTRIBUTE7
        '                         ,MSIB.ATTRIBUTE8
        '                         ,SUBSTR(MSIB.SEGMENT1,3) as KODE_PRODUK
        '                         ,NVL(SUBSTR(MSIB.DESCRIPTION,1,INSTR(MSIB.DESCRIPTION,'~')-1),MSIB.DESCRIPTION) as NAMA_PRODUK
        '                         ,SUBSTR(MSIB.DESCRIPTION,INSTR(MSIB.DESCRIPTION,'@', 1,LENGTH(MSIB.DESCRIPTION)- LENGTH(REPLACE(MSIB.DESCRIPTION, '@', '')) )) as SUB_ISI
        '                         from mtl_system_items_b msib
        '                         where 1=1
        '                         and msib.ORGANIZATION_ID = 83
        '                         and substr(msib.SEGMENT1,1,2) in ('FG','BP')
        '                         AND MSIB.INVENTORY_ITEM_STATUS_CODE = 'Active'
        '                         and upper(msib.DESCRIPTION) not like '%SALAH%'
        '                         AND SUBSTR(MSIB.SEGMENT1,3) = :SEGMENT1"
        'cmdOracle.CommandType = CommandType.Text
        'Dim produkOracleSecond As OracleParameter
        'produkOracleSecond = New OracleParameter("SEGMENT1", kodeProduk)
        'cmdOracle.Parameters.Clear()
        'cmdOracle.Parameters.Add(produkOracleSecond)
        'rdOracle = cmdOracle.ExecuteReader

        'If rdOracle.Read Then
        '    suhu = rdOracle.Item("ATTRIBUTE7")
        '    rilis = rdOracle.Item("ATTRIBUTE8")
        '    productCode = rdOracle.Item("KODE_PRODUK")
        '    namaProduk = rdOracle.Item("NAMA_PRODUK")
        '    subIsi = rdOracle.Item("SUB_ISI")
        'End If

        'rdOracle.Close()
        'rdOracle.Dispose()
        'connOracle.Close()
    End Sub

    Sub GetProdukLocal()
        Call AgregasiConnection()
        cmd = New MySqlCommand("select isi from agregasi_header order by id desc limit 1",
                               conn)
        isi = cmd.ExecuteScalar

        cmd2 = New MySqlCommand("select concat(substring(expired_date,3,2),'  ','20',substring(expired_date,1,2)) as ed 
                                 from agregasi_header 
                                 order by id desc limit 1",
                                conn)
        ed = cmd2.ExecuteScalar

        cmd3 = New MySqlCommand("select no_batch as batch from agregasi_header order by id desc limit 1",
                                conn)
        batch = cmd3.ExecuteScalar
        conn.Close()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Call AgregasiConnection()
        cmd = New MySqlCommand("select concat(kode_karton,rand_char) 
                                from agregasi_header 
                                order by id desc limit 1",
                               conn)
        Dim query As String = cmd.ExecuteScalar
        conn.Close()
        Dim gen As New QRCodeGenerator
        Dim data = gen.CreateQrCode(query, QRCodeGenerator.ECCLevel.Q)
        Dim code As New QRCode(data)
        PictureBox1.Image = code.GetGraphic(6)
        conn.Close()

        Call GetProdukOracle()
        Call GetProdukLocal()

        Dim colorDefault As Brush = New SolidBrush(Color.Black)

        Dim font As New Font("Arial", 16, FontStyle.Regular)
        'Dim font2 As New Font("Arial", 28, FontStyle.Bold)
        Dim font3 As New Font("Arial", 7, FontStyle.Regular)
        'Dim font4 As New Font("Arial", 12, FontStyle.Regular)
        Dim font4 As New Font("Arial", 11, FontStyle.Regular)
        Dim font5 As New Font("Arial", 10, FontStyle.Regular)
        Dim font6 As New Font("Arial", 9, FontStyle.Bold)

        'ed/batch
        Dim font7 As New Font("Arial", 14, FontStyle.Regular)
        Dim font8 As New Font("Arial", 7, FontStyle.Regular)
        Dim font9 As New Font("Arial", 10, FontStyle.Bold)
        Dim font10 As New Font("Arial", 12, FontStyle.Bold)
        Dim font11 As New Font("Arial", 8, FontStyle.Regular)
        Dim font12 As New Font("Arial", 20, FontStyle.Bold)
        Dim font13 As New Font("Arial", 34, FontStyle.Bold)
        Dim font14 As New Font("Arial", 24, FontStyle.Bold)

        Dim fontX As New Font("Arial", 9, FontStyle.Bold)

        'font for description
        Dim font2 As New Font("Arial", 46, FontStyle.Bold)
        Dim font15 As New Font("Arial", 42, FontStyle.Bold)
        Dim font16 As New Font("Arial", 40, FontStyle.Bold)
        Dim font17 As New Font("Arial", 38, FontStyle.Bold)
        Dim font18 As New Font("Arial", 34, FontStyle.Bold)
        Dim font19 As New Font("Arial", 32, FontStyle.Bold)
        Dim font20 As New Font("Arial", 30, FontStyle.Bold)
        Dim font21 As New Font("Arial", 28, FontStyle.Bold)
        Dim font22 As New Font("Arial", 26, FontStyle.Bold)

        Dim pen As Pen = New Pen(Color.Black, 7)
        Dim pen2 As Pen = New Pen(Color.Black, 4)

        e.Graphics.DrawRectangle(pen, 12, 20, 775, 330)

        e.Graphics.DrawImage(PictureBox1.Image, 21, 170, 130, 130)
        e.Graphics.DrawString("BPOM RI", fontX, Brushes.Black, 58, 165)
        'e.Graphics.DrawLine(pen, 25.0F, 100.0F, 25.0F, 25.0F)

        e.Graphics.DrawRectangle(pen2, 12, 20, 618, 135)
        'nama produk besar'
        Dim CurX As Integer = 32
        Dim CurY As Integer = 30
        Dim iWidth As Integer = 615
        Dim cellRect As RectangleF
        cellRect = New RectangleF()
        cellRect.Location = New Point(CurX, CurY)
        cellRect.Size = New Size(iWidth, 90)
        Dim strF As New StringFormat
        strF.FormatFlags = StringFormatFlags.NoFontFallback
        'e.Graphics.DrawString(namaProduk, font2, colorDefault, cellRect, strF)
        'e.Graphics.DrawString(desc, font2, Brushes.Black, cellRect, strF)
        'CIPROFLOXACIN HYDROCHLORIDE KAPLET FC
        If (desc.Length < 14) Then
            e.Graphics.DrawString(desc, font2, Brushes.Black, cellRect, strF)
        ElseIf (desc.Length = 14) Then
            e.Graphics.DrawString(desc, font15, Brushes.Black, cellRect, strF)
        ElseIf (desc.Length = 15) Then
            e.Graphics.DrawString(desc, font16, Brushes.Black, cellRect, strF)
        ElseIf (desc.Length = 16) Then
            e.Graphics.DrawString(desc, font17, Brushes.Black, cellRect, strF)
        ElseIf (desc.Length >= 17 And desc.Length <= 18) Then
            e.Graphics.DrawString(desc, font18, Brushes.Black, cellRect, strF)
        ElseIf (desc.Length >= 19 And desc.Length <= 20) Then
            e.Graphics.DrawString(desc, font19, Brushes.Black, cellRect, strF)
        ElseIf (desc.Length = 21) Then
            e.Graphics.DrawString(desc, font20, Brushes.Black, cellRect, strF)
        ElseIf (desc.Length = 22) Then
            e.Graphics.DrawString(desc, font21, Brushes.Black, cellRect, strF)
        ElseIf (desc.Length > 22) Then
            e.Graphics.DrawString(desc, font22, Brushes.Black, cellRect, strF)
        End If

        'kode produk & nama kecil
        'e.Graphics.DrawRectangle(pen2, 25, 110, 85, 25)
        Dim CurX2 As Integer = 40
        Dim CurY2 As Integer = 98
        Dim iWidth2 As Integer = 500
        Dim cellRect2 As RectangleF
        cellRect2 = New RectangleF()
        cellRect2.Location = New Point(CurX2, CurY2)
        cellRect2.Size = New Size(iWidth2, CurY2)
        'e.Graphics.DrawString(productCode + " " + namaProduk, font3, Brushes.Black, cellRect2, MidLeft)
        e.Graphics.DrawString(kodeLabel, font3, Brushes.Black, cellRect2, MidLeft)

        'Isi :
        e.Graphics.DrawRectangle(pen2, 157, 155, 630, 30)
        '-------------------------------------------------
        'e.Graphics.DrawRectangle(pen2, 170, 135, 40, 27)
        Dim CurX4 As Integer = 147
        Dim CurY4 As Integer = 114
        'Dim iWidth4 As Integer = 40
        Dim iWidth4 As Integer = 360
        Dim cellRect4 As RectangleF
        cellRect4 = New RectangleF()
        cellRect4.Location = New Point(CurX4, CurY4)
        cellRect4.Size = New Size(iWidth4, CurY4)
        e.Graphics.DrawString("Isi : " + isi.ToString + " " + ketItem.ToString, font4, Brushes.Black, cellRect4, MidCenter)
        '-------------------------------------------------
        'e.Graphics.DrawRectangle(pen2, 210, 135, 50, 27)
        'Dim CurX5 As Integer = 197
        'Dim CurY5 As Integer = 114
        'Dim iWidth5 As Integer = 50
        'Dim cellRect5 As RectangleF
        'cellRect5 = New RectangleF()
        'cellRect5.Location = New Point(CurX5, CurY5)
        'cellRect5.Size = New Size(iWidth5, CurY5)
        'e.Graphics.DrawString(isi, font4, Brushes.Black, cellRect5, MidRight)
        '-------------------------------------------------
        'e.Graphics.DrawRectangle(pen2, 260, 135, 50, 27)
        'Dim CurX6 As Integer = 247
        'Dim CurY6 As Integer = 114
        ''Dim iWidth6 As Integer = 50
        'Dim iWidth6 As Integer = 220
        'Dim cellRect6 As RectangleF
        'cellRect6 = New RectangleF()
        'cellRect6.Location = New Point(CurX6, CurY6)
        'cellRect6.Size = New Size(iWidth6, CurY6)
        'e.Graphics.DrawString(ketItem, font4, Brushes.Black, cellRect6, MidLeft)
        '-------------------------------------------------
        'e.Graphics.DrawRectangle(pen2, 310, 135, 110, 27)
        'Dim CurX7 As Integer = 297
        'Dim CurY7 As Integer = 114
        'Dim iWidth7 As Integer = 110
        'Dim cellRect7 As RectangleF
        'cellRect7 = New RectangleF()
        'cellRect7.Location = New Point(CurX7, CurY7)
        'cellRect7.Size = New Size(iWidth7, CurY7)
        'e.Graphics.DrawString("," + " " + kemasan, font4, Brushes.Black, cellRect7, MidLeft)
        '-------------------------------------------------
        'e.Graphics.DrawRectangle(pen2, 420, 135, 105, 27)
        'Dim CurX8 As Integer = 407
        'Dim CurY8 As Integer = 114
        'Dim iWidth8 As Integer = 105
        'Dim cellRect8 As RectangleF
        'cellRect8 = New RectangleF()
        'cellRect8.Location = New Point(CurX8, CurY8)
        'cellRect8.Size = New Size(iWidth8, CurY8)
        'e.Graphics.DrawString(subIsi, font4, Brushes.Black, cellRect8, MidLeft)

        'no karton
        e.Graphics.DrawRectangle(pen2, 495, 155, 135, 30)
        Dim CurX9 As Integer = 498
        Dim CurY9 As Integer = 160
        Dim iWidth9 As Integer = 133
        Dim cellRect9 As RectangleF
        cellRect9 = New RectangleF()
        cellRect9.Location = New Point(CurX9, CurY9)
        cellRect9.Size = New Size(iWidth9, CurY9)
        e.Graphics.DrawString("No. Karton :", font5, Brushes.Black, cellRect9, TopLeft)

        'berat
        e.Graphics.DrawRectangle(pen2, 630, 155, 154, 30)
        Dim CurX10 As Integer = 632
        Dim CurY10 As Integer = 160
        Dim iWidth10 As Integer = 143
        Dim cellRect10 As RectangleF
        cellRect10 = New RectangleF()
        cellRect10.Location = New Point(CurX10, CurY10)
        cellRect10.Size = New Size(iWidth10, CurY10)
        e.Graphics.DrawString("Berat :", font5, Brushes.Black, cellRect10, TopLeft)
        e.Graphics.DrawString("Kg", font5, Brushes.Black, cellRect10, TopRight)

        'no batch
        e.Graphics.DrawRectangle(pen2, 495, 185, 292, 60)
        Dim CurX11 As Integer = 500
        Dim CurY11 As Integer = 144
        Dim iWidth11 As Integer = 151
        Dim cellRect11 As RectangleF
        cellRect11 = New RectangleF()
        cellRect11.Location = New Point(CurX11, CurY11)
        cellRect11.Size = New Size(iWidth11, CurY11)
        e.Graphics.DrawString("No. Batch :", font7, colorDefault, cellRect11, MidLeft)
        cellRect11.Location = New Point(620, 150)
        cellRect11.Size = New Size(170, 130)
        e.Graphics.DrawString(batch, font13, Brushes.Black, cellRect11, MidRight)

        'exp date
        e.Graphics.DrawRectangle(pen2, 495, 245, 292, 55)
        Dim CurX12 As Integer = 500
        Dim CurY12 As Integer = 182
        Dim iWidth12 As Integer = 151
        Dim cellRect12 As RectangleF
        cellRect12 = New RectangleF()
        cellRect12.Location = New Point(CurX12, CurY12)
        cellRect12.Size = New Size(iWidth12, CurY12)
        e.Graphics.DrawString("Exp. Date :", font7, colorDefault, cellRect12, MidLeft)
        cellRect12.Location = New Point(617, 188)
        cellRect12.Size = New Size(160, 170)
        e.Graphics.DrawString(ed, font14, Brushes.Black, cellRect12, MidRight)

        'perhatian
        e.Graphics.DrawRectangle(pen2, 157, 185, 338, 60)
        Dim CurX13 As Integer = 143
        Dim CurY13 As Integer = 187
        Dim iWidth13 As Integer = 355
        Dim cellRect13 As RectangleF
        cellRect13 = New RectangleF()
        cellRect13.Location = New Point(CurX13, CurY13)
        cellRect13.Size = New Size(iWidth13, CurY13)
        e.Graphics.DrawString("PERHATIAN :", font6, colorDefault, cellRect13, TopCenter)
        cellRect13.Location = New Point(143, 202)
        cellRect13.Size = New Size(355, 202)
        e.Graphics.DrawString("HARAP DITIMBANG TERLEBIH DAHULU", font8, colorDefault, cellRect13, TopCenter)
        cellRect13.Location = New Point(143, 212)
        cellRect13.Size = New Size(355, 212)
        e.Graphics.DrawString("JIKA TERDAPAT PERBEDAAN YANG SIGNIFIKAN,", font8, colorDefault, cellRect13, TopCenter)
        cellRect13.Location = New Point(143, 222)
        cellRect13.Size = New Size(355, 222)
        e.Graphics.DrawString("SEGERA INFORMASIKAN KEPADA KAMI", font8, colorDefault, cellRect13, TopCenter)
        cellRect13.Location = New Point(143, 232)
        cellRect13.Size = New Size(355, 232)
        e.Graphics.DrawString("PENGADUAN DITERIMA JIKA KARTON BELUM DI BUKA", font8, colorDefault, cellRect13, TopCenter)

        'suhu
        e.Graphics.DrawRectangle(pen2, 157, 245, 257, 55)
        Dim CurX14 As Integer = 140
        Dim CurY14 As Integer = 250
        Dim iWidth14 As Integer = 285
        Dim cellRect14 As RectangleF
        cellRect14 = New RectangleF()
        cellRect14.Location = New Point(CurX14, CurY14)
        cellRect14.Size = New Size(iWidth14, CurY14)
        e.Graphics.DrawString("SIMPAN PADA SUHU DIBAWAH", font9, colorDefault, cellRect14, TopCenter)
        cellRect14.Location = New Point(140, 266)
        cellRect14.Size = New Size(285, 244)
        e.Graphics.DrawString(suhu + "°C  DAN HINDARKAN DARI", font9, colorDefault, cellRect14, TopCenter)
        cellRect14.Location = New Point(140, 280)
        cellRect14.Size = New Size(285, 258)
        e.Graphics.DrawString("CAHAYA MATAHARI.", font9, colorDefault, cellRect14, TopCenter)

        'release
        'e.Graphics.DrawRectangle(pen2, 442, 245, 70, 55)
        Dim CurX15 As Integer = 420
        Dim CurY15 As Integer = 183
        Dim iWidth15 As Integer = 70
        Dim cellRect15 As RectangleF
        cellRect15 = New RectangleF()
        cellRect15.Location = New Point(CurX15, CurY15)
        cellRect15.Size = New Size(iWidth15, CurY15)
        e.Graphics.DrawString(rilis.ToString, font12, Brushes.Black, cellRect15, MidCenter)

        'logo & pt ifars
        e.Graphics.DrawRectangle(pen2, 12, 300, 483, 48)
        Dim CurX16 As Integer = 85
        Dim CurY16 As Integer = 217
        Dim iWidth16 As Integer = 500
        Dim cellRect16 As RectangleF
        cellRect16 = New RectangleF()
        cellRect16.Location = New Point(CurX16, CurY16)
        cellRect16.Size = New Size(iWidth16, CurY16)
        e.Graphics.DrawImage(PictureBox2.Image, 41, 307, 33, 33)
        e.Graphics.DrawString("PT IFARS PHARMACEUTICAL LABORATORIES", font10, colorDefault, cellRect16, MidLeft)

        'diperiksa oleh
        e.Graphics.DrawRectangle(pen2, 495, 300, 135, 48)
        Dim CurX17 As Integer = 495
        Dim CurY17 As Integer = 305
        Dim iWidth17 As Integer = 135
        Dim cellRect17 As RectangleF
        cellRect17 = New RectangleF()
        cellRect17.Location = New Point(CurX17, CurY17)
        cellRect17.Size = New Size(iWidth17, CurY17)
        e.Graphics.DrawString("Diperiksa oleh", font11, colorDefault, cellRect17, TopCenter)
        cellRect17.Location = New Point(495, 184)
        cellRect17.Size = New Size(135, 164)
        e.Graphics.DrawString("......................", font9, colorDefault, cellRect17, BottomCenter)

        'tanggal
        e.Graphics.DrawRectangle(pen2, 630, 300, 154, 48)
        Dim CurX18 As Integer = 633
        Dim CurY18 As Integer = 305
        Dim iWidth18 As Integer = 137
        Dim cellRect18 As RectangleF
        cellRect18 = New RectangleF()
        cellRect18.Location = New Point(CurX18, CurY18)
        cellRect18.Size = New Size(iWidth18, CurY18)
        e.Graphics.DrawString("Tanggal", font11, colorDefault, cellRect18, TopCenter)
        cellRect18.Location = New Point(638, 184)
        cellRect18.Size = New Size(137, 164)
        e.Graphics.DrawString("......................", font9, colorDefault, cellRect18, BottomCenter)
    End Sub

    Private Sub TimerDateTime_Tick(sender As Object, e As EventArgs) Handles TimerDateTime.Tick
        LabelDateTime.Text = DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss")
    End Sub

    Sub ListDevices_VID_PID(Optional sFilter As String = "")
        Try
            Dim bCompare As Boolean = IIf(iv = 0, False, True)
            Dim info As Management.ManagementObject
            Dim search As System.Management.ManagementObjectSearcher
            Dim Name As String
            search = New System.Management.ManagementObjectSearcher("SELECT * From Win32_PnPEntity")
            For Each info In search.Get()
                Name = CType(info("Caption"), String)
                Dim ID As String = CType(info("DeviceID"), String)
                If sFilter = "" OrElse InStr(ID, sFilter) Then
                    If Not bCompare Then
                        ReDim Preserve vIDs(iv)
                        vIDs(iv) = ID : iv += 1
                    Else
                        Dim pos As Int32 = Array.IndexOf(vIDs, ID)
                        If pos > -1 Then
                            vIDs(pos) = ""
                        End If
                    End If
                    If bDsp Then Console.WriteLine(Name + " " + ID)
                End If
            Next
            For i As Int32 = 0 To vIDs.Length - 1
                If vIDs(i).Length Then
                    VID_PID_device = Split(vIDs(i), "\")(1)
                    Exit For
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Function Find_Device_By_VID_PID(Device_VID_PID As String) As Boolean
        Try
            ' See if the desired device shows up in the device manager. '
            Dim info As Management.ManagementObject
            Dim search As System.Management.ManagementObjectSearcher
            search = New System.Management.ManagementObjectSearcher("SELECT * From Win32_PnPEntity")
            For Each info In search.Get()
                ' Go through each device detected.'
                Dim ID As String = CType(info("DeviceID"), String)
                If InStr(info.Path.ToString, Device_VID_PID) Then
                    Return True
                End If
            Next
        Catch ex As Exception

        End Try
        'We did not find the device we were looking for '
        Return False
    End Function

    Private Sub GetFCS()
        'This will calculate the FCS value for the communications
        Dim L As Integer
        Dim A As String
        Dim TJ As String
        L = Len(TX)
        A = 0
        For J = 1 To L
            TJ = Mid$(TX, J, 1)
            A = Asc(TJ) Xor A
        Next J
        FCS = Hex$(A)
        If Len(FCS) = 1 Then FCS = "0" + FCS
    End Sub

    Private Sub communicate()
        'This will communicate to the Omron PLC
        Dim BufferTX As String
        Dim fcs_rxd As String
        Try
            RXD = ""
            BufferTX = TX + FCS + "*" + Chr(13)
            'Send the information out the serial port
            SerialPort1.Write(BufferTX)
            'Sleep for 50 msec so the information can be sent on the port
            System.Threading.Thread.Sleep(50)
            'Set the timeout for the serial port at 100 msec
            SerialPort1.ReadTimeout = 100
            'Read up to the carriage return
            RXD = (SerialPort1.ReadTo(Chr(13)))
        Catch ex As Exception
            'If an error occurs then indicate communicate error
            RXD = "Communicate Error"
        End Try
        'Get the FCS or the returned information
        fcs_rxd = RXD.Substring(RXD.Length - 3, 2)
        If RXD.Substring(0, 1) = "@" Then
            TX = RXD.Substring(0, RXD.Length - 3)
        ElseIf RXD.Substring(2, 1) = "@" Then
            TX = RXD.Substring(2, RXD.Length - 5)
            RXD = RXD.Substring(2, RXD.Length - 1)
        End If
        'Check the FCS of the return information. if they are not the same then an error has occurred.
        Call GetFCS()
        If FCS <> fcs_rxd Then
            RXD = "Communicate Error"
        End If
    End Sub

    'monitor plc
    Private Sub Monitor()
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If
        TX = "@00SC00"
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) = "00" Then
            Console.WriteLine("PLC Monitor")
        End If
        SerialPort1.Close()
    End Sub

    'arah start
    Private Sub StartArahKarton()
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If
        TX = "@00KSCIO 010005"
        'TX = "@00KSCIO 010007"  
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) = "00" Then
            Console.Write("Running")
        End If
        SerialPort1.Close()
    End Sub

    'arah stop
    Private Sub StopArahKarton()
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If
        TX = "@00KRCIO 010005"
        'TX = "@00KSCIO 00008"
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) = "00" Then
            Console.Write("Stop")
        End If
    End Sub

    'reject start
    Private Sub StartReject()
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If
        TX = "@00KSCIO 010003"
        'TX = "@00KSCIO 010007"
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) = "00" Then
            Console.Write("Running")
        End If
        SerialPort1.Close()
    End Sub
    'reject stop
    'Private Sub StopReject()
    '    If SerialPort1.IsOpen = False Then
    '        SerialPort1.Open()
    '    End If
    '    TX = "@00KRCIO 010003"
    '    Call GetFCS()
    '    Call communicate()
    '    If RXD.Substring(5, 2) = "00" Then
    '        Console.Write("Stop")
    '    End If
    'End Sub

    Private Sub FormQRCodeOmronScanner_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            Dim dialogResult As String
            dialogResult = MessageBox.Show("Keluar dari aplikasi?", "Keluar", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dialogResult = vbYes Then
                Environment.Exit(0)
            Else
                e.Cancel = True
            End If
        Else
            e.Cancel = True
        End If
    End Sub
End Class