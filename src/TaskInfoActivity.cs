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

namespace BurrLife
{
    [Activity(Label = "TaskInfoActivity")]
    public class TaskInfoActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TaskInfo);

            var id = Intent.Extras.GetInt("ID");
            var taskName = Intent.Extras.GetString("Name");
            var description = Intent.Extras.GetString("Description");
            var deadline = new DateTime(Intent.Extras.GetLong("Deadline"));

            var textViewName = FindViewById<TextView>(Resource.Id.editText1);
            var textViewDescription = FindViewById<TextView>(Resource.Id.editText2);
             var textViewDeadline = FindViewById<TextView>(Resource.Id.editText3);

            
            textViewName.Text = taskName;
            if (description == string.Empty) textViewDescription.Text = "Пусто";
            else
                textViewDescription.Text = description;
            
            textViewDeadline.Text = deadline.ToString("d MMMM yyyy");

            var btnSave = FindViewById<Button>(Resource.Id.button1);
            var btnDelete = FindViewById<Button>(Resource.Id.button2);

            btnSave.Click += (o, e) => {
                PageActivityies.MainActivity1.modelList[id] = new TaskPriorityModel(textViewName.Text, textViewDescription.Text,
                    PageActivityies.MainActivity1.modelList[id].deadline, PageActivityies.MainActivity1.modelList[id].priority);
                if(id < 3)
                {
                    PageActivityies.MainActivity1._used.Clear();
                    for (int i = 0; i < PageActivityies.MainActivity1.modelList.Count && i < 3; i++)
                        PageActivityies.MainActivity1._used.Add(PageActivityies.MainActivity1.modelList[i]);
                }
                PageActivityies.MainActivity1.adapter.NotifyDataSetChanged();
                PageActivityies.GoalActivity.adapter1.NotifyDataSetChanged();
                MainActivity.SerializeObject(PageActivityies.MainActivity1.path, 
                    PageActivityies.MainActivity1.modelList, typeof(List<TaskPriorityModel>));
                FinishActivity(0);
                base.OnBackPressed();
                    

            };
            btnDelete.Click += (o, e) =>
             {
                 BurrLife.PageActivityies.MainActivity1.modelList.RemoveAt(id);
                 if (id < 3)
                 {
                     PageActivityies.MainActivity1._used.Clear();
                     for (int i = 0; i < PageActivityies.MainActivity1.modelList.Count && i < 3; i++)
                         PageActivityies.MainActivity1._used.Add(PageActivityies.MainActivity1.modelList[i]);
                 }
                 PageActivityies.MainActivity1.adapter.NotifyDataSetChanged();
                 PageActivityies.GoalActivity.adapter1.NotifyDataSetChanged();
                 MainActivity.SerializeObject(PageActivityies.MainActivity1.path, 
                     PageActivityies.MainActivity1.modelList, typeof(List<TaskPriorityModel>));
                 FinishActivity(0);
                 base.OnBackPressed();
             };



        }
    }
}