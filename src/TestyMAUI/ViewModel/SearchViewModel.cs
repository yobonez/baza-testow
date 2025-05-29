using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestyLogic.Models;

namespace TestyMAUI.ViewModel
{
    public class SearchViewModel : ObservableObject
    {
        private readonly TestyDBContext _dbContext;
        public SearchViewModel(TestyDBContext dbContext)
        {
            
        }
    }
}
