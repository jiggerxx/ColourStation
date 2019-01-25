Public Class invoice_others

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim transnumber As String = TextBox1.Text
        Dim datetransac As String = transacdatepicker.Value.ToString("yyyy-MM-dd")
        Dim custname As String
        Dim custcode As String
        Dim totalcost As Double = Convert.ToDouble(TextBox3.Text)
        Dim discount As Double = Convert.ToDouble(TextBox4.Text)
        Dim cashier As String = TextBox5.Text
        Dim totalwdiscount As Double = Convert.ToDouble(TextBox6.Text)
        Dim mixtype As String = ""
        Dim agentkey As String
        Dim agentvalue As String
        Dim pintorkey As String
        Dim pintorvalue As String
        Dim totalunits As Double
        Dim paymenttype As String = ComboBox4.Text
        Dim prionumber As String = Label14.Text
        Dim unitperstock As Integer = 0
        Dim stockstobededucted As Double = 0


        Try
            agentkey = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
            agentvalue = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Value
        Catch ex As Exception
            agentkey = ""
            agentvalue = ""
        End Try

        Try
            pintorkey = DirectCast(ComboBox3.SelectedItem, KeyValuePair(Of String, String)).Key
            pintorvalue = DirectCast(ComboBox3.SelectedItem, KeyValuePair(Of String, String)).Value
        Catch ex As Exception
            pintorkey = ""
            pintorvalue = ""
        End Try

        Try
            custcode = DirectCast(ComboBox5.SelectedItem, KeyValuePair(Of String, String)).Key
            custname = DirectCast(ComboBox5.SelectedItem, KeyValuePair(Of String, String)).Value
        Catch ex As Exception
            custcode = ""
            custname = ""
        End Try


        If paymenttype = "Charge" And custcode = "" Then
            MessageBox.Show("Payment type selected is Charge. Customer must be selected to continue!")
        Else
            Try

                checkstate()
                dbconn.Open()
                With cmd
                    .Connection = dbconn
                    .CommandText = "INSERT INTO resibo VALUES('" & transnumber & "','" & datetransac & "','" & custcode & "','" & totalcost & "','" & discount & "','" & totalwdiscount & "','" & cashier & "', '" & paymenttype & "', '" & prionumber & "')"
                    .ExecuteNonQuery()
                    MessageBox.Show("Transaction #" + transnumber + " has been added!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    TextBox1.Clear()
                    TextBox3.Clear()
                    TextBox4.Clear()
                    TextBox5.Clear()
                    TextBox6.Clear()

                End With
            Catch ex As Exception
                MessageBox.Show("Transaction #" + transnumber + " not added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try
            dbconn.Close()
            dbconn.Dispose()

            If paymenttype = "Charge" Then
                Try

                    checkstate()
                    dbconn.Open()
                    With cmd
                        .Connection = dbconn
                        .CommandText = "insert into customer_utangs values('" & transnumber & "','" & custcode & "','0','" & totalwdiscount & "','" & ComboBox6.Text & "','Unpaid')"
                        .ExecuteNonQuery()

                    End With
                Catch ex As Exception
                    MessageBox.Show("error! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try
                dbconn.Close()
                dbconn.Dispose()
            End If

            '***************************************FOR LOOP IN SAVING PRODUCTS AND UNITS**************************************

            For x As Integer = 0 To cart_others.cartcounterX - 1

                Dim unitsearned As Double = 0
                Dim partialunits As Double = 0
                Dim wholenumberunits As Integer = 0
                Dim decimalunits As Double = 0
                Dim stocks As Integer = 0
                Dim excessexist As Boolean = False
                Dim remainingexistunits As Double = 0

                Try
                    checkstate()
                    dbconn.Open()

                    With cmd
                        .Connection = dbconn
                        .CommandText = "select products.*,units.* from csdb.products LEFT JOIN units ON products.unitID = units.unitID where prodcode='" & cart_others.cartdataArray(x, 0) & "'"
                        dr = cmd.ExecuteReader

                        While dr.Read
                            unitperstock = dr.Item("unit")
                        End While

                    End With

                Catch ex As Exception
                    MessageBox.Show(ex.Message + "error!", "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                dbconn.Close()
                dbconn.Dispose()

                partialunits = cart_others.cartdataArray(x, 3) * cart_others.cartdataArray(x, 5)
                wholenumberunits = Math.Truncate(partialunits)
                decimalunits = partialunits - wholenumberunits

                stockstobededucted = wholenumberunits / unitperstock

                If partialunits >= unitperstock Then

                    decimalunits = decimalunits + (partialunits - (stockstobededucted * unitperstock))
                    '***************************************UPDATE START STOCKS IF MORE THAN A GALLON**************************************
                    Try
                        checkstate()
                        dbconn.Open()

                        With cmd
                            .Connection = dbconn
                            .CommandText = "select * from csdb.products where prodcode='" & cart_others.cartdataArray(x, 0) & "'"
                            dr = cmd.ExecuteReader

                            While dr.Read
                                stocks = dr.Item("stock")
                            End While

                        End With

                    Catch ex As Exception
                        MessageBox.Show(ex.Message + "error!", "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                    dbconn.Close()
                    dbconn.Dispose()

                    stocks = stocks - stockstobededucted

                    Try
                        checkstate()
                        dbconn.Open()
                        With cmd
                            .Connection = dbconn
                            .CommandText = "update products set stock='" & stocks & "' where prodcode='" & cart_others.cartdataArray(x, 0) & "'"
                            .ExecuteNonQuery()

                        End With
                    Catch ex As Exception
                        MessageBox.Show("error!" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    End Try
                    dbconn.Close()
                    dbconn.Dispose()
                    '***************************************UPDATE END STOCKS IF MORE THAN A GALLON**************************************

                End If

                '***************************************SAVE REMAINING UNITS**************************************
                Try
                    checkstate()
                    dbconn.Open()

                    With cmd
                        .Connection = dbconn
                        .CommandText = "SELECT * FROM menudo_sobra WHERE prodcode='" & cart_others.cartdataArray(x, 0) & "'"
                        dr = cmd.ExecuteReader

                        While dr.Read
                            excessexist = True
                            remainingexistunits = dr.Item("units")
                        End While

                    End With

                Catch ex As Exception
                    MessageBox.Show(ex.Message + "error!", "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                dbconn.Close()
                dbconn.Dispose()

                If excessexist = True Then

                    remainingexistunits = remainingexistunits + decimalunits

                    If remainingexistunits >= unitperstock Then

                        wholenumberunits = Math.Truncate(remainingexistunits)
                        decimalunits = remainingexistunits - wholenumberunits

                        stockstobededucted = wholenumberunits / unitperstock

                        If decimalunits <> 0 Then
                            decimalunits = decimalunits + (remainingexistunits - (stockstobededucted * unitperstock))
                        End If


                        Try
                            checkstate()
                            dbconn.Open()

                            With cmd
                                .Connection = dbconn
                                .CommandText = "select * from csdb.products where prodcode='" & cart_others.cartdataArray(x, 0) & "'"
                                dr = cmd.ExecuteReader

                                While dr.Read
                                    stocks = dr.Item("stock")
                                End While

                            End With

                        Catch ex As Exception
                            MessageBox.Show(ex.Message + "error!", "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                        dbconn.Close()
                        dbconn.Dispose()

                        stocks = stocks - stockstobededucted

                        Try
                            checkstate()
                            dbconn.Open()
                            With cmd
                                .Connection = dbconn
                                .CommandText = "update products set stock='" & stocks & "' where prodcode='" & cart_others.cartdataArray(x, 0) & "'"
                                .ExecuteNonQuery()

                            End With
                        Catch ex As Exception
                            MessageBox.Show("error!" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        End Try
                        dbconn.Close()
                        dbconn.Dispose()

                    End If

                    If decimalunits <> 0 Then
                        Try
                            checkstate()
                            dbconn.Open()
                            With cmd
                                .Connection = dbconn
                                .CommandText = "UPDATE menudo_sobra SET units='" & decimalunits & "' WHERE prodcode='" & cart_others.cartdataArray(x, 0) & "'"
                                .ExecuteNonQuery()

                            End With

                        Catch ex As Exception
                            MessageBox.Show("Error!" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        End Try
                        dbconn.Close()
                        dbconn.Dispose()
                    Else
                        Try
                            checkstate()
                            dbconn.Open()
                            With cmd
                                .Connection = dbconn
                                .CommandText = "DELETE FROM menudo_sobra WHERE prodcode='" & cart_others.cartdataArray(x, 0) & "'"
                                .ExecuteNonQuery()

                            End With

                        Catch ex As Exception
                            MessageBox.Show("Error!" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        End Try
                        dbconn.Close()
                        dbconn.Dispose()
                    End If

                Else

                    stockstobededucted = Math.Truncate(stockstobededucted)

                    If stockstobededucted = 0 Then
                        Try
                            checkstate()
                            dbconn.Open()
                            With cmd
                                .Connection = dbconn
                                .CommandText = "INSERT INTO menudo_sobra VALUES('" & cart_others.cartdataArray(x, 0) & "','" & partialunits & "')"
                                .ExecuteNonQuery()
                            End With
                        Catch ex As Exception
                            MessageBox.Show("error! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                        dbconn.Close()
                        dbconn.Dispose()

                    ElseIf decimalunits <> 0 Then

                        Try
                            checkstate()
                            dbconn.Open()
                            With cmd
                                .Connection = dbconn
                                .CommandText = "INSERT INTO menudo_sobra VALUES('" & cart_others.cartdataArray(x, 0) & "','" & decimalunits & "')"
                                .ExecuteNonQuery()
                            End With
                        Catch ex As Exception
                            MessageBox.Show("error! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                        dbconn.Close()
                        dbconn.Dispose()
                    End If

                End If


                '***************************************SAVE REMAINING UNITS**************************************


                '**************************************INSERT TO menudo_records table**************************************

                Try

                    checkstate()
                    dbconn.Open()
                    With cmd
                        .Connection = dbconn
                        .CommandText = "INSERT INTO menudo_records VALUES('" & transnumber & "','" & cart_others.cartdataArray(x, 0) & "','" & cart_others.cartdataArray(x, 3) & "','" & cart_others.cartdataArray(x, 4) & "','" & cart_others.cartdataArray(x, 5) & "')"
                        .ExecuteNonQuery()

                    End With
                Catch ex As Exception
                    MessageBox.Show("error! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try
                dbconn.Close()
                dbconn.Dispose()

                '**************************************INSERT TO menudo_records table**************************************

                totalunits = totalunits + (cart_others.cartdataArray(x, 3) * cart_others.cartdataArray(x, 5))

            Next

            '***************************************FOR LOOP IN SAVING PRODUCTS AND UNITS**************************************

            If cart.mixxerCounterX <> 0 Then
                For x = 0 To cart.mixxerCounterX - 1
                    Try
                        checkstate()
                        dbconn.Open()
                        With cmd
                            .Connection = dbconn
                            .CommandText = "insert into mixxer_transac values('" & cart.mixxerArray(x, 0) & "','" & cart.mixxerArray(x, 1) & "','" & transnumber & "','" & datetransac & "','" & cart.mixxerArray(x, 3) & "','" & cart.mixxerArray(x, 2) & "')"
                            .ExecuteNonQuery()
                        End With
                    Catch ex As Exception
                        MessageBox.Show("error in mixxer query! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                    dbconn.Close()
                    dbconn.Dispose()
                Next
            End If

            If agentkey <> "" Then

                Try
                    checkstate()
                    dbconn.Open()
                    With cmd
                        .Connection = dbconn
                        .CommandText = "insert into agents_transac values('" & agentkey & "','" & transnumber & "','" & datetransac & "','" & cart.totalunits & "','" & cart.agentcanvassertotal & "')"
                        .ExecuteNonQuery()
                    End With
                Catch ex As Exception
                    MessageBox.Show("error in agents query! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try
                dbconn.Close()
                dbconn.Dispose()
            End If

            If pintorkey <> "" Then

                Dim totalpintor As Double = 0

                totalpintor = (cart.pintorreadymixtotal + cart.pintormixtotal) - ((cart.pintorreadymixtotal + cart.pintormixtotal) * 0.12)

                Try
                    checkstate()
                    dbconn.Open()
                    With cmd
                        .Connection = dbconn
                        .CommandText = "insert into pintor_transac values('" & pintorkey & "','" & transnumber & "','" & cart.pintorreadymixtotal & "','" & cart.pintormixtotal & "','" & totalpintor & "','" & datetransac & "')"
                        .ExecuteNonQuery()

                    End With
                Catch ex As Exception
                    MessageBox.Show("Error in pintor query! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try
                dbconn.Close()
                dbconn.Dispose()

            End If

            printreceipt.datafrom = "others"
            printreceipt.selectedtransacnum = transnumber
            printreceipt.ShowDialog()

            Me.Close()
            Me.Dispose()

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

            cart_others.draftcounterX = 0
            cart_others.draftpays = 0
            cart_others.draftmixxerCounterx = 0

            cart_others.pintorreadymixtotaldraft = 0
            cart_others.pintormixtotaldraft = 0
            cart_others.agentcanvassertotaldraft = 0
            cart_others.totalunitsdraft = 0
        End If

    End Sub

    Private Sub invoice_others_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim firstprionum As Boolean = False
        Dim firsttransacnum As Boolean = False
        Dim prionum As Integer = 0
        Dim transacnum As Integer = 0
        Dim datenow As String = Date.Today.ToString("yyyy-MM-dd")

        TextBox5.Text = main.Button12.Text

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT MAX(prionum) as maxprionum FROM resibo WHERE transacdate='" & datenow & "'"
                dr = cmd.ExecuteReader

                While dr.Read
                    prionum = dr.Item("maxprionum")
                    firstprionum = True
                End While

            End With

        Catch ex As Exception

        End Try
        dbconn.Close()
        dbconn.Dispose()

        If firstprionum Then
            Label14.Text = "" & prionum + 1
        Else
            Label14.Text = "1"
        End If

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT MAX(transacnum) as maxtransacnum FROM resibo  WHERE transacdate='" & datenow & "'"
                dr = cmd.ExecuteReader

                While dr.Read
                    transacnum = dr.Item("maxtransacnum")
                    firsttransacnum = True
                End While

            End With

        Catch ex As Exception

        End Try
        dbconn.Close()
        dbconn.Dispose()

        If firsttransacnum Then
            TextBox1.Text = "" & transacnum + 1
        Else
            TextBox1.Text = Date.Today.ToString("yyyyMMdd") & "1"
        End If

        ComboBox4.SelectedIndex = 0
        loadmixxer()
        loadagents()
        loadpintor()
        loadcustomer()
        TextBox4.Text = 0
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        addagent.ShowDialog()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        addagent.ShowDialog()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        addagent.ShowDialog()
    End Sub

    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or Asc(e.KeyChar) = 8)
        If (e.KeyChar.ToString = ".") And (TextBox4.Text.Contains(e.KeyChar.ToString)) Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub TextBox4_Leave(sender As Object, e As EventArgs) Handles TextBox4.Leave
        Dim discount As Double
        Dim total As Double = Convert.ToDouble(TextBox3.Text)
        Dim finaltotal As Double = 0

        Try
            discount = Convert.ToDouble(TextBox4.Text)
        Catch ex As Exception
            TextBox4.Text = 0
            discount = 0
        End Try

        'discount = (discount / 100)
        'finaltotal = total * discount
        'finaltotal = total - finaltotal

        finaltotal = total - discount

        TextBox6.Text = finaltotal
    End Sub

    Private Sub TextBox4_Enter(sender As Object, e As EventArgs) Handles TextBox4.Enter
        If TextBox4.Text = 0 Then
            TextBox4.Clear()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        addcustomerinfo.ShowDialog()
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        If ComboBox4.Text = "Charge" Then
            ComboBox6.Enabled = True
            ComboBox6.SelectedIndex = 0
        Else
            ComboBox6.Enabled = False
            ComboBox6.SelectedIndex = -1
        End If
    End Sub
End Class