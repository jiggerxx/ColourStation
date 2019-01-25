Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Public Class printreceipt

    Public datafrom As String
    Public selectedtransacnum As String

    Private conn As String = "Data Source=localhost; Database= csdb; User ID =root; Password=;"
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim dsConn As New MySqlConnection(conn)
        Dim dsDA As New MySqlDataAdapter
        Dim rd As CrystalDecisions.CrystalReports.Engine.ReportDocument

        Try

            Dim d As New DataSet

            dsConn.Open()

            Dim strSQL As String = ""

            If datafrom = "others" Then
                strSQL = "SELECT CONCAT(products.barcode,', ',products.prodname,', ',type.type) as articles,CONCAT(customers.lname,', ',customers.fname,' ',customers.mname) as custname,resibo.*, menudo_records.*, products.*, units.*, type.*, customers.* FROM resibo LEFT JOIN menudo_records ON resibo.transacnum = menudo_records.transacnum LEFT JOIN products ON menudo_records.prodcode = products.prodcode LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN customers ON resibo.custcode = customers.custcode WHERE resibo.transacnum='" & selectedtransacnum & "'"
                dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
                dsDA.Fill(d, "resibo2")

                rd = New resibo2
                rd.SetDataSource(d)

                Form2.CrystalReportViewer1.ReportSource = rd
                Form2.ShowDialog()
                Form2.Dispose()

                dsConn.Close()
                dsConn.Dispose()

            ElseIf datafrom = "invoice" Then
                strSQL = "SELECT CONCAT(products.barcode,', ',products.prodname,', ',type.type) as articles,CONCAT(customers.lname,', ',customers.fname,' ',customers.mname) as custname,resibo.*, resibo_products.*, products.*, units.*, type.*, customers.* FROM resibo LEFT JOIN resibo_products ON resibo.transacnum = resibo_products.transacnum LEFT JOIN products ON resibo_products.prodcode = products.prodcode LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN customers ON resibo.custcode = customers.custcode WHERE resibo.transacnum='" & selectedtransacnum & "'"
                dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
                dsDA.Fill(d, "resibo")

                rd = New resibo
                rd.SetDataSource(d)

                Form2.CrystalReportViewer1.ReportSource = rd
                Form2.ShowDialog()
                Form2.Dispose()

                dsConn.Close()
                dsConn.Dispose()
            End If

            

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub printreceipt_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class