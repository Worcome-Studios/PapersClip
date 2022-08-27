Public Class ScreenShots
    Dim ImageDirList As New ArrayList
    Dim ImageDirListNavigator As Integer = -1

    Private Sub ScreenShots_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.Screenshots_IsEnabled = True Then
            If My.Settings.Screenshots_directory = Nothing Then
                TextBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            Else
                TextBox1.Text = My.Settings.Screenshots_directory
            End If
            If My.Settings.Screenshots_NameFormat = Nothing Then
                ComboBox2.Text = "DD-MM-AAAA HH-MM"
            Else
                ComboBox2.Text = My.Settings.Screenshots_NameFormat
            End If
            CheckBox1.CheckState = CheckState.Checked
            Button3.Enabled = True
            GroupBox1.Enabled = True
            Panel1.Enabled = True
            GetImageList()
        End If
    End Sub
    Private Sub ScreenShots_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Select Case Me.WindowState
            Case FormWindowState.Maximized
                Me.CenterToScreen()
                Me.WindowState = FormWindowState.Normal
            Case FormWindowState.Minimized
                Me.CenterToScreen()
                Me.Hide()
            Case FormWindowState.Normal
                Me.CenterToScreen()
                Me.Show()
                Me.Focus()
        End Select
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.CheckState = CheckState.Checked Then
            Panel1.Enabled = True
            GroupBox1.Enabled = True
            Button3.Enabled = True
            My.Settings.Screenshots_IsEnabled = True
        Else
            Panel1.Enabled = False
            GroupBox1.Enabled = False
            Button3.Enabled = False
            My.Settings.Screenshots_IsEnabled = False
        End If
        My.Settings.Save()
        My.Settings.Reload()
    End Sub
    Private Sub lblAbrir_Click(sender As Object, e As EventArgs) Handles Label5.Click
        Dim FolderBrowser As New FolderBrowserDialog
        FolderBrowser.Description = "Seleccione la carpeta en donde se guardaran las capturas de pantalla"
        FolderBrowser.RootFolder = Environment.SpecialFolder.Desktop
        FolderBrowser.SelectedPath = My.Settings.Screenshots_directory
        FolderBrowser.ShowNewFolderButton = True
        If FolderBrowser.ShowDialog() = DialogResult.OK Then
            My.Settings.Screenshots_directory = FolderBrowser.SelectedPath
            My.Settings.Save()
            My.Settings.Reload()
            TextBox1.Text = FolderBrowser.SelectedPath
        End If
    End Sub
    Private Sub btnConfigurar_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Panel1.Visible = True Then
            Panel1.Visible = False
        Else
            Panel1.Visible = True
        End If
    End Sub
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        ImageDirListNavigator = ListBox1.SelectedIndex
    End Sub
    Private Sub btnVer_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If PictureBox1.Visible = True Then
            PictureBox1.Visible = False
            Button1.Text = "Ver >"
        Else
            PictureBox1.Visible = True
            Try
                PictureBox1.ImageLocation = ImageDirList(ImageDirListNavigator)
            Catch ex As Exception
                Console.WriteLine("[btnVer_Click@ScreenShots]Error: " & ex.Message)
            End Try
            Button1.Text = "< Ocultar"
        End If
    End Sub
    Private Sub ListBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListBox1.MouseDoubleClick
        If PictureBox1.Visible = True Then
            PictureBox1.Visible = False
            Button1.Text = "Ver >"
        Else
            PictureBox1.Visible = True
            Try
                PictureBox1.ImageLocation = ImageDirList(ImageDirListNavigator)
            Catch ex As Exception
                Console.WriteLine("[btnVer_Click@ScreenShots]Error: " & ex.Message)
            End Try
            Button1.Text = "< Ocultar"
        End If
    End Sub
    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            My.Computer.FileSystem.DeleteFile(ImageDirList(ListBox1.SelectedIndex))
            ImageDirList.RemoveAt(ListBox1.SelectedIndex)
            ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
        Catch ex As Exception
            Console.WriteLine("[btnEliminar_Click@ScreenShots]Error: " & ex.Message)
        End Try
    End Sub
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Try
            Process.Start(PictureBox1.ImageLocation)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub btnGuardarYAplicar_Click(sender As Object, e As EventArgs) Handles Button4.Click
        My.Settings.Screenshots_directory = TextBox1.Text
        My.Settings.Screenshots_NameFormat = ComboBox2.Text
        My.Settings.Screenshots_Format = ComboBox1.Text
        My.Settings.Save()
        My.Settings.Reload()
        If Panel1.Visible = True Then
            Panel1.Visible = False
        Else
            Panel1.Visible = True
        End If
    End Sub
    Private Sub lblHelpNameFormats_Click(sender As Object, e As EventArgs) Handles Label8.Click
        MsgBox("Use los nombres que se le proporcionan las opciones." &
                vbCrLf & "dd = Dia" &
                vbCrLf & "MM = Mes" &
                vbCrLf & "yyyy = Año" &
                vbCrLf & "hh = Hora" &
                vbCrLf & "mm = Minutos" &
                vbCrLf & "Numero + 1 = Un contador que aumentara con la cantidad de imagenes", MsgBoxStyle.Information, "Worcome Security")
    End Sub

    Sub GetImageList()
        ImageDirList.Clear()
        ListBox1.Items.Clear()
        Try
            For Each Imagen As String In My.Computer.FileSystem.GetFiles(My.Settings.Screenshots_directory, FileIO.SearchOption.SearchAllSubDirectories, "*.jpg", "*.png", "*.bmp", "*.gif", "*.jpeg", "*.tiff")
                ImageDirList.Add(Imagen)
                Dim StringCutter As String = Imagen
                StringCutter = StringCutter.Remove(0, StringCutter.LastIndexOf("\") + 1)
                ListBox1.Items.Add(StringCutter)
            Next
        Catch ex As Exception
            Console.WriteLine("[GetImageList@ScreenShots]Error: " & ex.Message)
        End Try
    End Sub

    Sub SaveTheImage()
        Try
            If Clipboard.ContainsImage Then
                If My.Settings.Screenshots_NameFormat = "Numero + 1" Then
                    If My.Settings.Screenshots_Format = ".jpg" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & "Screenshot_" & ListBox1.Items.Count + 1 & My.Settings.Screenshots_Format, Imaging.ImageFormat.Jpeg)
                    ElseIf My.Settings.Screenshots_Format = ".bmp" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & "Screenshot_" & ListBox1.Items.Count + 1 & My.Settings.Screenshots_Format, Imaging.ImageFormat.Bmp)
                    ElseIf My.Settings.Screenshots_Format = ".png" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & "Screenshot_" & ListBox1.Items.Count + 1 & My.Settings.Screenshots_Format, Imaging.ImageFormat.Png)
                    ElseIf My.Settings.Screenshots_Format = ".tiff" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & "Screenshot_" & ListBox1.Items.Count + 1 & My.Settings.Screenshots_Format, Imaging.ImageFormat.Tiff)
                    ElseIf My.Settings.Screenshots_Format = ".gif" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & "Screenshot_" & ListBox1.Items.Count + 1 & My.Settings.Screenshots_Format, Imaging.ImageFormat.Gif)
                    ElseIf My.Settings.Screenshots_Format = ".jpeg" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & "Screenshot_" & ListBox1.Items.Count + 1 & My.Settings.Screenshots_Format, Imaging.ImageFormat.Jpeg)
                    End If
                Else
                    If My.Settings.Screenshots_Format = ".jpg" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & DateTime.Now.ToString(My.Settings.Screenshots_NameFormat) & My.Settings.Screenshots_Format, Imaging.ImageFormat.Jpeg)
                    ElseIf My.Settings.Screenshots_Format = ".bmp" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & DateTime.Now.ToString(My.Settings.Screenshots_NameFormat) & My.Settings.Screenshots_Format, Imaging.ImageFormat.Bmp)
                    ElseIf My.Settings.Screenshots_Format = ".png" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & DateTime.Now.ToString(My.Settings.Screenshots_NameFormat) & My.Settings.Screenshots_Format, Imaging.ImageFormat.Png)
                    ElseIf My.Settings.Screenshots_Format = ".tiff" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & DateTime.Now.ToString(My.Settings.Screenshots_NameFormat) & My.Settings.Screenshots_Format, Imaging.ImageFormat.Tiff)
                    ElseIf My.Settings.Screenshots_Format = ".gif" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & DateTime.Now.ToString(My.Settings.Screenshots_NameFormat) & My.Settings.Screenshots_Format, Imaging.ImageFormat.Gif)
                    ElseIf My.Settings.Screenshots_Format = ".jpeg" Then
                        Clipboard.GetImage.Save(My.Settings.Screenshots_directory & "\" & DateTime.Now.ToString(My.Settings.Screenshots_NameFormat) & My.Settings.Screenshots_Format, Imaging.ImageFormat.Jpeg)
                    End If
                End If
                Clipboard.Clear()
                GetImageList()
            End If
        Catch ex As Exception
            Console.WriteLine("[SaveTheImage@ScreenShots]Error: " & ex.Message)
        End Try
    End Sub
End Class