using Microsoft.Expression.Interactivity.Core;
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
    public class UnitViewModel : BaseViewModel
    {
        private ObservableCollection<Unit> _List;
        public ObservableCollection<Unit> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private string _searchText;
        public string SearchText
        {
            get
            { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        private ObservableCollection<Unit> _filterList;
        public ObservableCollection<Unit> filterList { get => _filterList; set { _filterList = value; OnPropertyChanged(); } }



        private Unit _SelectedItem;
        public Unit SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                }
            }
        }

        private string _DisplayName;

        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }
        #region commands
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SearchCommand { get; set; }



        #endregion
        public UnitViewModel()
        {
            List = new ObservableCollection<Unit>(DataProvider.Ins.DB.Units);

            AddCommand = new RelayCommand<object>((p) => 
                {
                    if (string.IsNullOrEmpty(DisplayName)) return false;

                    var displayList = DataProvider.Ins.DB.Units.Where(x => x.DisplayName == DisplayName);

                    if(displayList == null || displayList.Count() != 0)
                    {
                        return false;
                    }
                    return true;
                },
                (p) =>
                {
                    var unit = new Unit() { DisplayName = DisplayName };
                    DataProvider.Ins.DB.Units.Add(unit);
                    DataProvider.Ins.DB.SaveChanges();

                    List.Add(unit);
                });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName) || SelectedItem == null) return false;

                var displayList = DataProvider.Ins.DB.Units.Where(x => x.DisplayName == DisplayName);

                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }
                return true;
            },
                (p) =>
                {
                    var unit = DataProvider.Ins.DB.Units.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                    unit.DisplayName = DisplayName;
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
                List = new ObservableCollection<Unit>(DataProvider.Ins.DB.Units);
            }
            else
            {
                
                filterList = new ObservableCollection<Unit>(DataProvider.Ins.DB.Units);
                filterList.Clear();
                foreach ( Unit item in List)
                {
                    var searchTextLower = _searchText.ToLowerInvariant();
                    if (item.DisplayName.ToLowerInvariant().Contains(searchTextLower))
                        filterList.Add(item);
                    List = filterList;            
                } 
            }
           
           /* if (string.IsNullOrWhiteSpace(SearchText))
            {
                List = new ObservableCollection<Unit>(DataProvider.Ins.DB.Units);
            }
            else
            {
                var searchList = DataProvider.Ins.DB.Units;
                foreach(Unit item in searchList) 
                {
                    if(item.DisplayName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    {
                        searchList.Add(items);
                    }

                    List = searchList;
                }
            }*/
        }
    }
     
}
