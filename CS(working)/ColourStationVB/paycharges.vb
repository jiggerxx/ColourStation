Public Class paycharges
    Dim displayedbalance As Double = 0
    Private Sub ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged
        Dim selectedcustomerkey As String
        Dim selectedcustomervalue As String
        Dim customerval As New Dictionary(Of String, String)()
        Dim customerauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            selectedcustomerkey = DirectCast(ComboBox5.SelectedItem, KeyValuePair(Of String, String)).Key
            selectedcustomervalue = DirectCast(ComboBox5.SelectedItem, KeyValuePair(Of String, String)).Value
        Catch ex As Exception
            selectedcustomerkey = ""
            selectedcustomervalue = ""

            
            Label20.Text = "------------"
            Label22.Text = "------------"
            Label24.Text = "------------"
            Label26.Text = "------------"
        End Try

        Try
            ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.customer_utangs WHERE custcode='" & selectedcustomerkey & "'"
                dr = cmd.ExecuteReader

                While dr.Read

                    customerval.Add(dr.Item("transacnum"), dr.Item("transacnum"))
                    customerauto.AddRange(New String() {dr.Item("transacnum")})

                    tandf = True
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error1!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        If tandf Then
            Try
                ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                ComboBox1.AutoCompleteCustomSource = customerauto
                ComboBox1.DataSource = New BindingSource(customerval, Nothing)
                ComboBox1.DisplayMember = "Value"
                ComboBox1.ValueMember = "Key"
                ComboBox1.SelectedIndex = -1

                Label11.Text = "------------"
                Label7.Text = "------------"
                Label5.Text = "------------"
                Label6.Text = "------------"
                Label13.Text = "------------"
            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Load Transactions")
            End Try
        End If

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.customers WHERE custcode='" & selectedcustomerkey & "'"
                dr = cmd.ExecuteReader

                While dr.Read
                    Label24.Text = dr.Item("lname") + ", " + dr.Item("fname") + ", " + dr.Item("mname")
                    Label20.Text = dr.Item("contact_number")
                    Label22.Text = dr.Item("street_purok") + ", " + dr.Item("barangay")
                    Label26.Text = dr.Item("city") + ", " + dr.Item("province")
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error2!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        Button2.Enabled = False
        

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim selectedcustomerkey As String
        Dim selectedcustomervalue As String
        Dim selectedtransackey As String
        Dim selectedtransacvalue As String
        Dim datetransac As Date
        Dim utangterm As Integer

        Try
            selectedcustomerkey = DirectCast(ComboBox5.SelectedItem, KeyValuePair(Of String, String)).Key
            selectedcustomervalue = DirectCast(ComboBox5.SelectedItem, KeyValuePair(Of String, String)).Value
            selectedtransackey = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
            selectedtransacvalue = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Value
        Catch ex As Exception
            selectedcustomerkey = ""
            selectedcustomervalue = ""
            selectedtransackey = ""
            selectedtransacvalue = ""
        End Try

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT customer_utangs.*,(SELECT resibo.finaltotal FROM resibo WHERE transacnum = '" & selectedtransackey & "') as totalutangs,(SELECT resibo.transacdate FROM resibo WHERE transacnum = '" & selectedtransackey & "') as datetransac FROM customer_utangs WHERE customer_utangs.custcode='" & selectedcustomerkey & "' AND customer_utangs.transacnum='" & selectedtransackey & "'"
                dr = cmd.ExecuteReader

                While dr.Read
                    Label7.Text = dr.Item("totalutangs")
                    Label5.Text = dr.Item("partial")
                    Label6.Text = dr.Item("balance")
                    utangterm = dr.Item("term")
                    datetransac = dr.Item("datetransac")
                    Label13.Text = dr.Item("status")

                    displayedbalance = dr.Item("balance")

                    If dr.Item("status") = "Paid" Then
                        Button2.Enabled = False
                    Else
                        Button2.Enabled = True
                    End If

                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error1!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        Label11.Text = DateAdd(DateInterval.Day, utangterm, datetransac).ToString("MMMM dd,yyyy")

        Try

            checkstate()
            dbconn.Open()

            DataGridView1.Rows.Clear()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT customer_payments.*,cheque_infos.*,customer_utangs.*,resibo.* FROM customer_utangs LEFT JOIN customer_payments ON customer_utangs.transacnum = customer_payments.transacnum AND customer_utangs.custcode = customer_payments.custcode LEFT JOIN resibo ON customer_utangs.transacnum = resibo.transacnum LEFT JOIN cheque_infos ON customer_payments.reference_num = cheque_infos.reference_num WHERE customer_utangs.custcode='" & selectedcustomerkey & "' AND customer_utangs.transacnum='" & selectedtransackey & "' ORDER BY customer_payments.date_paid ASC"
                dr = cmd.ExecuteReader

                While dr.Read
                    DataGridView1.Rows.Add(New String() {dr.Item("reference_num").ToString, dr.Item("date_paid"), dr.Item("payment").ToString, dr.Item("payment_type").ToString, dr.Item("status").ToString})
                End While
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message + "Error2!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim amountpayment As Double = TextBox1.Text
        Dim selectedcustomerkey As String
        Dim selectedcustomervalue As String
        Dim selectedtransackey As String
        Dim selectedtransacvalue As String
        Dim partialpayment As Double = 0
        Dim remainingbalance As Double = 0
        Dim referencenum As String = TextBox4.Text
        Dim paymenttype As String = ""
        Dim bankname = TextBox5.Text
        Dim chequenumber = TextBox3.Text
        Dim chequedate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim datetransac As Date
        Dim utangterm As Integer

        Try
            selectedcustomerkey = DirectCast(ComboBox5.SelectedItem, KeyValuePair(Of String, String)).Key
            selectedcustomervalue = DirectCast(ComboBox5.SelectedItem, KeyValuePair(Of String, String)).Value
            selectedtransackey = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
            selectedtransacvalue = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Value
        Catch ex As Exception
            selectedcustomerkey = ""
            selectedcustomervalue = ""
            selectedtransackey = ""
            selectedtransacvalue = ""
        End Try

        If paymentTypeCombo.Text = "Cheque" Then
            paymenttype = "Cheque"
        ElseIf paymentTypeCombo.Text = "Cash" Then
            paymenttype = "Cash"
        End If

        If amountpayment <= 0 Then
            MessageBox.Show("Payment can not be 0 or less!")
        ElseIf amountpayment > displayedbalance Then
            MessageBox.Show("Payment can not be more than the balance!")
        ElseIf paymenttype = "" Then
            MessageBox.Show("Please select payment type first!.")
        Else
            Try
                If dbconn.State = ConnectionState.Open Then
                    dbconn.Close()
                End If
                dbconn.Open()
                With cmd
                    .Connection = dbconn
                    .CommandText = "INSERT INTO customer_payments VALUES('" & referencenum & "','" & selectedcustomerkey & "','" & selectedtransackey & "','" & DateTime.Now.Date.ToString("yyyy-MM-dd") & "','" & amountpayment & "','" & paymenttype & "')"
                    .ExecuteNonQuery()
                    MessageBox.Show("Payment saved!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    TextBox1.Clear()
                End With
            Catch ex As Exception
                MessageBox.Show("Unit Not Added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

            dbconn.Close()
            dbconn.Dispose()

            If paymenttype = "Cheque" Then
                Try
                    If dbconn.State = ConnectionState.Open Then
                        dbconn.Close()
                    End If
                    dbconn.Open()
                    With cmd
                        .Connection = dbconn
                        .CommandText = "INSERT INTO cheque_infos VALUES('" & referencenum & "','" & bankname & "','" & chequenumber & "','" & chequedate & "','Predep','')"
                        .ExecuteNonQuery()
                        TextBox1.Clear()
                    End With
                Catch ex As Exception
                    MessageBox.Show("Unit Not Added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

                dbconn.Close()
                dbconn.Dispose()
            ElseIf paymenttype = "Cash" Then
                Try

                    checkstate()
                    dbconn.Open()

                    With cmd
                        .Connection = dbconn
                        .CommandText = "SELECT * FROM customer_utangs WHERE custcode='" & selectedcustomerkey & "' AND transacnum='" & selectedtransackey & "'"
                        dr = cmd.ExecuteReader

                        While dr.Read

                            partialpayment = dr.Item("partial")
                            remainingbalance = dr.Item("balance")

                        End While
                    End With

                Catch ex As Exception
                    MessageBox.Show(ex.Message + "Error1!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

                dbconn.Close()
                dbconn.Dispose()

                partialpayment = amountpayment + partialpayment
                remainingbalance = remainingbalance - amountpayment

                Dim statusupdate As String = ""

                If remainingbalance = 0 Then
                    statusupdate = "Paid"
                Else
                    statusupdate = "Unpaid"
                End If

                Try
                    checkstate()
                    dbconn.Open()

                    With cmd

                        .Connection = dbconn
                        .CommandText = "UPDATE customer_utangs SET partial = '" & partialpayment & "', balance = '" & remainingbalance & "',status = '" & statusupdate & "' WHERE transacnum='" & selectedtransackey & "' and custcode = '" & selectedcustomerkey & "'"
                        .ExecuteNonQuery()

                    End With
                Catch ex As Exception
                    MessageBox.Show("Update in customer_utangs error! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                dbconn.Close()
                dbconn.Dispose()
            End If
        End If

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT customer_utangs.*,(SELECT resibo.finaltotal FROM resibo WHERE transacnum = '" & selectedtransackey & "') as totalutangs,(SELECT resibo.transacdate FROM resibo WHERE transacnum = '" & selectedtransackey & "') as datetransac FROM customer_utangs WHERE customer_utangs.custcode='" & selectedcustomerkey & "' AND customer_utangs.transacnum='" & selectedtransackey & "'"
                dr = cmd.ExecuteReader

                While dr.Read
                    Label7.Text = dr.Item("totalutangs")
                    Label5.Text = dr.Item("partial")
                    Label6.Text = dr.Item("balance")
                    utangterm = dr.Item("term")
                    datetransac = dr.Item("datetransac")
                    Label13.Text = dr.Item("status")

                    displayedbalance = dr.Item("balance")

                    If dr.Item("status") = "Paid" Then
                        Button2.Enabled = False
                    Else
                        Button2.Enabled = True
                    End If

                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error1!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        Label11.Text = DateAdd(DateInterval.Day, utangterm, datetransac).ToString("MMMM dd,yyyy")

        Try

            checkstate()
            dbconn.Open()

            DataGridView1.Rows.Clear()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT customer_payments.*,cheque_infos.*,customer_utangs.*,resibo.* FROM customer_utangs LEFT JOIN customer_payments ON customer_utangs.transacnum = customer_payments.transacnum AND customer_utangs.custcode = customer_payments.custcode LEFT JOIN resibo ON customer_utangs.transacnum = resibo.transacnum LEFT JOIN cheque_infos ON customer_payments.reference_num = cheque_infos.reference_num WHERE customer_utangs.custcode='" & selectedcustomerkey & "' AND customer_utangs.transacnum='" & selectedtransackey & "' ORDER BY customer_payments.date_paid ASC"
                dr = cmd.ExecuteReader

                While dr.Read
                    DataGridView1.Rows.Add(New String() {dr.Item("reference_num").ToString, dr.Item("date_paid"), dr.Item("payment").ToString, dr.Item("payment_type").ToString, dr.Item("status").ToString})
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error2!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        TextBox1.Text = 0.0
        TextBox4.Clear()
        paymentTypeCombo.SelectedIndex = -1
        DateTimePicker1.Enabled = False
        TextBox5.ReadOnly = True
        TextBox5.Clear()
        TextBox3.ReadOnly = True
        TextBox3.Clear()

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or Asc(e.KeyChar) = 8)
        If (e.KeyChar.ToString = ".") And (TextBox1.Text.Contains(e.KeyChar.ToString)) Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub TextBox1_Enter(sender As Object, e As EventArgs) Handles TextBox1.Enter
        If String.IsNullOrEmpty(TextBox1.Text) Or TextBox1.Text = 0.0 Then
            TextBox1.Text = ""
        End If
    End Sub

    Private Sub TextBox1_Leave(sender As Object, e As EventArgs) Handles TextBox1.Leave
        If TextBox1.Text = "" Or TextBox1.Text = "0" Then
            TextBox1.Text = "0.0"
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles paymentTypeCombo.SelectedIndexChanged
       
        If paymentTypeCombo.SelectedItem = "Cash" Then
            TextBox3.ReadOnly = True
            TextBox5.ReadOnly = True
            DateTimePicker1.Enabled = False
        ElseIf paymentTypeCombo.SelectedItem = "Cheque" Then
            TextBox3.ReadOnly = False
            TextBox5.ReadOnly = False
            DateTimePicker1.Enabled = True
        End If
    End Sub
End Class