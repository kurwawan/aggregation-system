Imports MySql.Data.MySqlClient
Imports Oracle.ManagedDataAccess.Client

Module ModuleConnection
    Public conn As MySqlConnection
    Public rd, rd2 As MySqlDataReader
    Public da, da2 As MySqlDataAdapter
    Public cmd, cmd2, cmd3, cmd4, cmdCount, cmdCount2, cmdCount3, cmdCheck, cmdCheck2, cmdCount4, cmdCount5 As MySqlCommand
    Public ds, ds2 As DataSet
    Public simpan, ubah, hapus, simpan2, simpanServer, ubahServer As String

    Public connOracle As OracleConnection
    Public cmdOracle As OracleCommand
    Public rdOracle As OracleDataReader

    Public Sub AgregasiConnection()
        Try
            'Dim sqlConnAgregasi As String = "server=localhost;user=root;password=;database=qr_code;port=3308"
            Dim sqlConnAgregasi As String = "server=localhost;user=root;password=;database=coba_qr_code;port=3308"
            conn = New MySqlConnection(sqlConnAgregasi)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
        Catch ex As Exception
            MsgBox("Koneksi Database Agregasi Bermasalah.", MsgBoxStyle.Exclamation, "Error")
        End Try
    End Sub

    Public Sub ProductManagementConnection()
        Try
            Dim sqlConnAgregasi As String = "server=localhost;user=root;password=;database=ap_product_management_2;port=3308"
            conn = New MySqlConnection(sqlConnAgregasi)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
        Catch ex As Exception
            MsgBox("Koneksi Database Product Management Bermasalah.", MsgBoxStyle.Exclamation, "Error")
        End Try
    End Sub

    Public Sub AgregasiServerConnection()
        Try
            'Dim sqlConnAgregasiServer As String = "server=192.168.2.194;user=root;password=AdmRoot;database=qr_code"
            Dim sqlConnAgregasiServer As String = "server=192.168.2.10;user=root;password=AdmRoot;database=qr_code;port=3306"
            conn = New MySqlConnection(sqlConnAgregasiServer)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
        Catch ex As Exception
            MsgBox("Koneksi Database Server QR Code Bermasalah.", MsgBoxStyle.Exclamation, "Error")
        End Try
    End Sub

    Public Sub ProductManagementServerConnection()
        Try
            Dim sqlConnProductManagementServer As String = "server=192.168.2.194;user=root;password=AdmRoot;database=ap_product_management_2"
            conn = New MySqlConnection(sqlConnProductManagementServer)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
        Catch ex As Exception
            MsgBox("Koneksi Database Product Management Server Bermasalah.", MsgBoxStyle.Exclamation, "Error")
        End Try
    End Sub

    Public Sub OracleConnection()
        Try
            Dim sqlConnOracle As String
            'sqlConnOracle = "Data Source=192.168.2.4:1528/DEV;User Id=APPS;Password=apps;"
            sqlConnOracle = "Data Source=192.168.2.2:1521/PROD;User Id=APPS;Password=apps;"
            connOracle = New OracleConnection(sqlConnOracle)
            If connOracle.State = ConnectionState.Closed Then
                connOracle.Open()
            End If
        Catch ex As Exception
            MsgBox("Koneksi Oralce Bermasalah.", MsgBoxStyle.Exclamation, "Error")
        End Try
    End Sub

End Module
