using QuanlyKhooooo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanlyKhooooo.ViewModel
{
    public class OutputViewModel : BaseViewModel
    {
        private ObservableCollection<Model.Output> _list;
        public ObservableCollection<Model.Output> List { get => _list; set { _list = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Customer> _Customers;
        public ObservableCollection<Model.Customer> Customers { get => _Customers; set { _Customers = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Object> _Object;
        public ObservableCollection<Model.Object> Object { get => _Object; set { _Object = value; OnPropertyChanged(); } }

        private ObservableCollection<Inventory> _InventoryList;
        public ObservableCollection<Inventory> InventoryList { get => _InventoryList; set { _InventoryList = value; OnPropertyChanged(); } }

        //search
        private ObservableCollection<Output> _filterList;
        public ObservableCollection<Output> filterList { get => _filterList; set { _filterList = value; OnPropertyChanged(); } }

        private string _searchText;
        public string SearchText
        {
            get
            { return _searchText; }
            set
            {
                _searchText = value; OnPropertyChanged();
            }
        }
        //het


        private Model.Output _SelectedItem;
        public Model.Output SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Id = SelectedItem.Id;
                    DateOutput = SelectedItem.DateOutput;
                    Counts = SelectedItem.Counts;
                    OutputPrice = SelectedItem.OutputPrice;
                    Status = SelectedItem.Status;
                    SelectedObject = SelectedItem.Object;
                    SelectedCustomer = SelectedItem.Customer;
                }
            }
        }


        private Model.Object _SelectedObject;
        public Model.Object SelectedObject
        {
            get => _SelectedObject; set
            {
                _SelectedObject = value;
                OnPropertyChanged();
            }
        }

        private Model.Customer _SelectedCustomer;
        public Model.Customer SelectedCustomer
        {
            get => _SelectedCustomer; set
            {
                _SelectedCustomer = value;
                OnPropertyChanged();
            }
        }

        private Model.Inventory _SelectedInventory;
        public Model.Inventory SelectedInventory
        {
            get => _SelectedInventory; set
            {
                _SelectedInventory = value;
                OnPropertyChanged();
            }
        }





        private string _Id;
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private DateTime? _DateOutput;
        public DateTime? DateOutput { get => _DateOutput; set { _DateOutput = value; OnPropertyChanged(); } }

        private string _IdObject;
        public string IdObject { get => _IdObject; set { _IdObject = value; OnPropertyChanged(); } }

        private int? _Counts;
        public int? Counts { get => _Counts; set { _Counts = value; OnPropertyChanged(); } }

        private int _IdCustomer;
        public int IdCustomer { get => _IdCustomer; set { _IdCustomer = value; OnPropertyChanged(); } }

        private double? _OutputPrice;
        public double? OutputPrice { get => _OutputPrice; set { _OutputPrice = value; OnPropertyChanged(); } }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }


        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public OutputViewModel()
        {
            List = new ObservableCollection<Model.Output>(DataProvider.Ins.DB.Outputs);
            Object = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);
            Customers = new ObservableCollection<Model.Customer>(DataProvider.Ins.DB.Customers);

            AddCommand = new RelayCommand<object>((p) =>
            {
                //thêm ràng buộc còn tồn kho thì mới xuất được
                //update: đã fix
                return true;
            },
            (p) =>
            {
                int Checked = 0;
                var objectList = DataProvider.Ins.DB.Objects;

                if (DateOutput == null || SelectedObject == null || SelectedCustomer == null || Counts == null || OutputPrice == null)
                {
                    MessageBox.Show("Bạn chưa nhập đủ", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                else
                {
                    var ob = new Model.Output() { DateOutput = DateOutput, IdObject = SelectedObject.Id, IdCustomer = SelectedCustomer.Id, Counts = Counts, OutputPrice = OutputPrice, Status = Status, Id = Guid.NewGuid().ToString() };


                    foreach (var item in objectList)
                    {
                        var inputList = DataProvider.Ins.DB.Inputs.Where(x => x.IdObject == item.Id);
                        var outputList = DataProvider.Ins.DB.Outputs.Where(x => x.IdObject == item.Id);

                        int sumInput = 0;
                        int sumOutput = 0;

                        if (inputList != null && inputList.Count() > 0)
                        {
                            sumInput = (int)inputList.Sum(x => x.Counts);
                        }
                        if (outputList != null && outputList.Count() > 0)
                        {
                            sumOutput = (int)outputList.Sum(x => x.Counts);
                        }

                        Inventory inventorykk = new Inventory();

                        inventorykk.Counttt = sumInput - sumOutput;
                        inventorykk.Object = item;

                        if (inventorykk.Counttt < ob.Counts && item.Id == ob.IdObject)
                        {
                            Checked = 1; break;
                        }

                    }

                    if (Checked == 1)
                    {
                        MessageBox.Show("Lượng hàng trong kho không đủ!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    else
                    {
                        DataProvider.Ins.DB.Outputs.Add(ob);
                        DataProvider.Ins.DB.SaveChanges();
                        List.Add(ob);
                        List = new ObservableCollection<Model.Output>(DataProvider.Ins.DB.Outputs);
                    }
                }                             
            });


            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedObject == null || SelectedCustomer == null || SelectedItem == null) return false;

                var displayList = DataProvider.Ins.DB.Outputs.Where(x => x.Id == SelectedItem.Id);
                if (displayList != null && displayList.Count() != 0) return true;

                return false;
            },
           (p) =>
           {
               var ob = DataProvider.Ins.DB.Outputs.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
               ob.Id = Id;
               ob.DateOutput = DateOutput;
               ob.IdObject = SelectedObject.Id;
               ob.IdCustomer = SelectedCustomer.Id; 
               ob.Counts = Counts; 
               ob.OutputPrice = OutputPrice;
               ob.Status = Status;  

               DataProvider.Ins.DB.SaveChanges();

               SelectedItem.Id = Id;    
               /*
               SelectedItem.DateOutput = DateOutput;
               SelectedItem.IdObject = SelectedObject.Id;
               SelectedItem.IdCustomer = SelectedCustomer.Id;
               SelectedItem.Counts = Counts;
               SelectedItem.OutputPrice = OutputPrice;  
               SelectedItem.Status = Status;
               */
               List = new ObservableCollection<Model.Output>(DataProvider.Ins.DB.Outputs);
           });

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return true;
            },
              (p) =>
              {
                  FindItems();
              });
        }

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

                if (inputList != null && inputList.Count() > 0)
                {
                    sumInput = (int)inputList.Sum(p => p.Counts);
                }
                if (outputList != null && outputList.Count() > 0)
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

        private void FindItems()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                List = new ObservableCollection<Output>(DataProvider.Ins.DB.Outputs);
            }
            else
            {
                filterList = new ObservableCollection<Output>(DataProvider.Ins.DB.Outputs);
                filterList.Clear();
                foreach (Output item in List)
                {
                    var searchTextLower = _searchText.ToLowerInvariant();
                    if (item.IdObject.ToLowerInvariant().Contains(searchTextLower)) //cai nay moi la tim kiem theo id chu chua phai tim kiem theo ten
                        filterList.Add(item);
                    List = filterList;
                }
            }

        }
    }

}
