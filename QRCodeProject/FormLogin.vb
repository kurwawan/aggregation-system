Imports MySql.Data.MySqlClient
Imports BCryptor = BCrypt.Net.BCrypt

Public Class FormLogin
    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        MaximizeBox = False
        MinimizeBox = False
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.ControlBox = False
        TxtBoxPassword.UseSystemPasswordChar = True

        TxtBoxUsername.Focus()
        Me.AcceptButton = BtnLogin
    End Sub

    Private Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles BtnLogin.Click
        'server
        Call AgregasiServerConnection()
        Dim bRet As Boolean = False
        cmd = New MySqlCommand("select password_hash from user 
                                where username=@username",
                               conn)
        cmd.Parameters.Clear()
        cmd.Parameters.Add("username", MySqlDbType.String).Value = TxtBoxUsername.Text

        cmdCheck = New MySqlCommand("select id from user 
                                where username=@username",
                               conn)
        cmdCheck.Parameters.Clear()
        cmdCheck.Parameters.Add("username", MySqlDbType.String).Value = TxtBoxUsername.Text

        da = New MySqlDataAdapter(cmdCheck)
        ds = New DataSet
        da.Fill(ds, "user")

        Dim password = cmd.ExecuteScalar

        If TxtBoxUsername.Text = "" Or TxtBoxPassword.Text = "" Then
            MsgBox("User Name dan Password tidak sesuai.", MsgBoxStyle.Exclamation, "Login Gagal")
        Else
            bRet = BCryptor.Verify(TxtBoxPassword.Text, password)
            If bRet = True Then
                cmd.Dispose()
                conn.Close()
                Login.UserID = ds.Tables(0).Rows(0).Item(0)

                Me.Hide()
                TxtBoxUsername.Clear()
                TxtBoxPassword.Clear()

                FormQRCodeOmronScanner.Show()
            Else
                MsgBox("User Name dan Password tidak sesuai.", MsgBoxStyle.Exclamation, "Login Gagal")
            End If
        End If
    End Sub

    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        Dim dialogResult As String
        dialogResult = MessageBox.Show("Keluar dari aplikasi?", "Keluar", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If dialogResult = vbYes Then
            Environment.Exit(0)
        End If
    End Sub

    Private Sub FormLogin_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
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