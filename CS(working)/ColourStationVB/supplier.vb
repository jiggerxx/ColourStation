Public Class supplier

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim supcode As String = TextBox1.Text
        Dim supname As String = TextBox2.Text
        Dim supprovince As String = TextBox3.Text
        Dim supcity As String = TextBox4.Text
        Dim supbarangay As String = TextBox5.Text
        Dim supstreet As String = TextBox6.Text
        Dim paymentdues As String = TextBox7.Text

        Try

            checkstate()
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "INSERT INTO supplier VALUES('" & supcode & "','" & supname & "','" & supprovince & "','" & supcity & "','" & supbarangay & "','" & supstreet & "','" & paymentdues & "')"
                .ExecuteNonQuery()
                MessageBox.Show("Supplier " + supname + " has been added!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                TextBox1.Clear()
                TextBox2.Clear()
                TextBox3.Clear()
                TextBox4.Clear()
                TextBox5.Clear()
                TextBox6.Clear()
                TextBox7.Clear()

            End With
        Catch ex As Exception
            MessageBox.Show("Supplier Not Added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        loadsuppliers()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class