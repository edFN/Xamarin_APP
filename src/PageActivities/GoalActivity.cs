
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurrLife.PageActivityies
{
 
    public class GoalActivity : Fragment
    {
        public static TaskAdapter adapter1;
        
        public GoalActivity() { }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View v = inflater.Inflate(Resource.Layout.goal_activity, container, false);
            ListView view1 = v.FindViewById<ListView>(Resource.Id.listItem);
            adapter1 = new TaskAdapter(this.Context, BurrLife.PageActivityies.MainActivity1.modelList);
            view1.SetAdapter(adapter1);
            view1.ItemLongClick += delegate (object sender, AdapterView.ItemLongClickEventArgs args)
            {
                AlertDialog.Builder alertBuilder = new AlertDialog.Builder(Context);
                alertBuilder.SetTitle("Удаление задачи").
                
                SetMessage($"Удалить задачу '{BurrLife.PageActivityies.MainActivity1.modelList[args.Position].taskName}' ?").
                SetPositiveButton("Да", (object o, DialogClickEventArgs cargs) =>
                {
                    
                    BurrLife.PageActivityies.MainActivity1.modelList.RemoveAt(args.Position);
                    adapter1.NotifyDataSetChanged();
                    if (args.Position < 3)
                    {
                        PageActivityies.MainActivity1._used.Clear();
                        for (int i = 0; i < PageActivityies.MainActivity1.modelList.Count && i < 3; i++)
                            PageActivityies.MainActivity1._used.Add(PageActivityies.MainActivity1.modelList[i]);
                        PageActivityies.MainActivity1.adapter.NotifyDataSetChanged();
                    }
                    MainActivity.SerializeObject(BurrLife.PageActivityies.MainActivity1.path, BurrLife.PageActivityies.MainActivity1.modelList, 
                        typeof(List<TaskPriorityModel>));


                }).SetNegativeButton("Нет", (o, e) => { });
                alertBuilder.Show();
            };


            return v;
        }
      
          //  base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.goal_activity);
        
      

    }
}