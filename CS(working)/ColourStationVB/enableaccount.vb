Public Class enableaccount

    Private Sub enableaccount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loaddisabledaccounts()
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
        Dim selectedkey As String = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Key
        Dim selectedvalue As String = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Value

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "UPDATE user SET status='1' WHERE employeeID='" & selectedkey & "'"
                .ExecuteNonQuery()
                MessageBox.Show("User " + TextBox2.Text + " " + TextBox2.Text + " " + TextBox2.Text + " disabled!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

                TextBox2.Clear()
                TextBox3.Clear()
                TextBox4.Clear()
                TextBox5.Clear()
                ComboBox1.SelectedIndex = -1
                ComboBox2.SelectedIndex = -1
                loaddisabledaccounts()
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error1!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class