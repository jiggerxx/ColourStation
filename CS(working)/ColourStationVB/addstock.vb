Public Class addstock
    Dim counterX As Integer = 0
    Dim dataArray(100, 7) As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim transacnum As String = TextBox1.Text
        Dim transactype As String = ComboBox3.SelectedItem
        Dim transacttax As String = TextBox3.Text
        Dim datereceived As String = DateTimePicker2.Value.ToString("yyyy-MM-dd")
        Dim transacdate As String = DateTimePicker3.Value.ToString("yyyy-MM-dd")
        Dim prodkey As String() = ComboBox1.Text.Split(New Char() {" "c})
        Dim prodvalue As String() = ComboBox1.Text.Split(New Char() {" "c})
        Dim supkey As String = ""
        Dim supvalue As String = ""
        Dim remarks As String = RichTextBox1.Text
        Dim forwarder As String = TextBox5.Text
        Dim waybillnum As String = TextBox4.Text
        Dim receivedby As String = TextBox6.Text
        Dim stocks As Integer
        Dim overalltotal As Double = 0

        Try
            supkey = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Key
            supvalue = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Value
        Catch ex As Exception

        End Try

        For x = 0 To counterX - 1
            overalltotal = overalltotal + CDbl(dataArray(x, 6))
        Next

        Try

            If dbconn.State = ConnectionState.Open Then
                dbconn.Close()
            End If
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "INSERT INTO stocks_acquired VALUES('" & transacnum & "','" & transactype & "','" & transacttax & "','" & datereceived & "','" & transacdate & "','" & supkey & "','" & remarks & "','" & forwarder & "','" & waybillnum & "','" & receivedby & "','Unpaid','" & overalltotal & "','" & overalltotal & "')"
                .ExecuteNonQuery()
                MessageBox.Show("Transaction #" + transacnum + " has been added!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End With
        Catch ex As Exception
            MessageBox.Show("" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        dbconn.Close()
        dbconn.Dispose()

        For x As Integer = 0 To counterX - 1
            stocks = 0
            Try

                checkstate()
                dbconn.Open()
                With cmd
                    .Connection = dbconn
                    .CommandText = "INSERT INTO acquired_products VALUES('" & transacnum & "','" & dataArray(x, 0) & "','" & dataArray(x, 2) & "','" & dataArray(x, 5) & "')"
                    .ExecuteNonQuery()

                End With
            Catch ex As Exception
                MessageBox.Show("Error! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try
            dbconn.Close()
            dbconn.Dispose()

            Try
                checkstate()
                dbconn.Open()

                With cmd
                    .Connection = dbconn
                    .CommandText = "SELECT * FROM csdb.products where prodcode='" & dataArray(x, 0) & "'"
                    dr = cmd.ExecuteReader

                    While dr.Read
                        stocks = dr.Item("stock")
                    End While

                End With

            Catch ex As Exception
                MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            dbconn.Close()
            dbconn.Dispose()

            stocks += Convert.ToInt32(dataArray(x, 2))

            Try
                checkstate()
                dbconn.Open()
                With cmd
                    .Connection = dbconn
                    .CommandText = "UPDATE products SET stock='" & stocks & "' WHERE prodcode='" & dataArray(x, 0) & "'"
                    .ExecuteNonQuery()
                End With
            Catch ex As Exception
                MessageBox.Show("Error!" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            dbconn.Close()
            dbconn.Dispose()


        Next

        DataGridView1.Rows.Clear()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        ComboBox1.SelectedIndex = -1
        ComboBox3.SelectedIndex = -1
        TextBox3.Clear()
        RichTextBox1.Clear()
        counterX = 0

    End Sub


    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        supplier.ShowDialog()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim total As Integer = 0
        Dim dbprodcode As String = ""
        Dim prodcode As String() = ComboBox1.Text.Split(New Char() {" "c})
        Dim exist As Boolean = False
        Dim added As Boolean = False
        Dim dealersprice As Double = TextBox7.Text
        Dim totalamountperproduct As Double = 0

        For x = 0 To counterX - 1
            If dataArray(x, 0) = prodcode(0) Then
                total = Convert.ToInt32(dataArray(x, 2)) + NumericUpDown1.Value
                dataArray(x, 2) = total.ToString
                totalamountperproduct = total * dealersprice
                dataArray(x, 6) = totalamountperproduct
                exist = True
            End If
        Next

        If exist.Equals(False) Then

            Try
                dbconn.Open()

                With cmd
                    .Connection = dbconn
                    .CommandText = "SELECT products.*,type.*,units.* FROM csdb.products LEFT JOIN units ON products.unitID=units.unitID LEFT JOIN type ON products.prodtype =type.typeID where prodcode='" & prodcode(0) & "'"
                    dr = cmd.ExecuteReader

                    While dr.Read

                        dataArray(counterX, 0) = dr.Item("prodcode")
                        dataArray(counterX, 1) = dr.Item("prodname")
                        dataArray(counterX, 2) = NumericUpDown1.Value
                        dataArray(counterX, 3) = dr.Item("unit")
                        dataArray(counterX, 4) = dr.Item("type")
                        dataArray(counterX, 5) = dealersprice
                        dataArray(counterX, 6) = dealersprice * NumericUpDown1.Value
                        dataArray(counterX, 7) = dr.Item("srp")
                        added = True

                    End While

                End With
            Catch ex As Exception
                MessageBox.Show(ex.Message + "Error123!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            dbconn.Close()
            dbconn.Dispose()

        End If

        If added.Equals(True) Then
            counterX = counterX + 1
        End If

        DataGridView1.Rows.Clear()
        For x = 0 To counterX - 1
            DataGridView1.Rows.Add({dataArray(x, 0), dataArray(x, 1), dataArray(x, 2), dataArray(x, 3), dataArray(x, 4), dataArray(x, 5), dataArray(x, 7), dataArray(x, 6).ToString})
        Next

    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub DataGridView1_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles DataGridView1.UserDeletingRow
        Dim delindex As Integer = e.Row.Index

        For a As Integer = delindex To counterX - 1

            dataArray(a, 0) = dataArray(a + 1, 0)
            dataArray(a, 1) = dataArray(a + 1, 1)
            dataArray(a, 2) = dataArray(a + 1, 2)
            dataArray(a, 3) = dataArray(a + 1, 3)
            dataArray(a, 4) = dataArray(a + 1, 4)

        Next
        counterX = counterX - 1

    End Sub

    Private Sub addstock_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
        Me.Close()
    End Sub

    Private Sub TextBox7_Leave(sender As Object, e As EventArgs) Handles TextBox7.Leave
        If TextBox7.Text = "" Or TextBox7.Text = "0" Then
            TextBox7.Text = "0.0"
        End If
    End Sub

    Private Sub TextBox7_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox7.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or Asc(e.KeyChar) = 8)
        If (e.KeyChar.ToString = ".") And (TextBox7.Text.Contains(e.KeyChar.ToString)) Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub TextBox7_Enter(sender As Object, e As EventArgs) Handles TextBox7.Enter
        If String.IsNullOrEmpty(TextBox7.Text) Or TextBox7.Text = 0.0 Then
            TextBox7.Text = ""
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim supkey As String = ""
        Dim intervaldate As Integer = 0

        Try
            supkey = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Key
        Catch ex As Exception
            TextBox8.Text = ""
        End Try

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM supplier WHERE supplier_code='" & supkey & "'"
                dr = cmd.ExecuteReader

                While dr.Read

                    intervaldate = dr.Item("payment_dues")
                    TextBox8.Text = DateAdd(DateInterval.Day, intervaldate, Date.Now).ToString("MMMM dd,yyyy")

                End While

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        supplier.ShowDialog()
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.SelectedIndex = 0 Or ComboBox3.SelectedIndex = 1 Or ComboBox3.SelectedIndex = 2 Then
            TextBox3.Text = "VAT"
        ElseIf ComboBox3.SelectedIndex = 3 Then
            TextBox3.Text = "NONVAT"
        ElseIf ComboBox3.SelectedIndex = 4 Then
            TextBox3.Text = "OTHERS"
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text.Count - 0 Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            DataGridView1.Rows.Remove(DataGridView1.SelectedRows(0))
            counterX -= 1
        Catch ex As Exception
            MessageBox.Show(ex.Message + vbNewLine + "SELECT ROW FIRST TO BE REMOVE", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ComboBox1_TextUpdate(sender As Object, e As EventArgs) Handles ComboBox1.TextUpdate
        If ComboBox1.Text.Equals("") Then
            ComboBox1.DroppedDown = False
        Else
            addStockTextChanged()
        End If
    End Sub
End Class