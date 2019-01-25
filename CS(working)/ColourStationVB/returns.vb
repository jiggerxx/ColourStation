Public Class returns
    Public productsArray(100, 3) As String
    Private Sub returns_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadtransactions()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim transackey As String = ""
        Dim productscounter As Integer = 0

        Try
            transackey = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
        Catch ex As Exception
            transackey = ""
        End Try

        Try
            checkstate()
            dbconn.Open()

            DataGridView1.Rows.Clear()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT resibo_products.*,products.*,units.*,type.* FROM resibo_products LEFT JOIN products ON resibo_products.prodcode = products.prodcode LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN type ON products.prodtype = type.typeID WHERE resibo_products.transacnum='" & transackey & "'"
                dr = cmd.ExecuteReader

                While dr.Read
                    DataGridView1.Rows.Add(New String() {dr.Item("prodcode"), dr.Item("prodname"), dr.Item("unit"), dr.Item("type"), dr.Item("srp"), dr.Item("qty"), "0"})
                End While

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM resibo WHERE transacnum='" & transackey & "'"
                dr = cmd.ExecuteReader

                While dr.Read

                    transacdate.Text = dr.Item("transacdate")
                    cashiername.Text = dr.Item("cashier")
                    paymenttype.Text = dr.Item("payment_type")
                    total.Text = dr.Item("total")
                    discount.Text = dr.Item("discount")
                    totaldiscount.Text = dr.Item("finaltotal")

                End While

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.ColumnIndex = 6 Or e.ColumnIndex = 7 Then
            DataGridView1.ReadOnly = False
        Else
            DataGridView1.ReadOnly = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim transackkey As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
        Dim productcounter As Integer = 0
        Dim checker As Boolean
        Dim newqty As Integer = 0
        Dim strsql As String = ""
        Dim tobededucted As Double = 0
        Dim totalbalance As Double = 0
        Dim fullcounter As Integer = 0

        For i As Integer = 0 To DataGridView1.Rows.Count() - 1
            checker = DataGridView1.Rows(i).Cells(7).Value

            If checker And DataGridView1.Rows(i).Cells(6).Value <> 0 And DataGridView1.Rows(i).Cells(6).Value <= DataGridView1.Rows(i).Cells(5).Value Then
                productsArray(productcounter, 0) = DataGridView1.Rows(i).Cells(0).Value
                productsArray(productcounter, 1) = DataGridView1.Rows(i).Cells(4).Value
                productsArray(productcounter, 2) = DataGridView1.Rows(i).Cells(5).Value
                productsArray(productcounter, 3) = DataGridView1.Rows(i).Cells(6).Value

                productcounter = productcounter + 1
            End If

            If checker And DataGridView1.Rows(i).Cells(6).Value = DataGridView1.Rows(i).Cells(5).Value Then
                fullcounter = fullcounter + 1
            End If

        Next

        '****************************FOR UPDATING INVENTORY****************************'


        If DataGridView1.Rows.Count() = productcounter And fullcounter = DataGridView1.Rows.Count() Then

            Try
                checkstate()
                dbconn.Open()

                With cmd
                    .Connection = dbconn
                    .CommandText = "SELECT * FROM resibo WHERE transacnum='" & transackkey & "'"
                    dr = cmd.ExecuteReader

                    While dr.Read

                        totalbalance = dr.Item("finaltotal")

                    End While

                End With
            Catch ex As Exception
                MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            dbconn.Close()
            dbconn.Dispose()

            Try
                checkstate()
                dbconn.Open()
                With cmd
                    .Connection = dbconn
                    .CommandText = "DELETE FROM resibo_products WHERE transacnum='" & transackkey & "'"
                    .ExecuteNonQuery()
                End With
            Catch ex As Exception
                MessageBox.Show("resibo_products not updated! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            dbconn.Close()
            dbconn.Dispose()

            Try
                checkstate()
                dbconn.Open()
                With cmd
                    .Connection = dbconn
                    .CommandText = "DELETE FROM resibo WHERE transacnum='" & transackkey & "'"
                    .ExecuteNonQuery()
                End With
            Catch ex As Exception
                MessageBox.Show("resibo not updated! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            dbconn.Close()
            dbconn.Dispose()

            Try
                checkstate()
                dbconn.Open()
                With cmd
                    .Connection = dbconn
                    .CommandText = "DELETE FROM customer_utangs WHERE transacnum='" & transackkey & "'"
                    .ExecuteNonQuery()
                End With
            Catch ex As Exception
                MessageBox.Show("customer_utangs not updated! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            dbconn.Close()
            dbconn.Dispose()

            transacdate.Text = "-------------"
            cashiername.Text = "-------------"
            paymenttype.Text = "-------------"
            total.Text = "-------------"
            discount.Text = "-------------"
            totaldiscount.Text = "-------------"

            loadtransactions()

        Else

            For x As Integer = 0 To productcounter - 1

                tobededucted = productsArray(x, 1) * productsArray(x, 3)
                totalbalance = totalbalance + tobededucted
                'resibo_products update
                newqty = productsArray(x, 2) - productsArray(x, 3)

                Try
                    checkstate()
                    dbconn.Open()
                    With cmd
                        .Connection = dbconn

                        If productsArray(x, 2) = productsArray(x, 3) Then
                            strsql = "DELETE FROM resibo_products WHERE transacnum='" & transackkey & "' and prodcode='" & productsArray(x, 0) & "'"
                        Else
                            strsql = "UPDATE resibo_products SET qty='" & newqty & "' WHERE transacnum='" & transackkey & "' and prodcode='" & productsArray(x, 0) & "'"
                        End If

                        .CommandText = strsql
                        .ExecuteNonQuery()
                    End With
                Catch ex As Exception
                    MessageBox.Show("resibo_products not updated! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                dbconn.Close()
                dbconn.Dispose()

                'product stocks update
                Try
                    checkstate()
                    dbconn.Open()
                    With cmd
                        .Connection = dbconn
                        .CommandText = "UPDATE products SET stock = stock + '" & productsArray(x, 3) & "' WHERE prodcode='" & productsArray(x, 0) & "'"
                        .ExecuteNonQuery()
                    End With
                Catch ex As Exception
                    MessageBox.Show("products not updated! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                dbconn.Close()
                dbconn.Dispose()

                'total payments update
                Try
                    checkstate()
                    dbconn.Open()
                    With cmd
                        .Connection = dbconn
                        .CommandText = "UPDATE resibo SET finaltotal = finaltotal - '" & tobededucted & "',total = total - '" & tobededucted & "'  WHERE transacnum='" & transackkey & "'"
                        .ExecuteNonQuery()
                    End With
                Catch ex As Exception
                    MessageBox.Show("resibo not updated! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                dbconn.Close()
                dbconn.Dispose()

                'utang total balance update
                Try
                    checkstate()
                    dbconn.Open()
                    With cmd
                        .Connection = dbconn
                        .CommandText = "UPDATE customer_utangs SET balance = balance - '" & tobededucted & "' WHERE transacnum='" & transackkey & "'"
                        .ExecuteNonQuery()
                    End With
                Catch ex As Exception
                    MessageBox.Show("customer_utangs not updated! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                dbconn.Close()
                dbconn.Dispose()

            Next



        End If

        Try
            checkstate()
            dbconn.Open()

            DataGridView1.Rows.Clear()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT resibo_products.*,products.*,units.*,type.* FROM resibo_products LEFT JOIN products ON resibo_products.prodcode = products.prodcode LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN type ON products.prodtype = type.typeID WHERE resibo_products.transacnum='" & transackkey & "'"
                dr = cmd.ExecuteReader

                While dr.Read
                    DataGridView1.Rows.Add(New String() {dr.Item("prodcode"), dr.Item("prodname"), dr.Item("unit"), dr.Item("type"), dr.Item("srp"), dr.Item("qty"), "0"})
                End While

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM resibo WHERE transacnum='" & transackkey & "'"
                dr = cmd.ExecuteReader

                While dr.Read

                    transacdate.Text = dr.Item("transacdate")
                    cashiername.Text = dr.Item("cashier")
                    paymenttype.Text = dr.Item("payment_type")
                    total.Text = dr.Item("total")
                    discount.Text = dr.Item("discount")
                    totaldiscount.Text = dr.Item("finaltotal")

                End While

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()

        MessageBox.Show("Total Available Balance : " + Convert.ToString(totalbalance), "", MessageBoxButtons.OK, MessageBoxIcon.Information)

        '****************************FOR UPDATING INVENTORY****************************'

        Try
            checkstate()
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "INSERT INTO return_log (transacnum,totalreturned,datereturned) VALUES('" & transackkey & "','" & totalbalance & "','" & Date.Now.Date.ToString("yyyy-MM-dd") & "')"
                .ExecuteNonQuery()

            End With
        Catch ex As Exception
            MessageBox.Show("return_log not added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()
    End Sub

    Private Sub DataGridView1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DataGridView1.KeyPress
        If e.KeyChar <> ControlChars.Back Then
            e.Handled = Not (Char.IsDigit(e.KeyChar))
        End If
    End Sub
End Class