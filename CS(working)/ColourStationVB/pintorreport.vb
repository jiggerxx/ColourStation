Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Public Class pintorreport
    Private conn As String = "Data Source=localhost; Database= csdb; User ID =root; Password=;"
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dsConn As New MySqlConnection(conn)
        Dim dsDA As New MySqlDataAdapter
        Dim selectedstartdate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim selectedenddate As String = DateTimePicker2.Value.ToString("yyyy-MM-dd")
        Dim rd As CrystalDecisions.CrystalReports.Engine.ReportDocument

        Try

            Dim d As New DataSet

            dsConn.Open()

            Dim strSQL As String = ""

            strSQL = "SELECT DISTINCT '" & selectedstartdate & "' as startdate,'" & selectedenddate & "' as enddate,accounts.*,pintor_transac.accnum,pintor_transac.datetransac, (SELECT SUM(totalearnings) FROM pintor_transac WHERE datetransac BETWEEN '" & selectedstartdate & "' AND '" & selectedenddate & "' AND accnum = accounts.accnum) as totalearningstotals FROM pintor_transac LEFT JOIN accounts ON pintor_transac.accnum = accounts.accnum WHERE datetransac BETWEEN '" & selectedstartdate & "' AND '" & selectedenddate & "'"
            dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
            dsDA.Fill(d, "pintorreportdata")

            rd = New pintorreporting
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