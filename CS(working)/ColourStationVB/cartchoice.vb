Public Class cartchoice

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        loadproductswithstocks()
        loadproductsCart()
        loadmixxer()
        showcart()
        cart.TextBox1.Focus()
        cart.prodcode.Text = "-"
        cart.prodname.Text = "-"
        cart.produnit.Text = "-"
        cart.prodtype.Text = "-"
        cart.srp.Text = "-"
        cart.qty.Text = "-"
        cart.totalcost.Text = "-"
        cart.NumericUpDown1.Minimum = 0
        cart.NumericUpDown1.Value = 0

        Me.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        loadproducts1gal()
        loadmixxer()
        showcartothers()
        cart.TextBox1.Focus()

        cart_others.DataGridView1.Rows.Clear()
        cart_others.prodcode.Text = "-"
        cart_others.prodname.Text = "-"
        cart_others.TextBox1.Text = "0.0"
        cart_others.TextBox2.Text = "50"
        cart_others.prodtype.Text = "-"
        cart_others.srp.Text = "-"
        cart_others.qty.Text = "-"
        cart_others.ComboBox1.SelectedIndex = -1
        cart_others.totalcost.Text = "-"
        cart_others.Label14.Text = "-------"

        cart_others.cartcounterX = 0
        cart_others.draftcounterX = 0
        cart_others.totalpays = 0
        cart_others.draftpays = 0
        cart_others.mixxerCounterX = 0
        cart_others.draftmixxerCounterx = 0
        cart_others.pintorreadymixtotal = 0
        cart_others.pintormixtotal = 0
        cart_others.agentcanvassertotal = 0
        cart_others.totalunits = 0

        cart_others.pintorreadymixtotaldraft = 0
        cart_others.pintormixtotaldraft = 0
        cart_others.agentcanvassertotaldraft = 0
        cart_others.totalunitsdraft = 0
        
        Me.Close()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class