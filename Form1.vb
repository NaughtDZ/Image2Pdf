Imports System.IO '虽然写在这里，但是pdfsharp和sytem里都有Drawing等字类同名，请下面详写
Imports PdfSharp
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing
Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog() '读图片文件夹
        TextBox1.Text = FolderBrowserDialog1.SelectedPath
        TrackBar1.Value = 0
    End Sub

    Dim filelist() As String
    Private Function GetAllImage(ByVal imgfold As String)
        Dim allfile() As String
        Try
            allfile = System.IO.Directory.GetFiles(imgfold)
        Catch e As Exception
            MsgBox(e.ToString)
            Exit Function
        End Try
        Dim imgfile As Collection = New Collection
        For Each things In allfile
            If things.ToLower Like "*.jpg" OrElse things.ToLower Like "*.pmg" OrElse things.ToLower Like "*.jpeg" OrElse things.ToLower Like "*.bmp" Then
                'Debug.WriteLine("筛选之后：" & things)
                imgfile.Add(things)
            End If
        Next
        Return imgfile
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog()
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Conver.Click
        Conver.Enabled = False
        TrackBar1.Value = 0
        Dim w8t2con = GetAllImage(TextBox1.Text)
        If w8t2con.Count = 0 Then
            MsgBox("没有找到图片")
            Conver.Enabled = True
            Exit Sub
        End If '直接有问题直接退出，不做分支了
        Merg2pdf(w8t2con)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SaveFileDialog1.AddExtension = True
        SaveFileDialog1.Filter = "PDF文件(*.pdf)|*.pdf"
    End Sub

    Private Sub Merg2pdf(ByVal w8t2con)
        TrackBar1.Maximum = w8t2con.Count
        Dim document As PdfDocument = New PdfDocument '创建pdf文件
        For Each img In w8t2con
            Dim page As PdfPage = document.AddPage() '创建新页
            Dim gfx As XGraphics = XGraphics.FromPdfPage(page) '创建画布在page上
            Dim ximg As XImage = XImage.FromFile(img) '创建gfx可用的image
            Dim g As Graphics
            g = Graphics.FromImage(Image.FromFile(img))
            Debug.WriteLine(ximg.HorizontalResolution) 'pdf打印页面大小与DPI有关
            page.Width = ximg.PixelWidth '设置页面为图片分辨率,piexel是分辨率，width是通过dpi转换后的大小
            page.Height = ximg.PixelHeight
            Debug.WriteLine("{0} x:{1} y:{2}", img, page.Width, page.Height)
            Debug.WriteLine("x:{0} y:{1}", g.DpiX / TextBox3.Text, g.DpiY / TextBox3.Text)
            gfx.ScaleTransform(ximg.HorizontalResolution / TextBox3.Text)
            gfx.DrawImage(ximg, 0, 0)
            TrackBar1.Value += 1
        Next
        document.Save(TextBox2.Text)
        Beep()
        document.Close()
        Conver.Enabled = True
    End Sub

End Class
