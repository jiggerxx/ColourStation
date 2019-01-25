Public Class addagent

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim fname As String = TextBox1.Text
        Dim mname As String = TextBox2.Text
        Dim lname As String = TextBox3.Text
        Dim type As String = ComboBox1.SelectedItem

        Try
            checkstate()
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "INSERT INTO accounts (accfname,accmname,acclname,acctype) VALUES('" & fname & "','" & mname & "','" & lname & "','" & type & "')"
                .ExecuteNonQuery()
                MessageBox.Show("Account " + fname + " " + mname + " " + lname + " has been added!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                TextBox1.Clear()
                TextBox2.Clear()
                TextBox3.Clear()
                ComboBox1.SelectedIndex = -1

            End With
        Catch ex As Exception
            MessageBox.Show("Account not added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        loadagents()
        loadmixxer()
        loadpintor()

    End Sub

    Private Sub addagent_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class