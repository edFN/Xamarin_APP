using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BurrLife
{
    class AsyncGenerateRoutine : AsyncTask<string, Java.Lang.Void, List<RoutineTask>>
    {
        protected override List<RoutineTask> RunInBackground(params string[] @params)
        {
            PublishProgress(new Java.Lang.Void[] { });
            List<RoutineTask> m = new List<RoutineTask>();
            if (File.Exists(@params[0]))
            {

                XmlSerializer formatter = new XmlSerializer(typeof(List<RoutineTask>));
                using (FileStream fs = new FileStream(@params[0], FileMode.OpenOrCreate))
                    try
                    {
                        var temp = (List<RoutineTask>)formatter.Deserialize(fs);
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