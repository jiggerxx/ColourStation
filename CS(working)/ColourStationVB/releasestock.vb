Public Class releasestock
    Dim counterX As Integer = 0
    Dim dataArray(100, 6) As String
    Private Sub Button4_Click(sender As Object, e As EventArgs)
        supplier.ShowDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim releasereceiptnum As String = TextBox2.Text
        Dim dateee As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim prodkey As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
        Dim prodvalue As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Value
        Dim qty As Integer = NumericUpDown1.Value
        Dim remarks As String = RichTextBox1.Text
        Dim stocks As Integer = 0
        Dim destination As String = TextBox3.Text
        Dim releasedby As String = TextBox4.Text
        Dim driver As String = TextBox5.Text

        Try

            If dbconn.State = ConnectionState.Open Then
                dbconn.Close()
            End If
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "INSERT INTO stocks_release VALUES('" & releasereceiptnum & "','" & dateee & "','" & destination & "','" & remarks & "','" & releasedby & "','" & driver & "')"
                .ExecuteNonQuery()
                MessageBox.Show("Release transaction #" + releasereceiptnum + " has been added!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                
                TextBox3.Clear()
                TextBox2.Clear()
                RichTextBox1.Clear()

            End With
        Catch ex As Exception
            MessageBox.Show("Release transaction #" + releasereceiptnum + " is not added. " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        dbconn.Close()
        dbconn.Dispose()

        For x As Integer = 0 To counterX - 1

            Try

                If dbconn.State = ConnectionState.Open Then
                    dbconn.Close()
                End If
                dbconn.Open()
                With cmd
                    .Connection = dbconn
                    .CommandText = "INSERT INTO released_products VALUES('" & releasereceiptnum & "','" & dataArray(x, 0) & "','" & dataArray(x, 5) & "')"
                    .ExecuteNonQuery()

                    RichTextBox1.Clear()

                End With
            Catch ex As Exception
                MessageBox.Show("Error!" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try
            dbconn.Close()
            dbconn.Dispose()

            Try
                If dbconn.State = ConnectionState.Open Then
                    dbconn.Close()
                End If
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

            stocks = stocks - dataArray(x, 5)

            Try

                If dbconn.State = ConnectionState.Open Then
                    dbconn.Close()
                End If
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
        
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub releasestock_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim stocks As Integer = 0
        ' Dim comprodcode As String() = ComboBox1.Text.Split(New Char() {" "c})
        Try
            Dim cprodcode As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
        
        
            Try
                dbconn.Open()

                With cmd
                    .Connection = dbconn
                .CommandText = "select * from csdb.products where prodcode='" & cprodcode & "'"
                    dr = cmd.ExecuteReader

                    While dr.Read
                        stocks = dr.Item("stock")
                    End While

                End With

            Catch ex As Exception
            'MessageBox.Show(ex.Message + "error33!", "error33", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Catch ex As Exception
        End Try
        dbconn.Close()
        dbconn.Dispose()

        TextBox1.Text = stocks
        NumericUpDown1.Maximum = stocks

        If stocks.Equals(0) Then
            Button1.Enabled = False
            Button2.Enabled = False
        Else
            Button1.Enabled = True
            Button2.Enabled = True
            NumericUpDown1.Minimum = 1
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim total As Integer = 0
        Dim prodcode As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
        Dim exist As Boolean = False
        Dim added As Boolean = False

        For x = 0 To counterX - 1
            If dataArray(x, 0).ToString.Equals(prodcode) Then
                total = Convert.ToInt32(dataArray(x, 5)) + NumericUpDown1.Value

                If total <= NumericUpDown1.Maximum Then
                    dataArray(x, 5) = total.ToString
                Else
                    MessageBox.Show("Maximum Limit Reached For This Product")
                End If

                exist = True
            End If
        Next


        If exist.Equals(False) Then

            Try
                dbconn.Open()

                With cmd
                    .Connection = dbconn
                    .CommandText = "SELECT products.*,type.*,units.* FROM csdb.products LEFT JOIN units ON products.unitID=units.unitID LEFT JOIN type ON products.prodtype =type.typeID where prodcode='" & prodcode & "'"
                    dr = cmd.ExecuteReader

                    While dr.Read

                        dataArray(counterX, 0) = dr.Item("prodcode")
                        dataArray(counterX, 1) = dr.Item("prodname")
                        dataArray(counterX, 3) = dr.Item("unit")
                        dataArray(counterX, 4) = dr.Item("type")

                        added = True

                    End While

                    dataArray(counterX, 5) = NumericUpDown1.Value
                    dataArray(counterX, 2) = TextBox1.Text

                End With
            Catch ex As Exception
                MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            dbconn.Close()
            dbconn.Dispose()

        End If

        If added.Equals(True) Then
            counterX = counterX + 1
        End If

        DataGridView1.Rows.Clear()
        For x = 0 To counterX - 1
            DataGridView1.Rows.Add(New String() {dataArray(x, 0), dataArray(x, 1), dataArray(x, 2), dataArray(x, 3), dataArray(x, 4), dataArray(x, 5)})
        Next
    End Sub

    Private Sub DataGridView1_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles DataGridView1.UserDeletingRow
        Dim delindex As Integer = e.Row.Index

        For a As Integer = delindex To counterX - 1

            dataArray(a, 0) = dataArray(a + 1, 0)
            dataArray(a, 1) = dataArray(a + 1, 1)
            dataArray(a, 2) = dataArray(a + 1, 2)
            dataArray(a, 3) = dataArray(a + 1, 3)
            dataArray(a, 4) = dataArray(a + 1, 4)
            dataArray(a, 5) = dataArray(a + 1, 5)

        Next
        counterX = counterX - 1

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            DataGridView1.Rows.Remove(DataGridView1.SelectedRows(0))
            counterX -= 1
        Catch ex As Exception
            MessageBox.Show(ex.Message + vbNewLine + "SELECT ROW FIRST TO BE REMOVE", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class