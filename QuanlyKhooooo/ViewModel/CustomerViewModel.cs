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
    public class CustomerViewModel : BaseViewModel
    {
        private ObservableCollection<Customer> _List;
        public ObservableCollection<Customer> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Customer> _filterList;
        public ObservableCollection<Customer> filterList { get => _filterList; set { _filterList = value; OnPropertyChanged(); } }

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

        private Customer _SelectedItem;
         public Customer SelectedItem
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
        public ICommand AddCustomerCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public CustomerViewModel()
        {
            List = new ObservableCollection<Customer>(DataProvider.Ins.DB.Customers);
           
            AddCustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (DisplayName == null || Phone == null || Address == null || Email == null || ContractDate == null)
                {
                    MessageBox.Show("Bạn chưa nhập đủ", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                else
                {
                    var cus = new Customer() { DisplayName = DisplayName, Phone = Phone, Address = Address, Email = Email, ContractDate = ContractDate, MoreInfo = MoreInfo };
                    DataProvider.Ins.DB.Customers.Add(cus);
                    DataProvider.Ins.DB.SaveChanges();
                    List.Add(cus);
                    List = new ObservableCollection<Customer>(DataProvider.Ins.DB.Customers);

                }

            });


            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null) return false;

                var displayList = DataProvider.Ins.DB.Customers.Where(x => x.Id == SelectedItem.Id);
                if (displayList != null && displayList.Count() != 0) return true;
                return false;
            },

           (p) =>
           {
               var cus = DataProvider.Ins.DB.Customers.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
               cus.DisplayName = DisplayName;
               cus.Phone = Phone;
               cus.Address = Address;
               cus.Email = Email;
               cus.ContractDate = ContractDate;
               cus.MoreInfo = MoreInfo;
               DataProvider.Ins.DB.SaveChanges();

               SelectedItem.DisplayName = DisplayName;
               List = new ObservableCollection<Customer>(DataProvider.Ins.DB.Customers);

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
            if (string.IsNullOrEmpty(SearchText))
            {
                List = new ObservableCollection<Customer>(DataProvider.Ins.DB.Customers);
            }
            else
            {
                filterList = new ObservableCollection<Customer>(DataProvider.Ins.DB.Customers);
                filterList.Clear();
                foreach (Customer item in List)
                {
                    var searchTextLower = SearchText.ToLower();
                    if (item.DisplayName.ToLower().Contains(searchTextLower))
                        filterList.Add(item);
                    List = filterList;
                }
            }

        }
    }
}
