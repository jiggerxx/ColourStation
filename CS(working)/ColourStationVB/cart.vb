Public Class cart

    Public cartcounterX As Integer = 0
    Public draftcounterX As Integer = 0
    Public cartdataArray(100, 5) As String
    Public draftarray(100, 5) As String
    Public totalpays As Double = 0
    Public draftpays As Double = 0
    Public mixxerArray(100, 3) As String
    Public mixxerCounterX As Integer = 0
    Public draftmixxerCounterx As Integer = 0
    Public draftmixxerArray(100, 3) As String
    Public pintorreadymixtotal As Double = 0
    Public pintormixtotal As Double = 0
    Public agentcanvassertotal As Double = 0
    Public totalunits As Double = 0

    Public pintorreadymixtotaldraft As Double = 0
    Public pintormixtotaldraft As Double = 0
    Public agentcanvassertotaldraft As Double = 0
    Public totalunitsdraft As Double = 0
    Private hchem As Integer

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim totalqty As Integer = 0
        Dim prodcode As String() = ComboBox1.Text.Split(New Char() {" "c})
        Dim exist As Boolean = False
        Dim added As Boolean = False
        Dim price As Double = 0
        Dim mixxerkey As String = ""
        Dim mixtype As String = ""
        Dim mixxerexist As Boolean = False
        Dim productlocator As Integer = 0
        Dim unitdetected As Double = 0


        Try
            mixxerkey = DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, String)).Key
        Catch ex As Exception
            mixxerkey = ""
        End Try

        If RadioButton1.Checked = True Then
            mixtype = "Ready Mix"
        ElseIf RadioButton2.Checked = True Then
            mixtype = "Mixing"
        End If

        For x = 0 To cartcounterX - 1
            If cartdataArray(x, 0).ToString.Equals(prodcode(0)) Then
                totalqty = Convert.ToDouble(cartdataArray(x, 5)) + NumericUpDown1.Value

                If totalqty <= NumericUpDown1.Maximum Then
                    cartdataArray(x, 5) = totalqty.ToString
                    totalpays = totalpays + (Convert.ToDouble(cartdataArray(x, 4)) * NumericUpDown1.Value)
                    totalunits = totalunits + (Convert.ToDouble(cartdataArray(x, 3)) * NumericUpDown1.Value)
                Else
                    MessageBox.Show("Maximum Limit Reached For This Product", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                unitdetected = cartdataArray(x, 3)
                productlocator = x
                exist = True
            End If
        Next


        If exist.Equals(False) Then

            Try
                dbconn.Open()

                With cmd
                    .Connection = dbconn
                    .CommandText = "SELECT products.*,type.*,units.* FROM csdb.products LEFT JOIN units ON products.unitID=units.unitID LEFT JOIN type ON products.prodtype =type.typeID where prodcode='" & prodcode(0) & "'"
                    dr = cmd.ExecuteReader

                    While dr.Read

                        cartdataArray(cartcounterX, 0) = dr.Item("prodcode")
                        cartdataArray(cartcounterX, 1) = dr.Item("prodname")
                        cartdataArray(cartcounterX, 4) = dr.Item("srp")
                        cartdataArray(cartcounterX, 2) = dr.Item("type")
                        cartdataArray(cartcounterX, 3) = dr.Item("unit")
                        unitdetected = dr.Item("unit")
                        totalunits = totalunits + (Convert.ToDouble(dr.Item("unit")) * NumericUpDown1.Value)
                        price = dr.Item("srp")

                        added = True

                    End While

                    cartdataArray(cartcounterX, 5) = NumericUpDown1.Value
                    price = price * NumericUpDown1.Value
                    totalpays = totalpays + price
                End With
            Catch ex As Exception
                MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            dbconn.Close()
            dbconn.Dispose()

        End If

        agentcanvassertotal = (totalunits / 4) * 5

        If ComboBox2.SelectedIndex >= 0 Then

            For x = 0 To mixxerCounterX - 1
                If mixxerArray(x, 0).ToString.Equals(mixxerkey) And mixxerArray(x, 1).ToString.Equals(mixtype) Then

                    If mixtype.Equals("Ready Mix") And hchem = 1 Then
                        mixxerArray(x, 2) = Convert.ToDouble(mixxerArray(x, 2)) + (((NumericUpDown1.Value * unitdetected) / 4) * 5)
                        mixxerArray(x, 3) = Convert.ToDouble(mixxerArray(x, 3)) + (NumericUpDown1.Value * unitdetected)
                        pintorreadymixtotal = pintorreadymixtotal + (((NumericUpDown1.Value * unitdetected) / 4) * 20)
                    ElseIf mixtype.Equals("Mixing") Then
                        mixxerArray(x, 2) = Convert.ToDouble(mixxerArray(x, 2)) + (((NumericUpDown1.Value * unitdetected) / 4) * 10)
                        mixxerArray(x, 3) = Convert.ToDouble(mixxerArray(x, 3)) + (NumericUpDown1.Value * unitdetected)
                        pintormixtotal = pintormixtotal + (((NumericUpDown1.Value * unitdetected) / 4) * 50)
                    End If

                    mixxerexist = True
                End If
            Next

            If mixxerexist = False Then
                mixxerArray(mixxerCounterX, 0) = mixxerkey
                mixxerArray(mixxerCounterX, 1) = mixtype

                If mixtype = "Ready Mix" And hchem = 1 Then
                    mixxerArray(mixxerCounterX, 2) = ((NumericUpDown1.Value * unitdetected) / 4) * 5
                    mixxerArray(mixxerCounterX, 3) = Convert.ToDouble(mixxerArray(mixxerCounterX, 3)) + (NumericUpDown1.Value * unitdetected)
                    pintorreadymixtotal = pintorreadymixtotal + (((NumericUpDown1.Value * unitdetected) / 4) * 20)
                ElseIf mixtype = "Mixing" Then
                    mixxerArray(mixxerCounterX, 2) = ((NumericUpDown1.Value * unitdetected) / 4) * 10
                    mixxerArray(mixxerCounterX, 3) = Convert.ToDouble(mixxerArray(mixxerCounterX, 3)) + (NumericUpDown1.Value * unitdetected)
                    pintormixtotal = pintormixtotal + (((NumericUpDown1.Value * unitdetected) / 4) * 50)
                End If

                mixxerCounterX = mixxerCounterX + 1

            End If

            ComboBox2.SelectedIndex = -1
            RadioButton1.Checked = False
            RadioButton2.Checked = False
            RadioButton1.Enabled = False
            RadioButton2.Enabled = False

        End If

        If added.Equals(True) Then
            cartcounterX = cartcounterX + 1
        End If

        DataGridView1.Rows.Clear()
        For x = 0 To cartcounterX - 1
            DataGridView1.Rows.Add(New String() {cartdataArray(x, 0), cartdataArray(x, 1), cartdataArray(x, 2), cartdataArray(x, 3), cartdataArray(x, 4), cartdataArray(x, 5)})
        Next

        totalcost.Text = totalpays

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DataGridView1.Rows.Clear()
        prodcode.Text = "-"
        prodname.Text = "-"
        produnit.Text = "-"
        prodtype.Text = "-"
        srp.Text = "-"
        qty.Text = "-"
        ComboBox1.SelectedIndex = -1
        totalcost.Text = "-"

        cartcounterX = 0
        draftcounterX = 0
        totalpays = 0
        draftpays = 0
        mixxerCounterX = 0
        draftmixxerCounterx = 0
        pintorreadymixtotal = 0
        pintormixtotal = 0
        agentcanvassertotal = 0
        totalunits = 0

        pintorreadymixtotaldraft = 0
        pintormixtotaldraft = 0
        agentcanvassertotaldraft = 0
        totalunitsdraft = 0
    End Sub

    Private Sub DataGridView1_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles DataGridView1.UserDeletingRow
        Dim delindex As Integer = e.Row.Index

        For a As Integer = delindex To cartcounterX - 1

            cartdataArray(a, 0) = cartdataArray(a + 1, 0)
            cartdataArray(a, 1) = cartdataArray(a + 1, 1)
            cartdataArray(a, 2) = cartdataArray(a + 1, 2)
            cartdataArray(a, 3) = cartdataArray(a + 1, 3)
            cartdataArray(a, 4) = cartdataArray(a + 1, 4)
            cartdataArray(a, 5) = cartdataArray(a + 1, 5)

        Next
        cartcounterX = cartcounterX - 1
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If totalpays <> 0 Then
            invoice.TextBox3.Text = totalcost.Text
            invoice.TextBox6.Text = totalcost.Text
            invoice.ShowDialog()
        Else
            MessageBox.Show("Please purchase atleast 1(one) product to pay!", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub



    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            TextBox1.SelectAll()

            Try
                Dim comprodcode As String = TextBox1.Text

                dbconn.Open()

                With cmd
                    .Connection = dbconn
                    .CommandText = "SELECT products.*,type.*,units.* FROM csdb.products LEFT JOIN units ON products.unitID=units.unitID LEFT JOIN type ON products.prodtype =type.typeID where barcode='" & comprodcode & "'"
                    dr = cmd.ExecuteReader

                    While dr.Read

                        prodcode.Text = dr.Item("barcode")
                        prodname.Text = dr.Item("prodname")
                        produnit.Text = dr.Item("unit") & " " & dr.Item("unitword")
                        prodtype.Text = dr.Item("type")
                        srp.Text = dr.Item("srp")
                        qty.Text = dr.Item("stock")

                        NumericUpDown1.Maximum = dr.Item("stock")

                        If dr.Item("stock") > 0 Then
                            NumericUpDown1.Minimum = 1
                        Else
                            NumericUpDown1.Minimum = 0
                        End If

                    End While

                End With
            Catch ex As Exception
                'MessageBox.Show(ex.Message + "Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            dbconn.Close()
            dbconn.Dispose()
        End If
    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click
        If Label11.ForeColor <> Color.Red Then
            Label11.ForeColor = Color.Red
            Label10.ForeColor = Color.Black
            TextBox1.Visible = True
            ComboBox1.Visible = False
            TextBox1.Focus()
        End If
    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs) Handles Label10.Click
        If Label10.ForeColor <> Color.Red Then
            Label10.ForeColor = Color.Red
            Label11.ForeColor = Color.Black
            ComboBox1.Visible = True
            TextBox1.Visible = False
        End If
    End Sub

    Private Sub cart_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        If cartcounterX <> 0 Then
            draftcounterX = cartcounterX
            For x As Integer = 0 To draftcounterX - 1
                draftarray(x, 0) = cartdataArray(x, 0)
                draftarray(x, 1) = cartdataArray(x, 1)
                draftarray(x, 2) = cartdataArray(x, 2)
                draftarray(x, 3) = cartdataArray(x, 3)
                draftarray(x, 4) = cartdataArray(x, 4)
                draftarray(x, 5) = cartdataArray(x, 5)
            Next
            cartcounterX = 0
            draftpays = totalpays
            totalpays = 0
            totalcost.Text = 0

            draftmixxerCounterx = mixxerCounterX

            For x As Integer = 0 To draftmixxerCounterx - 1
                draftmixxerArray(x, 0) = mixxerArray(x, 0)
                draftmixxerArray(x, 1) = mixxerArray(x, 1)
                draftmixxerArray(x, 2) = mixxerArray(x, 2)
            Next
            mixxerCounterX = 0

            pintorreadymixtotaldraft = pintorreadymixtotal
            pintormixtotaldraft = pintormixtotal
            agentcanvassertotaldraft = agentcanvassertotal
            totalunitsdraft = totalunits

            pintorreadymixtotal = 0
            pintormixtotal = 0
            agentcanvassertotal = 0
            totalunits = 0

            MessageBox.Show("Draft Saved", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

            DataGridView1.Rows.Clear()
        Else
            MessageBox.Show("No orders to draft!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        If draftcounterX <> 0 Then
            cartcounterX = draftcounterX
            For x As Integer = 0 To draftcounterX - 1
                cartdataArray(x, 0) = draftarray(x, 0)
                cartdataArray(x, 1) = draftarray(x, 1)
                cartdataArray(x, 2) = draftarray(x, 2)
                cartdataArray(x, 3) = draftarray(x, 3)
                cartdataArray(x, 4) = draftarray(x, 4)
                cartdataArray(x, 5) = draftarray(x, 5)
            Next
            draftcounterX = 0
            totalpays = draftpays
            totalcost.Text = totalpays

            mixxerCounterX = draftmixxerCounterx

            For x As Integer = 0 To mixxerCounterX - 1
                mixxerArray(x, 0) = draftmixxerArray(x, 0)
                mixxerArray(x, 1) = draftmixxerArray(x, 1)
                mixxerArray(x, 2) = draftmixxerArray(x, 2)
            Next
            draftmixxerCounterx = 0

            pintorreadymixtotal = pintorreadymixtotaldraft
            pintormixtotal = pintormixtotaldraft
            agentcanvassertotal = agentcanvassertotaldraft
            totalunits = totalunitsdraft

            pintorreadymixtotaldraft = 0
            pintormixtotaldraft = 0
            agentcanvassertotaldraft = 0
            totalunitsdraft = 0

            DataGridView1.Rows.Clear()
            For x = 0 To cartcounterX - 1
                DataGridView1.Rows.Add(New String() {cartdataArray(x, 0), cartdataArray(x, 1), cartdataArray(x, 2), cartdataArray(x, 3), cartdataArray(x, 4), cartdataArray(x, 5)})
            Next
        Else
            MessageBox.Show("No draft saved!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub ComboBox2_TextChanged(sender As Object, e As EventArgs) Handles ComboBox2.TextChanged
        If ComboBox2.SelectedText = "" Then
            RadioButton1.Checked = False
            RadioButton2.Checked = False
            RadioButton1.Enabled = True
            RadioButton2.Enabled = True
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedIndex >= 0 Then
            If hchem = 0 Then
                RadioButton2.Enabled = True
                RadioButton2.Checked = True
            ElseIf hchem = 1 Then
                RadioButton1.Checked = True
                RadioButton1.Enabled = True
                RadioButton2.Enabled = True
            End If
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        addagent.ShowDialog()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub ComboBox1_TextChanged(sender As Object, e As EventArgs) Handles ComboBox1.TextChanged
        
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text <> "" Then
            cartSelectItem()
        Else
            loadproductsCart()
        End If

    End Sub

    Private Sub ComboBox1_TextUpdate(sender As Object, e As EventArgs) Handles ComboBox1.TextUpdate

        If ComboBox1.Text.Equals("") Then
            ComboBox1.DroppedDown = False
        Else
            loadproductswithstockstextchange()
            cartTextChanged()
        End If
    End Sub
End Class