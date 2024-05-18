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
    public class InputViewModel : BaseViewModel
    {
        private ObservableCollection<Model.Input> _list;
        public ObservableCollection<Model.Input> List { get => _list; set { _list = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Object> _Object;
        public ObservableCollection<Model.Object> Object { get => _Object; set { _Object = value; OnPropertyChanged(); } }

        //search
        private ObservableCollection<Input> _filterList;
        public ObservableCollection<Input> filterList { get => _filterList; set { _filterList = value; OnPropertyChanged(); } }

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



        private Model.Input _SelectedItem;
        public Model.Input SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Id = SelectedItem.Id;
                    DateInput = SelectedItem.DateInput;
                    Counts = SelectedItem.Counts;
                    InputPrice = SelectedItem.InputPrice;
                    Status = SelectedItem.Status;
                    SelectedObject = SelectedItem.Object;
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

        private string _Id;
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private DateTime? _DateInput;
        public DateTime? DateInput { get => _DateInput; set { _DateInput = value; OnPropertyChanged(); } }

        private string _IdObject;
        public string IdObject { get => _IdObject; set { _IdObject = value; OnPropertyChanged(); } }

        private int? _Counts;
        public int? Counts { get => _Counts; set { _Counts = value; OnPropertyChanged(); } }

        private double? _InputPrice;
        public double? InputPrice { get => _InputPrice; set { _InputPrice = value; OnPropertyChanged(); } }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public InputViewModel()
        {

            List = new ObservableCollection<Model.Input>(DataProvider.Ins.DB.Inputs);
            Object = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);

            AddCommand = new RelayCommand<object>((p) =>
            {
                return true;
            },
            (p) =>
            {
                if (DateInput == null || Counts == null || SelectedObject == null || InputPrice == null)
                {
                    MessageBox.Show("Bạn chưa nhập đủ", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var ob = new Model.Input() { DateInput = DateInput, Counts = Counts, IdObject = SelectedObject.Id, InputPrice = InputPrice, Status = Status, Id = Guid.NewGuid().ToString() };
                    DataProvider.Ins.DB.Inputs.Add(ob);
                    DataProvider.Ins.DB.SaveChanges();

                    List.Add(ob);
                    List = new ObservableCollection<Model.Input>(DataProvider.Ins.DB.Inputs);
                }


            });


             EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedObject == null || SelectedItem == null) return false;

                var displayList = DataProvider.Ins.DB.Inputs.Where(x => x.Id == SelectedItem.Id);
                if (displayList != null && displayList.Count() != 0) return true;

                return false;
            },
           (p) =>
           {
               var ob = DataProvider.Ins.DB.Inputs.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
               ob.Id = Id;
               ob.DateInput = DateInput;
               ob.Counts = Counts;
               ob.IdObject = SelectedObject.Id;
               ob.InputPrice = InputPrice;
               DataProvider.Ins.DB.SaveChanges();

               SelectedItem.Id = Id;
               List = new ObservableCollection<Model.Input>(DataProvider.Ins.DB.Inputs);
               /*
               SelectedItem.DateInput = DateInput;
               SelectedItem.Counts = Counts;
               SelectedItem.IdObject = SelectedObject.Id;
               SelectedItem.Status = Status;
               SelectedItem.InputPrice = InputPrice;
               */
               List = new ObservableCollection<Model.Input>(DataProvider.Ins.DB.Inputs);

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

        private void FindItems()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                List = new ObservableCollection<Input>(DataProvider.Ins.DB.Inputs);
            }
            else
            {
                filterList = new ObservableCollection<Input>(DataProvider.Ins.DB.Inputs);
                filterList.Clear();
                foreach (Input item in List)
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
