Imports Oracle.ManagedDataAccess.Client
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.CrystalReports
Imports System.Net.Sockets
Imports System.Drawing.Printing
Imports QRCoder

Public Class FormTestingPrint
    Dim TopLeft As StringFormat = New StringFormat()
    Dim TopCenter As StringFormat = New StringFormat()
    Dim TopRight As StringFormat = New StringFormat()
    Dim MidLeft As StringFormat = New StringFormat()
    Dim MidCenter As StringFormat = New StringFormat()
    Dim MidRight As StringFormat = New StringFormat()
    Dim BottomLeft As StringFormat = New StringFormat()
    Dim BottomCenter As StringFormat = New StringFormat()
    Dim BottomRight As StringFormat = New StringFormat()

    Dim nieProduk, kemasan, puom, suhu, rilis, productCode, namaProduk, subIsi, isi, ed, batch As String
    Private Sub FormTestingPrint_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim devicePrinters As String
        For Each devicePrinters In PrinterSettings.InstalledPrinters
            ComboBox1.Items.Add(devicePrinters)
        Next
        ComboBox1.SelectedIndex = 0

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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PrintDocument1.PrinterSettings.PrinterName = ComboBox1.Text
        PrintDocument1.Print()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Call AgregasiConnection()
        'cmd = New MySqlCommand("select concat(kode_karton,rand_char) 
        '                        from agregasi_header 
        '                        order by id desc limit 1",
        '                       conn)
        'Dim query As String = cmd.ExecuteScalar
        'conn.Close()

        'Dim gen As New QRCodeGenerator
        'Dim data = gen.CreateQrCode(query, QRCodeGenerator.ECCLevel.Q)
        'Dim code As New QRCode(data)
        'PictureBox1.Image = code.GetGraphic(6)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        PictureBox1.Image = Nothing
        'PrintPreviewDialog1.PrintPreviewControl.InvalidatePreview()
    End Sub

    Private Sub GetProdukOracle()
        Call AgregasiConnection()
        Dim queryProduk As String = "select substring(kode_kemas,5,15) from agregasi_line 
                                    where substring(kode_kemas,5,15) = substring('(90)DBL9509204704A1(10)13123(17)250331(21)025005EUT',5,15)
                                    order by id desc limit 1"
        cmd = New MySqlCommand(queryProduk, conn)
        nieProduk = cmd.ExecuteScalar
        conn.Close()

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
                AND MSIB.ATTRIBUTE2 = :NO_NIE"
        cmdOracle.CommandType = CommandType.Text
        Dim produkOracle As OracleParameter
        produkOracle = New OracleParameter("NO_NIE", nieProduk)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(produkOracle)
        rdOracle = cmdOracle.ExecuteReader

        If rdOracle.Read Then
            kemasan = rdOracle.Item("KEMASAN")
            puom = rdOracle.Item("PRIMARY_UNIT_OF_MEASURE")
        End If

        cmdOracle = connOracle.CreateCommand
        cmdOracle.CommandText = "select MSIB.ATTRIBUTE7
                                 ,MSIB.ATTRIBUTE8
                                 ,SUBSTR(MSIB.SEGMENT1,3) as KODE_PRODUK
                                 ,NVL(SUBSTR(MSIB.DESCRIPTION,1,INSTR(MSIB.DESCRIPTION,'~')-1),MSIB.DESCRIPTION) as NAMA_PRODUK
                                 ,SUBSTR(MSIB.DESCRIPTION,INSTR(MSIB.DESCRIPTION,'@', 1,LENGTH(MSIB.DESCRIPTION)- LENGTH(REPLACE(MSIB.DESCRIPTION, '@', '')) )) as SUB_ISI
                                 from mtl_system_items_b msib
                                 where 1=1
                                 and msib.ORGANIZATION_ID = 83
                                 and substr(msib.SEGMENT1,1,2) in ('FG','BP')
                                 AND MSIB.INVENTORY_ITEM_STATUS_CODE = 'Active'
                                 and upper(msib.DESCRIPTION) not like '%SALAH%'
                                 AND MSIB.ATTRIBUTE2 = :NO_NIE"
        cmdOracle.CommandType = CommandType.Text
        Dim produkOracleSecond As OracleParameter
        produkOracleSecond = New OracleParameter("NO_NIE", nieProduk)
        cmdOracle.Parameters.Clear()
        cmdOracle.Parameters.Add(produkOracleSecond)
        rdOracle = cmdOracle.ExecuteReader

        If rdOracle.Read Then
            suhu = rdOracle.Item("ATTRIBUTE7")
            rilis = rdOracle.Item("ATTRIBUTE8")
            productCode = rdOracle.Item("KODE_PRODUK")
            namaProduk = rdOracle.Item("NAMA_PRODUK")
            subIsi = rdOracle.Item("SUB_ISI")
        End If

        rdOracle.Close()
        rdOracle.Dispose()
        connOracle.Close()
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

        Dim colorDefault As Brush = New SolidBrush(Color.FromArgb(64, 64, 64))

        Dim font As New Font("Arial", 16, FontStyle.Regular)
        Dim font2 As New Font("Arial", 32, FontStyle.Bold)
        Dim font3 As New Font("Arial", 7, FontStyle.Regular)
        Dim font4 As New Font("Arial", 12, FontStyle.Regular)
        Dim font5 As New Font("Arial", 10, FontStyle.Regular)
        Dim font6 As New Font("Arial", 9, FontStyle.Bold)

        'ed/batch
        Dim font7 As New Font("Arial", 20, FontStyle.Regular)

        Dim font8 As New Font("Arial", 7, FontStyle.Regular)
        Dim font9 As New Font("Arial", 11, FontStyle.Bold)
        Dim font10 As New Font("Arial", 13, FontStyle.Bold)
        Dim font11 As New Font("Arial", 8, FontStyle.Regular)
        Dim fontX As New Font("Arial", 9, FontStyle.Bold)

        Dim pen As Pen = New Pen(Color.Black, 7)
        Dim pen2 As Pen = New Pen(Color.Black, 1)
        Dim pen3 As Pen = New Pen(Color.Transparent, 1)

        e.Graphics.DrawRectangle(pen, 12, 20, 775, 310)

        e.Graphics.DrawImage(PictureBox1.Image, 21, 150, 130, 130)
        e.Graphics.DrawString("BPOM RI", fontX, Brushes.Black, 58, 145)
        'e.Graphics.DrawLine(pen, 25.0F, 100.0F, 25.0F, 25.0F)

        e.Graphics.DrawRectangle(pen2, 12, 20, 635, 115)
        'nama produk besar'
        Dim CurX As Integer = 12
        Dim CurY As Integer = 30
        Dim iWidth As Integer = 650
        Dim cellRect As RectangleF
        cellRect = New RectangleF()
        cellRect.Location = New Point(CurX, CurY)
        cellRect.Size = New Size(iWidth, CurY + 25)
        e.Graphics.DrawString(namaProduk, font2, colorDefault, cellRect, TopCenter)

        'kode produk kecil
        'e.Graphics.DrawRectangle(pen2, 25, 110, 85, 25)
        Dim CurX2 As Integer = 12
        Dim CurY2 As Integer = 83
        Dim iWidth2 As Integer = 80
        Dim cellRect2 As RectangleF
        cellRect2 = New RectangleF()
        cellRect2.Location = New Point(CurX2, CurY2)
        cellRect2.Size = New Size(iWidth2, CurY2)
        e.Graphics.DrawString(productCode, font3, Brushes.Black, cellRect2, MidRight)

        'nama produk kecil
        'e.Graphics.DrawRectangle(pen2, 110, 110, 475, 25)
        Dim CurX3 As Integer = 97
        Dim CurY3 As Integer = 83
        Dim iWidth3 As Integer = 450
        Dim cellRect3 As RectangleF
        cellRect3 = New RectangleF()
        cellRect3.Location = New Point(CurX3, CurY3)
        cellRect3.Size = New Size(iWidth3, CurY3)
        e.Graphics.DrawString(namaProduk, font3, Brushes.Black, cellRect3, MidLeft)

        'Isi :
        e.Graphics.DrawRectangle(pen2, 157, 135, 630, 30)
        '-------------------------------------------------
        'e.Graphics.DrawRectangle(pen2, 170, 135, 40, 27)
        Dim CurX4 As Integer = 157
        Dim CurY4 As Integer = 100
        Dim iWidth4 As Integer = 40
        Dim cellRect4 As RectangleF
        cellRect4 = New RectangleF()
        cellRect4.Location = New Point(CurX4, CurY4)
        cellRect4.Size = New Size(iWidth4, CurY4)
        e.Graphics.DrawString("Isi :", font4, Brushes.Black, cellRect4, MidRight)
        '-------------------------------------------------
        'e.Graphics.DrawRectangle(pen2, 210, 135, 50, 27)
        Dim CurX5 As Integer = 197
        Dim CurY5 As Integer = 100
        Dim iWidth5 As Integer = 50
        Dim cellRect5 As RectangleF
        cellRect5 = New RectangleF()
        cellRect5.Location = New Point(CurX5, CurY5)
        cellRect5.Size = New Size(iWidth5, CurY5)
        e.Graphics.DrawString(isi, font4, Brushes.Black, cellRect5, MidRight)
        '-------------------------------------------------
        'e.Graphics.DrawRectangle(pen2, 260, 135, 50, 27)
        Dim CurX6 As Integer = 247
        Dim CurY6 As Integer = 100
        Dim iWidth6 As Integer = 50
        Dim cellRect6 As RectangleF
        cellRect6 = New RectangleF()
        cellRect6.Location = New Point(CurX6, CurY6)
        cellRect6.Size = New Size(iWidth6, CurY6)
        e.Graphics.DrawString(puom, font4, Brushes.Black, cellRect6, MidLeft)
        '-------------------------------------------------
        'e.Graphics.DrawRectangle(pen2, 310, 135, 110, 27)
        Dim CurX7 As Integer = 297
        Dim CurY7 As Integer = 100
        Dim iWidth7 As Integer = 110
        Dim cellRect7 As RectangleF
        cellRect7 = New RectangleF()
        cellRect7.Location = New Point(CurX7, CurY7)
        cellRect7.Size = New Size(iWidth7, CurY7)
        e.Graphics.DrawString("," + " " + kemasan, font4, Brushes.Black, cellRect7, MidLeft)
        '-------------------------------------------------
        'e.Graphics.DrawRectangle(pen2, 420, 135, 105, 27)
        Dim CurX8 As Integer = 407
        Dim CurY8 As Integer = 100
        Dim iWidth8 As Integer = 105
        Dim cellRect8 As RectangleF
        cellRect8 = New RectangleF()
        cellRect8.Location = New Point(CurX8, CurY8)
        cellRect8.Size = New Size(iWidth8, CurY8)
        e.Graphics.DrawString(subIsi, font4, Brushes.Black, cellRect8, MidLeft)

        'no karton
        e.Graphics.DrawRectangle(pen2, 512, 135, 135, 30)
        Dim CurX9 As Integer = 515
        Dim CurY9 As Integer = 135
        Dim iWidth9 As Integer = 133
        Dim cellRect9 As RectangleF
        cellRect9 = New RectangleF()
        cellRect9.Location = New Point(CurX9, CurY9)
        cellRect9.Size = New Size(iWidth9, CurY9)
        e.Graphics.DrawString("No. Karton :", font5, Brushes.Black, cellRect9, TopLeft)

        'berat
        e.Graphics.DrawRectangle(pen2, 647, 135, 137, 30)
        Dim CurX10 As Integer = 649
        Dim CurY10 As Integer = 135
        Dim iWidth10 As Integer = 126
        Dim cellRect10 As RectangleF
        cellRect10 = New RectangleF()
        cellRect10.Location = New Point(CurX10, CurY10)
        cellRect10.Size = New Size(iWidth10, CurY10)
        e.Graphics.DrawString("Berat :", font5, Brushes.Black, cellRect10, TopLeft)
        e.Graphics.DrawString("kg", font5, Brushes.Black, cellRect10, TopRight)

        'no batch
        e.Graphics.DrawRectangle(pen2, 512, 165, 275, 60)
        Dim CurX11 As Integer = 515
        Dim CurY11 As Integer = 130
        Dim iWidth11 As Integer = 151
        Dim cellRect11 As RectangleF
        cellRect11 = New RectangleF()
        cellRect11.Location = New Point(CurX11, CurY11)
        cellRect11.Size = New Size(iWidth11, CurY11)
        e.Graphics.DrawString("No. Batch :", font7, colorDefault, cellRect11, MidLeft)
        cellRect11.Location = New Point(669, 130)
        cellRect11.Size = New Size(122, 130)
        e.Graphics.DrawString(batch, font7, colorDefault, cellRect11, MidLeft)

        'exp date
        e.Graphics.DrawRectangle(pen2, 512, 225, 275, 55)
        Dim CurX12 As Integer = 515
        Dim CurY12 As Integer = 170
        Dim iWidth12 As Integer = 151
        Dim cellRect12 As RectangleF
        cellRect12 = New RectangleF()
        cellRect12.Location = New Point(CurX12, CurY12)
        cellRect12.Size = New Size(iWidth12, CurY12)
        e.Graphics.DrawString("Exp. Date :", font7, colorDefault, cellRect12, MidLeft)
        cellRect12.Location = New Point(657, 170)
        cellRect12.Size = New Size(122, 170)
        e.Graphics.DrawString(ed, font7, colorDefault, cellRect12, MidLeft)

        'perhatian
        e.Graphics.DrawRectangle(pen2, 157, 165, 355, 60)
        Dim CurX13 As Integer = 157
        Dim CurY13 As Integer = 167
        Dim iWidth13 As Integer = 355
        Dim cellRect13 As RectangleF
        cellRect13 = New RectangleF()
        cellRect13.Location = New Point(CurX13, CurY13)
        cellRect13.Size = New Size(iWidth13, CurY13)
        e.Graphics.DrawString("PERHATIAN", font6, colorDefault, cellRect13, TopCenter)
        cellRect13.Location = New Point(157, 182)
        cellRect13.Size = New Size(355, 182)
        e.Graphics.DrawString("HARAP DITIMBANG TERLEBIH DAHULU", font8, colorDefault, cellRect13, TopCenter)
        cellRect13.Location = New Point(157, 192)
        cellRect13.Size = New Size(355, 192)
        e.Graphics.DrawString("JIKA TERDAPAT PERBEDAAN YANG SIGNIFIKAN, SEGERA", font8, colorDefault, cellRect13, TopCenter)
        cellRect13.Location = New Point(157, 202)
        cellRect13.Size = New Size(355, 202)
        e.Graphics.DrawString("INFORMASI KEPADA KAMI", font8, colorDefault, cellRect13, TopCenter)
        cellRect13.Location = New Point(157, 212)
        cellRect13.Size = New Size(355, 212)
        e.Graphics.DrawString("PENGADUAN DITERIMA JIKA KARTON BELUM DI BUKA", font8, colorDefault, cellRect13, TopCenter)

        'suhu
        e.Graphics.DrawRectangle(pen2, 157, 225, 285, 55)
        Dim CurX14 As Integer = 157
        Dim CurY14 As Integer = 228
        Dim iWidth14 As Integer = 285
        Dim cellRect14 As RectangleF
        cellRect14 = New RectangleF()
        cellRect14.Location = New Point(CurX14, CurY14)
        cellRect14.Size = New Size(iWidth14, CurY14)
        e.Graphics.DrawString("SIMPAN PADA SUHU", font9, colorDefault, cellRect14, TopCenter)
        cellRect14.Location = New Point(157, 244)
        cellRect14.Size = New Size(285, 244)
        e.Graphics.DrawString("DI BAWAH  " + suhu + "°C  DAN", font9, colorDefault, cellRect14, TopCenter)
        cellRect14.Location = New Point(157, 258)
        cellRect14.Size = New Size(285, 258)
        e.Graphics.DrawString("HINDARKAN DARI CAHAYA", font9, colorDefault, cellRect14, TopCenter)

        'release
        e.Graphics.DrawRectangle(pen2, 442, 225, 70, 55)
        Dim CurX15 As Integer = 442
        Dim CurY15 As Integer = 170
        Dim iWidth15 As Integer = 70
        Dim cellRect15 As RectangleF
        cellRect15 = New RectangleF()
        cellRect15.Location = New Point(CurX15, CurY15)
        cellRect15.Size = New Size(iWidth15, CurY15)
        e.Graphics.DrawString(rilis, font7, colorDefault, cellRect15, MidCenter)

        'logo & pt ifars
        e.Graphics.DrawRectangle(pen2, 12, 280, 500, 48)
        Dim CurX16 As Integer = 75
        Dim CurY16 As Integer = 205
        Dim iWidth16 As Integer = 500
        Dim cellRect16 As RectangleF
        cellRect16 = New RectangleF()
        cellRect16.Location = New Point(CurX16, CurY16)
        cellRect16.Size = New Size(iWidth16, CurY16)
        e.Graphics.DrawImage(PictureBox2.Image, 41, 288, 33, 33)
        e.Graphics.DrawString("PT IFARS PHARMACEUTICAL LABORATORIES", font10, colorDefault, cellRect16, MidLeft)

        'diperiksa oleh
        e.Graphics.DrawRectangle(pen2, 512, 280, 135, 48)
        Dim CurX17 As Integer = 512
        Dim CurY17 As Integer = 285
        Dim iWidth17 As Integer = 135
        Dim cellRect17 As RectangleF
        cellRect17 = New RectangleF()
        cellRect17.Location = New Point(CurX17, CurY17)
        cellRect17.Size = New Size(iWidth17, CurY17)
        e.Graphics.DrawString("Diperiksa oleh", font11, colorDefault, cellRect17, TopCenter)
        cellRect17.Location = New Point(512, 164)
        cellRect17.Size = New Size(135, 164)
        e.Graphics.DrawString("......................", font9, colorDefault, cellRect17, BottomCenter)

        'tanggal
        e.Graphics.DrawRectangle(pen2, 647, 280, 137, 48)
        Dim CurX18 As Integer = 647
        Dim CurY18 As Integer = 285
        Dim iWidth18 As Integer = 137
        Dim cellRect18 As RectangleF
        cellRect18 = New RectangleF()
        cellRect18.Location = New Point(CurX18, CurY18)
        cellRect18.Size = New Size(iWidth18, CurY18)
        e.Graphics.DrawString("Tanggal", font11, colorDefault, cellRect18, TopCenter)
        cellRect18.Location = New Point(647, 164)
        cellRect18.Size = New Size(137, 164)
        e.Graphics.DrawString("......................", font9, colorDefault, cellRect18, BottomCenter)
    End Sub

End Class