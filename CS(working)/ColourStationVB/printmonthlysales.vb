Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Public Class printmonthlysales
    Private conn As String = "Data Source=localhost; Database= csdb; User ID =root; Password=;"

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim dsConn As New MySqlConnection(conn)
        Dim dsDA As New MySqlDataAdapter
        Dim selecteddate As String = DateTimePicker1.Value.Month
        Dim rd As CrystalDecisions.CrystalReports.Engine.ReportDocument

        Dim selectedyeardate As String = DateTimePicker1.Value.Year
        Dim monthselect As String = ""

        If selecteddate = 1 Then
            monthselect = "January " & selectedyeardate
        ElseIf selecteddate = 2 Then
            monthselect = "February " & selectedyeardate
        ElseIf selecteddate = 3 Then
            monthselect = "March " & selectedyeardate
        ElseIf selecteddate = 4 Then
            monthselect = "April " & selectedyeardate
        ElseIf selecteddate = 5 Then
            monthselect = "May " & selectedyeardate
        ElseIf selecteddate = 6 Then
            monthselect = "June " & selectedyeardate
        ElseIf selecteddate = 7 Then
            monthselect = "July " & selectedyeardate
        ElseIf selecteddate = 8 Then
            monthselect = "August " & selectedyeardate
        ElseIf selecteddate = 9 Then
            monthselect = "September " & selectedyeardate
        ElseIf selecteddate = 10 Then
            monthselect = "October " & selectedyeardate
        ElseIf selecteddate = 11 Then
            monthselect = "November " & selectedyeardate
        ElseIf selecteddate = 12 Then
            monthselect = "December " & selectedyeardate
        End If

        Try

            Dim d As New DataSet

            dsConn.Open()

            Dim strSQL As String = ""

            strSQL = "SELECT resibo.*,'" & monthselect & "' as maxdate,CONCAT(customers.lname,', ',customers.fname,' ',customers.mname) as customername,(select sum(resibo.finaltotal) from resibo where MONTH(resibo.transacdate) = '" & selecteddate & "') as sumtotal,resibo_products.*,products.*,type.*,units.*,customers.* FROM resibo LEFT JOIN resibo_products ON resibo.transacnum = resibo_products.transacnum LEFT JOIN products ON resibo_products.prodcode = products.prodcode LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN customers ON resibo.custcode = customers.custcode WHERE MONTH(resibo.transacdate) = '" & selecteddate & "' and YEAR(resibo.transacdate) = '" & selectedyeardate & "'"
            dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
            dsDA.Fill(d, "printsales")

            rd = New salesreportmonthly
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