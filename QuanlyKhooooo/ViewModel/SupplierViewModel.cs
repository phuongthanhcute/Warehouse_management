using QuanlyKhooooo.Model;
using QuanlyKhooooo.View;
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
    public class SupplierViewModel : BaseViewModel
    {
        private ObservableCollection<Supplier> _list;
        public ObservableCollection<Supplier> List { get => _list; set { _list = value; OnPropertyChanged(); } }

        private ObservableCollection<Supplier> _filterList;
        public ObservableCollection<Supplier> filterList { get => _filterList; set { _filterList = value; OnPropertyChanged(); } }

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

        private Supplier _SelectedItem;
         public Supplier SelectedItem
         {
             get => _SelectedItem; set
             {
                 _SelectedItem = value; OnPropertyChanged();
                 if (SelectedItem != null)
                 {
                     DisplayName = SelectedItem.DisplayName; 
                     Phone = SelectedItem.Phone;
                     Email = SelectedItem.Email;
                     Address = SelectedItem.Address;
                     MoreInfo = SelectedItem.MoreInfo;
                     ContractDate = SelectedItem.ContractDate;
                 }
             }
         }
        
        private int _Id;
        public int Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }
        
        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private string _MoreInfo;
        public string MoreInfo { get => _MoreInfo; set { _MoreInfo = value; OnPropertyChanged(); } }

        private DateTime? _ContractDate;
        public DateTime? ContractDate { get => _ContractDate; set { _ContractDate = value; OnPropertyChanged(); } }
        public ICommand AddSupplierCommand {  get; set; }
        public ICommand EditCommand { get; set; }
        
        public ICommand SearchCommand { get; set; }

        public SupplierViewModel()
        {
            List = new ObservableCollection<Supplier>(DataProvider.Ins.DB.Suppliers);
            AddSupplierCommand = new RelayCommand<object>((p) =>
            {
                return true;
            },
            (p) =>
            {
                if(DisplayName == null || Phone == null || Address == null || Email == null || ContractDate == null)
                {
                    MessageBox.Show("Bạn chưa nhập đủ", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                else
                {
                    var sup = new Supplier() { DisplayName = DisplayName, Phone = Phone, Address = Address, Email = Email, ContractDate = ContractDate, MoreInfo = MoreInfo };
                    DataProvider.Ins.DB.Suppliers.Add(sup);
                    DataProvider.Ins.DB.SaveChanges();
                    List.Add(sup);
                }
               
            });

            EditCommand = new RelayCommand<object>((p) => 
            {
                if (SelectedItem == null) return false;

                var displayList = DataProvider.Ins.DB.Suppliers.Where(x => x.Id == SelectedItem.Id);
                if (displayList != null && displayList.Count() != 0) return true;
                return false;
            },
            
            (p) =>
            {
                var sup = DataProvider.Ins.DB.Suppliers.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                sup.DisplayName = DisplayName;
                sup.Phone = Phone;
                sup.Address = Address;
                sup.Email = Email;
                sup.ContractDate = ContractDate;
                sup.MoreInfo = MoreInfo;
                DataProvider.Ins.DB.SaveChanges();

                SelectedItem.DisplayName = DisplayName;
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
                List = new ObservableCollection<Supplier>(DataProvider.Ins.DB.Suppliers);
            }
            else
            {
                filterList = new ObservableCollection<Supplier>(DataProvider.Ins.DB.Suppliers);
                filterList.Clear();
                foreach (Supplier item in List)
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
