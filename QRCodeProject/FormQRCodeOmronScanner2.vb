Imports Oracle.ManagedDataAccess.Client
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.CrystalReports
Imports System.Net.Sockets
Imports System.Drawing.Printing

Public Class FormQRCodeOmronScanner2
    Dim print As String
    Dim agregasiReport As New ReportAgregasi
    Dim countKarton As Integer = 0 ' (9999999 => Limit)
    Dim countKarton2 As Integer
    Dim countKemasUI As Integer
    Dim countKemasDB As Integer
    Dim kodeProduk, kodeProduk2 As String
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

    Private Sub FormQRCodeOmronScanner2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        MaximizeBox = False
        MinimizeBox = False
        Me.FormBorderStyle = FormBorderStyle.FixedDialog

        TimerDateTime.Start()
        LblStatusInsert.Visible = False
        BtnStop.Visible = False

        'helper
        LblJmlKemasDB.Visible = False
        Label12.Visible = False
        LblJmlKartonDB.Visible = False

        BtnStop.Enabled = False
        BtnStopManual.Enabled = False

        Dim devicePrinters As String
        For Each devicePrinters In PrinterSettings.InstalledPrinters
            ComboBox1.Items.Add(devicePrinters)
        Next
        ComboBox1.SelectedIndex = 0

        Call AgregasiConnection()
        cmdCount5 = New MySqlCommand(
            "select cast(substring(kode_karton,45,7) as unsigned integer) as new 
            from agregasi_scan order by id desc limit 1", 'agregasi_karton
            conn)
        Dim getCountKodeKarton = cmdCount5.ExecuteScalar()
        countKarton = CInt(getCountKodeKarton)
        Call CheckLimitKarton()

        cmdCount4 = New MySqlCommand(
            "select count(kode_kemas) 
            from agregasi_scan where status_print = 'FALSE'",
            conn)
        Dim getCountKodeKemas = cmdCount4.ExecuteScalar
        countKemasUI = CInt(getCountKodeKemas)
        LblJmlKemasUI.Text = countKemasUI

        Call isiGrid()
    End Sub

    Private Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnStart.Click
        Try
            LblStatusColor.BackColor = Color.Green
            Call Monitor()

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
        da = New MySqlDataAdapter("select kode_kemas from agregasi_scan     
                                   where kode_karton='0' and isi_kemas='0' and status_print='FALSE' order by id ASC",
                                  conn)
        ds = New DataSet
        da.Fill(ds, "agregasi_scan")
        DataGridView1.DataSource = ds.Tables("agregasi_scan")
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
            'Call AgregasiConnection()

            Try
                Call AgregasiConnection()
                cmdCount = New MySqlCommand(
                    "select cast(count(kode_kemas) as unsigned integer) as new from agregasi_scan",
                    conn)
                Dim countCode As Integer = CInt(cmdCount.ExecuteScalar)

                cmdCount2 = New MySqlCommand(
                    "select cast(substring(kode_karton,45,7) as unsigned integer) as karton 
                from agregasi_scan where substring(kode_kemas,1,42) = substring('" & responseData & "',1,42) 
                order by karton desc limit 1",
                    conn)
                Dim countKodeKarton = cmdCount2.ExecuteScalar()

                cmdCount5 = New MySqlCommand(
                    "select cast(substring(kode_karton,45,7) as unsigned integer) as karton 
                from agregasi_scan where substring(kode_kemas,1,42) = substring('" & responseData & "',1,42) 
                order by karton desc limit 1",
                    conn)
                Dim countKodeKartonSecond = cmdCount5.ExecuteScalar()

                'select utk cek kode karton
                cmdCheck = New MySqlCommand(
                    "select cast(substring(kode_karton,45,7) as unsigned integer) as karton 
                from agregasi_scan where substring(kode_kemas,1,42) = substring('" & responseData & "',1,42) 
                order by karton desc limit 1",
                    conn)
                rd = cmdCheck.ExecuteReader
                rd.Read()

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
                cmdCheck2 = New MySqlCommand("SELECT kode_kemas FROM agregasi_scan WHERE kode_kemas LIKE '" & responseData & "'", conn)
                rd2 = cmdCheck2.ExecuteReader
                rd2.Read()
                'compare kode kemas => insert data
                If responseData.Length <> 51 Or rd2.HasRows = True Then
                    'Call AgregasiConnection()
                    Console.Write("gagal")
                    LblStatusInsert.Text = "Gagal"
                    LblStatusInsert.ForeColor = Color.Red
                    'Call StartReject()
                    rd2.Close()
                    cmdCheck2.Dispose()
                    conn.Close()
                ElseIf responseData.Length = 51 And rd2.HasRows = False Then
                    'Call AgregasiConnection()
                    Call InsertKodeKemas() 'sudah dispose dan close

                    Console.Write("berhasil")
                    LblStatusInsert.Text = "Berhasil"
                    LblStatusInsert.ForeColor = Color.Green
                    'Call StopReject()

                    countKemasUI += 1
                    LblJmlKemasUI.Text = countKemasUI

                    Call AgregasiConnection()
                    If countCode Mod Convert.ToInt32(LblBatasKemas.Text) = (Convert.ToInt32(LblBatasKemas.Text) - 1) And countCode <> 0 Then
                        TimerToScan.Dispose()
                        TimerToScan.Stop()
                        Call UpdateKodeKarton()
                        Call GetIsiOracle()
                        Call GetSuhuReleasOracle()
                        'print
                        Call SettingPrint()
                        countKemasUI = 0
                    End If
                    Call GetIsiKartonOracle()
                    rd2.Close()
                    cmdCheck2.Dispose()
                    conn.Close()

                    TimerToScan.Start()
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
        agregasiReport.Refresh()
        agregasiReport.PrintOptions.PrinterName = ComboBox1.Text
        agregasiReport.PrintToPrinter(0, False, 0, 0)
    End Sub

    Sub InsertKodeKemas()
        Call AgregasiConnection()

        If responseData.Length = 51 Then
            simpan = "INSERT INTO agregasi_scan (kode_kemas, nie, no_batch, expired_date) 
                      VALUES ('" & responseData & "', substring('" & responseData & "',5,15), substring('" & responseData & "',24,5), substring('" & responseData & "',33,6))"
            'cmd = conn.CreateCommand
            cmd = New MySqlCommand(simpan)
            cmd.Connection = conn
            cmd.ExecuteNonQuery()
            'With cmd
            '    .CommandText = simpan
            '    .Connection = conn
            '    .Parameters.Add("p1", MySqlDbType.String).Value = responseData 'TextBox1.Text
            '    .Parameters.Add("p2", MySqlDbType.String).Value = responseData 'TextBox1.Text
            '    .Parameters.Add("p3", MySqlDbType.String).Value = responseData 'TextBox1.Text
            '    .Parameters.Add("p4", MySqlDbType.String).Value = responseData 'TextBox1.Text
            '    .ExecuteNonQuery()
            'End With
            Call isiGrid()

            cmd.Dispose()
            conn.Close()
        End If
    End Sub

    Sub UpdateKodeKarton()
        ubah = "update agregasi_scan SET kode_karton=concat(left(kode_kemas, 42),'KT',@p1), 
                    isi_kemas=@p2, 
                    status_print=@p3 
                where kode_karton='0' 
                      and status_print='FALSE'"
        simpan = "insert into agregasi_karton(
                    id_agregasi, kode_karton, rand_char, 
                    nie, no_batch, expired_date, 
                    isi, status_print, created_at
                  ) select id, kode_karton, CONCAT( 
                                CHAR( FLOOR(65 + (RAND() * 25))), 
                                CHAR( FLOOR(65 + (RAND() * 25))), 
                                CHAR( FLOOR(65 + (RAND() * 25))) ), 
                           nie, no_batch, expired_date, 
                           isi_kemas, status_print, created_at 
                  from agregasi_scan order by id DESC limit 1"

        cmd = conn.CreateCommand
        With cmd
            .CommandText = ubah
            .Connection = conn
            .Parameters.Add("p1", MySqlDbType.String).Value = LblJmlKartonDB.Text
            .Parameters.Add("p2", MySqlDbType.Int32).Value = LblJmlKemasUI.Text
            .Parameters.Add("p3", MySqlDbType.String).Value = "TRUE"
            .ExecuteNonQuery()
        End With

        With cmd
            .CommandText = simpan
            .Connection = conn
            .ExecuteNonQuery()
        End With

        cmd.Dispose()
        conn.Close()
    End Sub

    Sub UpdateKodeKartonManual()
        Call AgregasiConnection()
        cmdCount5 = New MySqlCommand(
            "select cast(substring(kode_karton,45,7) as unsigned integer) as new from agregasi_scan 
             where substring(kode_kemas,1,42) = (select substring(kode_kemas,1,42) 
             from agregasi_scan order by id DESC limit 1) 
             order by cast(substring(kode_karton,45,7) as unsigned integer) desc limit 1",
            conn)
        Dim countKodeKartonSecond = cmdCount5.ExecuteScalar()
        countKarton = CInt(countKodeKartonSecond) + 1
        Call CheckLimitKarton()

        ubah = "UPDATE agregasi_scan SET 
                       kode_karton=concat(left(kode_kemas, 42),'KT',@p1), 
                       isi_kemas=@p2, status_print=@p3 
                where kode_karton='0'
                       and status_print='FALSE'"
        simpan = "insert into agregasi_karton(
                    id_agregasi, kode_karton, rand_char, 
                    nie, no_batch, expired_date, 
                    isi, status_print, created_at
                  ) select id, kode_karton, CONCAT( 
                                   CHAR( FLOOR(65 + (RAND() * 25))), 
                                   CHAR( FLOOR(65 + (RAND() * 25))), 
                                   CHAR( FLOOR(65 + (RAND() * 25))) ), 
                    nie, no_batch, expired_date, 
                    isi_kemas, status_print, created_at 
                  from agregasi_scan order by id DESC limit 1"

        cmd = conn.CreateCommand
        With cmd
            .CommandText = ubah
            .Connection = conn
            .Parameters.Add("p1", MySqlDbType.String).Value = LblJmlKartonUI.Text
            .Parameters.Add("p2", MySqlDbType.Int32).Value = LblJmlKemasUI.Text
            .Parameters.Add("p3", MySqlDbType.String).Value = "TRUE"
            .ExecuteNonQuery()
        End With

        With cmd
            .CommandText = simpan
            .Connection = conn
            .ExecuteNonQuery()
        End With
        cmd.Dispose()
        conn.Close()
    End Sub

    Private Sub BtnStopManual_Click(sender As Object, e As EventArgs) Handles BtnStopManual.Click
        LblStatusColor.BackColor = Color.Red
        countKemasUI = 0

        Call ProductManagementConnection()
        Dim queryKodeProduk As String = "select B.kd_produk from 
                               qr_code.agregasi_scan A inner join ap_product_management_2.tbl_nie B 
                               on A.nie = B.nomor_nie inner join ap_product_management_2.tbl_produk C 
                               on B.kd_produk = C.kd_produk order by A.id 
                               desc limit 1"
        cmd = New MySqlCommand(queryKodeProduk, conn)
        kodeProduk = cmd.ExecuteScalar

        Call OracleConnection()
        cmdOracle = connOracle.CreateCommand
        cmdOracle.CommandText = "SELECT SUBSTR(MSIB.SEGMENT1,3) KODE, MUC.CONVERSION_RATE
                ,MSIB.PRIMARY_UNIT_OF_MEASURE
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
        Dim segment1 As OracleParameter
        segment1 = New OracleParameter("SEGMENT1", kodeProduk)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(segment1)
        rdOracle = cmdOracle.ExecuteReader
        While (rdOracle.Read())
            DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text1"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("PRIMARY_UNIT_OF_MEASURE")
            DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text2"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("KEMASAN")
        End While

        cmdOracle = connOracle.CreateCommand
        cmdOracle.CommandText = "select MSIB.SEGMENT1, MSIB.DESCRIPTION
                                 ,MSIB.ATTRIBUTE7
                                 ,MSIB.ATTRIBUTE8
                                 from mtl_system_items_b msib
                                 where 1=1
                                 and msib.ORGANIZATION_ID = 83
                                 and substr(msib.SEGMENT1,1,2) in ('FG','BP')
                                 AND MSIB.INVENTORY_ITEM_STATUS_CODE = 'Active'
                                 and upper(msib.DESCRIPTION) not like '%SALAH%'
                                 AND substr(msib.SEGMENT1,3) = :KD_PRODUK"
        cmdOracle.CommandType = CommandType.Text
        Dim kodeProdukOracle As OracleParameter
        kodeProdukOracle = New OracleParameter("KD_PRODUK", kodeProduk)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(kodeProdukOracle)
        rdOracle = cmdOracle.ExecuteReader
        If rdOracle.Read Then
            Dim temp As String = rdOracle.Item("ATTRIBUTE7").ToString
            If Not IsDBNull(rdOracle) Then
                If temp = "" Then
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text7"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text10"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text4"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text21"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text14"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text23"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                Else
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text7"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE7").ToString
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text14"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE8").ToString
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text15"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                End If
            End If
        End If

        print = MsgBox("Print Karton?", MsgBoxStyle.Information, "Cetak Karton")
        If print = vbOK Then
            Call UpdateKodeKartonManual()
            Call SettingPrint()

            rdOracle.Close()
            rdOracle.Dispose()
            connOracle.Close()
        End If
        TimerToScan.Stop()
        Call status()
    End Sub

    Private Sub GetIsiOracle()
        Call ProductManagementConnection()
        Dim queryKodeProduk As String = "select B.kd_produk from 
                               qr_code.agregasi_scan A inner join ap_product_management_2.tbl_nie B 
                               on A.nie = B.nomor_nie inner join ap_product_management_2.tbl_produk C 
                               on B.kd_produk = C.kd_produk order by A.id 
                               desc limit 1"
        cmd = New MySqlCommand(queryKodeProduk, conn)
        kodeProduk = cmd.ExecuteScalar

        Call OracleConnection()
        cmdOracle = connOracle.CreateCommand
        cmdOracle.CommandText = "SELECT SUBSTR(MSIB.SEGMENT1,3) KODE, MUC.CONVERSION_RATE
                ,MSIB.PRIMARY_UNIT_OF_MEASURE
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
        Dim segment1 As OracleParameter
        segment1 = New OracleParameter("SEGMENT1", kodeProduk)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(segment1)
        rdOracle = cmdOracle.ExecuteReader
        If rdOracle.Read Then
            DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text1"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("PRIMARY_UNIT_OF_MEASURE")
            DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text2"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("KEMASAN")
        End If
        rdOracle.Close()
        rdOracle.Dispose()
        connOracle.Close()
    End Sub

    Private Sub GetIsiKartonOracle()
        Call ProductManagementConnection()
        Dim queryKodeProduk As String = "select B.kd_produk from 
                               qr_code.agregasi_scan A inner join ap_product_management_2.tbl_nie B 
                               on A.nie = B.nomor_nie inner join ap_product_management_2.tbl_produk C 
                               on B.kd_produk = C.kd_produk order by A.id 
                               desc limit 1"
        cmd = New MySqlCommand(queryKodeProduk, conn)
        kodeProduk = cmd.ExecuteScalar

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
        Dim segment1 As OracleParameter
        segment1 = New OracleParameter("SEGMENT1", kodeProduk)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(segment1)
        rdOracle = cmdOracle.ExecuteReader
        If rdOracle.Read Then
            LblBatasKemas.Text = rdOracle.Item("CONVERSION_RATE").ToString
        End If
        rdOracle.Close()
        rdOracle.Dispose()
        connOracle.Close()
    End Sub

    Private Sub GetSuhuReleasOracle()
        Call ProductManagementConnection()
        Dim queryKodeProduk As String = "select B.kd_produk from 
                               qr_code.agregasi_scan A inner join ap_product_management_2.tbl_nie B 
                               on A.nie = B.nomor_nie inner join ap_product_management_2.tbl_produk C 
                               on B.kd_produk = C.kd_produk order by A.id 
                               desc limit 1"
        cmd = New MySqlCommand(queryKodeProduk, conn)
        kodeProduk2 = cmd.ExecuteScalar

        Call OracleConnection()
        cmdOracle = connOracle.CreateCommand
        cmdOracle.CommandText = "select MSIB.SEGMENT1, MSIB.DESCRIPTION
                                 ,MSIB.ATTRIBUTE7
                                 ,MSIB.ATTRIBUTE8
                                 from mtl_system_items_b msib
                                 where 1=1
                                 and msib.ORGANIZATION_ID = 83
                                 and substr(msib.SEGMENT1,1,2) in ('FG','BP')
                                 AND MSIB.INVENTORY_ITEM_STATUS_CODE = 'Active'
                                 and upper(msib.DESCRIPTION) not like '%SALAH%'
                                 AND substr(msib.SEGMENT1,3) = :KD_PRODUK"
        cmdOracle.CommandType = CommandType.Text
        Dim kodeProdukOracle As OracleParameter
        kodeProdukOracle = New OracleParameter("KD_PRODUK", kodeProduk2)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(kodeProdukOracle)
        rdOracle = cmdOracle.ExecuteReader
        If rdOracle.Read Then
            Dim temp As String = rdOracle.Item("ATTRIBUTE7").ToString
            If Not IsDBNull(rdOracle) Then
                If temp = "" Then
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text7"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text10"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text4"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text21"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text14"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text23"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                Else
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text7"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE7").ToString
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text14"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE8").ToString
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text15"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = ""
                End If
            End If
        End If
        rdOracle.Close()
        rdOracle.Dispose()
        connOracle.Close()
    End Sub

    Private Sub TimerDateTime_Tick(sender As Object, e As EventArgs) Handles TimerDateTime.Tick
        LabelDateTime.Text = DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss")
    End Sub

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
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) = "00" Then
            Console.Write("Running")
        End If
        SerialPort1.Close()
    End Sub
    'reject stop
    Private Sub StopReject()
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If
        TX = "@00KRCIO 010003"
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) = "00" Then
            Console.Write("Stop")
        End If
    End Sub

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