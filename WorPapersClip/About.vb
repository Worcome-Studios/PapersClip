Public Class About

    Private Sub About_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.OfflineMode = False Then
            CheckBox1.CheckState = CheckState.Unchecked
        Else
            CheckBox1.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        AppSupport.Show()
        AppSupport.Focus()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AppUpdate.Show()
        AppUpdate.Focus()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.CheckState = CheckState.Checked Then
            My.Settings.OfflineMode = True
        Else
            My.Settings.OfflineMode = False
        End If
        My.Settings.Save()
        My.Settings.Reload()
    End Sub

    Private Sub About_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        If MessageBox.Show("¿Quiere ver los avisos de seguridad?", "Worcome Security", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
            MsgBox("Aviso de Seguridad 1/3" & vbCrLf & "Los clips no son cifrados ni protegidos", MsgBoxStyle.Information, "Worcome Security")
            MsgBox("Aviso de Seguridad 2/3" & vbCrLf & "El modulo HK esta pendiente de las combinaciones cada 150ms", MsgBoxStyle.Information, "Worcome Security")
            MsgBox("Aviso de Seguridad 3/3" & vbCrLf & "PapersClip no funciona si es cerrado", MsgBoxStyle.Information, "Worcome Security")
        End If
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        AppHelper.Show()
        AppHelper.Focus()
    End Sub
End Class