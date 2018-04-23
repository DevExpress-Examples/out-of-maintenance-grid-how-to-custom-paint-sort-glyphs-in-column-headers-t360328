Imports DevExpress.Data
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms

Namespace CustomDrawColumnHeader_SortButton
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			Dim list As New List(Of MyRecord)()
			list.Add(New MyRecord(0, "Steven Baum", "USA"))
			list.Add(New MyRecord(1, "Robert McKinsey", "USA"))
			list.Add(New MyRecord(2, "Robert McKinsey", "UK"))
			list.Add(New MyRecord(3, "Daniella Lloyd", "UK"))

			gridControl1.DataSource = list
			gridControl1.MainView.PopulateColumns()
			TryCast(gridControl1.MainView, ColumnView).Columns("Country").SortOrder = ColumnSortOrder.Ascending
		End Sub

		Private Sub gridView1_CustomDrawColumnHeader(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs) Handles gridView1.CustomDrawColumnHeader
			If e.Column Is Nothing Then
				Return
			End If
			'A rectangle for the sort glyph
			Dim sortBounds As Rectangle = Rectangle.Empty
			Try
				'Store the rectangle for the sort glyph in sortBounds and
				'then clear this region in the e.Info object
				UpdateInnerElements(e, False, sortBounds)
				'Draw the column header without the sort glyph
				e.Painter.DrawObject(e.Info)
			Finally
				'Restore the rectangle for the sort glyph
				UpdateInnerElements(e, True, sortBounds)
			End Try

			If Not sortBounds.IsEmpty Then
				'Perform custom painting of the sort glyph
				Dim newSortBounds As Rectangle = sortBounds
				newSortBounds.Offset(-10, -1)

				DrawCustomSortedShape(e.Cache, newSortBounds, e.Column.SortOrder, sortGlyphCollection)
			End If
			e.Handled = True

		End Sub


		Private Sub UpdateInnerElements(ByVal e As ColumnHeaderCustomDrawEventArgs, ByVal restore As Boolean, ByRef sortBounds As Rectangle)
			'Locate an element corresponding to the sort glyph
			For Each item As DevExpress.Utils.Drawing.DrawElementInfo In e.Info.InnerElements
				If TypeOf item.ElementPainter Is DevExpress.Utils.Drawing.SortedShapeObjectPainter Then
					If restore Then
						'Restore the rectangle for the sort glyph
						item.ElementInfo.Bounds = sortBounds
					Else
						'Store the rectangle for the sort glyph in sortBounds and
						'then clear this region in the e.Info object
						sortBounds = item.ElementInfo.Bounds
						item.ElementInfo.Bounds = Rectangle.Empty
					End If
				End If
			Next item
		End Sub

		Private Shared Sub DrawCustomSortedShape(ByVal cache As GraphicsCache, ByVal r As Rectangle, ByVal so As ColumnSortOrder, ByVal imCol As ImageCollection)
			'Draw a custom image for the sort glyph
			If so = ColumnSortOrder.None Then
				Return
			End If
			Dim i As Integer = 0
			If so = ColumnSortOrder.Descending Then
				i = 1
			End If
			cache.Paint.DrawImageUnscaled(cache.Graphics, imCol.Images(i), New Point(r.X, r.Y))
		End Sub

		Private Sub timer1_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles timer1.Tick
			Text = "'Country' Column Sort Order = " & gridView1.Columns("Country").SortOrder.ToString()
		End Sub
	End Class

	Public Class MyRecord
		Public Property ID() As Integer
		Public Property Country() As String
		Public Property Name() As String
		Public Sub New(ByVal id As Integer, ByVal name As String, ByVal country As String)
			Me.ID = id
			Me.Name = name
			Me.Country = country
		End Sub
	End Class
End Namespace
