Public Class nearoutofstock



    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        viewnearoutstocks()
        loadnearoutofstockclick(TextBox1.Text.ToString)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text.Equals("") Then
            viewnearoutstocks()
            loadnearoutofstock1()
        Else
            loadnearoutofstockclick(TextBox1.Text.ToString)
        End If
        'Timer1.Stop()
        'Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' Timer1.Stop()
        'viewnearoutstocks()
        'loadnearoutofstockchange(TextBox1.Text.ToString)
        'TextBox1.Focus().Equals(True)

    End Sub
End Class