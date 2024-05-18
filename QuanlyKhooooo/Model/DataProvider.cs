﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QuanlyKhooooo.Model
{
    public class DataProvider
    {

        private static DataProvider _ins;
        public static DataProvider Ins
        {
            get
            {

                if (_ins == null)
                    _ins = new DataProvider();
                return _ins;
            }

            set { _ins = value; }
        }

        public QuanLyKhoDBEntities2 DB { get; set; }
        private DataProvider()
        {
            DB = new QuanLyKhoDBEntities2();
        }
    }
      
}
