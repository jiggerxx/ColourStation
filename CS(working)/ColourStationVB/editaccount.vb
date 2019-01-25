Imports System.Text.RegularExpressions
Public Class editaccount

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub editaccount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadactiveaccounts()
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        ComboBox1.SelectedIndex = -1
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged

        Dim tandf As Boolean = False

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM user where employeeID='" & DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Key & "'"
                dr = cmd.ExecuteReader

                While dr.Read

                    TextBox2.Text = dr.Item("fname")
                    TextBox3.Text = dr.Item("mname")
                    TextBox4.Text = dr.Item("lname")
                    TextBox5.Text = dr.Item("username")
                    ComboBox1.SelectedItem = dr.Item("acctype")

                    tandf = True

                End While

            End With
        Catch ex As Exception
            'MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()

        If tandf <> True Then
            'MessageBox.Show("Product does not exist!")
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim fname As String = TextBox2.Text
        Dim mname As String = TextBox3.Text
        Dim lname As String = TextBox4.Text
        Dim username As String = TextBox5.Text
        Dim acctype As String = ComboBox1.SelectedText
        Dim current As String = passwordBox12.Text
        Dim newpass As String = passwordBox1.Text
        Dim newpassre As String = TextBox7.Text
        Dim selectedkey As String = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Key
        Dim selectedvalue As String = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Value
        Dim cipherText As String
        Dim cipherText2 As String

        If (String.IsNullOrEmpty(current) Or String.IsNullOrEmpty(fname) Or String.IsNullOrEmpty(mname) Or String.IsNullOrEmpty(lname) Or ComboBox1.Text = "" Or String.IsNullOrEmpty(username) Or String.IsNullOrEmpty(newpass) Or String.IsNullOrEmpty(newpassre)) Then
            MessageBox.Show("Please complete all fields to proceed!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            If (newpass.Count > 0) Then
                    If (newpass = newpassre) Then

                        Dim wrapper2 As New EncryptDecryptVB(current)
                        cipherText2 = wrapper2.EncryptData(current)
                        Dim oldpw As String = ""

                        Try
                            checkstate()
                            dbconn.Open()

                            With cmd
                                .Connection = dbconn
                                .CommandText = "SELECT * FROM csdb.user WHERE employeeID='" & selectedkey & "'"
                                dr = cmd.ExecuteReader

                                While dr.Read
                                    oldpw = dr.Item("password")
                                End While
                            End With

                        Catch ex As Exception
                            MessageBox.Show(ex.Message + "Error1!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try

                        dbconn.Close()
                        dbconn.Dispose()

                        If cipherText2.Equals(oldpw) Then
                            Try

                                checkstate()
                                Dim wrapper As New EncryptDecryptVB(newpass)
                                cipherText = wrapper.EncryptData(newpass)
                                dbconn.Open()
                                With cmd
                                    .Connection = dbconn
                                    .CommandText = "UPDATE user SET fname='" & fname & "',mname='" & mname & "',lname='" & lname & "',username='" & username & "',password='" & cipherText & "',acctype='" & acctype & "' WHERE employeeID='" & selectedkey & "'"
                                    .ExecuteNonQuery()
                                    MessageBox.Show("User " + fname + " " + mname + " " + lname + " has been updated!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    TextBox2.Clear()
                                    TextBox3.Clear()
                                    TextBox4.Clear()
                                    TextBox5.Clear()
                                    ComboBox1.SelectedIndex = -1
                                    ComboBox2.SelectedIndex = -1
                                    passwordBox12.Clear()
                                    passwordBox1.Clear()
                                    TextBox7.Clear()

                                End With
                            Catch ex As Exception
                                MessageBox.Show("Account not updated! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            End Try

                            dbconn.Close()
                            dbconn.Dispose()

                        Else
                            MessageBox.Show("Please Enter Your Correct Current Password!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                    Else
                        MessageBox.Show("Password doesn't match!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
            Else
                MessageBox.Show("Password too short! (Input atleast 8 characters)", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If


    End Sub


    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles TextBox7.TextChanged
        If (passwordBox1.Text.Equals(TextBox7.Text)) Then
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

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class