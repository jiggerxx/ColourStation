Public Class addunitname

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim units As String = TextBox1.Text

        Try

            If dbconn.State = ConnectionState.Open Then
                dbconn.Close()
            End If
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "INSERT INTO unitword (unitname) VALUES('" & units & "')"
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
       
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
    End Sub

    Private Sub TextBox1_Enter(sender As Object, e As EventArgs) Handles TextBox1.Enter
    End Sub
End Class