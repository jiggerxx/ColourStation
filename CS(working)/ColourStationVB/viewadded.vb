Public Class viewadded
    Public dataArray(9999, 12) As String
    Dim selectedDR As String = ""

    Private Sub viewadded_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        
    End Sub

    Private Sub DataGridView1_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDoubleClick
        Dim rowclicked As Integer = 0
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            rowclicked = e.RowIndex
        End If

        DataGridView2.Rows.Clear()

        Try
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT acquired_products.*, products.*, units.*, type.* FROM acquired_products LEFT JOIN products ON acquired_products.prodcode = products.prodcode LEFT JOIN type ON products.prodtype=type.typeID LEFT JOIN units ON products.unitID = units.unitID where acquired_products.drnum = '" & dataArray(rowclicked, 0) & "'"
                dr = cmd.ExecuteReader

                While dr.Read

                    DataGridView2.Rows.Add(New String() {dr.Item("prodcode"), dr.Item("prodname"), dr.Item("qty"), dr.Item("unit"), dr.Item("type"), dr.Item("dealersprice"), dr.Item("srp")})

                End While

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()

        selectedDR = dataArray(rowclicked, 0)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim searchtxt As String = ComboBox1.Text
        Dim tandf As Boolean = True
        Dim x As Integer = 0

        Try
            dbconn.Open()

            With cmd
                .Connection = dbconn

                If searchtxt = "" Then
                    .CommandText = "SELECT stocks_acquired.*,supplier.* FROM csdb.stocks_acquired LEFT JOIN supplier ON stocks_acquired.supplier_code=supplier.supplier_code"
                Else
                    .CommandText = "SELECT stocks_acquired.*,supplier.* FROM csdb.stocks_acquired LEFT JOIN supplier ON stocks_acquired.supplier_code=supplier.supplier_code where stocks_acquired.drnum LIKE '%" & searchtxt & "%'"
                End If

                dr = cmd.ExecuteReader

                While dr.Read

                    dataArray(x, 0) = dr.Item("drnum")
                    dataArray(x, 1) = dr.Item("transactype")
                    dataArray(x, 2) = dr.Item("datereceived")
                    dataArray(x, 3) = dr.Item("transacdate")
                    dataArray(x, 4) = dr.Item("supplier_name")
                    dataArray(x, 5) = dr.Item("forwarder")
                    dataArray(x, 6) = dr.Item("waybillnum")
                    dataArray(x, 7) = dr.Item("receivedby")
                    dataArray(x, 8) = dr.Item("remarks")
                    dataArray(x, 9) = dr.Item("payment_status")
                    dataArray(x, 10) = dr.Item("amount")
                    dataArray(x, 11) = dr.Item("balance")


                    x = x + 1

                    tandf = False
                End While
                If tandf = True Then
                    MessageBox.Show("Receipt Not Found!", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    DataGridView1.Rows.Clear()
                    DataGridView2.Rows.Clear()
                End If

            End With


        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()

        For a As Integer = 0 To x - 1
            DataGridView1.Rows.Add(New String() {dataArray(a, 0), dataArray(a, 1), "", dataArray(a, 2), dataArray(a, 3), dataArray(a, 4), dataArray(a, 5), dataArray(a, 6), dataArray(a, 7), dataArray(a, 8), dataArray(a, 9), dataArray(a, 10)})
        Next

        selectedDR = ""
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        MessageBox.Show(selectedDR)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        selectedDR = ""
    End Sub
End Class