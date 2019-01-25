Public Class paymentsnotice
    Public dataArray(100, 13) As String
    'Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)
    '    Try
    '        If CheckBox1.Checked Then
    '            For Each row As DataGridViewRow In DataGridView1.Rows
    '                Dim cell As DataGridViewCheckBoxCell = row.Cells(8)
    '                cell.Value = True
    '            Next
    '        Else
    '            For Each row As DataGridViewRow In DataGridView1.Rows
    '                Dim cell As DataGridViewCheckBoxCell = row.Cells(8)
    '                cell.Value = False
    '            Next
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    'Private Sub paymentsnotice_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    loadunpaidpayments()
    'End Sub

    'Private Sub Button1_Click(sender As Object, e As EventArgs)
    '    For i As Integer = 0 To DataGridView1.Rows.Count() - 1
    '        Dim checkbox As Boolean = DataGridView1.Rows(i).Cells(8).Value
    '        Dim drnum As String = DataGridView1.Rows(i).Cells(0).Value

    '        If checkbox Then
    '            checkstate()
    '            dbconn.Open()
    '            With cmd
    '                .Connection = dbconn
    '                .CommandText = "UPDATE stocks_acquired SET payment_status = 'Paid' WHERE drnum = '" & drnum & "'"
    '                .ExecuteNonQuery()

    '            End With

    '            dbconn.Close()
    '            dbconn.Dispose()

    '            MessageBox.Show(drnum)
    '        End If

    '    Next
    'End Sub

    Private Sub Panel5_Paint(sender As Object, e As PaintEventArgs) Handles Panel5.Paint

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        showpaymentnotice()
        loadunpaidpaymentsclick(TextBox1.Text.ToString)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text.Equals("") Then
            showpaymentnotice()
            loadunpaidpayments()
        End If
    End Sub
End Class