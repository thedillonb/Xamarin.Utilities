using System;
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
                this.RaisePropertyChanged();
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
                this.RaisePropertyChanged();
            }
        }

        private Func<Task> _moreTask;
        public Func<Task> MoreTask
        {
            get { return _moreTask; }
            set
            {
                if (_moreTask == value) return;
                _moreTask = value;
                this.RaisePropertyChanged();
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