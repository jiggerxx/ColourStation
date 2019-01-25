Public Class overstock

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text.Equals("") Then
            viewoverstocks()
            loadoverstock()
        Else
            loadoverstockclick(TextBox1.Text.ToString)
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        viewoverstocks()
        loadoverstockclick(TextBox1.Text.ToString)
    End Sub
End Class