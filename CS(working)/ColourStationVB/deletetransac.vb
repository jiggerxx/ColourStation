Public Class deletetransac

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim deltransacnum As String = TextBox1.Text

        Try

            checkstate()
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "DELETE FROM csdb.resibo WHERE transacnum='" & deltransacnum & "'"
                .ExecuteNonQuery()
                MessageBox.Show("Transaction #" & deltransacnum & " is deleted!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                TextBox1.Clear()

            End With
        Catch ex As Exception
            MessageBox.Show("Transaction Not Found!" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class