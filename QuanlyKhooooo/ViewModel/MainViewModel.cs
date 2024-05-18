using MaterialDesignThemes.Wpf;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Xaml.Behaviors.Core;
using QuanlyKhooooo.Model;
using QuanlyKhooooo.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanlyKhooooo.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        #region commands
        private ObservableCollection<Inventory> _InventoryList;
        public ObservableCollection<Inventory> InventoryList { get => _InventoryList; set { _InventoryList = value; OnPropertyChanged(); } }

        public bool Isloaded = false;
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand UnitCommand { get; set; }
        public ICommand SupplierCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ObjectsCommand { get; set; }
        public ICommand InputCommand { get; set; }
        public ICommand OutputCommand { get; set; }
        #endregion

       
        public MainViewModel()
        {
            #region methods
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Isloaded = true;

                if (p == null) return;
                p.Hide();
                Login login = new Login();
                login.ShowDialog();

                if (login.DataContext == null) return;

                var loginVM = login.DataContext as LoginViewModel;

                if (loginVM.IsLogin)
                {
                    p.Show();
                    LoadInventoryData();
                }
                else p.Close();
            });

            UnitCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                UnitWindow unitwd = new UnitWindow();
                unitwd.ShowDialog();
            });

            SupplierCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SupplierWindow sup = new SupplierWindow();
                sup.ShowDialog();
            });

            CustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CustomerWindow cus = new CustomerWindow();
                cus.ShowDialog();
            });

            ObjectsCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ObjectsWindow obj = new ObjectsWindow();
                obj.ShowDialog();
            });

            InputCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                InputWindow inpt = new InputWindow();
                inpt.ShowDialog(); 
                LoadInventoryData();


            });

            OutputCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OutputWindow inpt = new OutputWindow();
                inpt.ShowDialog();
                LoadInventoryData();

            });
        }
        #endregion

        void LoadInventoryData()
        {
            InventoryList = new ObservableCollection<Inventory>();

            var objectList = DataProvider.Ins.DB.Objects;

            int i = 1;
            foreach (var item in objectList)
            {
                var inputList = DataProvider.Ins.DB.Inputs.Where(p => p.IdObject == item.Id);
                var outputList = DataProvider.Ins.DB.Outputs.Where(p => p.IdObject == item.Id);

                int sumInput = 0;
                int sumOutput = 0;

                if (inputList != null && inputList.Count()>0)
                {
                    sumInput = (int)inputList.Sum(p => p.Counts);
                }
                if (outputList != null && outputList.Count()>0)
                {
                    sumOutput = (int)outputList.Sum(p => p.Counts);
                }

                Inventory inventorykk = new Inventory();
                inventorykk.STT = i;
                inventorykk.Counttt = sumInput - sumOutput;
                inventorykk.Object = item;

                InventoryList.Add(inventorykk);
                i++;
            }
        }


    }
}
