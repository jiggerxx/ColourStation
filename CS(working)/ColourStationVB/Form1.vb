Public Class Login

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Register.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username As String = loginusername.Text
        Dim password As String = loginpassword.Text
        Dim active As Integer = 1
        Dim position As String

        If (String.IsNullOrEmpty(username) Or String.IsNullOrWhiteSpace(password)) Then
            MessageBox.Show("Please fill all fields!", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else


            Dim wrapper As New EncryptDecryptVB(password)
            password = wrapper.EncryptData(password)


            Dim tandf As Boolean
            Try
                checkstate()
                dbconn.Open()

                With cmd
                    .Connection = dbconn
                    .CommandText = "SELECT * FROM csdb.user"
                    dr = cmd.ExecuteReader

                    While dr.Read
                        If username.Equals(dr.GetString("username")) And password.Equals(dr.GetString("password")) Then

                            Dim name As String = dr.Item("fname")
                            Dim mname As String = dr.Item("mname")
                            Dim lname As String = dr.Item("lname")
                            position = dr.Item("acctype")
                            MessageBox.Show("Welcome " + name + " " + mname + " " + lname + "!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            tandf = True

                            main.Button12.Text = position & " - " & name

                            main.Show()
                            Me.Hide()
                        End If
                    End While
                    If tandf = False Then
                        MessageBox.Show("Account Not Found!", "", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        loginpassword.Clear()
                        loginpassword.Focus()


                    End If

                End With

            Catch ex As Exception
                'MessageBox.Show(ex.Message + "Account Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                loginusername.Clear()
                loginpassword.Clear()
            End Try
            dbconn.Close()
            dbconn.Dispose()
        End If
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Application.Exit()
        End
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.user where acctype='Admin'"
                dr = cmd.ExecuteReader

                While dr.Read
                    LinkLabel1.Visible = False
                End While

            End With

        Catch ex As Exception
        End Try
        dbconn.Close()
        dbconn.Dispose()
    End Sub
End Class
