using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using BurrLife.PageActivityies;
using Java.Interop;
using System;
using System.Collections.Generic;

namespace BurrLife
{
    //TODO :implement textView  adapter
    public class TaskViewHolder : RecyclerView.ViewHolder
    {
        public TextView name { get; set; }
        public TaskViewHolder(View view) : base(view)
        {
            name = view.FindViewById<TextView>(Resource.Id.textItem);

        }
    }

    public class TaskAdapter : BaseAdapter
    {
        private Context context;
        private List<TaskPriorityModel> listModel;

        public TaskAdapter(Context context, List<TaskPriorityModel> model) {

            this.context = context;
            this.listModel = model;

        }


        public override int Count => listModel.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return (listModel[position]);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public TaskPriorityModel getProduct(int position)
        {
            return (TaskPriorityModel)listModel[position];
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            var layout = LayoutInflater.From(context);
            if (view == null)
            {
                view = layout.Inflate(Resource.Layout.listCustom, parent, false);
            }




            TaskPriorityModel model = getProduct(position);


            TextView text = view.FindViewById<TextView>(Resource.Id.textItem);
            text.Text = model.taskName;



            return view;
        }




    }
}



