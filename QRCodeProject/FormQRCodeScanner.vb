Imports Oracle.ManagedDataAccess.Client
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.CrystalReports

Public Class FormQRCodeScanner
    Dim print As String
    Dim agregasiReport As New ReportAgregasi

    Dim countKarton As Integer = 0 ' (9999999 => Limit)
    Dim countKarton2 As Integer

    Dim countKemasUI As Integer
    Dim countKemasDB As Integer

    Dim kodeProduk, kodeProduk2 As String
    Protected Overrides Sub OnLocationChanged(e As EventArgs)
        Static loc As Point = Me.Location
        Me.Location = loc
    End Sub

    Private Sub FormBarcodeScanner_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToParent()
        MaximizeBox = False
        MinimizeBox = False
        Me.FormBorderStyle = FormBorderStyle.FixedDialog

        TimerDateTime.Start()
        TxtBoxBatasKemas.Focus()
        TxtBoxBatasKemas.MaxLength = 5
        TextBox1.ReadOnly = True
        LblStatusInsert.Visible = False

        'helper
        LblJmlKemasDB.Visible = False
        Label12.Visible = False
        LblJmlKartonDB.Visible = False

        BtnStop.Enabled = False
        BtnStopManual.Enabled = False

        Call AgregasiConnection()
        cmdCount5 = New MySqlCommand(
            "select cast(substring(kode_karton,45,7) as unsigned integer) as new 
            from agregasi_scan order by id desc limit 1", 'agregasi_karton
            conn)
        Dim getCountKodeKarton = cmdCount5.ExecuteScalar()
        countKarton = CInt(getCountKodeKarton)
        Call CheckLimitKarton()

        'cmdCount4 = New MySqlCommand(
        '    "select cast(substring(kode_kemas,46,7) as int) 
        '    from agregasi_scan order by kode_kemas desc limit 1 ",
        '    conn)
        'Dim getCountKodeKemas = cmdCount4.ExecuteScalar
        'countKemas = CInt(getCountKodeKemas)
        'Label11.Text = countKemas

        Call isiGrid()
        MsgBox(Login.UserID)
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        'If e.KeyChar = ChrW(13) Then
        '    MsgBox(TextBox1.Text, MsgBoxStyle.Information)
        'End If
        'TextBox1.Enabled = True
        If Me.TextBox1.Focused = False Then
            'TextBox1.Focus()
            TextBox1.Text = e.KeyChar.ToString
            TextBox1.SelectionStart = TextBox1.Text.Length
            e.Handled = True
        End If
    End Sub

    Private Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnStart.Click
        LblStatusColor.BackColor = Color.Green

        TimerToScan.Start()
        TextBox1.Focus()
        'Call StatusData()
        Call status()
    End Sub

    Private Sub BtnStop_Click(sender As Object, e As EventArgs) Handles BtnStop.Click
        LblStatusColor.BackColor = Color.Red
        TimerToScan.Stop()
        'Call StatusData()
        Call ClearTextBox()
        Call status()
    End Sub

    Sub ClearTextBox()
        TextBox1.Clear()
    End Sub

    'TODO: function showing db to datagridview
    Sub isiGrid()
        Dim i As Integer
        Dim fs As New Font(DataGridView1.ColumnHeadersDefaultCellStyle.Font.Size, 14)
        If String.IsNullOrEmpty(TxtBoxBatasKemas.Text) Then
            TxtBoxBatasKemas.Text = CInt("0")
            TxtBoxBatasKemas.Focus()
        End If
        'Dim countGrid As Integer = Convert.ToInt32(TxtBoxBatasKemas.Text) + 1

        Call AgregasiConnection()
        da = New MySqlDataAdapter("select kode_kemas from agregasi_scan 
                                   where kode_karton='0' and isi_kemas='0' and status_print='FALSE'",
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

            'If (DataGridView1.RowCount Mod countGrid = 1) Then 'kalau isi 0 error 'Attempted to divide by zero.'
            '    DataGridView1.DataSource.Clear()
            'End If
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
            TextBox1.ReadOnly = False
            LblStatus.Text = "ON"
            LblStatus.ForeColor = Color.Green

            BtnStart.Enabled = False
            BtnStop.Enabled = True
            BtnStopManual.Enabled = True
            Me.ControlBox = False
            LblStatusInsert.Visible = True
            If TxtBoxBatasKemas.Text = "" Then
                MsgBox("Batas Kemas wajib di isi.", MsgBoxStyle.Exclamation, "Cetak Karton")
                If vbOK Then
                    BtnStop.PerformClick()
                End If
            ElseIf TxtBoxBatasKemas.Text = "0" Then
                MsgBox("Batas Kemas wajib lebih dari 0.", MsgBoxStyle.Exclamation, "Cetak Karton")
                If vbOK Then
                    BtnStop.PerformClick()
                    TxtBoxBatasKemas.Clear()
                End If
            Else
                TxtBoxBatasKemas.ReadOnly = True
            End If
        ElseIf TimerToScan.Enabled = False Then
            TextBox1.ReadOnly = True
            LblStatus.Text = "OFF"
            LblStatus.ForeColor = Color.Red

            BtnStart.Enabled = True
            BtnStop.Enabled = False
            BtnStopManual.Enabled = False
            Me.ControlBox = True
            LblStatusInsert.Visible = False
            TxtBoxBatasKemas.ReadOnly = False
            TxtBoxBatasKemas.Focus()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles TimerToScan.Tick
        Call AgregasiConnection()
        cmdCount = New MySqlCommand(
            "select count(kode_kemas) from agregasi_scan 
            where kode_karton='0' and isi_kemas='0' and status_print='FALSE'",
            conn)
        Dim countCode As Integer = cmdCount.ExecuteScalar

        cmdCount2 = New MySqlCommand(
            "select cast(substring(kode_karton,45,7) as unsigned integer) as karton 
            from agregasi_scan where substring(kode_kemas,1,42) = substring('" & TextBox1.Text & "',1,42) 
            order by karton desc limit 1",
            conn)
        Dim countKodeKarton = cmdCount2.ExecuteScalar()

        cmdCount5 = New MySqlCommand(
            "select cast(substring(kode_karton,45,7) as unsigned integer) as karton 
            from agregasi_scan where substring(kode_kemas,1,42) = substring('" & TextBox1.Text & "',1,42) 
            order by karton desc limit 1",
            conn)
        Dim countKodeKartonSecond = cmdCount5.ExecuteScalar()

        cmdCount3 = New MySqlCommand(
            "select count(kode_kemas) 
            from agregasi_scan 
            where substring(kode_kemas,1,19) = substring('" & TextBox1.Text & "',1,19) ",
            conn)
        Dim countKodeKemas = cmdCount3.ExecuteScalar

        cmdCount4 = New MySqlCommand(
            "select count(kode_kemas) 
            from agregasi_scan 
            where substring(kode_kemas,1,19) = substring('" & TextBox1.Text & "',1,19) ",
            conn)
        'cmdCount4 = New MySqlCommand(
        '    "SELECT CASE 
        '                WHEN COUNT(kode_kemas) MOD 5 = 0 THEN COUNT(kode_kemas) - COUNT(kode_kemas)  
        '                WHEN COUNT(kode_kemas) MOD 5 != 0 THEN COUNT(kode_kemas)-COUNT(kode_kemas)+1
        '            END result
        '     FROM agregasi_scan
        '     WHERE substring(kode_kemas,1,19) = substring('" & TextBox1.Text & "',1,19)",
        '    conn)
        Dim countKodeKemasSecond As Integer = cmdCount4.ExecuteScalar

        'kondisi db null setelah insert data pertama => tampil kode kemas +1
        If (countCode = 0 And TextBox1.Text.Length = 48) Then
            Call InsertKodeKemas()
            LblStatusInsert.Text = "Berhasil"
            LblStatusInsert.ForeColor = Color.Green

            'countKemas = CInt(countKodeKemasSecond) + 1
            'Label11.Text = countKemas
            'countKemasUI += 1
            'LblJmlKemasUI.Text = (DataGridView1.Rows.Count - 1).ToString 'countKemasUI

            countKemasDB += 1
            LblJmlKemasDB.Text = countKemasDB
        Else
            LblStatusInsert.Text = "Gagal"
            LblStatusInsert.ForeColor = Color.Red
        End If

        'compare ada data yg sama/tidak
        Call CompareKodeKemas()

        cmdCheck = New MySqlCommand(
            "select cast(substring(kode_karton,45,7) as unsigned integer) as karton 
            from agregasi_scan where substring(kode_kemas,1,42) = substring('" & TextBox1.Text & "',1,42) 
            order by karton desc limit 1",
            conn)
        rd = cmdCheck.ExecuteReader
        rd.Read()

        For i As Integer = 0 To countCode - 1 'total baris 32 (terhitung 0-31)
            'count kemas ui & db
            'If TextBox1.Text.Length = 48 Or (countKemas = 0 And TextBox1.Text.Length = 48)
            '    'countKemas = CInt(countKodeKemasSecond) + 1
            '    'Label11.Text = countKemas
            'End If
            If TextBox1.Text.Length = 48 Or (countKemasUI = 0 And TextBox1.Text.Length = 48) Then
                countKemasUI += 1
                LblJmlKemasUI.Text = (DataGridView1.Rows.Count - 1).ToString 'countKemasUI

                LblStatusInsert.Text = "Berhasil"
                LblStatusInsert.ForeColor = Color.Green
            End If
            If countCode Mod Convert.ToInt32(TxtBoxBatasKemas.Text) = 0 And TextBox1.Text.Length = 48 Then
                countKemasUI -= 1
                LblJmlKemasUI.Text = countKemasDB
            End If
            'count kemas db
            'If countCode Mod 5 = 0 And TextBox1.Text.Length = 48 Then
            '    'count isi kemas pada setiap karton
            '    'countKemas = CInt(countKodeKemas) + 1
            '    Label8.Text = countKemas2
            'End If
            If TextBox1.Text.Length = 48 Or (countKemasUI = 0 And TextBox1.Text.Length = 48) Then
                countKemasDB += 1
                LblJmlKemasDB.Text = countKemasDB
            End If
            If countCode Mod Convert.ToInt32(TxtBoxBatasKemas.Text) = 0 And TextBox1.Text.Length = 48 Then
                countKemasDB -= 1
                LblJmlKemasDB.Text = countKemasDB
            End If

            'update karton ke ui
            If countCode = 0 And TextBox1.Text.Length = 48 Then
                countKarton = CInt(countKodeKartonSecond) + 1
                Call CheckLimitKarton()
            ElseIf TextBox1.Text.Length = 48 And countCode Mod Convert.ToInt32(TxtBoxBatasKemas.Text) = 4 Then
                countKarton = CInt(countKodeKartonSecond) + 1
                Call CheckLimitKarton()
            End If

            'update karton ke db
            If (countCode Mod Convert.ToInt32(TxtBoxBatasKemas.Text) = 4) And TextBox1.Text.Length = 48 Then
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
            ElseIf countCode Mod Convert.ToInt32(TxtBoxBatasKemas.Text) = 0 And TextBox1.Text.Length = 48 Then
                If (rd.HasRows = False) Then 'jika beda balik ke 0
                    countKarton2 = 0
                End If
            End If
            cmdCheck.Dispose()
            conn.Close()
            Call AgregasiConnection()

            'notif alert print
            If countCode Mod Convert.ToInt32(TxtBoxBatasKemas.Text) = 0 Then
                If TimerToScan.Interval = 3000 Then
                    TimerToScan.Dispose()
                    TimerToScan.Stop()
                    Call UpdateKodeKarton()
                    Call GetIsiOracle()
                    Call GetSuhuReleaseOracle()
                    print = MsgBox("Print Karton?", MsgBoxStyle.Information, "Cetak Karton")
                    If print = vbOK Then
                        Call SettingPrint()
                        If countCode Mod Convert.ToInt32(TxtBoxBatasKemas.Text) = 0 And TextBox1.Text.Length = 48 Then
                            countKemasUI = 0
                            countKemasUI += 1
                            LblJmlKemasUI.Text = (DataGridView1.Rows.Count - 1).ToString 'countKemasUI

                            countKemasDB = 0
                            countKemasDB += 1
                            LblJmlKemasDB.Text = countKemasDB
                        End If
                        'Timer keadaan stop
                        TimerToScan.Stop()
                        Exit For
                    End If
                End If
            Else
                Call InsertKodeKemas()
            End If
        Next
        'kondisi setelah print
        If TimerToScan.Enabled = False Then
            If TextBox1.Text = "" And TextBox1.Text <> "" Then
                TimerToScan.Stop()
            ElseIf TextBox1.Text <> "" And TextBox1.Text.Length = 48 Then
                TimerToScan.Stop()

                TimerToScan.Start()
                Call AgregasiConnection()
                Call InsertKodeKemas()
            End If
        End If
        TextBox1.Clear()
        'button auto stop
        If TimerToScan.Enabled = False Then
            BtnStop.PerformClick()
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
        'agregasiReport.Load()
        agregasiReport.Refresh()
        agregasiReport.PrintOptions.PrinterName = "HP LaserJet P1006"
        agregasiReport.PrintToPrinter(0, False, 0, 0)
    End Sub

    Sub InsertKodeKemas()
        If TextBox1.Text.Length = 48 Then
            simpan = "INSERT INTO agregasi_scan (kode_kemas, nie, no_batch, expired_date, created_by) 
                      VALUES (@p1, substring(@p2,5,15), substring(@p3,24,5), substring(@p4,33,6), @cb)"
            cmd = conn.CreateCommand
            With cmd
                .CommandText = simpan
                .Connection = conn
                .Parameters.Add("p1", MySqlDbType.String).Value = TextBox1.Text
                .Parameters.Add("p2", MySqlDbType.String).Value = TextBox1.Text
                .Parameters.Add("p3", MySqlDbType.String).Value = TextBox1.Text
                .Parameters.Add("p4", MySqlDbType.String).Value = TextBox1.Text
                .Parameters.Add("cb", MySqlDbType.String).Value = Login.UserID
                .ExecuteNonQuery()
            End With

            Call isiGrid()
            Call ClearTextBox()
            'LblStatusInsert.Text = "Berhasil"
            'LblStatusInsert.ForeColor = Color.Green
        End If
        'If TextBox1.Text.Length <> 48 Then
        '    Call ClearTextBox()
        '    LblStatusInsert.Text = "Gagal"
        '    LblStatusInsert.ForeColor = Color.Red
        'End If
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

    Sub CompareKodeKemas()
        cmdCount = New MySqlCommand("select count(kode_kemas) from agregasi_scan", conn)
        Dim countCode As Integer = cmdCount.ExecuteScalar

        cmdCheck = New MySqlCommand("SELECT kode_kemas FROM agregasi_scan WHERE kode_kemas LIKE '" & TextBox1.Text & "'", conn)
        rd = cmdCheck.ExecuteReader
        rd.Read()

        Dim isCompare As String
        For index As Integer = 0 To countCode - 1
            If rd.HasRows = True Then
                TimerToScan.Stop()
                isCompare = MsgBox("Kode Kemas telah tersedia", MsgBoxStyle.Exclamation, "Peringatan")
                If isCompare = vbOK Then
                    Call ClearTextBox()
                    TextBox1.Focus()
                End If
                Exit For
            End If
        Next
        cmdCheck.Dispose()
        conn.Close()

        TimerToScan.Start()
        Call AgregasiConnection()
    End Sub

    Private Sub BtnStopManual_Click(sender As Object, e As EventArgs) Handles BtnStopManual.Click
        LblStatusColor.BackColor = Color.Red

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
        '======================================================================
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
                Else
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text7"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE7").ToString
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text14"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE8").ToString
                End If
            End If
        End If

        Call UpdateKodeKartonManual()

        print = MsgBox("Print Karton?", MsgBoxStyle.Information, "Cetak Karton")
        If print = vbOK Then
            Call SettingPrint()

            rdOracle.Close()
            rdOracle.Dispose()
            connOracle.Close()
        End If
        TimerToScan.Stop()
        Call ClearTextBox()
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

    Private Sub GetSuhuReleaseOracle()
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
                Else
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text7"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE7").ToString
                    DirectCast(agregasiReport.ReportDefinition.ReportObjects("Text14"), CrystalDecisions.CrystalReports.Engine.TextObject).Text = rdOracle.Item("ATTRIBUTE8").ToString
                End If
            End If
        End If
        rdOracle.Close()
        rdOracle.Dispose()
        connOracle.Close()
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TxtBoxBatasKemas.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TimerDateTime_Tick(sender As Object, e As EventArgs) Handles TimerDateTime.Tick
        LabelDateTime.Text = DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss")
    End Sub

    Private Sub FormQRCodeScanner_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
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