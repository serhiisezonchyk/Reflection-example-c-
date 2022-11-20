using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reflection_l2
{
    public partial class Form1 : Form
    {
        private string path;
        private Assembly assembly;
        public Form1()
        {
            InitializeComponent();
        }

        //Get path to assembly file
        private string selectAssemblyFile()
        {
            openFileDialog1.Filter = "Dll files (*.dll)|*.dll|Exe files(*.exe) | *.exe | All files(*.*) | *.* ";
            openFileDialog1.Title = "Select assembly file";
            return (openFileDialog1.ShowDialog() == DialogResult.OK) ? openFileDialog1.FileName : null;
        }
        //Downloading assembly
        private Assembly openAssembly(string path)
        {
            try
            {
                Assembly a = Assembly.LoadFrom(path);
                this.path = path;
                return a;
            }
            catch (Exception)
            {
                MessageBox.Show("Download of this assembley unsuccessfully!",
                "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        //Add all classes and interfaces of assembly to treenode
        void addRoot(TreeNode root, Type[] types)
        {
            TreeNode node = null;
            foreach (Type type in types)
            {
                node = new TreeNode();
                node.Text = type.ToString();
                //If class
                if (type.IsClass)
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                    addFirstLevel(node, type);
                    root.Nodes.Add(node);
                }
                //If interface
                else if (type.IsInterface)
                {
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    addFirstLevel(node, type);
                    root.Nodes.Add(node);
                }
                else if (type.IsEnum)
                {
                    node.ImageIndex = 8;
                    node.SelectedImageIndex = 8;
                    addFirstLevel(node, type);
                    root.Nodes.Add(node);
                }
                else if (type.IsValueType)
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                    addFirstLevel(node, type);
                    root.Nodes.Add(node);
                }
                else if (type.IsAbstract) {
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                    addFirstLevel(node, type);
                    root.Nodes.Add(node);
                }

            }
        }

        //Load all fields, constructors and methods
        private void addFirstLevel(TreeNode node, Type type)
        {
            TreeNode node1 = null;

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance 
                | BindingFlags.NonPublic 
                | BindingFlags.Public 
                | BindingFlags.Static);

            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance 
                | BindingFlags.NonPublic 
                | BindingFlags.Public 
                | BindingFlags.Static);

            ConstructorInfo[] constructors = type.GetConstructors();

            //Load fields
            foreach (FieldInfo field in fields)
            {
                node1 = new TreeNode();
                if (field.IsPublic)
                {
                    node1.ImageIndex = 3;
                    node1.SelectedImageIndex = 3;
                }
                else if (field.IsPrivate)
                {
                    node1.ImageIndex = 4;
                    node1.SelectedImageIndex = 4;
                }
                else {
                    node1.ImageIndex = 5;
                    node1.SelectedImageIndex = 5;
                }
                node1.Text = "Field type: "+field.FieldType.Name+"=>"+ field.Name;
                node.Nodes.Add(node1);
            }

            //Load constructors
            foreach (ConstructorInfo constructor in constructors)
            {

                node1 = new TreeNode();
                node1.Text = "Constructor=>"+constructor.Name ;
                if (constructor.IsPublic)
                {
                    node1.ImageIndex = 3;
                    node1.SelectedImageIndex = 3;
                }
                else if (constructor.IsPrivate)
                {
                    node1.ImageIndex = 4;
                    node1.SelectedImageIndex = 4;
                }
                else {
                    node1.ImageIndex = 5;
                    node1.SelectedImageIndex = 5;
                }
                node.Nodes.Add(node1);
            }
            //Load methods
            foreach (MethodInfo method in methods)
            {
                String s = "";
                ParameterInfo[] parametrs = method.GetParameters();
                foreach (ParameterInfo parametr in parametrs)
                {
                    s += parametr.ParameterType.Name + ", ";
                }

                node1 = new TreeNode();
                s = s.Trim();
                string str = s.TrimEnd(',');
                node1.Text = "Method type:" + method.ReturnType.Name + "=>" + method.Name + "("
                + str + ")";
                if (method.IsPublic)
                {
                    node1.ImageIndex = 3;
                    node1.SelectedImageIndex = 3;
                }
                else if (method.IsPrivate)
                {
                    node1.ImageIndex = 4;
                    node1.SelectedImageIndex = 4;
                }
                else {
                    node1.ImageIndex = 5;
                    node1.SelectedImageIndex = 5;
                }
                node.Nodes.Add(node1);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            string path = selectAssemblyFile();
            if (path != null)
            {
                assembly = openAssembly(path);
            }
            if (assembly != null)
            {
                TreeNode root = new TreeNode();
                root.Text = assembly.GetName().Name;
                root.ImageIndex = 0;
                root.SelectedImageIndex = 0;
                treeView1.Nodes.Add(root);
                Type[] types = assembly.GetTypes();
                addRoot(root, types);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string fullPath = treeView1.SelectedNode.FullPath;
            string[] pathElements = fullPath.Split('\\');
            int nodeLevel = treeView1.SelectedNode.Level;
            DopName window = null;
            switch (nodeLevel)
            {
                case 0:
                    window = new DopName("\"" + treeView1.SelectedNode.Text + "\" Details info", GetAssemblyInfo());
                    window.Show();
                    break;
                case 1:
                    window = new DopName("\"" + treeView1.SelectedNode.Text + "\" Details info", GetMembersOfClassInfo(pathElements));
                    window.Show();
                    break;
                case 2:
                    window = new DopName("\"" + treeView1.SelectedNode.Text + "\" Details info", GetMemberInfo(pathElements));
                    window.Show();
                    break;
                default:
                    return;
            }
        }
        private string GetAssemblyInfo()
        {
            string pathRed = "";
            pathRed += path;
            pathRed = pathRed.Replace(@"\", "//");
            return "Full path of opened assembly:\r\n" + pathRed + "\r\n";
        }

        private string GetMembersOfClassInfo(string[] pathElements)
        {
            string info = "";

            Type type = Type.GetType(pathElements[1] + ", " + assembly.FullName);

            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic
                                  | BindingFlags.Instance
                                  | BindingFlags.Public
                                  | BindingFlags.Static);

            MethodInfo[] methods = type.GetMethods(BindingFlags.NonPublic
                                    | BindingFlags.Instance
                                    | BindingFlags.Public
                                    | BindingFlags.Static);

            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.NonPublic
                                              | BindingFlags.Instance
                                              | BindingFlags.Public
                                              | BindingFlags.Static);

            info += ClassDetail.GetFullName(type) + "\r\n";
            info += ClassDetail.GetFieldsString(fields) + "\r\n";
            info += ClassDetail.GetConstructorsString(constructors) + "\r\n";
            info += ClassDetail.GetMethodsString(methods) + "\r\n";

            return info;
        }
        private string GetMemberInfo(string[] pathElements)
        {
            string info = "";
            string[] separators = new string[] { " ", "(", ", ", ")" };
            string splitPath = pathElements[2].Substring(pathElements[2].IndexOf(">")+1);
            string[] splitName = splitPath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            Type type = Type.GetType(pathElements[1] + ", " + assembly.FullName);
            MemberInfo[] members = type.GetMember(splitName[0], BindingFlags.NonPublic
                                                                | BindingFlags.Instance
                                                                | BindingFlags.Public
                                                                | BindingFlags.Static);

            foreach (MemberInfo member in members)
                info += ClassDetail.GetMemberString(member) + "\r\n";

            return info;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 window = new AboutBox1();
            window.Show();
        }
    }


}

