Public Class addunit

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim units As String = TextBox1.Text

        Try

            If dbconn.State = ConnectionState.Open Then
                dbconn.Close()
            End If
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "INSERT INTO units (unit) VALUES('" & units & "')"
                .ExecuteNonQuery()
                MessageBox.Show(units + " has been added!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                TextBox1.Clear()

            End With
        Catch ex As Exception
            MessageBox.Show("Unit Not Added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dbconn.Close()
        dbconn.Dispose()

        loadunits()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub TextBox1_Leave(sender As Object, e As EventArgs) Handles TextBox1.Leave
        If TextBox1.Text = "" Or TextBox1.Text = "0" Then
            TextBox1.Text = "0.0"
        End If
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or Asc(e.KeyChar) = 8)
        If (e.KeyChar.ToString = ".") And (TextBox1.Text.Contains(e.KeyChar.ToString)) Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub TextBox1_Enter(sender As Object, e As EventArgs) Handles TextBox1.Enter
        If String.IsNullOrEmpty(TextBox1.Text) Or TextBox1.Text = 0.0 Then
            TextBox1.Text = ""
        End If
    End Sub
End Class