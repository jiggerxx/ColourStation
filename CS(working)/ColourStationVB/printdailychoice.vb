﻿Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Public Class printdailychoice
    Private conn As String = "Data Source=localhost; Database= csdb; User ID =root; Password=;"
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim dsConn As New MySqlConnection(conn)
        Dim dsDA As New MySqlDataAdapter
        Dim selecteddate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim rd As CrystalDecisions.CrystalReports.Engine.ReportDocument

        Try

            Dim d As New DataSet

            dsConn.Open()

            Dim strSQL As String = ""

            If ComboBox1.SelectedItem = "Acquired" Then

                If ComboBox2.SelectedIndex = 0 Then
                    strSQL = "SELECT '" & ComboBox2.SelectedItem.ToString & "' as selectedtype, CONCAT(supplier_province,', ',supplier_city,', ',supplier_barangay,', ',supplier_street) as suppaddress, stocks_acquired.*,supplier.* FROM stocks_acquired LEFT JOIN supplier ON stocks_acquired.supplier_code = supplier.supplier_code WHERE stocks_acquired.datereceived = '" & selecteddate & "'"
                Else
                    strSQL = "SELECT '" & ComboBox2.SelectedItem.ToString & "' as selectedtype, CONCAT(supplier_province,', ',supplier_city,', ',supplier_barangay,', ',supplier_street) as suppaddress, stocks_acquired.*,supplier.* FROM stocks_acquired LEFT JOIN supplier ON stocks_acquired.supplier_code = supplier.supplier_code WHERE stocks_acquired.datereceived = '" & selecteddate & "' AND transac_tax = '" & ComboBox2.SelectedItem.ToString & "'"
                End If


                dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
                dsDA.Fill(d, "printbytransacnumber")

                rd = New acquiredreport
                rd.SetDataSource(d)
            ElseIf ComboBox1.SelectedItem = "Released" Then
                strSQL = "SELECT stocks_release.*,released_products.*,products.*,units.*,type.* FROM stocks_release LEFT JOIN released_products ON stocks_release.releasereceiptnum = released_products.releasereceiptnum LEFT JOIN products ON  released_products.prodcode = products.prodcode LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN type ON products.prodtype = type.typeID WHERE stocks_release.daterelease = '" & selecteddate & "'"
                dsDA.SelectCommand = New MySqlCommand(strSQL, dsConn)
                dsDA.Fill(d, "dailyreportingrelease")

                rd = New releasereport
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

    Private Sub printdailychoice_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

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

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub
End Class