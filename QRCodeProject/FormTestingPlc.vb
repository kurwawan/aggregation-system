Public Class FormTestingPlc
    Public TX As String
    Public FCS As String
    Public RXD As String

    Private Sub FormTestingPlc_Load(sender As Object, e As EventArgs) Handles MyBase.Load

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

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False

        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If

        TX = "@00RD00000010"
        Call GetFCS()
        Call communicate()
        SerialPort1.Close()
        If RXD.Substring(5, 2) = "00" Then
            LblReadDm.Text = RXD.Substring(7, 4)
        End If

        'look status PLC omron and open again connection serial port
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If

        TX = "@00MS"
        Call GetFCS()
        Call communicate()

        Dim rdStatus As String
        rdStatus = TX + FCS + "*"
        If rdStatus = "@00MS00000856*" Then
            BtnMonitor.BackColor = Color.Green
            BtnProgram.BackColor = Color.Empty
            BtnRun.BackColor = Color.Empty
        End If
        If rdStatus = "@00MS00030855*" Then
            BtnMonitor.BackColor = Color.Empty
            BtnProgram.BackColor = Color.Green
            BtnRun.BackColor = Color.Empty
        End If
        If rdStatus = "@00MS00020854*" Then
            BtnMonitor.BackColor = Color.Empty
            BtnProgram.BackColor = Color.Empty
            BtnRun.BackColor = Color.Green
        End If
        SerialPort1.Close()
        Timer1.Enabled = True
    End Sub

    Private Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnStart.Click
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If
        TX = "@00KSCIO 010000"
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) = "00" Then
            LblStatus.Text = "Running"
            BtnStop.Enabled = True
            BtnStart.Enabled = False
        End If
        SerialPort1.Close()
    End Sub

    Private Sub BtnStop_Click(sender As Object, e As EventArgs) Handles BtnStop.Click
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If
        TX = "@00KRCIO 010000"
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) = "00" Then
            LblStatus.Text = "STOP"
            BtnStop.Enabled = False
            BtnStart.Enabled = True
        End If
    End Sub

    Private Sub BtnMonitor_Click(sender As Object, e As EventArgs) Handles BtnMonitor.Click
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

    Private Sub BtnProgram_Click(sender As Object, e As EventArgs) Handles BtnProgram.Click
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If
        TX = "@00SC02"
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) = "00" Then
            Console.WriteLine("PLC Program")
        End If
    End Sub

    Private Sub BtnRun_Click(sender As Object, e As EventArgs) Handles BtnRun.Click
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If
        TX = "@00SC03"
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) Then
            Console.WriteLine("PLC Running")
        End If
    End Sub

    Private Sub BtnStart2_Click(sender As Object, e As EventArgs) Handles BtnStart2.Click
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If
        TX = "@00KSCIO 010002"
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) = "00" Then
            LblStatus2.Text = "Running"
            BtnStop2.Enabled = True
            BtnStart2.Enabled = False
        End If
        SerialPort1.Close()
    End Sub

    Private Sub BtnStop2_Click(sender As Object, e As EventArgs) Handles BtnStop2.Click
        If SerialPort1.IsOpen = False Then
            SerialPort1.Open()
        End If
        TX = "@00KRCIO 010002"
        Call GetFCS()
        Call communicate()
        If RXD.Substring(5, 2) = "00" Then
            LblStatus2.Text = "STOP"
            BtnStop2.Enabled = False
            BtnStart2.Enabled = True
        End If
    End Sub
End Class