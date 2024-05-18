using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanlyKhooooo.ViewModel
{
    public class HeaderViewModel : BaseViewModel
    {
        #region commands

        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }


        #endregion

        public HeaderViewModel()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {

                FrameworkElement window = GetWindowParent(p);
                var wd = window as Window;
                if (wd != null)
                {
                    wd.Close();
                }
            });

            MaximizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {

                FrameworkElement window = GetWindowParent(p);
                var wd = window as Window;
                if (wd != null)
                {
                    if (wd.WindowState != WindowState.Maximized)
                    {
                        wd.WindowState = WindowState.Maximized;
                    }
                    else
                        wd.WindowState = WindowState.Normal;
                }
            });

            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {

                FrameworkElement window = GetWindowParent(p);
                var wd = window as Window;
                if (wd != null)
                {
                    if (wd.WindowState != WindowState.Minimized)
                    {
                        wd.WindowState = WindowState.Minimized;
                    }
                    else
                        wd.WindowState = WindowState.Maximized;
                }
            });

            MouseMoveWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {

                FrameworkElement window = GetWindowParent(p);
                var wd = window as Window;
                if (wd != null)
                {
                    wd.DragMove();  
                }
            });
        }

        FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return parent;
        }
    }
}