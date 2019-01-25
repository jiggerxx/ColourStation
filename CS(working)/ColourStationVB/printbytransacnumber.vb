Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Public Class printbytransacnumber
    Private conn As String = "Data Source=localhost; Database= csdb; User ID =root; Password=;"
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub printbytransacnumber_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadtransacnums()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim transackey As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
        Dim transacvalue As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Value
        Dim dsConn As New MySqlConnection(conn)
        Dim dsDA As New MySqlDataAdapter
        Dim rd As CrystalDecisions.CrystalReports.Engine.ReportDocument


        Try

            Dim d As New DataSet

            dsConn.Open()

            Dim strSQL As String = ""

            strSQL = "SELECT stocks_acquired.*,CONCAT(supplier_province,', ',supplier_city,', ',supplier_barangay,', ',supplier_street) as suppaddress,acquired_products.*,products.*,units.*,type.*,supplier.* FROM stocks_acquired LEFT JOIN acquired_products ON stocks_acquired.drnum = acquired_products.drnum LEFT JOIN products ON  acquired_products.prodcode = products.prodcode LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN type ON products.prodtype = type.typeID LEFT JOIN supplier ON stocks_acquired.supplier_code = supplier.supplier_code WHERE stocks_acquired.drnum = '" & transackey & "'"
            dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
            dsDA.Fill(d, "printbytransacnumber")

            rd = New printbytransacreport
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