Public Class main

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        changepic()
        whitetext()
        Panel4.Height = Button3.Height
        Panel4.Top = Button3.Top
        Button3.Image = My.Resources.addext230x30
        Button3.ForeColor = Color.FromArgb(236, 159, 32)
        Button3.BackColor = Color.FromArgb(63, 78, 99)
        showpayutangs()

        loadcustomerswithutangs()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        changepic()
        whitetext()
        Panel4.Height = Button2.Height
        Panel4.Top = Button2.Top
        Button2.Image = My.Resources.addstock230x30
        Button2.ForeColor = Color.FromArgb(236, 159, 32)
        Button2.BackColor = Color.FromArgb(63, 78, 99)
        showaddstock()
        loadproducts()
        loadsuppliers()
        addstock.Refresh()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        changepic()
        whitetext()
        Panel4.Height = Button4.Height
        Panel4.Top = Button4.Top
        Button4.Image = My.Resources.released230x30
        Button4.ForeColor = Color.FromArgb(236, 159, 32)
        Button4.BackColor = Color.FromArgb(63, 78, 99)
        showreleaseprodsform()
        loadproductswithstocks()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        changepic()
        whitetext()
        Panel4.Height = Button9.Height
        Panel4.Top = Button9.Top
        Button9.Image = My.Resources.edit230x30
        Button9.ForeColor = Color.FromArgb(236, 159, 32)
        Button9.BackColor = Color.FromArgb(63, 78, 99)
        showeditprod()
        loadunits()
        loadtypes()
        loadproducts()

        editproduct.TextBox1.Clear()
        editproduct.TextBox3.Clear()
        editproduct.TextBox2.Clear()
        editproduct.TextBox4.Clear()
        editproduct.ComboBox2.SelectedIndex = -1
        editproduct.ComboBox3.SelectedIndex = -1
        editproduct.ComboBox4.SelectedIndex = -1
        editproduct.RichTextBox1.Clear()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        changepic()
        whitetext()
        Panel4.Height = Button8.Height
        Panel4.Top = Button8.Top
        Button8.Image = My.Resources.eyew230x30
        Button8.ForeColor = Color.FromArgb(236, 159, 32)
        Button8.BackColor = Color.FromArgb(63, 78, 99)
        showacquiredprods()
        viewaddedOnload()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        changepic()
        whitetext()
        Panel4.Height = Button5.Height
        Panel4.Top = Button5.Top
        Button5.Image = My.Resources.eyew230x30
        Button5.ForeColor = Color.FromArgb(236, 159, 32)
        Button5.BackColor = Color.FromArgb(63, 78, 99)
        showreleaseprods()
        loadreleasednums()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        changepic()
        whitetext()
        Panel4.Height = Button10.Height
        Panel4.Top = Button10.Top
        Button10.Image = My.Resources.print230x30
        Button10.ForeColor = Color.FromArgb(236, 159, 32)
        Button10.BackColor = Color.FromArgb(63, 78, 99)
        showrprintchoice()
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        changepic()
        whitetext()
        Panel4.Height = Button23.Height
        Panel4.Top = Button23.Top
        Button23.Image = My.Resources.addd230x30
        Button23.ForeColor = Color.FromArgb(236, 159, 32)
        Button23.BackColor = Color.FromArgb(63, 78, 99)
        showaddproduct()
        loadunits()
        loadtypes()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        loadoutofstock()
        loadnearoutofstock()
        loadoverstock()
        loadunpaidpayments()

        changepic()
        whitetext()
        Panel4.Height = Button11.Height
        Panel4.Top = Button11.Top
        Button11.Image = My.Resources.homeicon230x30
        Button11.ForeColor = Color.FromArgb(236, 159, 32)
        Button11.BackColor = Color.FromArgb(63, 78, 99)
        home()
    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs)
        Register.ShowDialog()
    End Sub

    Private Sub Button26_Click(sender As Object, e As EventArgs)
        editaccount.ShowDialog()
    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs)
        enableaccount.ShowDialog()
    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs)
        disableaccount.ShowDialog()
    End Sub

    Private Sub Button30_Click(sender As Object, e As EventArgs) Handles Button30.Click
        changepic()
        whitetext()
        Panel4.Height = Button30.Height
        Panel4.Top = Button30.Top
        Button30.Image = My.Resources.cart230x30
        Button30.ForeColor = Color.FromArgb(236, 159, 32)
        Button30.BackColor = Color.FromArgb(63, 78, 99)
        cartchoice.ShowDialog()

    End Sub

    Private Sub Button34_Click(sender As Object, e As EventArgs)
        addagent.ShowDialog()
    End Sub

    Private Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadoutofstock()
        loadnearoutofstock()
        loadoverstock()
        loadunpaidpayments()
        home()

        Panel4.Height = Button11.Height
        Panel4.Top = Button11.Top
        Button11.Image = My.Resources.homeicon230x30
        Button11.ForeColor = Color.FromArgb(236, 159, 32)
        Button11.BackColor = Color.FromArgb(63, 78, 99)
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Button12.BackColor = Color.FromArgb(212, 143, 28)
        usermanagement.ShowDialog()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Login.loginpassword.Clear()
        Login.loginusername.Clear()
        Login.Show()
        Me.Hide()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        changepic()
        whitetext()
        Panel4.Height = Button6.Height
        Panel4.Top = Button6.Top
        Button6.Image = My.Resources.return_orange
        Button6.ForeColor = Color.FromArgb(236, 159, 32)
        Button6.BackColor = Color.FromArgb(63, 78, 99)
        loadtransactions()
        returns.transacdate.Text = "-------------"
        returns.cashiername.Text = "-------------"
        returns.paymenttype.Text = "-------------"
        returns.total.Text = "-------------"
        returns.discount.Text = "-------------"
        returns.totaldiscount.Text = "-------------"
        showreturns()
    End Sub
End Class