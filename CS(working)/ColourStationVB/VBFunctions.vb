Module VBFunctions
    Public Function showreturns()
        Dim returnsvb As New Panel

        main.Panel1.Controls.Clear()
        returnsvb = returns.Panel1
        main.Panel1.Controls.Add(returnsvb)

        Return Nothing
    End Function
    Public Function showpaymentnotice()
        Dim paymentnotice As New Panel

        main.Panel1.Controls.Clear()
        paymentnotice = paymentsnotice.Panel1
        main.Panel1.Controls.Add(paymentnotice)

        Return Nothing
    End Function
    Public Function showpayutangs()
        Dim payutangs As New Panel

        main.Panel1.Controls.Clear()
        payutangs = paycharges.Panel1
        main.Panel1.Controls.Add(payutangs)

        Return Nothing
    End Function
    Public Function viewoutstocks()

        Dim outofstock As New Panel

        main.Panel1.Controls.Clear()
        outofstock = outofstocklist.Panel1
        main.Panel1.Controls.Add(outofstock)

        Return Nothing
    End Function
    Public Function viewnearoutstocks()

        Dim nearoutstocks As New Panel

        main.Panel1.Controls.Clear()
        nearoutstocks = nearoutofstock.Panel1
        main.Panel1.Controls.Add(nearoutstocks)

        Return Nothing
    End Function
    Public Function viewoverstocks()

        Dim overstocks As New Panel

        main.Panel1.Controls.Clear()
        overstocks = overstock.Panel1
        main.Panel1.Controls.Add(overstocks)

        Return Nothing
    End Function
    Public Function changepic()

        main.Button3.Image = My.Resources.addext30x30
        main.Button2.Image = My.Resources.addstock30x30
        main.Button4.Image = My.Resources.released30x30
        main.Button9.Image = My.Resources.edit30x30
        main.Button8.Image = My.Resources.eyew30x30
        main.Button5.Image = My.Resources.eyew30x30
        main.Button10.Image = My.Resources.print30x30
        main.Button23.Image = My.Resources.addd30x30
        main.Button11.Image = My.Resources.homeicon30x30
        main.Button30.Image = My.Resources.cart30x30
        main.Button6.Image = My.Resources._return

        Return Nothing
    End Function
    Public Function whitetext()

        main.Button11.ForeColor = Color.FromArgb(255, 255, 255)
        main.Button23.ForeColor = Color.FromArgb(255, 255, 255)
        main.Button2.ForeColor = Color.FromArgb(255, 255, 255)
        main.Button9.ForeColor = Color.FromArgb(255, 255, 255)
        main.Button3.ForeColor = Color.FromArgb(255, 255, 255)
        main.Button10.ForeColor = Color.FromArgb(255, 255, 255)
        main.Button8.ForeColor = Color.FromArgb(255, 255, 255)
        main.Button5.ForeColor = Color.FromArgb(255, 255, 255)
        main.Button4.ForeColor = Color.FromArgb(255, 255, 255)
        main.Button30.ForeColor = Color.FromArgb(255, 255, 255)
        main.Button6.ForeColor = Color.FromArgb(255, 255, 255)

        main.Button11.BackColor = Color.FromArgb(53, 65, 83)
        main.Button23.BackColor = Color.FromArgb(53, 65, 83)
        main.Button2.BackColor = Color.FromArgb(53, 65, 83)
        main.Button9.BackColor = Color.FromArgb(53, 65, 83)
        main.Button3.BackColor = Color.FromArgb(53, 65, 83)
        main.Button10.BackColor = Color.FromArgb(53, 65, 83)
        main.Button8.BackColor = Color.FromArgb(53, 65, 83)
        main.Button5.BackColor = Color.FromArgb(53, 65, 83)
        main.Button4.BackColor = Color.FromArgb(53, 65, 83)
        main.Button30.BackColor = Color.FromArgb(53, 65, 83)
        main.Button6.BackColor = Color.FromArgb(53, 65, 83)

        Return Nothing
    End Function
    Public Function showcart()
        Dim carts As New Panel

        main.Panel1.Controls.Clear()
        carts = cart.Panel1
        main.Panel1.Controls.Add(carts)

        Return Nothing
    End Function
    Public Function showcartothers()
        Dim carts As New Panel

        main.Panel1.Controls.Clear()
        carts = cart_others.Panel1
        main.Panel1.Controls.Add(carts)

        Return Nothing
    End Function
    Public Function home()
        Dim homies As New Panel

        main.Panel1.Controls.Clear()
        homies = welcome.Panel1
        main.Panel1.Controls.Add(homies)

        Return Nothing
    End Function

    Public Function showaddproduct()
        Dim addprod As New Panel

        main.Panel1.Controls.Clear()
        addprod = addproduct.Panel1
        main.Panel1.Controls.Add(addprod)

        Return Nothing
    End Function

    Public Function showaddstock()
        Dim addstocks As New Panel

        main.Panel1.Controls.Clear()
        addstocks = addstock.Panel1
        main.Panel1.Controls.Add(addstocks)

        Return Nothing
    End Function

    Public Function showeditprod()
        Dim editprod As New Panel

        main.Panel1.Controls.Clear()
        editprod = editproduct.Panel1
        main.Panel1.Controls.Add(editprod)

        Return Nothing
    End Function

    Public Function showaddsupp()
        Dim addsupp As New Panel

        main.Panel1.Controls.Clear()
        addsupp = supplier.Panel1
        main.Panel1.Controls.Add(addsupp)

        Return Nothing
    End Function

    Public Function showaddtype()
        Dim addtypes As New Panel

        main.Panel1.Controls.Clear()
        addtypes = addtype.Panel1
        main.Panel1.Controls.Add(addtypes)

        Return Nothing
    End Function

    Public Function showaddunit()
        Dim addunits As New Panel

        main.Panel1.Controls.Clear()
        addunits = addunit.Panel1
        main.Panel1.Controls.Add(addunits)

        Return Nothing
    End Function

    Public Function showacquiredprods()
        Dim acquired As New Panel

        main.Panel1.Controls.Clear()
        acquired = viewadded.Panel1
        main.Panel1.Controls.Add(acquired)

        Return Nothing
    End Function

    Public Function showreleaseprods()
        Dim release As New Panel

        main.Panel1.Controls.Clear()
        release = View_Release.Panel1
        main.Panel1.Controls.Add(release)

        Return Nothing
    End Function

    Public Function showreleaseprodsform()
        Dim releaseform As New Panel

        main.Panel1.Controls.Clear()
        releaseform = releasestock.Panel1
        main.Panel1.Controls.Add(releaseform)

        Return Nothing
    End Function

    Public Function showrprintchoice()
        Dim printchoice As New Panel

        main.Panel1.Controls.Clear()
        printchoice = printingmenu.Panel1
        main.Panel1.Controls.Add(printchoice)

        Return Nothing
    End Function

    
End Module
