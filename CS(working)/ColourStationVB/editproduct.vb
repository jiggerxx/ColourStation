Public Class editproduct

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim unitword As String = ComboBox1.SelectedItem.ToString
        Dim prodname As String = TextBox2.Text
        Dim barcode As String = TextBox3.Text
        Dim unit As String
        Dim prodtype As String
        Dim dbprodcode As String = TextBox5.Text
        Dim proddesc As String = RichTextBox1.Text
        Dim srp As Integer = TextBox4.Text
        Dim hchem As Integer

        Try
            prodtype = DirectCast(ComboBox3.SelectedItem, KeyValuePair(Of String, String)).Key
        Catch ex As Exception
            prodtype = ""
        End Try

        Try
            unit = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Key
        Catch ex As Exception
            unit = ""
        End Try

        If CheckBox1.Checked = True Then
            hchem = 1
        Else
            hchem = 0
        End If

        Try

            checkstate()
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "UPDATE products SET barcode='" & barcode & "' ,prodname='" & prodname & "',prodtype='" & prodtype & "',unitID='" & unit & "',proddesc='" & proddesc & "',srp='" & srp & "',unitword='" & unitword & "',hchem='" & hchem & "' WHERE prodcode = '" & dbprodcode & "'"
                .ExecuteNonQuery()
                MessageBox.Show("Product " + prodname + " has been updated!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

                TextBox1.Clear()
                TextBox3.Clear()
                TextBox2.Clear()
                TextBox4.Clear()
                CheckBox1.Checked = False
                ComboBox1.SelectedIndex = -1
                ComboBox2.SelectedIndex = -1
                ComboBox3.SelectedIndex = -1
                ComboBox4.SelectedIndex = -1
                RichTextBox1.Clear()

            End With
        Catch ex As Exception
            MessageBox.Show("Product Not Updated! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dbconn.Close()
        dbconn.Dispose()
    End Sub

    Private Sub TextBox4_Enter(sender As Object, e As EventArgs) Handles TextBox4.Enter
        If String.IsNullOrEmpty(TextBox4.Text) Or TextBox4.Text = "0.0" Then
            TextBox4.Text = ""
        End If
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

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        supplier.ShowDialog()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim prodcode As String = TextBox1.Text
        Dim tandf As Boolean = False
        Dim unitword As String = ""
        Dim hchem As Integer

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM products where barcode='" & prodcode & "'"
                dr = cmd.ExecuteReader

                While dr.Read

                    TextBox5.Text = dr.Item("prodcode")
                    getTemporary(TextBox5.Text)
                    TextBox3.Text = dr.Item("barcode")
                    TextBox2.Text = dr.Item("prodname")
                    TextBox4.Text = dr.Item("srp")
                    RichTextBox1.Text = dr.Item("proddesc")
                    ComboBox2.SelectedValue = dr.Item("unitID")
                    ComboBox3.SelectedValue = dr.Item("prodtype")
                    unitword = dr.Item("unitword")
                    hchem = dr.Item("hchem")

                    tandf = True

                    If unitword <> "" Then
                        ComboBox1.SelectedItem = unitword
                    Else
                        ComboBox1.SelectedIndex = -1
                    End If

                    If hchem = 0 Then
                        CheckBox1.Checked = False
                    Else
                        CheckBox1.Checked = True
                    End If

                End While

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()

        If tandf <> True Then
            MessageBox.Show("Product does not exist!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        Dim unitword As String = ""
        Dim tandf As Boolean = False

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT * FROM products where prodcode='" & DirectCast(ComboBox4.SelectedItem, KeyValuePair(Of String, String)).Key & "'"
                dr = cmd.ExecuteReader

                While dr.Read

                    TextBox5.Text = dr.Item("prodcode")
                    getTemporary(TextBox5.Text)
                    TextBox3.Text = dr.Item("barcode")
                    TextBox2.Text = dr.Item("prodname")
                    TextBox4.Text = dr.Item("srp")
                    RichTextBox1.Text = dr.Item("proddesc")
                    ComboBox2.SelectedValue = dr.Item("unitID")
                    ComboBox3.SelectedValue = dr.Item("prodtype")

                    unitword = dr.Item("unitword")

                    tandf = True

                    If unitword <> "" Then
                        ComboBox1.SelectedItem = unitword
                    Else
                        ComboBox1.SelectedIndex = -1
                    End If

                    If dr.Item("hchem") = 1 Then
                        CheckBox1.Checked = True
                    Else
                        CheckBox1.Checked = False
                    End If

                    tandf = True

                End While

            End With
        Catch ex As Exception
            'MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        dbconn.Close()
        dbconn.Dispose()

        If tandf <> True Then
            'MessageBox.Show("Product does not exist!")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        addunit.ShowDialog()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        addtype.ShowDialog()
    End Sub

    Private Sub ComboBox4_TextChanged(sender As Object, e As EventArgs) Handles ComboBox4.TextChanged
        If ComboBox4.Text.Count = 0 Then
            TextBox1.Clear()
            TextBox3.Clear()
            TextBox2.Clear()
            TextBox4.Clear()
            ComboBox2.SelectedIndex = -1
            ComboBox3.SelectedIndex = -1
            ComboBox4.SelectedIndex = -1
            RichTextBox1.Clear()
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim barcode As String = TextBox3.Text
        Dim prodname As String = TextBox2.Text
        Dim srp As Double = TextBox4.Text
        Dim proddesc As String = RichTextBox1.Text
        Dim hchem As Integer
        Dim unitword As String = ComboBox1.SelectedItem.ToString
        Dim unit As String
        Dim prodtype As String

        Try
            prodtype = DirectCast(ComboBox3.SelectedItem, KeyValuePair(Of String, String)).Key
        Catch ex As Exception
            prodtype = ""
        End Try

        Try
            unit = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Key
        Catch ex As Exception
            unit = ""
        End Try

        If CheckBox1.Checked = True Then
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
                loadproducts()
            End With
        Catch ex As Exception
            MessageBox.Show("Product Not Added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()

    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        deleteproduct.ShowDialog()
    End Sub
End Class