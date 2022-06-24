
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.ViewPager.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurrLife
{
    

    class MainPagerAdapter : FragmentPagerAdapter
    {

        private FragmentManager manager;

        


        //Fragment manager
        public MainPagerAdapter(FragmentManager manger):base(manger)
        {
            this.manager = manger;
           
        }

   

        public override AndroidX.Fragment.App.Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0:
                    return new PageActivityies.GoalActivity();
                case 1:
                    return new PageActivityies.MainActivity1();
                case 2:
                    return new MakeRoutine();
                
            }
            return null;
            
        }
        public override ICharSequence GetPageTitleFormatted(int position)
        {
            switch (position)
            {
                case 0: return new Java.Lang.String("Задачи");
                case 1: return new Java.Lang.String("Главное");
                case 2: return new Java.Lang.String("Рутина");
       
            }
            return null;

            
        }
        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return 3;
            }
        }

    }

    class MainPagerAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}