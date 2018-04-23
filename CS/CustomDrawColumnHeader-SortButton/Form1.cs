using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomDrawColumnHeader_SortButton {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            List<MyRecord> list = new List<MyRecord>();
            list.Add(new MyRecord(0, "Steven Baum", "USA"));
            list.Add(new MyRecord(1, "Robert McKinsey", "USA"));
            list.Add(new MyRecord(2, "Robert McKinsey", "UK"));
            list.Add(new MyRecord(3, "Daniella Lloyd", "UK"));

            gridControl1.DataSource = list;
            gridControl1.MainView.PopulateColumns();
            (gridControl1.MainView as ColumnView).Columns["Country"].SortOrder = ColumnSortOrder.Ascending;
        }

        private void gridView1_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e) {
            if (e.Column == null) return;
            //A rectangle for the sort glyph
            Rectangle sortBounds = Rectangle.Empty;
            try {
                //Store the rectangle for the sort glyph in sortBounds and
                //then clear this region in the e.Info object
                UpdateInnerElements(e, false, ref sortBounds);
                //Draw the column header without the sort glyph
                e.Painter.DrawObject(e.Info);
            }
            finally {
                //Restore the rectangle for the sort glyph
                UpdateInnerElements(e, true, ref sortBounds);
            }

            if (!sortBounds.IsEmpty) {
                //Perform custom painting of the sort glyph
                Rectangle newSortBounds = sortBounds;
                newSortBounds.Offset(-10, -1);

                DrawCustomSortedShape(e.Graphics, newSortBounds, e.Column.SortOrder, sortGlyphCollection);
            }
            e.Handled = true;

        }


        void UpdateInnerElements(ColumnHeaderCustomDrawEventArgs e, bool restore, ref Rectangle sortBounds) {
            //Locate an element corresponding to the sort glyph
            foreach (DevExpress.Utils.Drawing.DrawElementInfo item in e.Info.InnerElements) {
                if (item.ElementPainter is DevExpress.Utils.Drawing.SortedShapeObjectPainter) {
                    if (restore) {
                        //Restore the rectangle for the sort glyph
                        item.ElementInfo.Bounds = sortBounds;
                    }
                    else {
                        //Store the rectangle for the sort glyph in sortBounds and
                        //then clear this region in the e.Info object
                        sortBounds = item.ElementInfo.Bounds;
                        item.ElementInfo.Bounds = Rectangle.Empty;
                    }
                }
            }
        }

        private static void DrawCustomSortedShape(Graphics g, Rectangle r, ColumnSortOrder so, ImageCollection imCol) {
            //Draw a custom image for the sort glyph
            if (so == ColumnSortOrder.None) return;
            int i = 0;
            if (so == ColumnSortOrder.Descending) i = 1;

            g.DrawImageUnscaled(imCol.Images[i], r.X, r.Y);
        }

        private void timer1_Tick(object sender, EventArgs e) {
            Text = "'Country' Column Sort Order = " + gridView1.Columns["Country"].SortOrder.ToString();
        }
    }

    public class MyRecord {
        public int ID { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public MyRecord(int id, string name, string country) {
            ID = id;
            Name = name;
            Country = country;
        }
    }
}
