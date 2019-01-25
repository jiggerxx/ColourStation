Public Class deleteaccount

    Private Sub deleteaccount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadallaccounts()
        Label9.Text = "------------"
        Label10.Text = "------------"
        Label11.Text = "------------"
        Label14.Text = "------------"
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim selectedaccountkey As String
        Dim selectedaccountval As String

        Try
            selectedaccountkey = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
            selectedaccountval = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Value
        Catch ex As Exception
            selectedaccountkey = ""
            selectedaccountval = ""
        End Try

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT accounts.* FROM accounts WHERE accounts.accnum = '" & selectedaccountkey & "'"
                dr = cmd.ExecuteReader

                While dr.Read
                    Label9.Text = dr.Item("accfname")
                    Label10.Text = dr.Item("accmname")
                    Label11.Text = dr.Item("acclname")
                    Label14.Text = dr.Item("acctype")

                End While
            End With

        Catch ex As Exception

        End Try

        dbconn.Close()
        dbconn.Dispose()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim delaccounts As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key

        Try

            checkstate()
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "DELETE FROM csdb.accounts WHERE accnum='" & delaccounts & "'"
                .ExecuteNonQuery()
                MessageBox.Show("Account " & Label9.Text & " " & Label10.Text & " " & Label11.Text & " is deleted!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                loadallaccounts()
                Label9.Text = "------------"
                Label10.Text = "------------"
                Label11.Text = "------------"
                Label14.Text = "------------"

            End With
        Catch ex As Exception
            MessageBox.Show("Account not found!" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()
    End Sub
End Class