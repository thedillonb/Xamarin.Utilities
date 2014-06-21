using System;
using System.ComponentModel;
using System.Threading.Tasks;
using ReactiveUI;
using System.Collections.Generic;

namespace Xamarin.Utilities.Core.ReactiveAddons
{
    public class ReactiveCollection<T> : ReactiveList<T>
    {
        private Func<IEnumerable<T>, IEnumerable<T>> _orderFunc;
        public Func<IEnumerable<T>, IEnumerable<T>> OrderFunc
        {
            get { return _orderFunc; }
            set
            {
                if (_orderFunc == value) return;
                _orderFunc = value;
                raisePropertyChanged(new PropertyChangedEventArgs("OrderFunc"));
            }
        }

        private Func<T, object> _groupFunc;
        public Func<T, object> GroupFunc
        {
            get { return _groupFunc; }
            set
            {
                if (_groupFunc == value) return;
                _groupFunc = value;
                raisePropertyChanged(new PropertyChangedEventArgs("GroupFunc"));
            }
        }

        private Task _moreTask;
        public Task MoreTask
        {
            get { return _moreTask; }
            set
            {
                if (_moreTask == value) return;
                _moreTask = value;
                raisePropertyChanged(new PropertyChangedEventArgs("MoreTask"));
            }
        }

        public ReactiveCollection()
        {
        }

        public ReactiveCollection(IEnumerable<T> values)
            : base(values)
        {
        }
    }
}