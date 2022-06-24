using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Java.Lang;
using System.IO;
using System.Xml.Serialization;

namespace BurrLife
{
    class AsyncGenerateList : AsyncTask<string, Java.Lang.Void, List<TaskPriorityModel>>
    {
        
        protected override List<TaskPriorityModel> RunInBackground(params string[] @params)
        {
            PublishProgress(new Java.Lang.Void[] { });
            List<TaskPriorityModel> m = new List<TaskPriorityModel>();
            if (File.Exists(@params[0]))
            {
               
                XmlSerializer formatter = new XmlSerializer(typeof(List<TaskPriorityModel>));
                using(FileStream fs = new FileStream(@params[0],FileMode.OpenOrCreate))
                try
                {
                        var temp = (List<TaskPriorityModel>)formatter.Deserialize(fs);
                        if (temp != null) m = temp;
                }
                catch (System.Xml.XmlException e) { 
                        System.Console.WriteLine("Error: " + e.Message);
                        File.Delete(@params[0]);    
                }
                
            }
            return m;

        }
        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
           
        }
    }
}