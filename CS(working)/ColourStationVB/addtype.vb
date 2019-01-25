Public Class addtype


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim type As String = TextBox1.Text

        Try

            checkstate()
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "INSERT INTO type (type) VALUES('" & type & "')"
                .ExecuteNonQuery()
                MessageBox.Show(type + " has been added!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                TextBox1.Clear()

            End With
        Catch ex As Exception
            MessageBox.Show("Type Not Added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dbconn.Close()
        dbconn.Dispose()

        loadtypes()
    End Sub

    Private Sub addtype_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class