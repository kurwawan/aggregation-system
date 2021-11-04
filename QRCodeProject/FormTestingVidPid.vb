Public Class FormTestingVidPid
    Dim vIDs() As String, iv As Int32 = 0
    Dim VID_PID_device = "VID_0781&PID_5581" ' Change to your's 
    Dim bDsp As Boolean = True
    Dim bAttached As Boolean = False


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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListDevices_VID_PID()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListDevices_VID_PID("USB" + Chr(92))
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Find_Device_By_VID_PID(VID_PID_device) Then
            Console.WriteLine("Connected!")
        End If
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
End Class