Imports System.Text.RegularExpressions
Public Class Register

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim doesexist As Boolean = False

        Try
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "select * from csdb.user where acctype='Admin'"
                dr = cmd.ExecuteReader

                While dr.Read
                    doesexist = True
                End While
            End With

            If doesexist = True Then
                ComboBox1.Items.Remove("Admin")
            End If

        Catch ex As Exception
        End Try
        dbconn.Close()
        dbconn.Dispose()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim employee As String = TextBox1.Text
        Dim fname As String = TextBox2.Text
        Dim mname As String = TextBox3.Text
        Dim lname As String = TextBox4.Text
        Dim position As String = ComboBox1.SelectedItem
        Dim username As String = TextBox5.Text
        Dim password As String = passwordBox.Text
        Dim repassword As String = TextBox7.Text
        Dim cipherText As String

        If (String.IsNullOrEmpty(employee) Or String.IsNullOrEmpty(fname) Or String.IsNullOrEmpty(mname) Or String.IsNullOrEmpty(lname) Or ComboBox1.Text = "" Or String.IsNullOrEmpty(username) Or String.IsNullOrEmpty(password) Or String.IsNullOrEmpty(repassword)) Then
            MessageBox.Show("Please complete all fields to proceed!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            If (password.Count > 0) Then

                If (password = repassword) Then

                    Try

                        If dbconn.State = ConnectionState.Open Then
                            dbconn.Close()
                        End If
                        Dim wrapper As New EncryptDecryptVB(password)
                        cipherText = wrapper.EncryptData(password)
                        dbconn.Open()
                        With cmd
                            .Connection = dbconn
                            .CommandText = "INSERT INTO user VALUES('" & employee & "','" & fname & "','" & mname & "','" & lname & "','" & username & "','" & cipherText & "','" & position & "','1')"
                            .ExecuteNonQuery()
                            MessageBox.Show("User " + fname + " " + mname + " " + lname + " has been added!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            TextBox1.Clear()
                            TextBox2.Clear()
                            TextBox3.Clear()
                            TextBox4.Clear()
                            TextBox5.Clear()
                            passwordBox.Clear()
                            TextBox7.Clear()

                            If position.Equals("Admin") Then
                                Login.Show()
                                Me.Hide()
                            End If


                        End With
                    Catch ex As Exception
                        MessageBox.Show("Account not added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    End Try

                    dbconn.Close()
                    dbconn.Dispose()

                Else
                    MessageBox.Show("Password doesn't match!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            Else
                MessageBox.Show("Password too short! (Input atleast 8 characters)", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles TextBox7.TextChanged
        If (passwordBox.Text.Equals(TextBox7.Text)) Then
            TextBox7.BackColor = Color.Green
            TextBox7.ForeColor = Color.White
        ElseIf (TextBox7.Text.Equals("")) Then
            TextBox7.BackColor = Color.White
            TextBox7.ForeColor = Color.Black
        Else
            TextBox7.BackColor = Color.Red
            TextBox7.ForeColor = Color.White
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class