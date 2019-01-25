Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Public Class printbydestination
    Private conn As String = "Data Source=localhost; Database= csdb; User ID =root; Password=;"
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub printbydestination_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loaddestinations()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim destinationkey As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
        Dim destination As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Value
        Dim dsConn As New MySqlConnection(conn)
        Dim dsDA As New MySqlDataAdapter
        Dim selecteddate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim selecteddate2 As String = DateTimePicker2.Value.ToString("yyyy-MM-dd")
        Dim rd As CrystalDecisions.CrystalReports.Engine.ReportDocument

        Try

            Dim d As New DataSet

            dsConn.Open()

            Dim strSQL As String = ""

            strSQL = "SELECT stocks_release.*,'" & selecteddate2 & "' as maxdate,'" & selecteddate & "' as mindate,released_products.*,products.*,units.*,type.* FROM stocks_release LEFT JOIN released_products ON stocks_release.releasereceiptnum = released_products.releasereceiptnum LEFT JOIN products ON  released_products.prodcode = products.prodcode LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN type ON products.prodtype = type.typeID WHERE destination = '" & destination & "' AND (stocks_release.daterelease BETWEEN '" & selecteddate & "' AND '" & selecteddate2 & "')"
            dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
            dsDA.Fill(d, "dailyreportingrelease")

            rd = New printbydestinationreport
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