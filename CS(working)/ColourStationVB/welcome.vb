Public Class welcome

    Private Sub Panel2_MouseClick(sender As Object, e As MouseEventArgs) Handles Panel2.MouseClick
        viewoutstocks()
        loadoverstock()
    End Sub

    Private Sub Label2_MouseClick(sender As Object, e As MouseEventArgs) Handles Label2.MouseClick
        viewoutstocks()
        loadoverstock()
    End Sub

    Private Sub Panel3_MouseClick(sender As Object, e As MouseEventArgs) Handles Panel3.MouseClick
        viewnearoutstocks()
        loadnearoutofstock()
    End Sub

    Private Sub Label5_MouseClick(sender As Object, e As MouseEventArgs) Handles Label5.MouseClick
        viewnearoutstocks()
        loadnearoutofstock()
    End Sub

    Private Sub Label4_MouseClick(sender As Object, e As EventArgs) Handles Label4.MouseClick
        viewnearoutstocks()
        loadnearoutofstock()
    End Sub

    Private Sub Label3_MouseClick(sender As Object, e As MouseEventArgs) Handles Label3.MouseClick
        viewoutstocks()
        loadoverstock()
    End Sub

    Private Sub Label8_MouseClick(sender As Object, e As MouseEventArgs) Handles Label8.MouseClick
        viewoutstocks()
        loadoverstock()
    End Sub

    Private Sub Label7_MouseClick(sender As Object, e As MouseEventArgs) Handles Label7.MouseClick
        viewoverstocks()
        loadoverstock()
    End Sub

    Private Sub Label6_MouseClick(sender As Object, e As MouseEventArgs) Handles Label6.MouseClick
        viewoverstocks()
        loadoverstock()
    End Sub

    Private Sub Panel4_MouseClick(sender As Object, e As MouseEventArgs) Handles Panel4.MouseClick
        viewoverstocks()
        loadoverstock()
    End Sub

    Private Sub Panel5_MouseClick(sender As Object, e As MouseEventArgs) Handles Panel5.MouseClick
        showpaymentnotice()
        loadunpaidpayments()
    End Sub

    Private Sub Label10_MouseClick(sender As Object, e As MouseEventArgs) Handles Label10.MouseClick
        showpaymentnotice()
        loadunpaidpayments()
    End Sub

    Private Sub Label9_MouseClick(sender As Object, e As MouseEventArgs) Handles Label9.MouseClick
        showpaymentnotice()
        loadunpaidpayments()
    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs) Handles Label10.Click

    End Sub

    Private Sub Panel5_Paint(sender As Object, e As PaintEventArgs) Handles Panel5.Paint

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub
End Class