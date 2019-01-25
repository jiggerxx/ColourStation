Public Class View_Release

    Public dataArray(9999, 4) As String
    Dim selectedDR As String = ""

    Private Sub View_Release_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        
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
                .CommandText = "SELECT released_products.*, products.*, units.*, type.* FROM released_products LEFT JOIN products ON released_products.prodcode = products.prodcode LEFT JOIN type ON products.prodtype=type.typeID LEFT JOIN units ON products.unitID = units.unitID where released_products.releasereceiptnum = '" & dataArray(rowclicked, 0) & "'"
                dr = cmd.ExecuteReader

                While dr.Read
                    DataGridView2.Rows.Add(New String() {dr.Item("prodcode"), dr.Item("prodname"), dr.Item("qty"), dr.Item("unit"), dr.Item("type")})
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
            checkstate()
            dbconn.Open()

            DataGridView1.Rows.Clear()

            With cmd
                .Connection = dbconn
                If searchtxt = "" Then
                    .CommandText = "SELECT stocks_release.* FROM csdb.stocks_release"
                Else
                    .CommandText = "SELECT stocks_release.* FROM csdb.stocks_release WHERE 	releasereceiptnum LIKE '%" & searchtxt & "%'"
                End If
                dr = cmd.ExecuteReader

                While dr.Read
                    dataArray(x, 0) = dr.Item("releasereceiptnum")
                    dataArray(x, 1) = dr.Item("daterelease")
                    dataArray(x, 2) = dr.Item("destination")
                    dataArray(x, 3) = dr.Item("remarks")

                    DataGridView1.Rows.Add(New String() {dataArray(x, 0), dataArray(x, 1), dataArray(x, 2), dataArray(x, 3)})

                    x = x + 1

                    tandf = True
                End While
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()

        DataGridView2.Rows.Clear()

        selectedDR = ""
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        selectedDR = ""
    End Sub

    Private Sub ComboBox1_TextChanged(sender As Object, e As EventArgs) Handles ComboBox1.TextChanged
    End Sub
End Class