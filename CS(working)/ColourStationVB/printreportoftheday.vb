Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Public Class printreportoftheday
    Private conn As String = "Data Source=localhost; Database= csdb; User ID =root; Password=;"
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim dsConn As New MySqlConnection(conn)
        Dim dsDA As New MySqlDataAdapter
        Dim selecteddate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim rd As CrystalDecisions.CrystalReports.Engine.ReportDocument

        Try
            Dim d As New DataSet

            dsConn.Open()

            Dim strSQL As String = ""

            strSQL = "SELECT (SELECT SUM(finaltotal) FROM resibo WHERE transacdate = '" & selecteddate & "') as totalsales, (SELECT SUM(finaltotal) FROM resibo WHERE payment_type = 'Charge' and transacdate = '" & selecteddate & "') as totalcharge, (SELECT SUM(discount) FROM resibo WHERE transacdate = '" & selecteddate & "') as totaldiscount, (SELECT SUM(earnings) FROM mixxer_transac WHERE datetransac = '" & selecteddate & "') as totalmixer, (SELECT SUM(totalearnings) FROM pintor_transac WHERE datetransac = '" & selecteddate & "') as totalpintor, (SELECT SUM(earnings) FROM agents_transac WHERE datetransac = '" & selecteddate & "') as totalagent, (SELECT SUM(totalreturned) FROM return_log WHERE datereturned = '" & selecteddate & "') as totalreturned,'" & selecteddate & "' as dateday"
            dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
            dsDA.Fill(d, "dayreport")

            rd = New reportofday
            rd.SetDataSource(d)

            Form2.CrystalReportViewer1.ReportSource = rd
            Form2.ShowDialog()
            Form2.Dispose()

            dsConn.Close()
            dsConn.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class