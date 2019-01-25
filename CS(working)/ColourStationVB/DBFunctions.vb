Imports MySql.Data.MySqlClient

Module DBFunctions

    Public dbconn As New MySqlConnection("server=localhost;userid=root;password=;database=csdb")
    Public cmd As New MySqlCommand
    Public dr As MySqlDataReader

    Public dbconn2 As New MySqlConnection("server=localhost;userid=root;password=;database=csdb")
    Public cmd2 As New MySqlCommand
    Public dr2 As MySqlDataReader

    Public dbconn3 As New MySqlConnection("server=localhost;userid=root;password=;database=csdb")
    Public cmd3 As New MySqlCommand
    Public dr3 As MySqlDataReader

    Dim prodname, prodcode, barcode As String
    Dim tandf As Boolean = False
    Dim count As Integer = 0

    Public Function checkstate() As Boolean
        Try
            If dbconn.State = ConnectionState.Open Then
                'MessageBox.Show("Database is open!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error)
                dbconn.Close()
            End If
        Catch ex As Exception
            MessageBox.Show("Database Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return Nothing
    End Function
    Public Function loadreleasednums()
        Dim releasevalue As New Dictionary(Of String, String)()
        Dim releaseauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            View_Release.ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM stocks_release"
                dr = cmd.ExecuteReader

                While dr.Read

                    releasevalue.Add(dr.Item("releasereceiptnum"), dr.Item("releasereceiptnum"))
                    releaseauto.AddRange(New String() {dr.Item("releasereceiptnum")})

                    tandf = True
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "loadrelease!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        If tandf Then
            Try
                View_Release.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                View_Release.ComboBox1.AutoCompleteCustomSource = releaseauto
                View_Release.ComboBox1.DataSource = New BindingSource(releasevalue, Nothing)
                View_Release.ComboBox1.DisplayMember = "Value"
                View_Release.ComboBox1.ValueMember = "Key"
                View_Release.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Load loadrelease", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Released Transactions Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function
    Public Function loadcustomerswithutangs()
        Dim custvalue As New Dictionary(Of String, String)()
        Dim custauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            paycharges.ComboBox5.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT customers.*,resibo.* FROM csdb.resibo LEFT JOIN customers ON resibo.custcode = customers.custcode WHERE resibo.payment_type = 'Charge'"
                dr = cmd.ExecuteReader

                While dr.Read

                    custvalue.Add(dr.Item("custcode"), dr.Item("lname") + ", " + dr.Item("fname") + " " + dr.Item("mname"))
                    custauto.AddRange(New String() {dr.Item("lname") + ", " + dr.Item("fname") + " " + dr.Item("mname")})

                    tandf = True
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "loadcustomer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        If tandf Then
            Try
                paycharges.ComboBox5.AutoCompleteSource = AutoCompleteSource.CustomSource
                paycharges.ComboBox5.AutoCompleteCustomSource = custauto
                paycharges.ComboBox5.DataSource = New BindingSource(custvalue, Nothing)
                paycharges.ComboBox5.DisplayMember = "Value"
                paycharges.ComboBox5.ValueMember = "Key"
                paycharges.ComboBox5.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Load loadcustomer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Customer Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function
    Public Function loadcustomer()
        Dim custvalue As New Dictionary(Of String, String)()
        Dim custauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            invoice.ComboBox5.DataSource = Nothing
            invoice_others.ComboBox5.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.customers"
                dr = cmd.ExecuteReader

                While dr.Read

                    custvalue.Add(dr.Item("custcode"), dr.Item("lname") + ", " + dr.Item("fname") + " " + dr.Item("mname"))
                    custauto.AddRange(New String() {dr.Item("lname") + ", " + dr.Item("fname") + " " + dr.Item("mname")})

                    tandf = True
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "loadcustomer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        If tandf Then
            Try
                invoice.ComboBox5.AutoCompleteSource = AutoCompleteSource.CustomSource
                invoice.ComboBox5.AutoCompleteCustomSource = custauto
                invoice.ComboBox5.DataSource = New BindingSource(custvalue, Nothing)
                invoice.ComboBox5.DisplayMember = "Value"
                invoice.ComboBox5.ValueMember = "Key"
                invoice.ComboBox5.SelectedIndex = -1

                invoice_others.ComboBox5.AutoCompleteSource = AutoCompleteSource.CustomSource
                invoice_others.ComboBox5.AutoCompleteCustomSource = custauto
                invoice_others.ComboBox5.DataSource = New BindingSource(custvalue, Nothing)
                invoice_others.ComboBox5.DisplayMember = "Value"
                invoice_others.ComboBox5.ValueMember = "Key"
                invoice_others.ComboBox5.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Load loadcustomer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Customer Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function
    Public Function loadunpaidpayments()
        Dim paymentsnoticecount As Integer = 0
        Dim x As Integer = 0

        paymentsnotice.DataGridView1.Rows.Clear()

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT stocks_acquired.*,supplier.* FROM csdb.stocks_acquired LEFT JOIN supplier ON stocks_acquired.supplier_code=supplier.supplier_code WHERE payment_status = 'Unpaid'"
                dr = cmd.ExecuteReader

                While dr.Read
                    paymentsnoticecount = paymentsnoticecount + 1

                    paymentsnotice.dataArray(x, 0) = dr.Item("drnum")
                    paymentsnotice.dataArray(x, 1) = dr.Item("transactype")
                    paymentsnotice.dataArray(x, 2) = dr.Item("transac_tax")
                    paymentsnotice.dataArray(x, 3) = dr.Item("datereceived")
                    paymentsnotice.dataArray(x, 4) = dr.Item("transacdate")
                    paymentsnotice.dataArray(x, 5) = dr.Item("supplier_name")
                    paymentsnotice.dataArray(x, 6) = dr.Item("forwarder")
                    paymentsnotice.dataArray(x, 7) = dr.Item("waybillnum")
                    paymentsnotice.dataArray(x, 8) = dr.Item("receivedby")
                    paymentsnotice.dataArray(x, 9) = dr.Item("remarks")
                    paymentsnotice.dataArray(x, 10) = dr.Item("payment_status")
                    paymentsnotice.dataArray(x, 11) = dr.Item("amount")
                    paymentsnotice.dataArray(x, 12) = dr.Item("balance")

                    paymentsnotice.dataArray(x, 13) = DateAdd(DateInterval.Day, Convert.ToDouble(dr.Item("payment_dues")), dr.Item("datereceived"))

                    paymentsnotice.DataGridView1.Rows.Add(paymentsnotice.dataArray(x, 0), paymentsnotice.dataArray(x, 1), paymentsnotice.dataArray(x, 2), paymentsnotice.dataArray(x, 3), paymentsnotice.dataArray(x, 4), paymentsnotice.dataArray(x, 5), paymentsnotice.dataArray(x, 6), paymentsnotice.dataArray(x, 7), paymentsnotice.dataArray(x, 8), paymentsnotice.dataArray(x, 9), paymentsnotice.dataArray(x, 10), paymentsnotice.dataArray(x, 11), paymentsnotice.dataArray(x, 12), paymentsnotice.dataArray(x, 13))

                    x = x + 1
                End While
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        welcome.Label10.Text = paymentsnoticecount

        Return Nothing
    End Function

    Public Function loadunpaidpaymentsclick(ByVal e As String)
        Dim paymentsnoticecount As Integer = 0
        Dim x As Integer = 0

        paymentsnotice.DataGridView1.Rows.Clear()

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT stocks_acquired.*,supplier.* FROM csdb.stocks_acquired LEFT JOIN supplier ON stocks_acquired.supplier_code=supplier.supplier_code WHERE payment_status = 'Unpaid' AND supplier.supplier_name LIKE '%" & e & "%'"
                dr = cmd.ExecuteReader

                While dr.Read
                    paymentsnoticecount = paymentsnoticecount + 1

                    paymentsnotice.dataArray(x, 0) = dr.Item("drnum")
                    paymentsnotice.dataArray(x, 1) = dr.Item("transactype")
                    paymentsnotice.dataArray(x, 2) = dr.Item("transac_tax")
                    paymentsnotice.dataArray(x, 3) = dr.Item("datereceived")
                    paymentsnotice.dataArray(x, 4) = dr.Item("transacdate")
                    paymentsnotice.dataArray(x, 5) = dr.Item("supplier_name")
                    paymentsnotice.dataArray(x, 6) = dr.Item("forwarder")
                    paymentsnotice.dataArray(x, 7) = dr.Item("waybillnum")
                    paymentsnotice.dataArray(x, 8) = dr.Item("receivedby")
                    paymentsnotice.dataArray(x, 9) = dr.Item("remarks")
                    paymentsnotice.dataArray(x, 10) = dr.Item("payment_status")
                    paymentsnotice.dataArray(x, 11) = dr.Item("amount")
                    paymentsnotice.dataArray(x, 12) = dr.Item("balance")

                    paymentsnotice.dataArray(x, 13) = DateAdd(DateInterval.Day, Convert.ToDouble(dr.Item("payment_dues")), dr.Item("datereceived"))

                    paymentsnotice.DataGridView1.Rows.Add(paymentsnotice.dataArray(x, 0), paymentsnotice.dataArray(x, 1), paymentsnotice.dataArray(x, 2), paymentsnotice.dataArray(x, 3), paymentsnotice.dataArray(x, 4), paymentsnotice.dataArray(x, 5), paymentsnotice.dataArray(x, 6), paymentsnotice.dataArray(x, 7), paymentsnotice.dataArray(x, 8), paymentsnotice.dataArray(x, 9), paymentsnotice.dataArray(x, 10), paymentsnotice.dataArray(x, 11), paymentsnotice.dataArray(x, 12), paymentsnotice.dataArray(x, 13))

                    x = x + 1
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        welcome.Label10.Text = paymentsnoticecount

        Return Nothing
    End Function

    Public Function loadoutofstock()
        Dim outstocks As Integer = 0
        outofstocklist.DataGridView1.Rows.Clear()
        Try

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT products.*,type.*,units.* FROM products LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN units ON products.unitID = units.unitID WHERE products.stock='0'"
                dr = cmd.ExecuteReader

                While dr.Read
                    outstocks = outstocks + 1
                    outofstocklist.DataGridView1.Rows.Add(dr.Item("prodcode"), dr.Item("prodname"), dr.Item("type"), dr.Item("unit"), dr.Item("stock"), dr.Item("srp"), dr.Item("proddesc"))
                End While
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        welcome.Label2.Text = outstocks

        Return Nothing
    End Function

    Public Function loadoutofstocksearchedclick(ByVal e)
        Dim outstocks As Integer = 0


        If e = "" Then
            MsgBox("No input!")
        Else

            outofstocklist.DataGridView1.Rows.Clear()
            Try

                checkstate()
                dbconn.Open()

                With cmd2
                    .Connection = dbconn
                    .CommandText = "SELECT products.*,type.*,units.* FROM products LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN units ON products.unitID = units.unitID WHERE products.stock = '0' AND  products.prodname LIKE '%" & e & "%'"
                    dr2 = cmd2.ExecuteReader

                    While dr2.Read
                        outstocks = outstocks + 1
                        outofstocklist.DataGridView1.Rows.Add(dr2.Item("prodcode"), dr2.Item("prodname"), dr2.Item("type"), dr2.Item("unit"), dr2.Item("srp"), dr2.Item("proddesc"))
                    End While
                End With

            Catch ex As Exception
                MessageBox.Show(ex.Message + "Error!", "Record not found!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
        dbconn.Close()
        dbconn.Dispose()

        welcome.Label7.Text = outstocks

        Return Nothing
    End Function

    
    Public Function loadnearoutofstock()
        Dim nearoutstocks As Integer = 0
        nearoutofstock.DataGridView1.Rows.Clear()
        Try

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT products.*,type.*,units.* FROM products LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN units ON products.unitID = units.unitID WHERE products.stock BETWEEN '1' AND '100' "
                dr = cmd.ExecuteReader

                While dr.Read
                    nearoutstocks = nearoutstocks + 1
                    nearoutofstock.DataGridView1.Rows.Add(dr.Item("prodcode"), dr.Item("prodname"), dr.Item("type"), dr.Item("unit"), dr.Item("stock"), dr.Item("srp"), dr.Item("proddesc"))
                End While
            End With

        Catch ex As Exception
            ' MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        welcome.Label5.Text = nearoutstocks

        Return Nothing
    End Function

    Public Function loadnearoutofstock1()
        Dim nearoutstocks As Integer = 0
        nearoutofstock.DataGridView1.Rows.Clear()
        Try

            checkstate()
            dbconn.Open()

            With cmd3
                .Connection = dbconn
                .CommandText = "SELECT products.*,type.*,units.* FROM products LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN units ON products.unitID = units.unitID WHERE products.stock BETWEEN '1' AND '100' "
                dr3 = cmd3.ExecuteReader

                While dr3.Read
                    nearoutstocks = nearoutstocks + 1
                    nearoutofstock.DataGridView1.Rows.Add(dr3.Item("prodcode"), dr3.Item("prodname"), dr3.Item("type"), dr3.Item("unit"), dr3.Item("stock"), dr3.Item("srp"), dr3.Item("proddesc"))
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        welcome.Label5.Text = nearoutstocks

        Return Nothing
    End Function

    Public Function loadnearoutofstockclick(ByVal e As String)
        Dim nearoutstocks As Integer = 0

        If e = "" Then
            MsgBox("No input!")
        Else
            nearoutofstock.DataGridView1.Rows.Clear()
            Try

                checkstate()
                dbconn.Open()

                With cmd
                    .Connection = dbconn
                    .CommandText = "SELECT products.*,type.*,units.* FROM products LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN units ON products.unitID = units.unitID WHERE products.stock BETWEEN '1' AND '100' AND products.prodname LIKE '%" & e & "%'"
                    dr = cmd.ExecuteReader

                    While dr.Read
                        nearoutstocks = nearoutstocks + 1
                        nearoutofstock.DataGridView1.Rows.Add(dr.Item("prodcode"), dr.Item("prodname"), dr.Item("type"), dr.Item("unit"), dr.Item("stock"), dr.Item("srp"), dr.Item("proddesc"))
                    End While
                End With

            Catch ex As Exception
                MessageBox.Show("Error!", "Record not found!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            dbconn.Close()
            dbconn.Dispose()
        End If
        welcome.Label5.Text = nearoutstocks

        Return Nothing
    End Function

   


    Public Function loadoverstock()
        Dim overstocks As Integer = 0

        overstock.DataGridView1.Rows.Clear()
        Try

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT products.*,type.*,units.* FROM products LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN units ON products.unitID = units.unitID WHERE products.stock > '3999'"
                dr = cmd.ExecuteReader

                While dr.Read
                    overstocks = overstocks + 1
                    overstock.DataGridView1.Rows.Add(dr.Item("prodcode"), dr.Item("prodname"), dr.Item("type"), dr.Item("unit"), dr.Item("stock"), dr.Item("srp"), dr.Item("proddesc"))
                End While
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        welcome.Label7.Text = overstocks

        Return Nothing
    End Function

    Public Function loadoverstockclick(ByVal e As String)
        Dim overstocks As Integer = 0

        overstock.DataGridView1.Rows.Clear()
        Try

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT products.*,type.*,units.* FROM products LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN units ON products.unitID = units.unitID WHERE products.stock > '3999' AND products.prodname = '%" & e & "%'"
                dr = cmd.ExecuteReader

                While dr.Read
                    overstocks = overstocks + 1
                    overstock.DataGridView1.Rows.Add(dr.Item("prodcode"), dr.Item("prodname"), dr.Item("type"), dr.Item("unit"), dr.Item("stock"), dr.Item("srp"), dr.Item("proddesc"))
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        welcome.Label7.Text = overstocks

        Return Nothing
    End Function

    Public Function loadpintor()
        Dim agentvalue As New Dictionary(Of String, String)()
        Dim agentauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            invoice.ComboBox3.DataSource = Nothing
            invoice_others.ComboBox3.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.accounts WHERE acctype='Pintor'"
                dr = cmd.ExecuteReader

                While dr.Read

                    agentvalue.Add(dr.Item("accnum"), dr.Item("acclname") + ", " + dr.Item("accfname") + " " + dr.Item("accmname"))
                    agentauto.AddRange(New String() {dr.Item("acclname") + ", " + dr.Item("accfname") + " " + dr.Item("accmname")})

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
                invoice.ComboBox3.AutoCompleteSource = AutoCompleteSource.CustomSource
                invoice.ComboBox3.AutoCompleteCustomSource = agentauto
                invoice.ComboBox3.DataSource = New BindingSource(agentvalue, Nothing)
                invoice.ComboBox3.DisplayMember = "Value"
                invoice.ComboBox3.ValueMember = "Key"
                invoice.ComboBox3.SelectedIndex = -1

                invoice_others.ComboBox3.AutoCompleteSource = AutoCompleteSource.CustomSource
                invoice_others.ComboBox3.AutoCompleteCustomSource = agentauto
                invoice_others.ComboBox3.DataSource = New BindingSource(agentvalue, Nothing)
                invoice_others.ComboBox3.DisplayMember = "Value"
                invoice_others.ComboBox3.ValueMember = "Key"
                invoice_others.ComboBox3.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Load Pintor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Pintor Found! Please Add One First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function
    Public Function loadagents()
        Dim agentvalue As New Dictionary(Of String, String)()
        Dim agentauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            invoice.ComboBox1.DataSource = Nothing
            invoice_others.ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.accounts WHERE acctype='Agent/Canvasser'"
                dr = cmd.ExecuteReader

                While dr.Read

                    agentvalue.Add(dr.Item("accnum"), dr.Item("acclname") + ", " + dr.Item("accfname") + " " + dr.Item("accmname"))
                    agentauto.AddRange(New String() {dr.Item("acclname") + ", " + dr.Item("accfname") + " " + dr.Item("accmname")})

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
                invoice.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                invoice.ComboBox1.AutoCompleteCustomSource = agentauto
                invoice.ComboBox1.DataSource = New BindingSource(agentvalue, Nothing)
                invoice.ComboBox1.DisplayMember = "Value"
                invoice.ComboBox1.ValueMember = "Key"
                invoice.ComboBox1.SelectedIndex = -1

                invoice_others.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                invoice_others.ComboBox1.AutoCompleteCustomSource = agentauto
                invoice_others.ComboBox1.DataSource = New BindingSource(agentvalue, Nothing)
                invoice_others.ComboBox1.DisplayMember = "Value"
                invoice_others.ComboBox1.ValueMember = "Key"
                invoice_others.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Load Agents", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Agent/Canvasser Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function
    Public Function loadmixxer()
        Dim mixxervalue As New Dictionary(Of String, String)()
        Dim mixxerauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            cart.ComboBox2.DataSource = Nothing
            cart_others.ComboBox2.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.accounts WHERE acctype='Mixxer'"
                dr = cmd.ExecuteReader

                While dr.Read

                    mixxervalue.Add(dr.Item("accnum"), dr.Item("acclname") + ", " + dr.Item("accfname") + " " + dr.Item("accmname"))
                    mixxerauto.AddRange(New String() {dr.Item("acclname") + ", " + dr.Item("accfname") + " " + dr.Item("accmname")})

                    tandf = True
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + " loadmixxer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        If tandf Then
            Try
                cart.ComboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource
                cart.ComboBox2.AutoCompleteCustomSource = mixxerauto
                cart.ComboBox2.DataSource = New BindingSource(mixxervalue, Nothing)
                cart.ComboBox2.DisplayMember = "Value"
                cart.ComboBox2.ValueMember = "Key"
                cart.ComboBox2.SelectedIndex = -1

                cart_others.ComboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource
                cart_others.ComboBox2.AutoCompleteCustomSource = mixxerauto
                cart_others.ComboBox2.DataSource = New BindingSource(mixxervalue, Nothing)
                cart_others.ComboBox2.DisplayMember = "Value"
                cart_others.ComboBox2.ValueMember = "Key"
                cart_others.ComboBox2.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Mixxer Accounts", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Mixxer Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function
    Public Function loadallaccounts()
        Dim agentvalue As New Dictionary(Of String, String)()
        Dim agentauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            invoice.ComboBox3.DataSource = Nothing
            invoice_others.ComboBox3.DataSource = Nothing
            deleteaccount.ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.accounts"
                dr = cmd.ExecuteReader

                While dr.Read

                    agentvalue.Add(dr.Item("accnum"), dr.Item("acclname") + ", " + dr.Item("accfname") + " " + dr.Item("accmname"))
                    agentauto.AddRange(New String() {dr.Item("acclname") + ", " + dr.Item("accfname") + " " + dr.Item("accmname")})

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
                deleteaccount.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                deleteaccount.ComboBox1.AutoCompleteCustomSource = agentauto
                deleteaccount.ComboBox1.DataSource = New BindingSource(agentvalue, Nothing)
                deleteaccount.ComboBox1.DisplayMember = "Value"
                deleteaccount.ComboBox1.ValueMember = "Key"
                deleteaccount.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Load All Accounts", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Accounts Found! Please Add One First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function
    Public Function loaddisabledaccounts()
        Dim accvalue As New Dictionary(Of String, String)()
        Dim accauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            enableaccount.ComboBox2.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.user WHERE status='0'"
                dr = cmd.ExecuteReader

                While dr.Read

                    accvalue.Add(dr.Item("employeeID"), dr.Item("fname") + ": " + dr.Item("employeeID"))
                    accauto.AddRange(New String() {dr.Item("fname") + ": " + dr.Item("employeeID")})

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
                enableaccount.ComboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource
                enableaccount.ComboBox2.AutoCompleteCustomSource = accauto
                enableaccount.ComboBox2.DataSource = New BindingSource(accvalue, Nothing)
                enableaccount.ComboBox2.DisplayMember = "Value"
                enableaccount.ComboBox2.ValueMember = "Key"
                enableaccount.ComboBox2.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Edit Accounts", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Records Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function
    Public Function loadactiveaccounts()
        Dim accvalue As New Dictionary(Of String, String)()
        Dim accauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            editaccount.ComboBox2.DataSource = Nothing
            disableaccount.ComboBox2.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.user WHERE status='1'"
                dr = cmd.ExecuteReader

                While dr.Read

                    accvalue.Add(dr.Item("employeeID"), dr.Item("fname") + ": " + dr.Item("employeeID"))
                    accauto.AddRange(New String() {dr.Item("fname") + ": " + dr.Item("employeeID")})

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
                editaccount.ComboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource
                editaccount.ComboBox2.AutoCompleteCustomSource = accauto
                editaccount.ComboBox2.DataSource = New BindingSource(accvalue, Nothing)
                editaccount.ComboBox2.DisplayMember = "Value"
                editaccount.ComboBox2.ValueMember = "Key"
                editaccount.ComboBox2.SelectedIndex = -1

                disableaccount.ComboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource
                disableaccount.ComboBox2.AutoCompleteCustomSource = accauto
                disableaccount.ComboBox2.DataSource = New BindingSource(accvalue, Nothing)
                disableaccount.ComboBox2.DisplayMember = "Value"
                disableaccount.ComboBox2.ValueMember = "Key"
                disableaccount.ComboBox2.SelectedIndex = -1
            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Edit Accounts", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Records Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function

    Public Function loaddestinations()
        Dim destinationvalue As New Dictionary(Of String, String)()
        Dim destinationauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            printbydestination.ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.stocks_release"
                dr = cmd.ExecuteReader

                While dr.Read

                    destinationvalue.Add(dr.Item("releasereceiptnum"), dr.Item("destination"))
                    destinationauto.AddRange(New String() {dr.Item("destination")})

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
                printbydestination.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                printbydestination.ComboBox1.AutoCompleteCustomSource = destinationauto
                printbydestination.ComboBox1.DataSource = New BindingSource(destinationvalue, Nothing)
                printbydestination.ComboBox1.DisplayMember = "Value"
                printbydestination.ComboBox1.ValueMember = "Key"
                printbydestination.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Destinations", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Records Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function

    Public Function loadtransactions()
        Dim transacnumvals As New Dictionary(Of String, String)()
        Dim transacnumauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            returns.ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.resibo"
                dr = cmd.ExecuteReader

                While dr.Read

                    transacnumvals.Add(dr.Item("transacnum"), dr.Item("transacnum"))
                    transacnumauto.AddRange(New String() {dr.Item("transacnum")})

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
                returns.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                returns.ComboBox1.AutoCompleteCustomSource = transacnumauto
                returns.ComboBox1.DataSource = New BindingSource(transacnumvals, Nothing)
                returns.ComboBox1.DisplayMember = "Value"
                returns.ComboBox1.ValueMember = "Key"
                returns.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Transaction Numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Records Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function

    Public Function loadtransacnums()
        Dim transacnumvals As New Dictionary(Of String, String)()
        Dim transacnumauto As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            printbytransacnumber.ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.stocks_acquired"
                dr = cmd.ExecuteReader

                While dr.Read

                    transacnumvals.Add(dr.Item("drnum"), dr.Item("drnum"))
                    transacnumauto.AddRange(New String() {dr.Item("drnum")})

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
                printbytransacnumber.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                printbytransacnumber.ComboBox1.AutoCompleteCustomSource = transacnumauto
                printbytransacnumber.ComboBox1.DataSource = New BindingSource(transacnumvals, Nothing)
                printbytransacnumber.ComboBox1.DisplayMember = "Value"
                printbytransacnumber.ComboBox1.ValueMember = "Key"
                printbytransacnumber.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Transaction Numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Records Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function

    Public Function loadsuppliers()
        Dim suppliersource As New Dictionary(Of String, String)()
        Dim supplierautocomplete As New AutoCompleteStringCollection
        Dim tandf As Boolean = False
        
        Try
            printbyrangewithsupplier.ComboBox1.DataSource = Nothing
            addstock.ComboBox2.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.supplier"
                dr = cmd.ExecuteReader

                While dr.Read

                    suppliersource.Add(dr.Item("supplier_code"), dr.Item("supplier_name") + "(" + dr.Item("payment_dues") + " days)")
                    supplierautocomplete.AddRange(New String() {dr.Item("supplier_name") + "(" + dr.Item("payment_dues") + " days)"})

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

                addstock.ComboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource
                addstock.ComboBox2.AutoCompleteCustomSource = supplierautocomplete
                addstock.ComboBox2.DataSource = New BindingSource(suppliersource, Nothing)
                addstock.ComboBox2.DisplayMember = "Value"
                addstock.ComboBox2.ValueMember = "Key"
                addstock.ComboBox2.SelectedIndex = -1

                printbyrangewithsupplier.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                printbyrangewithsupplier.ComboBox1.AutoCompleteCustomSource = supplierautocomplete
                printbyrangewithsupplier.ComboBox1.DataSource = New BindingSource(suppliersource, Nothing)
                printbyrangewithsupplier.ComboBox1.DisplayMember = "Value"
                printbyrangewithsupplier.ComboBox1.ValueMember = "Key"
                printbyrangewithsupplier.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Supplier", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Records Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function

    Public Function loadunits()
        Dim unitsource As New Dictionary(Of String, String)()
        Dim unitautocomplete As New AutoCompleteStringCollection

        Dim unitsource1 As New Dictionary(Of String, String)()
        Dim unitautocomplete1 As New AutoCompleteStringCollection

        Dim tandf As Boolean = False

        Try

            addproduct.ComboBox2.DataSource = Nothing
            editproduct.ComboBox2.DataSource = Nothing
            addproduct.ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.units"
                dr = cmd.ExecuteReader

                    While dr.Read

                        unitsource.Add(dr.Item("unitID"), dr.Item("unit"))
                        unitautocomplete.AddRange(New String() {dr.Item("unit")})

                    tandf = True


                End While
                dbconn.Close()
                dbconn.Dispose()
            End With

            checkstate()
            dbconn.Open()

            With cmd2
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.unitword"
                dr2 = cmd2.ExecuteReader

                While dr2.Read

                    unitsource1.Add(dr2.Item("unit_ID"), dr2.Item("unitname"))
                    unitautocomplete1.AddRange(New String() {dr2.Item("unitname")})

                    tandf = True

                   

                End While
                dbconn.Close()
                dbconn.Dispose()
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error1!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

      

        If tandf Then
            Try

                addproduct.ComboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource
                addproduct.ComboBox2.AutoCompleteCustomSource = unitautocomplete
                addproduct.ComboBox2.DataSource = New BindingSource(unitsource, Nothing)
                addproduct.ComboBox2.DisplayMember = "Value"
                addproduct.ComboBox2.ValueMember = "Key"
                addproduct.ComboBox2.SelectedIndex = -1

                addproduct.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                addproduct.ComboBox1.AutoCompleteCustomSource = unitautocomplete1
                addproduct.ComboBox1.DataSource = New BindingSource(unitsource1, Nothing)
                addproduct.ComboBox1.DisplayMember = "Value"
                addproduct.ComboBox1.ValueMember = "Key"
                addproduct.ComboBox1.SelectedIndex = -1

                editproduct.ComboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource
                editproduct.ComboBox2.AutoCompleteCustomSource = unitautocomplete
                editproduct.ComboBox2.DataSource = New BindingSource(unitsource, Nothing)
                editproduct.ComboBox2.DisplayMember = "Value"
                editproduct.ComboBox2.ValueMember = "Key"
                editproduct.ComboBox2.SelectedIndex = -1

                editproduct.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                editproduct.ComboBox1.AutoCompleteCustomSource = unitautocomplete1
                editproduct.ComboBox1.DataSource = New BindingSource(unitsource1, Nothing)
                editproduct.ComboBox1.DisplayMember = "Value"
                editproduct.ComboBox1.ValueMember = "Key"
                editproduct.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Unit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Records Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function

    Public Function loadtypes()

        Dim typesource As New Dictionary(Of String, String)()
        Dim typeautocomplete As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try

            addproduct.ComboBox3.DataSource = Nothing
            editproduct.ComboBox3.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.type"
                dr = cmd.ExecuteReader

                While dr.Read

                    typesource.Add(dr.Item("typeID"), dr.Item("type"))
                    typeautocomplete.AddRange(New String() {dr.Item("type")})

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
                addproduct.ComboBox3.AutoCompleteSource = AutoCompleteSource.CustomSource
                addproduct.ComboBox3.AutoCompleteCustomSource = typeautocomplete
                addproduct.ComboBox3.DataSource = New BindingSource(typesource, Nothing)
                addproduct.ComboBox3.DisplayMember = "Value"
                addproduct.ComboBox3.ValueMember = "Key"
                addproduct.ComboBox3.SelectedIndex = -1

                editproduct.ComboBox3.AutoCompleteSource = AutoCompleteSource.CustomSource
                editproduct.ComboBox3.AutoCompleteCustomSource = typeautocomplete
                editproduct.ComboBox3.DataSource = New BindingSource(typesource, Nothing)
                editproduct.ComboBox3.DisplayMember = "Value"
                editproduct.ComboBox3.ValueMember = "Key"
                editproduct.ComboBox3.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No Records Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function
    Public Function loadproducts1gal()

        Dim prodcomboSource As New Dictionary(Of String, String)()
        Dim prodautocomplete As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try
            cart_others.ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT products.*,units.* FROM products LEFT JOIN units ON products.unitID = units.unitID WHERE products.stock != 0 AND units.unit >= '1' AND unitword = 'Kg' OR unitword = 'Liters' "
                dr = cmd.ExecuteReader

                While dr.Read

                    prodcomboSource.Add(dr.Item("prodcode"), dr.Item("prodname") + " : " + dr.Item("barcode"))
                    prodautocomplete.AddRange(New String() {dr.Item("prodname") + " : " + dr.Item("barcode")})

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
                cart_others.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                cart_others.ComboBox1.AutoCompleteCustomSource = prodautocomplete
                cart_others.ComboBox1.DataSource = New BindingSource(prodcomboSource, Nothing)
                cart_others.ComboBox1.DisplayMember = "Value"
                cart_others.ComboBox1.ValueMember = "Key"
                cart_others.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Products", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            'MessageBox.Show("No Products Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function
    

    Public Function loadproductswithstocks()

        Dim prodcomboSource As New Dictionary(Of String, String)()
        Dim prodautocomplete As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try

            releasestock.ComboBox1.DataSource = Nothing
            cart.ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.products WHERE stock <> 0"
                dr = cmd.ExecuteReader

                While dr.Read

                    prodcomboSource.Add(dr.Item("prodcode"), dr.Item("prodname") + " : " + dr.Item("barcode"))
                    prodautocomplete.AddRange(New String() {dr.Item("prodname") + " : " + dr.Item("barcode")})

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

                releasestock.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                releasestock.ComboBox1.AutoCompleteCustomSource = prodautocomplete
                releasestock.ComboBox1.DataSource = New BindingSource(prodcomboSource, Nothing)
                releasestock.ComboBox1.DisplayMember = "Value"
                releasestock.ComboBox1.ValueMember = "Key"
                releasestock.ComboBox1.SelectedIndex = -1

                'cart.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                'cart.ComboBox1.AutoCompleteCustomSource = prodautocomplete
                'cart.ComboBox1.DataSource = New BindingSource(prodcomboSource, Nothing)
                'cart.ComboBox1.DisplayMember = "Value"
                'cart.ComboBox1.ValueMember = "Key"
                'cart.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Products", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else

            'MessageBox.Show("No Products Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function


    'Updates 12 - 13 - 2018
    '=====================================================

    Dim items As List(Of String) = New List(Of String)
    Dim list As List(Of String) = New List(Of String)
    Public Function loadproductsCart()
        Try

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.products WHERE stock <> 0 "
                dr = cmd.ExecuteReader

                While dr.Read
                    list.Add(dr.Item("prodcode") & ", " & dr.Item("prodname") & " : " & dr.Item("barcode"))

                End While
                cart.ComboBox1.Items.AddRange(list.ToArray())
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error1!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return Nothing
    End Function

    Public Function cartTextChanged()

        
        Try

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.products WHERE stock <> 0 AND (prodname LIKE '%" & cart.ComboBox1.Text & "%' OR prodcode LIKE '%" & cart.ComboBox1.Text & "%' OR barcode LIKE '%" & cart.ComboBox1.Text & "%')"
                dr = cmd.ExecuteReader

                cart.ComboBox1.Items.Clear()
                items.Clear()

                While dr.Read
                    prodname = dr.Item("prodname")
                    prodcode = dr.Item("prodcode")
                    barcode = dr.Item("barcode")
                    If prodname.Contains(cart.ComboBox1.Text) OrElse prodcode.Contains(cart.ComboBox1.Text) OrElse barcode.Contains(cart.ComboBox1.Text) Then
                        items.Add(dr.Item("prodcode") & ", " & dr.Item("prodname") & " : " & dr.Item("barcode"))
                        tandf = True
                    End If

                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error1!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        If tandf Then
            Try

                cart.ComboBox1.Items.AddRange(items.ToArray())
                cart.ComboBox1.SelectionStart = cart.ComboBox1.Text.Length
                cart.ComboBox1.DroppedDown = True

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Products", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            Try
                If cart.ComboBox1.Text.Length <= 0 Then
                    'MessageBox.Show("No Products Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    cart.ComboBox1.SelectionStart = cart.ComboBox1.Text.Length
                    cart.ComboBox1.DroppedDown = False
                End If
            Catch ex As Exception
            End Try
        End If

        Return Nothing

    End Function

    Public Function cartSelectItem()
        Try
            Dim comprodcode As String() = cart.ComboBox1.Text.Split(New Char() {" "c})

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT products.*,type.*,units.* FROM csdb.products LEFT JOIN units ON products.unitID=units.unitID LEFT JOIN type ON products.prodtype =type.typeID where prodcode='" & comprodcode(0) & "'"
                dr = cmd.ExecuteReader

                While dr.Read

                    cart.prodcode.Text = dr.Item("barcode")
                    cart.prodname.Text = dr.Item("prodname")
                    cart.produnit.Text = dr.Item("unit") & " " & dr.Item("unitword")
                    cart.prodtype.Text = dr.Item("type")
                    cart.srp.Text = dr.Item("srp")
                    cart.qty.Text = dr.Item("stock")

                    cart.NumericUpDown1.Maximum = dr.Item("stock")
                    cart.NumericUpDown1.Value = 1
                End While

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()
        Return Nothing
    End Function


    '=====================================================
    'End of Update 12 - 13 - 2018


    '=====================================================
    ' Update 12 - 19 - 2018
    Public Function addStockTextChanged()
        Try

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.products WHERE (prodname LIKE '%" & addstock.ComboBox1.Text & "%' OR prodcode LIKE '%" & addstock.ComboBox1.Text & "%' OR barcode LIKE '%" & addstock.ComboBox1.Text & "%')"
                dr = cmd.ExecuteReader

                addstock.ComboBox1.Items.Clear()
                items.Clear()

                While dr.Read
                    prodname = dr.Item("prodname")
                    prodcode = dr.Item("prodcode")
                    barcode = dr.Item("barcode")
                    If prodname.Contains(addstock.ComboBox1.Text) OrElse prodcode.Contains(addstock.ComboBox1.Text) OrElse barcode.Contains(addstock.ComboBox1.Text) Then
                        items.Add(dr.Item("prodcode") & ", " & dr.Item("prodname") & " : " & dr.Item("barcode"))
                        tandf = True
                    End If

                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error1!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        If tandf Then
            Try

                addstock.ComboBox1.Items.AddRange(items.ToArray())
                addstock.ComboBox1.SelectionStart = addstock.ComboBox1.Text.Length
                addstock.ComboBox1.DroppedDown = True
                'addstock.ComboBox1.DropDownHeight = addstock.ComboBox1.Items.Count * 1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Products", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            Try
                If addstock.ComboBox1.Text.Length <= 0 Then
                    'MessageBox.Show("No Products Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    addstock.ComboBox1.SelectionStart = addstock.ComboBox1.Text.Length
                    addstock.ComboBox1.DroppedDown = False
                End If
            Catch ex As Exception
            End Try
        End If
        Return Nothing
    End Function
    '=====================================================

    Public Function loadproductswithstockstextchange()

        Dim prodcomboSource As New Dictionary(Of String, String)()
        Dim prodautocomplete As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try

            releasestock.ComboBox1.DataSource = Nothing
            cart.ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.products WHERE stock <> 0 AND prodname LIKE '%" & cart.ComboBox1.Text & "%'"
                dr = cmd.ExecuteReader

                While dr.Read

                    prodcomboSource.Add(dr.Item("prodcode"), dr.Item("prodname") + " : " + dr.Item("barcode"))
                    prodautocomplete.AddRange(New String() {dr.Item("prodname") + " : " + dr.Item("barcode")})

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

                releasestock.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                releasestock.ComboBox1.AutoCompleteCustomSource = prodautocomplete
                releasestock.ComboBox1.DataSource = New BindingSource(prodcomboSource, Nothing)
                releasestock.ComboBox1.DisplayMember = "Value"
                releasestock.ComboBox1.ValueMember = "Key"
                releasestock.ComboBox1.SelectedIndex = -1

                'cart.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                'cart.ComboBox1.AutoCompleteCustomSource = prodautocomplete
                'cart.ComboBox1.DataSource = New BindingSource(prodcomboSource, Nothing)
                'cart.ComboBox1.DisplayMember = "Value"
                'cart.ComboBox1.ValueMember = "Key"
                'cart.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Products", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else

            'MessageBox.Show("No Products Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Return Nothing
    End Function
    Public Function loadproducts()

        Dim prodcomboSource As New Dictionary(Of String, String)()
        Dim prodautocomplete As New AutoCompleteStringCollection
        Dim tandf As Boolean = False

        Try

            editproduct.ComboBox4.DataSource = Nothing
            addstock.ComboBox1.DataSource = Nothing
            deleteproduct.ComboBox1.DataSource = Nothing

            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM csdb.products"
                dr = cmd.ExecuteReader

                While dr.Read

                    prodcomboSource.Add(dr.Item("prodcode"), dr.Item("prodname") + " : " + dr.Item("barcode"))
                    prodautocomplete.AddRange(New String() {dr.Item("prodname") + " : " + dr.Item("barcode")})

                    tandf = True
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

        If tandf Then
            Try
                editproduct.ComboBox4.AutoCompleteSource = AutoCompleteSource.CustomSource
                editproduct.ComboBox4.AutoCompleteCustomSource = prodautocomplete
                editproduct.ComboBox4.DataSource = New BindingSource(prodcomboSource, Nothing)
                editproduct.ComboBox4.DisplayMember = "Value"
                editproduct.ComboBox4.ValueMember = "Key"
                editproduct.ComboBox4.SelectedIndex = -1

                'addstock.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                'addstock.ComboBox1.AutoCompleteCustomSource = prodautocomplete
                'addstock.ComboBox1.DataSource = New BindingSource(prodcomboSource, Nothing)
                'addstock.ComboBox1.DisplayMember = "Value"
                'addstock.ComboBox1.ValueMember = "Key"
                'addstock.ComboBox1.SelectedIndex = -1

                deleteproduct.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
                deleteproduct.ComboBox1.AutoCompleteCustomSource = prodautocomplete
                deleteproduct.ComboBox1.DataSource = New BindingSource(prodcomboSource, Nothing)
                deleteproduct.ComboBox1.DisplayMember = "Value"
                deleteproduct.ComboBox1.ValueMember = "Key"
                deleteproduct.ComboBox1.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show(ex.ToString + " Error in Products", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            'MessageBox.Show("No Products Found! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        editproduct.TextBox1.Clear()
        editproduct.TextBox3.Clear()
        editproduct.TextBox2.Clear()
        editproduct.TextBox4.Clear()
        editproduct.ComboBox2.SelectedIndex = -1
        editproduct.ComboBox3.SelectedIndex = -1
        editproduct.ComboBox4.SelectedIndex = -1
        editproduct.RichTextBox1.Clear()

        Return Nothing
    End Function

    Public Function viewaddedOnload()

        checkstate()

        Dim tandf As Boolean = False
        Dim autocomplete As New AutoCompleteStringCollection
        Dim comboSource As New Dictionary(Of String, String)()
        Dim x As Integer = 0

        viewadded.ComboBox1.DataSource = Nothing

        Try
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT stocks_acquired.*,supplier.* FROM csdb.stocks_acquired LEFT JOIN supplier ON stocks_acquired.supplier_code=supplier.supplier_code"
                dr = cmd.ExecuteReader

                viewadded.ComboBox1.Items.Clear()
                viewadded.DataGridView1.Rows.Clear()

                While dr.Read

                    comboSource.Add(dr.Item("drnum"), dr.Item("drnum"))
                    autocomplete.AddRange(New String() {dr.Item("drnum")})
                    tandf = True

                    viewadded.dataArray(x, 0) = dr.Item("drnum")
                    viewadded.dataArray(x, 1) = dr.Item("transactype")
                    viewadded.dataArray(x, 2) = dr.Item("transac_tax")
                    viewadded.dataArray(x, 3) = dr.Item("datereceived")
                    viewadded.dataArray(x, 4) = dr.Item("transacdate")
                    viewadded.dataArray(x, 5) = dr.Item("supplier_name")
                    viewadded.dataArray(x, 6) = dr.Item("forwarder")
                    viewadded.dataArray(x, 7) = dr.Item("waybillnum")
                    viewadded.dataArray(x, 8) = dr.Item("receivedby")
                    viewadded.dataArray(x, 9) = dr.Item("remarks")
                    viewadded.dataArray(x, 10) = dr.Item("payment_status")
                    viewadded.dataArray(x, 11) = dr.Item("amount")
                    viewadded.dataArray(x, 12) = dr.Item("balance")

                    viewadded.DataGridView1.Rows.Add(New String() {viewadded.dataArray(x, 0), viewadded.dataArray(x, 1), "", viewadded.dataArray(x, 2), viewadded.dataArray(x, 3), viewadded.dataArray(x, 4), viewadded.dataArray(x, 5), viewadded.dataArray(x, 6), viewadded.dataArray(x, 7), viewadded.dataArray(x, 8), viewadded.dataArray(x, 9), viewadded.dataArray(x, 10), viewadded.dataArray(x, 11), viewadded.dataArray(x, 12)})

                    x = x + 1

                    tandf = True
                End While
                
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()

        If tandf <> True Then
            MessageBox.Show("No records found, Please add some first!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            viewadded.ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
            viewadded.ComboBox1.AutoCompleteCustomSource = autocomplete
            viewadded.ComboBox1.DataSource = New BindingSource(comboSource, Nothing)
            viewadded.ComboBox1.DisplayMember = "Value"
            viewadded.ComboBox1.ValueMember = "Key"

        End If

        

        Return Nothing
    End Function

    Public Function getTemporary(ByVal e As Integer)
        deleteproduct.Label19.Text = e
        Return Nothing
    End Function

End Module
