Imports MySql.Data.MySqlClient

Public Class addproduct

    Private Sub addproduct_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        'Dim units As New Dictionary(Of String, String)()
        'Dim unitauto As New AutoCompleteStringCollection
        'Dim tandf As Boolean = False
        ''Dim strSQL = "SELECT * FROM unitword"
        ''Dim da As New MySqlDataAdapter(strSQL, dbconn)
        ''Dim ds As New DataSet
        ''da.Fill(ds, "units")

        'Try

        '    ComboBox1.DataSource = Nothing

        '    checkstate()
        '    dbconn.Open()

        '    With ComboBox1

        '        '.DataSource = ds.Tables("units")
        '        '.DisplayMember = "unitname"
        '        '.ValueMember = "unit_ID"
        '        '.SelectedIndex = -1

        '        While dr.Read

        '            units.Add(dr.Item("unit_ID"), dr.Item("unitname"))
        '            unitauto.AddRange(New String() {dr.Item("unitname")})

        '            tandf = True

        '        End While
        '    End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message + "unitword!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try

        'dbconn.Close()
        'dbconn.Dispose()

        'If tandf Then
        '    Try
        '        ComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
        '        ComboBox1.AutoCompleteCustomSource = unitauto
        '        ComboBox1.DataSource = New BindingSource(units, Nothing)
        '        ComboBox1.DisplayMember = "Value"
        '        ComboBox1.ValueMember = "Key"
        '        ComboBox1.SelectedIndex = -1



        '    Catch ex As Exception
        '        MessageBox.Show(ex.ToString + " Error in Load unit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    End Try
        'Else
        '    MessageBox.Show("No unit! Please Add One First!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim prodname As String = TextBox2.Text
        Dim barcode As String = TextBox3.Text
        Dim unit As String
        Dim unitvalue As String
        Dim prodtype As String
        Dim prodvalue As String
        Dim proddesc As String = RichTextBox1.Text
        Dim srp As Integer = TextBox4.Text
        Dim unitword As String = ComboBox1.SelectedItem.ToString
        Dim hchem As Integer = 0

        Dim result As Integer = MessageBox.Show("Add item?" + vbNewLine + "Prodcut Name: " + prodname.ToUpper, "", MessageBoxButtons.YesNoCancel)
        If result = DialogResult.Cancel Then

        ElseIf result = DialogResult.No Then

        ElseIf result = DialogResult.Yes Then

            Try
                unit = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Key
                unitvalue = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Value
                prodtype = DirectCast(ComboBox3.SelectedItem, KeyValuePair(Of String, String)).Key
                prodvalue = DirectCast(ComboBox3.SelectedItem, KeyValuePair(Of String, String)).Value
            Catch ex As Exception
                unit = ""
                unitvalue = ""
                prodtype = ""
                prodvalue = ""
            End Try

            If CheckBox1.Checked Then
                hchem = 1
            Else
                hchem = 0
            End If

            Try

                checkstate()
                dbconn.Open()
                With cmd
                    .Connection = dbconn
                    .CommandText = "INSERT INTO products (barcode,prodname,prodtype,unitID,proddesc,stock,srp,unitword,hchem) VALUES('" & barcode & "','" & prodname & "','" & prodtype & "','" & unit & "','" & proddesc & "',0,'" & srp & "','" & unitword & "','" & hchem & "')"
                    .ExecuteNonQuery()
                    MessageBox.Show("Product " + prodname + " has been added!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    TextBox2.Clear()
                    TextBox3.Clear()
                    ComboBox1.SelectedIndex = -1
                    ComboBox2.SelectedIndex = -1
                    ComboBox3.SelectedIndex = -1
                    TextBox4.Text = 0.0
                    CheckBox1.Checked = False
                    RichTextBox1.Clear()

                End With
            Catch ex As Exception
                MessageBox.Show("Product Not Added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

            dbconn.Close()
            dbconn.Dispose()
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        supplier.ShowDialog()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        addunitname.ShowDialog()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        addtype.ShowDialog()
    End Sub

    Private Sub addproduct_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
        Me.Close()
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or Asc(e.KeyChar) = 8)
        If (e.KeyChar.ToString = ".") And (TextBox4.Text.Contains(e.KeyChar.ToString)) Then
            e.Handled = True
            Exit Sub
        End If
    End Sub


    Private Sub TextBox4_Leave(sender As Object, e As EventArgs) Handles TextBox4.Leave
        If TextBox4.Text = "" Or TextBox4.Text = "0" Then
            TextBox4.Text = "0.0"
        End If
    End Sub

    Private Sub TextBox4_Enter(sender As Object, e As EventArgs) Handles TextBox4.Enter
        If String.IsNullOrEmpty(TextBox4.Text) Or TextBox4.Text = 0.0 Then
            TextBox4.Text = ""
        End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)
        addunit.ShowDialog()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub ComboBox1_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub Button2_Click_2(sender As Object, e As EventArgs) Handles Button2.Click
        addunit.ShowDialog()

    End Sub
End Class