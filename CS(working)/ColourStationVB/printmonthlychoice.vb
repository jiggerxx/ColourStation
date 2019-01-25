Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Public Class printmonthlychoice
    Private conn As String = "Data Source=localhost; Database= csdb; User ID =root; Password=;"
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dsConn As New MySqlConnection(conn)
        Dim dsDA As New MySqlDataAdapter
        Dim selecteddate As String = DateTimePicker1.Value.Month
        'Dim selecteddate2 As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim rd As CrystalDecisions.CrystalReports.Engine.ReportDocument

        'selecteddate2 = DateTimePicker1.Value.AddDays(7).ToString("yyyy-MM-dd")

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

            If ComboBox1.SelectedItem = "Acquired" Then

                If ComboBox2.SelectedIndex = 0 Then
                    strSQL = "SELECT '" & ComboBox2.SelectedItem.ToString & "' as selectedtype,'" & monthselect & "' as maxdate,CONCAT(supplier_province,', ',supplier_city,', ',supplier_barangay,', ',supplier_street) as suppaddress,stocks_acquired.*,stocks_acquired.*,supplier.* FROM stocks_acquired LEFT JOIN supplier ON stocks_acquired.supplier_code = supplier.supplier_code WHERE MONTH(stocks_acquired.datereceived) = '" & selecteddate & "' and YEAR(stocks_acquired.datereceived) = '" & selectedyeardate & "'"
                Else
                    strSQL = "SELECT '" & ComboBox2.SelectedItem.ToString & "' as selectedtype,'" & monthselect & "' as maxdate,CONCAT(supplier_province,', ',supplier_city,', ',supplier_barangay,', ',supplier_street) as suppaddress,stocks_acquired.*,stocks_acquired.*,supplier.* FROM stocks_acquired LEFT JOIN supplier ON stocks_acquired.supplier_code = supplier.supplier_code WHERE MONTH(stocks_acquired.datereceived) = '" & selecteddate & "' and YEAR(stocks_acquired.datereceived) = '" & selectedyeardate & "' AND transac_tax = '" & ComboBox2.SelectedItem.ToString & "'"
                End If


                dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
                dsDA.Fill(d, "printbytransacnumber")

                rd = New acquiredreportmonthly
                rd.SetDataSource(d)
            ElseIf ComboBox1.SelectedItem = "Released" Then
                strSQL = "SELECT stocks_release.*,'" & monthselect & "' as maxdate,released_products.*,products.*,units.*,type.* FROM stocks_release LEFT JOIN released_products ON stocks_release.releasereceiptnum = released_products.releasereceiptnum LEFT JOIN products ON  released_products.prodcode = products.prodcode LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN type ON products.prodtype = type.typeID WHERE MONTH(stocks_release.daterelease) = '" & selecteddate & "' and YEAR(stocks_release.daterelease) = '" & selectedyeardate & "'"
                dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
                dsDA.Fill(d, "dailyreportingrelease")

                rd = New releasereportmonthly
                rd.SetDataSource(d)
            End If

            Form2.CrystalReportViewer1.ReportSource = rd
            Form2.ShowDialog()
            Form2.Dispose()

            dsConn.Close()
            dsConn.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub printmonthlychoice_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = 0 Then
            ComboBox2.Enabled = True
        Else
            ComboBox2.Enabled = False
        End If
    End Sub
End Class