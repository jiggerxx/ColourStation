Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Public Class printmenudobyrange
    Private conn As String = "Data Source=localhost; Database= csdb; User ID =root; Password=;"
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim startdate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim enddate As String = DateTimePicker2.Value.ToString("yyyy-MM-dd")

        Dim dsConn As New MySqlConnection(conn)
        Dim dsDA As New MySqlDataAdapter
        Dim rd As CrystalDecisions.CrystalReports.Engine.ReportDocument

        Try

            Dim d As New DataSet

            dsConn.Open()

            Dim strSQL As String = ""

            strSQL = "SELECT menudo_records.*,CONCAT(customers.lname,', ',customers.fname,' ',customers.mname) as customername,resibo.*,customers.*,(SELECT SUM(resibo.finaltotal) FROM resibo WHERE resibo.transacnum = menudo_records.transacnum) as totalsales, '" & startdate & "' as startdate, '" & enddate & "' as enddate FROM menudo_records LEFT JOIN resibo ON menudo_records.transacnum = resibo.transacnum LEFT JOIN customers ON resibo.custcode = customers.custcode WHERE resibo.transacdate BETWEEN '" & startdate & "' AND '" & enddate & "'"
            dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
            dsDA.Fill(d, "menudoreport")

            rd = New menudoreport
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