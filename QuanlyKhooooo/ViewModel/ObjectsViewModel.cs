using QuanlyKhooooo.Model;
using QuanlyKhooooo.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanlyKhooooo.ViewModel
{
    public class ObjectsViewModel : BaseViewModel
    {
        private ObservableCollection<Model.Object> _list;
        public ObservableCollection<Model.Object> List { get => _list; set { _list = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Unit> _Unit;
        public ObservableCollection<Model.Unit> Unit { get => _Unit; set { _Unit = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Supplier> _Supplier;
        public ObservableCollection<Model.Supplier> Supplier { get => _Supplier; set { _Supplier = value; OnPropertyChanged(); } }

        //search
        private ObservableCollection<Model.Object> _filterList;
        public ObservableCollection<Model.Object> filterList { get => _filterList; set { _filterList = value; OnPropertyChanged(); } }

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

        private Model.Object _SelectedItem;
        public Model.Object SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    QRCode = SelectedItem.QRCode;
                    BarCode = SelectedItem.BarCode;
                    SelectedUnit = SelectedItem.Unit;
                    SelectedSupplier = SelectedItem.Supplier;
                }
            }
        }


        private Model.Unit _SelectedUnit;
        public Model.Unit SelectedUnit
        {
            get => _SelectedUnit; set
            {
                _SelectedUnit = value;
                OnPropertyChanged();
            }
        }

        private Model.Supplier _SelectedSupplier;
        public Model.Supplier SelectedSupplier
        {
            get => _SelectedSupplier; set
            {
                _SelectedSupplier = value;
                OnPropertyChanged();
            }
        }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _IdUnit;
        public string IdUnit { get => _IdUnit; set { _IdUnit = value; OnPropertyChanged(); } }

        private string _IdSupplier;
        public string IdSupplier { get => _IdSupplier; set { _IdSupplier = value; OnPropertyChanged(); } }

        private string _QRCode;
        public string QRCode { get => _QRCode; set { _QRCode = value; OnPropertyChanged(); } }

        private string _BarCode;
        public string BarCode { get => _BarCode; set { _BarCode = value; OnPropertyChanged(); } }
        public ICommand AddObjectsCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public ObjectsViewModel()
        {
            List = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);
            Unit = new ObservableCollection<Model.Unit>(DataProvider.Ins.DB.Units);
            Supplier = new ObservableCollection<Model.Supplier>(DataProvider.Ins.DB.Suppliers);

            AddObjectsCommand = new RelayCommand<object>((p) =>
            {
            if (DisplayName == null ||  SelectedSupplier == null || SelectedUnit == null) return false;
                return true;
            },
            (p) =>
            {
                    var ob = new Model.Object() { DisplayName = DisplayName, QRCode = QRCode, BarCode = BarCode, IdUnit = SelectedUnit.Id, IdSupplier = SelectedSupplier.Id, Id = Guid.NewGuid().ToString() };
                    DataProvider.Ins.DB.Objects.Add(ob);
                    DataProvider.Ins.DB.SaveChanges();

                    List.Add(ob);
                
                
            });


            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedSupplier == null || SelectedUnit == null || SelectedItem == null) return false;

                var displayList = DataProvider.Ins.DB.Objects.Where(x => x.Id == SelectedItem.Id);
                if (displayList != null && displayList.Count()!=0) return true;

                    return false;
            },
           (p) =>
           {
               var ob = DataProvider.Ins.DB.Objects.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
               ob.DisplayName = DisplayName;
               ob.IdUnit = SelectedUnit.Id;
               ob.IdSupplier = SelectedSupplier.Id;
               ob.BarCode = BarCode;
               ob.QRCode = QRCode;
               DataProvider.Ins.DB.SaveChanges();

               SelectedItem.DisplayName = DisplayName;
               List = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);



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
                List = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);
            }
            else
            {
                filterList = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);
                filterList.Clear();
                foreach (Model.Object item in List)
                {
                    var searchTextLower = _searchText.ToLowerInvariant();
                    if (item.DisplayName.ToLowerInvariant().Contains(searchTextLower))
                        filterList.Add(item);
                    List = filterList;
                }
            }

        }
    }

 
}
