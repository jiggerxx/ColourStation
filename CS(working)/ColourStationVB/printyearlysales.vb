Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Public Class printyearlysales
    Private conn As String = "Data Source=localhost; Database= csdb; User ID =root; Password=;"

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim dsConn As New MySqlConnection(conn)
        Dim dsDA As New MySqlDataAdapter
        Dim selecteddate As String = DateTimePicker1.Value.Year
        Dim rd As CrystalDecisions.CrystalReports.Engine.ReportDocument

        Try

            Dim d As New DataSet

            dsConn.Open()

            Dim strSQL As String = ""

            strSQL = "SELECT resibo.transacnum,CONCAT(customers.lname,', ',customers.fname,' ',customers.mname) as customername,'" & selecteddate & "' as maxdate,resibo.transacdate,resibo.custcode,resibo.total,resibo.discount,resibo.finaltotal,resibo.cashier,(select sum(resibo.finaltotal) from resibo where YEAR(resibo.transacdate) = '" & selecteddate & "') as sumtotal,resibo_products.*,products.*,type.*,units.*,customers.* FROM resibo LEFT JOIN resibo_products ON resibo.transacnum = resibo_products.transacnum LEFT JOIN products ON resibo_products.prodcode = products.prodcode LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN customers ON resibo.custcode = customers.custcode WHERE YEAR(resibo.transacdate) = '" & selecteddate & "'"
            dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
            dsDA.Fill(d, "printsales")

            rd = New salesreportyearly
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