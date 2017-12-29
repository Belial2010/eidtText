using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XZ.Edit;
using XZ.Edit.Entity;
using XZ.Edit.Forms;

namespace XZ.EditApp {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private bool pIsChange = false;

        private void Form1_Load(object sender, EventArgs e) {
            this.InitText();
            this.editTextBox1.Font = new Font("Courier New", 10);
            this.editTextBox1.SetLanguage("CSharp");
            this.editTextBox1.SetWordStyleEvent = (w) => {
                if (w == "Word")
                    return new WFontColor(this.editTextBox1.Font, Color.FromArgb(43, 145, 175));

                return null;
            };
            this.editTextBox1.Text = this.richTextBox1.Text;
            //this.njEditTextBox1.AddWrods(new string[] { "CursorAndIME", "DefTextEditorProperties", "CharCommand", "Graphics", "LineStrings", "InsertCharComplete", "List" });
            this.editTextBox1.Focus();

            //this.editTextBox1.ToolTipMessageEvent += editTextBox1_ToolTipMessageEvent;
            this.editTextBox1.InsertCharEvent += editTextBox1_InsertCharEvent;
            //this.njEditTextBox2.SetLanguage("CSharp");
        }
        CompletionWindow cw = null;
        void editTextBox1_InsertCharEvent(char c, Point charLocation, Edit.Interfaces.IEdit edit, Point xyLocation) {
            if (char.IsLetterOrDigit(c) || c == '.' || c == CharCommand.Char_BackSpace) {
                var ls = edit.GetParser.PLineString[charLocation.Y];
                if (cw == null || cw.IsDisposed) {
                    cw = new CompletionWindow(edit, new CompletionData[]{
                    new CompletionData("StringBuilder",this.GetStringBuilder(),ResList.ClassIndex),
                    new CompletionData("string","表示文本，即一系列 Unicode 字符",ResList.ClassIndex),
                    new CompletionData("int","表示 32 位有符号的整数。",ResList.ClassIndex),
                    new CompletionData("ToolTipMessageEventArgs","",ResList.EventIndex),
                    new CompletionData("ToolTipMessage","",ResList.MethodIndex),
                    new CompletionData("IsLetterOrDigit","",ResList.MethodIndex),
                    new CompletionData("CompletionData","",ResList.PropertyIndex),
                    new CompletionData("AppendLine","",ResList.MethodIndex),
                    new CompletionData("ShowToolTip","",ResList.FieldIndex),
                    new CompletionData("FileTools","",ResList.InterfaceIndex),
                    new CompletionData("InitializeComponent","",ResList.NameSpaceIndex),
                    new CompletionData("SetLanguage","",ResList.MethodIndex)
                });
                    cw.SetlocationX(xyLocation.X, charLocation.X);
                    cw.ShowCompletionWindow();
                }
                this.cw.ChangeChar();
            }
        }

        private string GetStringBuilder() {
            //var sbStr = new StringBuilder();
            //sbStr.AppendLine("表示可变字符字符串。此类不能被继承");
            StringBuilder sbStr = new StringBuilder();
            sbStr.AppendLine("void editTextBox1_InsertCharEvent(char c, Point charLocation, Edit.Interfaces.IEdit edit, Point xyLocation) {");
            sbStr.AppendLine("            if (char.IsLetterOrDigit(c) || c == '.' || c == CharCommand.Char_BackSpace) {");
            sbStr.AppendLine("                var ls = edit.GetParser.PLineString[charLocation.Y];");
            sbStr.AppendLine("                if (cw == null || cw.IsDisposed) {");
            sbStr.AppendLine("                    cw = new CompletionWindow(edit, new CompletionData[]{");
            sbStr.AppendLine("                    new CompletionData(\"StringBuilder\",this.GetStringBuilder(),ResList.ClassIndex),");
            sbStr.AppendLine("                    new CompletionData(\"string\",\"表示文本，即一系列 Unicode 字符\",ResList.ClassIndex),");
            sbStr.AppendLine("                    new CompletionData(\"int\",\"表示 32 位有符号的整数。\",ResList.ClassIndex),");
            sbStr.AppendLine("                    new CompletionData(\"ToolTipMessageEventArgs\",\"\",ResList.EventIndex),");
            sbStr.AppendLine("                    new CompletionData(\"ToolTipMessage\",\"\",ResList.MethodIndex),");
            sbStr.AppendLine("                    new CompletionData(\"IsLetterOrDigit\",\"\",ResList.MethodIndex),");
            sbStr.AppendLine("                    new CompletionData(\"CompletionData\",\"\",ResList.PropertyIndex),");
            sbStr.AppendLine("                    new CompletionData(\"AppendLine\",\"\",ResList.MethodIndex),");
            sbStr.AppendLine("                    new CompletionData(\"ShowToolTip\",\"\",ResList.FieldIndex),");
            sbStr.AppendLine("                    new CompletionData(\"FileTools\",\"\",ResList.InterfaceIndex),");
            sbStr.AppendLine("                    new CompletionData(\"InitializeComponent\",\"\",ResList.NameSpaceIndex),");
            sbStr.AppendLine("                    new CompletionData(\"SetLanguage\",\"\",ResList.MethodIndex)");
            sbStr.AppendLine("                });");
            sbStr.AppendLine("                    cw.SetlocationX(xyLocation.X, charLocation.X);");
            sbStr.AppendLine("                    cw.ShowCompletionWindow();");
            sbStr.AppendLine("");
            sbStr.AppendLine("                }");
            sbStr.AppendLine("                this.cw.ChangeChar();");
            sbStr.AppendLine("            }");
            sbStr.AppendLine("        }");

            return sbStr.ToString();
        }

        void editTextBox1_ToolTipMessageEvent(Edit.Interfaces.IEdit sender, Edit.Entity.ToolTipMessageEventArgs e) {
            StringBuilder sbStr = new StringBuilder();
            sbStr.AppendLine("摘要:");
            sbStr.AppendLine("    string MD5(string value);");
            sbStr.AppendLine("    将字符串进行MD5加密");
            sbStr.AppendLine("    ");
            sbStr.AppendLine("参数：");
            sbStr.AppendLine("  value:");
            sbStr.AppendLine("    需要加密的字符串");
            sbStr.AppendLine("    ");
            sbStr.AppendLine("返回结果:");
            sbStr.AppendLine("    加密的32位长度的字符串");
            sbStr.AppendLine("    ");
            e.ShowToolTip(sbStr.ToString());
        }

        private void InitText() {
            string path = @"d:\1.cs";
            StringBuilder sbStr = new StringBuilder();
            if (File.Exists(path)) {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                    StreamReader streamReader = new StreamReader(fileStream, Encoding.GetEncoding("GB2312"));
                    while (streamReader.Peek() > -1) {
                        sbStr.AppendLine(streamReader.ReadLine());
                    }
                }
            } else {
                sbStr.AppendLine("using System;");
                sbStr.AppendLine("using System.Collections.Generic;");
                sbStr.AppendLine("using System.Linq;");
                sbStr.AppendLine("using System.Text;");
                sbStr.AppendLine("");
                sbStr.AppendLine("");
                sbStr.AppendLine("namespace XZ.Edit {");
                sbStr.AppendLine("");
                sbStr.AppendLine("");
                sbStr.AppendLine("    /// <summary>");
                sbStr.AppendLine("    /// 设置字符样式 ");
                sbStr.AppendLine("    /// </summary>");
                sbStr.AppendLine("    public class CharFontStyle {");
                sbStr.AppendLine("");
                sbStr.AppendLine("        private LanguageMode pLanguageMode;");
                sbStr.AppendLine("");
                sbStr.AppendLine("        public void SetLanguageMode(LanguageMode mode){");
                sbStr.AppendLine("            this.pLanguageode == mode;");
                sbStr.AppendLine("");
                sbStr.AppendLine("            //string _str = \"不忘初心，\"方得始终\";");
                sbStr.AppendLine("");
                sbStr.AppendLine("");
                sbStr.AppendLine("        }");
                sbStr.AppendLine("");
                sbStr.AppendLine("		/// <summary>");
                sbStr.AppendLine("        /// 获取行字符串");
                sbStr.AppendLine("        /// </summary>");
                sbStr.AppendLine("        /// <param name=\"text\"></param>");
                sbStr.AppendLine("        /// <returns></returns>");
                sbStr.AppendLine("        public List<LineString> GetPLineStrings(string text) { ");
                sbStr.AppendLine("		");
                sbStr.AppendLine("		}");
                sbStr.AppendLine("");
                sbStr.AppendLine("        /// <summary>");
                sbStr.AppendLine("        /// 获取行字符串");
                sbStr.AppendLine("        /// </summary>");
                sbStr.AppendLine("        /// <param name=\"text\"></param>");
                sbStr.AppendLine("        /// <returns></returns>");
                sbStr.AppendLine("        public List<LineString> GetPLineStrings(string text) { ");
                sbStr.AppendLine("		var dd = 0;");
                sbStr.AppendLine("");
                sbStr.AppendLine("            if (string.IsNullOrEmpty(text)){");
                sbStr.AppendLine("if(this.name == \"\"){");
                sbStr.AppendLine("	");
                sbStr.AppendLine("}");
                sbStr.AppendLine("			}");
                sbStr.AppendLine("				return new List<LineString>(\"\");");
                sbStr.AppendLine("			");
                sbStr.AppendLine("");
                sbStr.AppendLine("            var ls = new List<LineString>();");
                sbStr.AppendLine("            var array = text.Split(CharCommand.Char_Newline);");
                sbStr.AppendLine("            foreach (var line in array)");
                sbStr.AppendLine("                ls.Add(this.GetLineString(line));");
                sbStr.AppendLine("");
                sbStr.AppendLine("            return ls;");
                sbStr.AppendLine("        }");
                sbStr.AppendLine("");
                sbStr.AppendLine("        /// <summary>");
                sbStr.AppendLine("        /// 获取行字符串");
                sbStr.AppendLine("        /// </summary>");
                sbStr.AppendLine("        /// <param name=\"line\"></param>");
                sbStr.AppendLine("        /// <returns></returns>");
                sbStr.AppendLine("        public LineString GetLineString(string line) {");
                sbStr.AppendLine("var name = '=';");
                sbStr.AppendLine("            line = line.TrimEnd(CharCommand.Char_Enter);");
                sbStr.AppendLine("            int length = 0;");
                sbStr.AppendLine("            Word w;");
                sbStr.AppendLine("            WFontColor wfc;");
                sbStr.AppendLine("            var lineString = new LineString();");
                sbStr.AppendLine("            lineString.PWrod = new List<Word>();");
                sbStr.AppendLine("            lineString.Text = line;");
                sbStr.AppendLine("            while (length == line.Length) {");
                sbStr.AppendLine("                char c = line[length];");
                sbStr.AppendLine("                switch (c) {");
                sbStr.AppendLine("                    case '\t':");
                sbStr.AppendLine("                        w = new Word();");
                sbStr.AppendLine("                        w.PEWordType = EWordType.Tab;");
                sbStr.AppendLine("                        break;");
                sbStr.AppendLine("                    default:");
                sbStr.AppendLine("                        if (pLanguageMode.CompartChars.Contains(c)) {");
                sbStr.AppendLine("                            w = new Word();");
                sbStr.AppendLine("                            w.PEWordType = EWordType.Compart;");
                sbStr.AppendLine("                            w.Text = c.ToString();");
                sbStr.AppendLine("                            if (pLanguageMode.CompartCharFont.TryGetValue(c, out wfc))");
                sbStr.AppendLine("                                w.PFont = wfc;");
                sbStr.AppendLine("                        } else {");
                sbStr.AppendLine("                            w = new Word();");
                sbStr.AppendLine("                            w.PEWordType = EWordType.Word;");
                sbStr.AppendLine("                            w.Text = c.ToString() + GetTag(line, ref length);");
                sbStr.AppendLine("                            if (pLanguageMode.WordFonts.TryGetValue(w.Text, out wfc))");
                sbStr.AppendLine("                                w.PFont = wfc;");
                sbStr.AppendLine("                        }");
                sbStr.AppendLine("                        break;");
                sbStr.AppendLine("                }");
                sbStr.AppendLine("                lineString.PWrod.Add(w);");
                sbStr.AppendLine("                length++;");
                sbStr.AppendLine("            }");
                sbStr.AppendLine("            return lineString;");
                sbStr.AppendLine("        }");
                sbStr.AppendLine("");
                sbStr.AppendLine("");
                sbStr.AppendLine("    }");
                sbStr.AppendLine("}");
                sbStr.AppendLine("");
                sbStr.AppendLine("");

            }

            this.richTextBox1.Text = sbStr.ToString();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) {
            this.pIsChange = true;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.pIsChange) {
                this.editTextBox1.Text = this.richTextBox1.Text;
                this.pIsChange = false;
            }
        }

        private void tsm_find_Click(object sender, EventArgs e) {
            var ft = new FindText();
            ft.CallBack = (fdText) => {
                this.editTextBox1.FindTexts(fdText);
            };
            ft.ShowDialog();
        }

        private void 注释ToolStripMenuItem_Click(object sender, EventArgs e) {
            this.editTextBox1.AddComment("//");
        }

        private void 取消注释ToolStripMenuItem_Click(object sender, EventArgs e) {
            this.editTextBox1.AddComment("//", true);
        }
    }
}
