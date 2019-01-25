Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Public Class printbyrangewithsupplier
    Private conn As String = "Data Source=localhost; Database= csdb; User ID =root; Password=;"
    Private Sub printbyrangewithsupplier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadsuppliers()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim prodkey As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
        Dim prodvalue As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Value
        Dim dsConn As New MySqlConnection(conn)
        Dim dsDA As New MySqlDataAdapter
        Dim selecteddate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim selecteddate2 As String = DateTimePicker2.Value.ToString("yyyy-MM-dd")
        Dim rd As CrystalDecisions.CrystalReports.Engine.ReportDocument

        Try

            Dim d As New DataSet

            dsConn.Open()

            Dim strSQL As String = ""

            strSQL = "SELECT '" & selecteddate2 & "' as maxdate,'" & selecteddate & "' as mindate,CONCAT(supplier_province,', ',supplier_city,', ',supplier_barangay,', ',supplier_street) as suppaddress,stocks_acquired.*,stocks_acquired.*,supplier.* FROM stocks_acquired LEFT JOIN supplier ON stocks_acquired.supplier_code = supplier.supplier_code WHERE stocks_acquired.supplier_code = '" & prodkey & "' AND (stocks_acquired.datereceived BETWEEN '" & selecteddate & "' AND '" & selecteddate2 & "')"
            dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
            dsDA.Fill(d, "printbytransacnumber")

            rd = New printrangewithsupplierreport
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