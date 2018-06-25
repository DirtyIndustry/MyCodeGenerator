using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClassPropertyGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string input = InputBox.Text;
            string[] inputarray = input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (TypeBox.SelectedIndex == 0)
            {
                声明变量(inputarray);
            }
            else if (TypeBox.SelectedIndex == 1)
            {
                声明Command(inputarray);
            }
            else if (TypeBox.SelectedIndex == 2)
            {
                生成Xaml(inputarray);
            }
            else if (TypeBox.SelectedIndex == 3)
            {
                生成Html(inputarray);
            }
        }

        private void 声明变量(string[] inputarray)
        {
            string privatefields = "";
            string publicproperties = "";
            string defaultvalues = "";
            foreach (string s in inputarray)
            {
                if (s.Equals(""))
                {
                    continue;
                }
                string oldname = "";
                string newname = "";
                string type = "";
                MakeTypeName(s, out oldname, out type);
                MakeUpperName(oldname, out newname);

                privatefields += "private " + type + " " + oldname + ";" + Environment.NewLine;
                publicproperties += "public " + type + " " + newname + " { "
                    + "get => " + oldname + "; set { " + oldname + " = value; OnPropertyChanged(\"" + newname + "\"); } }" + Environment.NewLine;

                if (oldname.EndsWith("Enum"))
                    continue;
                if (type.Equals("int"))
                    defaultvalues += oldname + " = 0;" + Environment.NewLine;
                else if (type.Equals("bool"))
                    defaultvalues += oldname + " = true;" + Environment.NewLine;
                else if (type.Equals("string"))
                    defaultvalues += oldname + " = \"\";" + Environment.NewLine;
                else if (type.Equals("DateTime"))
                    defaultvalues += oldname + " = DateTime.MinValue;" + Environment.NewLine;
                else if (type.StartsWith("List<"))
                    defaultvalues += oldname + " = new " + type + "();" + Environment.NewLine;
                
            }

            OutputBox.Text = privatefields + Environment.NewLine;
            OutputBox.Text += "----------------------------------" + Environment.NewLine;
            OutputBox.Text += publicproperties + Environment.NewLine;
            OutputBox.Text += "----------------------------------" + Environment.NewLine;
            OutputBox.Text += defaultvalues + Environment.NewLine;
        }

        private void 声明Command(string[] inputarray)
        {
            string command = "";
            foreach (string s in inputarray)
            {
                if (s.Equals(""))
                {
                    continue;
                }
                command += "public ICommand " + s + Environment.NewLine;
                command += "{ get { return new RelayCommand(" + s + "Execute); } }" + Environment.NewLine;
                command += "private void " + s + "Execute()" + Environment.NewLine;
                command += "{ }" + Environment.NewLine;
            }

            OutputBox.Text = command;
        }

        private void 生成Xaml(string[] inputarray)
        {
            string gridWidth = "500";
            string gridMargin = "5";
            string firstcolumnwidth = "120";

            string xaml = "";
            foreach (string s in inputarray)
            {
                if (s.Equals(""))
                {
                    continue;
                }
                string name = "";
                string type = "";
                MakeTypeName(s, out name, out type);
                MakeUpperName(name, out name);

                string grid = "";
                if (name.EndsWith("Enum"))
                {
                    continue;
                }
                else if (type.Equals("int"))
                {
                    grid += "<Grid Width=\"" + gridWidth + "\" Margin=\"" + gridMargin + "\">" + Environment.NewLine;
                    grid += "<Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<ColumnDefinition Width=\"" + firstcolumnwidth + "\" />" + Environment.NewLine;
                    grid += "<ColumnDefinition />" + Environment.NewLine;
                    grid += "</Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<TextBlock Text=\"" + name + "\" />" + Environment.NewLine;
                    grid += "<ComboBox Grid.Column=\"1\" ItemsSource=\"{Binding "+name+"Enum}\" SelectedIndex=\"{Binding "+name+", Mode = TwoWay, UpdateSourceTrigger = PropertyChanged}\" />" + Environment.NewLine;
                    grid += "</Grid>" + Environment.NewLine;
                }
                else if (name.Equals("EntryName"))
                {
                    grid += "<Grid Width=\"" + gridWidth + "\" Margin=\"" + gridMargin + "\">" + Environment.NewLine;
                    grid += "<Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<ColumnDefinition Width=\"" + firstcolumnwidth + "\" />" + Environment.NewLine;
                    grid += "<ColumnDefinition />" + Environment.NewLine;
                    grid += "</Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<TextBlock Text=\"" + name + "\" />" + Environment.NewLine;
                    grid += "<TextBox Grid.Column=\"1\" Text=\"{Binding " + name + ", Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}\" >" + Environment.NewLine;
                    grid += "<interact:Interaction.Triggers>" + Environment.NewLine;
                    grid += "<interact:EventTrigger EventName=\"TextChanged\">" + Environment.NewLine;
                    grid += "<interact:InvokeCommandAction Command=\"{ Binding Path = Data.CommandCheckEntry, Source = { StaticResource DataContextProxy }}\" />" + Environment.NewLine;
                    grid += "</interact:EventTrigger>" + Environment.NewLine;
                    grid += "</interact:Interaction.Triggers>" + Environment.NewLine;
                    grid += "</TextBox>" + Environment.NewLine;
                    grid += "</Grid>" + Environment.NewLine;
                }
                else if (type.Equals("string"))
                {
                    grid += "<Grid Width=\"" + gridWidth + "\" Margin=\"" + gridMargin + "\">" + Environment.NewLine;
                    grid += "<Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<ColumnDefinition Width=\"" + firstcolumnwidth + "\" />" + Environment.NewLine;
                    grid += "<ColumnDefinition />" + Environment.NewLine;
                    grid += "</Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<TextBlock Text=\"" + name + "\" />" + Environment.NewLine;
                    grid += "<TextBox Grid.Column=\"1\" Text=\"{Binding " + name + ", Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}\" />" + Environment.NewLine;
                    grid += "</Grid>" + Environment.NewLine;
                }
                else if (type.Equals("bool"))
                {
                    grid += "<Grid Width=\"" + gridWidth + "\" Margin=\"" + gridMargin + "\">" + Environment.NewLine;
                    grid += "<Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<ColumnDefinition Width=\"" + firstcolumnwidth + "\" />" + Environment.NewLine;
                    grid += "<ColumnDefinition />" + Environment.NewLine;
                    grid += "</Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<TextBlock Text=\"" + name + "\" />" + Environment.NewLine;
                    grid += "<CheckBox Grid.Column=\"1\" IsChecked=\"{Binding " + name + ", Mode=TwoWay}\" />" + Environment.NewLine;
                    grid += "</Grid>" + Environment.NewLine;
                }
                else if (type.Equals("DateTime"))
                {
                    grid += "<Grid Width=\"" + gridWidth + "\" Margin=\"" + gridMargin + "\">" + Environment.NewLine;
                    grid += "<Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<ColumnDefinition Width=\"" + firstcolumnwidth + "\" />" + Environment.NewLine;
                    grid += "<ColumnDefinition />" + Environment.NewLine;
                    grid += "</Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<TextBlock Text=\"" + name + "\" />" + Environment.NewLine;
                    grid += "<uic:WinformDateTimePicker Grid.Column=\"1\" Format=\"Custom\" CustomFormat=\"dddd yyyy/MM/dd HH:mm:ss\" Value=\"{Binding " + name + ", Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}\" />" + Environment.NewLine;
                    grid += "</Grid>" + Environment.NewLine;
                }
                else if (type.StartsWith("List<"))
                {
                    grid += "<Grid Width=\"" + gridWidth + "\" Margin=\"" + gridMargin + "\">" + Environment.NewLine;
                    grid += "<Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<ColumnDefinition Width=\"" + firstcolumnwidth + "\" />" + Environment.NewLine;
                    grid += "<ColumnDefinition />" + Environment.NewLine;
                    grid += "</Grid.ColumnDefinitions>" + Environment.NewLine;
                    grid += "<TextBlock Text=\"" + name + "\" />" + Environment.NewLine;
                    grid += "<DataGrid Grid.Column=\"1\" Height=\"100\" ItemsSource=\"{Binding " + name + ", Mode=TwoWay}\" />" + Environment.NewLine;
                    grid += "</Grid>" + Environment.NewLine;
                }

                xaml += grid;
                System.Diagnostics.Debug.WriteLine(type + " " + name + ";");
            }

            OutputBox.Text = xaml;
        }

        private void 生成Html(string[] inputarray)
        {
            string ObjectName = "spiderToEdit";

            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<string> ignoreList = new List<string>();
            string html = "";
            string javascript = "";

            html += "<el-form :model=\"" + ObjectName + "\" :rules=\"spiderValidRule\" ref=\"editformRef\" status-icon label-position=\"left\" label-width=\"100px\">" + Environment.NewLine;

            foreach (string s in inputarray)
            {
                if (s.Equals(""))
                {
                    continue;
                }
                string name = "";
                string type = "";
                MakeTypeName(s, out name, out type);
                MakeUpperName(name, out name);

                dic.Add(name, type);
            }

            foreach (KeyValuePair<string, string> pair in dic)
            {
                string name = pair.Key;
                string type = pair.Value;

                string el_form_item = "";
                string objectproperty = "";
                if (ignoreList.Contains(name))
                {
                    continue;
                }
                else if (name.EndsWith("Enum"))
                {
                    string indexname = name.Substring(0, name.Length - 4);
                    if (dic.ContainsKey(indexname))
                    {
                        el_form_item += "<el-form-item label=\"" + indexname + "\" prop=\"" + 首字母小写(indexname) + "\">" + Environment.NewLine;
                        el_form_item += "<el-select placeholder=\"请选择\" v-model=\"" + ObjectName + "." + indexname + "\">" + Environment.NewLine;
                        el_form_item += "<el-option v-for=\"item in " + 首字母小写(name) + "\" :key=\"item\" :value=\"item\"></el-option>" + Environment.NewLine;
                        el_form_item += "</el-select>" + Environment.NewLine;
                        el_form_item += "</el-form-item>" + Environment.NewLine;
                        
                        objectproperty += indexname + ": ''," + Environment.NewLine;

                        ignoreList.Add(indexname);
                    }
                }
                else if (dic.ContainsKey(name + "Enum"))
                {
                    string enumname = name + "Enum";
                    el_form_item += "<el-form-item label=\"" + name + "\" prop=\"" + 首字母小写(name) + "\">" + Environment.NewLine;
                    el_form_item += "<el-select placeholder=\"请选择\" v-model=\"" + ObjectName + "." + name + "\">" + Environment.NewLine;
                    el_form_item += "<el-option v-for=\"item in " + 首字母小写(enumname) + "\" :key=\"item\" :value=\"item\"></el-option>" + Environment.NewLine;
                    el_form_item += "</el-select>" + Environment.NewLine;
                    el_form_item += "</el-form-item>" + Environment.NewLine;

                    objectproperty += name + ": ''," + Environment.NewLine;

                    ignoreList.Add(enumname);
                }
                else if (type.Equals("string"))
                {
                    el_form_item += "<el-form-item label=\"" + name + "\" prop=\"" + 首字母小写(name) + "\">" + Environment.NewLine;
                    el_form_item += "<el-input v-model=\"" + ObjectName + "." + name + "\"></el-input>" + Environment.NewLine;
                    el_form_item += "</el-form-item>" + Environment.NewLine;

                    objectproperty += name + ": ''," + Environment.NewLine;
                }
                else if (type.Equals("bool"))
                {
                    el_form_item += "<el-form-item label=\"" + name + "\" prop=\"" + 首字母小写(name) + "\">" + Environment.NewLine;
                    el_form_item += "<el-switch v-model=\"" + ObjectName + "." + name + "\"></el-switch>" + Environment.NewLine;
                    el_form_item += "</el-form-item>" + Environment.NewLine;

                    objectproperty += name + ": true," + Environment.NewLine;
                }
                else if (type.Equals("DateTime"))
                {
                    el_form_item += "<el-form-item label=\"" + name + "\" prop=\"" + 首字母小写(name) + "\">" + Environment.NewLine;
                    el_form_item += "<el-date-picker type=\"datetime\" v-model=\"" + ObjectName + "." + name + "\" placeholder=\"选择日期时间\" value-format=\"yyyy-MM-ddTHH:mm\"></el-date-picker>" + Environment.NewLine;
                    el_form_item += "</el-form-item>" + Environment.NewLine;

                    objectproperty += name + ": ''," + Environment.NewLine;
                }
                else if (type.StartsWith("List<"))
                {
                    string itemtype = type.Substring(5, type.Length - 6);
                    el_form_item += "<el-form-item label=\"" + name + "\" prop=\"" + 首字母小写(name) + "\">" + Environment.NewLine;
                    el_form_item += "<EditableTable :tableData=\"" + ObjectName + "." + name + "\" :rowModel=\"" + 首字母小写(itemtype) + "\"></EditableTable>" + Environment.NewLine;
                    el_form_item += "</el-form-item>" + Environment.NewLine;

                    objectproperty += name + ": []," + Environment.NewLine;
                }
                html += el_form_item;
                javascript += objectproperty;
            }

            html += "<el-form-item>" + Environment.NewLine;
            html += "<el-button @click=\"submitForm('editformRef')\" type=\"primary\">确认</el-button>" + Environment.NewLine;
            html += "<el-button @click=\"cancelEdit\">取消</el-button>" + Environment.NewLine;
            html += "</el-form-item>" + Environment.NewLine;
            html += "</el-form>" + Environment.NewLine;
            OutputBox.Text = html;
            OutputBox.Text += Environment.NewLine + "----------------------------------------------" + Environment.NewLine;
            OutputBox.Text += javascript;
        }

        private void MakeTypeName(string line, out string name, out string type)
        {
            // 去等号之后部分
            string declairstring = line.Split('=')[0].Trim();
            // 去末尾分号
            if (declairstring.EndsWith(";"))
            {
                declairstring = declairstring.Substring(0, declairstring.Length - 1);
            }
            // 去开头private,public和const
            if (declairstring.StartsWith("private "))
            {
                declairstring = declairstring.Substring(8);
            }
            if (declairstring.StartsWith("public "))
            {
                declairstring = declairstring.Substring(7);
            }
            if (declairstring.StartsWith("const "))
            {
                declairstring = declairstring.Substring(6);
            }
            // 按空格分割 最后一位为name 之前为type
            string[] arr = declairstring.Split(' ');
            name = arr[arr.Length - 1];
            type = "";
            for (int i = 0; i < arr.Length - 1; i++)
            {
                type += arr[i];
            }
        }
        private void MakeUpperName(string privatename, out string publicname)
        {
            publicname = privatename;
            // 去name开头下划线
            if (publicname.StartsWith("_"))
            {
                publicname = publicname.Substring(1);
            }
            // name首字母大写
            if (publicname.Length > 1)
            {
                publicname = char.ToUpper(publicname[0]) + publicname.Substring(1);
            }
            else
            {
                publicname = char.ToUpper(publicname[0]).ToString();
            }
        }
        private string 首字母小写(string s)
        {
            string result;
            if (s.Length > 1)
            {
                result = char.ToLower(s[0]) + s.Substring(1);
            }
            else
            {
                result = char.ToLower(s[0]).ToString();
            }
            return result;
        }
    }
}
